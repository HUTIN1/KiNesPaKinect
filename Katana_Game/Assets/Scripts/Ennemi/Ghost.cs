using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
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
        myRigidBody.velocity = (transform.forward * moveSpeed );
    }

    void Update()
    {
        //Ennemy Mouvement
        transform.LookAt(player.transform.position);


        
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
