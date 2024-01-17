using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatonBouleDeFeu : MonoBehaviour
{
    public float lifeTime;
    public int FireBallDamage;

    void Update()
    {

     lifeTime -= Time.deltaTime;    
     if(lifeTime <= 0){
            Destroy(gameObject);
        }
    }

     void OnCollisionEnter (Collision other){
         if(other.gameObject.tag == "Enemy"){
             other.gameObject.GetComponent<EnemyManager>().EnemyHealthUpdate(FireBallDamage);
             Destroy(gameObject);
         }
         if(other.gameObject.tag == "Wall"){
            Destroy(gameObject);
         }
     }

}
