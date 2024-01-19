using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Vector3> targetPoints = new List<Vector3>();

    private Vector3 targetPos;
    private int targetNum = 0;
    public int speed = 1;

   void Start()
   {
     targetPos = targetPoints[0];
   }

   private void FixedUpdate()
   {
    float step = speed* Time.deltaTime;
    if (transform.position == targetPos)
    {
        targetNum=(targetNum +1) % (targetPoints.Count);
        targetPos = targetPoints[targetNum];
    }
    else
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
    }
   }

   
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.transform.tag == "Player")
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collider's transform is a child of the current transform
        if (other.transform.tag == "Player")
        {
            // Remove the parent only if it's a child
            other.transform.SetParent(null);
        }
    }
   public void OnDrawGizmosSelected()
   {
        for (int i = 0; i < (targetPoints.Count); i++)
        {
            Vector3 pointsDistance = targetPoints[(i + 1) % (targetPoints.Count)] - targetPoints[i];
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(targetPoints[i], targetPoints[i] + pointsDistance / 1.15f);
        }
 
        for (int i = 0; i < (targetPoints.Count); i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(targetPoints[i], 0.1f);
            
        }
    }
}
