using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public float lifeTime;
    public int KunaiDamage;

    void Update()
    {

     lifeTime -= Time.deltaTime;    
     if(lifeTime <= 0){
            Destroy(gameObject);
        }
    }

     void OnCollisionEnter (Collision other){
         if(other.gameObject.tag == "Ghost"){
             other.gameObject.GetComponent<Ghost>().EnemyHealthUpdate(KunaiDamage);
             Destroy(gameObject);
         }

        if(other.gameObject.tag == "Mummy"){
             other.gameObject.GetComponent<Mummy>().EnemyHealthUpdate(KunaiDamage);
             Destroy(gameObject);
         }

         if(other.gameObject.tag == "MummyBoss"){
             other.gameObject.GetComponent<MummyBoss>().EnemyHealthUpdate(KunaiDamage);
             Destroy(gameObject);
         }


         if(other.gameObject.tag == "GhostBoss"){
             other.gameObject.GetComponent<GhostBoss>().EnemyHealthUpdate(KunaiDamage);
             Destroy(gameObject);
         }
         if(other.gameObject.tag == "Wall"){
            Destroy(gameObject);
         }
     }
}
