using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBoss : MonoBehaviour
{
    [Header("Health")]
    public  int health;
    public int scoreValue;

    private int currentHealth;

    [Header("Mouvement")]
    private Rigidbody myRigidBody;
    public float moveSpeed;
    public PlayerMovement player;

    [Header("Mob")]
    [SerializeField]
    private GameObject mob;

    private float mobInterval = 5.0f;

    public GameObject Portal;

    public float sightRange;
    public bool playerInSightRange;

    public LayerMask WhatisPlayer;

    public bool walkPointSet;

    public float walkPointRange;

    public Vector3 walkPoint;

    public LayerMask whatIsGround;

    public bool isSight;
    public bool isChase;

    public Vector3 positionSpawn;
    public float rangeWalkOutSpawn;
    public Vector3 walkPointGlobal;
    




   
    void Start()
    {
       


        //Health
        currentHealth = health;
        
        //Ennemy Mouvement
        myRigidBody=GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerMovement>();
        positionSpawn = transform.position;

        //Spawnmob
         StartCoroutine(spawnEnemy(mobInterval,mob));

 
    }

    void FixedUpdate()
    {
        //Ennemy Mouvement
        myRigidBody.velocity = (transform.forward * moveSpeed );
    }

    void Update()
    {
        //Ennemy Mouvement
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatisPlayer);

        bool raytouch = Physics.Raycast(transform.position, transform.forward, 200f, WhatisPlayer);


        if (isSight)
        {
            isChase = playerInSightRange;
        } else
        {
            isChase = playerInSightRange && raytouch;
        }

        walkPointGlobal = Move(isChase);
        int nbloop = 0 ;
        while (!CheckDestination(walkPointGlobal) && nbloop < 5){
            isChase = false;
            walkPointSet = false;
            walkPointGlobal = Move(isChase);
            nbloop += 1;
        
        }



        
        //Health
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
            Portal.SetActive(true);
        }

    }

   


    //Update Ennemy health
    public void EnemyHealthUpdate( int damage){
        currentHealth -= damage;
    }


    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
         // Position du boss 
        Vector3 coordinates = gameObject.transform.position;
        float x = coordinates.x;
        float y = coordinates.y;
        float z = coordinates.z;
        GameObject newEnemy = Instantiate(enemy, new Vector3(x+Random.Range(-2f,2f),y + Random.Range(-2f,2f), z), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval,enemy));
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

    private bool CheckDestination(Vector3 position)
    {
        bool out1 = false;
        if (position.x < positionSpawn.x + rangeWalkOutSpawn && position.y < positionSpawn.y  + rangeWalkOutSpawn && position.z < positionSpawn.z  + rangeWalkOutSpawn){
            out1 = true;
        } 
        return out1;
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
