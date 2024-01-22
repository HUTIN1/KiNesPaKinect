using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana_damage : MonoBehaviour
{
	public Serveur client;
	private bool s = false;
	
	private Vector3 speed;
	private Vector3 previousPosition;
	private int KatanaDamage = 0;
	
    void Start()
    {
        client = GameObject.Find("CameraHolder").GetComponent<Serveur>();
        
    }
	
	void Update()
	{ 
	
		if(s != client.s)
		{
			s = !s;
			
			Vector3 currentPosition = transform.GetChild(1).gameObject.transform.position - transform.position;
			
			speed = (currentPosition - previousPosition) / Time.deltaTime;
			
			previousPosition = currentPosition;
			
			Debug.Log("Collision " + speed.magnitude);  
		}
		
		KatanaDamage = (int) speed.magnitude / 5;
		if(KatanaDamage > 5)
		{
			KatanaDamage = 5;
		} 
	}
	
	
    void OnCollisionEnter (Collision other){
     
     	if(tag != "Garde")
     	{
		 	if(other.gameObject.tag == "Ghost"){
		         other.gameObject.GetComponent<Ghost>().EnemyHealthUpdate(KatanaDamage);
		     }

		    if(other.gameObject.tag == "Mummy"){
		         other.gameObject.GetComponent<Mummy>().EnemyHealthUpdate(KatanaDamage);
		     }

		     if(other.gameObject.tag == "MummyBoss"){
		         other.gameObject.GetComponent<MummyBoss>().EnemyHealthUpdate(KatanaDamage);
		     }

		     if(other.gameObject.tag == "GhostBoss"){
		         other.gameObject.GetComponent<GhostBoss>().EnemyHealthUpdate(KatanaDamage);
		     }
		}

	}
}