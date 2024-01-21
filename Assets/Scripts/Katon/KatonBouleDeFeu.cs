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
         if(other.gameObject.tag == "Ghost"){
             other.gameObject.GetComponent<Ghost>().EnemyHealthUpdate(FireBallDamage);
             Destroy(gameObject);
         }

        if(other.gameObject.tag == "Mummy"){
             other.gameObject.GetComponent<Mummy>().EnemyHealthUpdate(FireBallDamage);
             Destroy(gameObject);
         }

         if(other.gameObject.tag == "MummyBoss"){
             other.gameObject.GetComponent<MummyBoss>().EnemyHealthUpdate(FireBallDamage);
             Destroy(gameObject);
         }


         if(other.gameObject.tag == "GhostBoss"){
             other.gameObject.GetComponent<GhostBoss>().EnemyHealthUpdate(FireBallDamage);
             Destroy(gameObject);
         }
         if(other.gameObject.tag == "Wall"){
            Destroy(gameObject);
         }
     }

}
