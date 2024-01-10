using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System;



[System.Serializable]
public class Calibration
{
    public int C;
    public float[] M;
    public float[] R;
    public float[] T;
    public float[] F;
    public float[] U;
    public static Calibration CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Calibration>(jsonString);
    }
}





public class Calibrage : MonoBehaviour
{

    Thread receiveThread;
    UdpClient client;
    int port;
    Calibration cal;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        port = 5065;
        InitUDP();

        
    }

    void InitUDP()
    {
        print ("UDP Initialized");

        receiveThread = new Thread (new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }



    private void ReceiveData()
    {
        client = new UdpClient (port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                print (">> " + text);

                cal = Calibration.CreateFromJSON(text);

            } 
            catch(Exception e)
            {
                print (e.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (cal!=null)
        {
            Vector2 fparams;
            fparams.x = cal.M[0];
            fparams.y = cal.M[4];

            Vector2 resolution;
            resolution.x = 2*cal.M[2];
            resolution.y = 2*cal.M[5];
            float vfov =  2.0f * Mathf.Atan(0.5f * resolution.y / fparams.y) * Mathf.Rad2Deg;
            cam.fieldOfView = vfov;
            cam.aspect = resolution.x / resolution.y;


            Vector3 f = new Vector3(cal.F[0], cal.F[1], cal.F[2]); // from OpenCV
            Vector3 u = new Vector3(cal.U[0], cal.U[1], cal.U[2]); // from OpenCV
            // notice that Y coordinates here are inverted to pass from OpenCV right-handed coordinates system to Unity left-handed one
            Quaternion rot = Quaternion.LookRotation(new Vector3(f.x, -f.y, f.z), new Vector3(u.x, -u.y, u.z));

            Vector3 t = new Vector3(cal.T[0], -cal.T[1], cal.T[2]);
            Vector3 pos = - (Quaternion.Inverse(rot) * t);

            // Vector3 orientation = Quaternion.Inverse(rot) * (new Vector3(0,0,1));
            
            // Quaternion q = new Quaternion(orientation[0], orientation[1], orientation[2], 1);
            cam.transform.position = pos;
            cam.transform.rotation = Quaternion.Inverse(rot);
            // Repere 2->1
            // pos = pos;
            // orientation = orientation;
        }

        
    }
}
