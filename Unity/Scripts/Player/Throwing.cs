using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrowA;
    public GameObject objectToThrowB;


    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.A;
    public float throwForceA;
    
    public KeyCode throwKey1 = KeyCode.E;
    public float throwForceB;

    public Serveur client;



    private void Start()
    {
        client = GameObject.Find("CameraHolder").GetComponent<Serveur>();
    }

    private void Update()
    {
        //Katone
        if(Input.GetKeyDown(throwKey) ||client.cal.Katone) 
        {
            Throw1();
        }

        //Shuriken
        if(Input.GetKeyDown(throwKey1)||client.cal.Shuriken) 
        {
            Throw2();
        }
    }

    private void Throw1()
    {

        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrowA, attackPoint.position, attackPoint.rotation);
        
        

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - cam.position).normalized;
        }
        forceDirection = cam.forward;
        // add force
        Vector3 forceToAdd = forceDirection * throwForceA  ; 


        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

 
    }

    private void Throw2()
    {

        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrowB, attackPoint.position, attackPoint.rotation);
        
        

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - cam.position).normalized;
        }
        forceDirection = cam.forward;
        // add force
        Vector3 forceToAdd = forceDirection * throwForceB  ; 


        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

 
    }

}
