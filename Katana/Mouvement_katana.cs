using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouvement_katana : MonoBehaviour
{
  
    public Serveur client;
    private bool s = false;
    
    private BoxCollider hitBox;

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.Find("CameraHolder").GetComponent<Serveur>();
        hitBox = GetComponent<BoxCollider>();
        transform.localPosition = new Vector3(0.25f, -0.25f, 0.75f);
        transform.localRotation =Quaternion.LookRotation(new Vector3(0, 1, 0));
            	
        
    }



    // Update is called once per frame
    void Update()
    {
        
        if (client.cal!=null)
        {
            
        	float[] P = client.cal.Kat_pos;
        	float[] V = client.cal.Kat_dir;

			if(s!=client.s)
			{
            	transform.localPosition = new Vector3(0.25f+P[0], P[1], 0.75f);
            	transform.localRotation =Quaternion.LookRotation(new Vector3(V[0], V[1], V[2]));
            	s = !s;
            }
            if (client.cal.Kat_garde)
            {
            	tag = "Garde";
            	hitBox.size = new Vector3(10, 20, 100);
            	hitBox.isTrigger = false;
            	
            }
            else
            {
            	tag = "Untagged";
            	hitBox.size = new Vector3(5, 10, 50);
            	hitBox.isTrigger = true;
            }
            
        }

        

        
    }}
