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

    public float sightRange;
    public bool playerInSightRange;

    public LayerMask WhatisPlayer;

    public bool walkPointSet;

    public float walkPointRange;

    public Vector3 walkPoint;

    public LayerMask whatIsGround;

    public bool isSight;
    public bool isChase;
    public Vector3 walkPointGlobal;

    


   
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
        //transform.LookAt(player.transform.position);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatisPlayer);

        bool raytouch = Physics.Raycast(transform.position, transform.forward, 200f, WhatisPlayer);
        raytouch = true;


        if (isSight)
        {
            isChase = playerInSightRange;
        } else
        {
            isChase = playerInSightRange && raytouch;
        }

        walkPointGlobal = Move(isChase);


        
        //Health
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }

    }

   
    

    //Update Ennemy health
    public void EnemyHealthUpdate( int damage){
        currentHealth -= damage;
    }

    private Vector3 ChasePlayer()
    {
        //agent.SetDestination(player_transform.position);
        transform.LookAt(player.transform.position);
        return player.transform.position;
    }

    private Vector3 Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if(walkPointSet) 
            //agent.SetDestination(walkPoint);
            transform.LookAt(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

        return walkPoint;
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }



    private Vector3 Move(bool isChase){
        if (isChase)
        {
            isSight = true;
            Debug.Log("Chase Player");
            walkPointGlobal = ChasePlayer();
        }
        else
        {
            isSight = false;
            Debug.Log("Patroling");
            walkPointGlobal = Patroling();  
        }
        return walkPointGlobal;
    }
}
