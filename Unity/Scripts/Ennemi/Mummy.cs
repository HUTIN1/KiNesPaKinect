using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mummy : MonoBehaviour
{
    [Header("Health")]
    public  int health;
    public int scoreValue;

    private int currentHealth;

    [Header("Mouvement")]
    private Rigidbody myRigidBody;
    public float moveSpeed;
    public PlayerMovement player;


   
    void Start()
    {
        //Health
        currentHealth = health;
        
        //Ennemy Mouvement
        myRigidBody=GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerMovement>();
        
    }

    void FixedUpdate()
    {
        //Ennemy Mouvement
        Vector3 moveDirection = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
        myRigidBody.velocity = moveDirection.normalized * moveSpeed;

    }

    void Update()
    {
        //Ennemy Mouvement
       // transform.LookAt(player.transform.position);
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Vector3.up);

        
        //Health
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
            //ScoreManager.scoreCount += scoreValue;
        }

    }

   


    //Update Ennemy health
    public void EnemyHealthUpdate( int damage){
        currentHealth -= damage;
    }
}
