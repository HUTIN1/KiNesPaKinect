using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;


    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.A;
    public float throwForce;
    public Serveur client;
  



    private void Start()
    {
        client = GameObject.Find("CameraHolder").GetComponent<Serveur>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(throwKey) || client.cal.Katone) 
        {
            Debug.Log("throw");
            Throw();
        }
    }

    private void Throw()
    {

        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, attackPoint.rotation);
        
        

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
        Vector3 forceToAdd = forceDirection * throwForce  ; 


        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

 
    }

}
