using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
        public int health;
        private int currentHealth; 

        public float flashLength;
        public float flashCounter;
        private Renderer renderer;
        private Color color;


        
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        renderer = GetComponent<Renderer>();
        color = renderer.material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
        gameObject.SetActive(false);
        }

        if(flashCounter >0){
            flashCounter -=Time.deltaTime;
            if(flashCounter <=0)
            {
                renderer.material.SetColor("_Color", color);
            }
        }

           
    }

    public void EnemyDamageOnPlayer(int damage){
        currentHealth -= damage;
        flashCounter = flashLength;
        renderer.material.SetColor("_Color",Color.red);
    }

}
