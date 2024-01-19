using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [Header("Health")]
    public  int health;
    public int scoreValue;

    private int currentHealth;

    [Header("Mouvement")]
    private Rigidbody myRigidBody;
    public float moveSpeed;
    public PlayerMovement player;
    public Transform player_transform;

    public float sightRange;
    public bool playerInSightRange;

    public LayerMask WhatisPlayer;

    public bool walkPointSet;

    public float walkPointRange;

    public Vector3 walkPoint;

    public LayerMask whatIsGround;



    void Start()
    {
        //Health
        currentHealth = health;
        agent = GetComponent<NavMeshAgent>();
        
        //Ennemy Mouvement
        myRigidBody=GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerMovement>();
        //player_transform = player.transform;
        
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
/*        Debug.Log("in sight range : " + playerInSightRange);
*/        if (playerInSightRange)
        {
            Debug.Log("Chase Player");
            ChasePlayer();
        }
        else
        {
            Debug.Log("Patroling");
            Patroling();  
        }


        
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

    private void ChasePlayer()
    {
        Debug.Log("position player : " + player_transform.position);
        //agent.SetDestination(player_transform.position);
        transform.LookAt(player.transform.position);
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if(walkPointSet) 
            //agent.SetDestination(walkPoint);
            transform.LookAt(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }
}
