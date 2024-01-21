using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MummyBoss : MonoBehaviour
{
    [Header("Health")]
    public  int health;
    public int scoreValue;

    private int currentHealth;
    private int respawnCount = 0; // Compteur de réapparitions
    private int maxRespawns = 3;  // Nombre maximal de réapparitions

    [Header("Mouvement")]
    private Rigidbody myRigidBody;
    public float moveSpeed;
    public PlayerMovement player;


     public GameObject smallerPrefab;

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
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Vector3.up);

        
        //Health
        if(currentHealth <= 0)
        {
             RecreateSmallerObject();
            respawnCount++;

            // Si le nombre de réapparitions atteint le maximum, détruire le MummyBoss
            if (respawnCount >= maxRespawns)
            {
                Destroy(gameObject);
            }
        }

    }

   


    //Update Ennemy health
    public void EnemyHealthUpdate( int damage){
        currentHealth -= damage;
    }

    void RecreateSmallerObject()
    {
        if (smallerPrefab != null)
        {
            // Créer un nouvel objet plus petit à la même position
            GameObject smallerObject = Instantiate(smallerPrefab, transform.position, transform.rotation);

            // Vous pouvez également ajuster la taille ou d'autres propriétés de l'objet plus petit si nécessaire
            // smallerObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // Détruire l'objet actuel
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Prefab de l'objet plus petit non défini.");
        }
    }
}
