using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System;


[System.Serializable]
public class Calibration
{
    public float C;
    public bool Jump;
    public float Angle;
    public bool Katone;
    public bool Forward;
    public float[] Kat_pos;
    public float[] Kat_dir;
    public bool Kat_garde;
    public bool S;
    public static Calibration CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Calibration>(jsonString);
    }
}

public class Serveur : MonoBehaviour
{
    [SerializeField]
    public Calibration cal;

    public int port = 5065;
    Thread receiveThread;
    UdpClient client;
    // Start is called before the first frame update
    void Start()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                //print(">> " + text);

                cal = Calibration.CreateFromJSON(text);
                s = cal.S;

            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("C : " + cal.C + ", Jump : " + cal.Jump + ", Angle : " + cal.Angle);
    }

    public float getC()
    {
        return cal.C;
    }

	public bool s = false;

}
