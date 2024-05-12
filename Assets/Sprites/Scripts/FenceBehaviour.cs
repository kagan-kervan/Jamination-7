using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FenceBehaviour : MonoBehaviour
{
    public float slow_percentage;
    private float initialSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetComponent<ZombieMovement>() != null)
            initialSpeed = collision.collider.GetComponent<ZombieMovement>().moveSpeed;
       for(int i = 0; i < collision.contactCount; i++) 
        {
            ZombieMovement zombie = collision.contacts[i].collider.GetComponent<ZombieMovement>();
            if(zombie != null)
            {
                zombie.moveSpeed = zombie.moveSpeed * (1- slow_percentage);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            ZombieMovement zombie = collision.contacts[i].collider.GetComponent<ZombieMovement>();
            if (zombie != null)
            {
                zombie.moveSpeed = initialSpeed;
            }
        }
    }
}
