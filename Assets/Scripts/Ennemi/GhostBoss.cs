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



   
    void Start()
    {
       


        //Health
        currentHealth = health;
        
        //Ennemy Mouvement
        myRigidBody=GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerMovement>();

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
        transform.LookAt(player.transform.position);


        
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
        
}
