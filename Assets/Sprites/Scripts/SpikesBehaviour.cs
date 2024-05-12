using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesBehaviour : MonoBehaviour
{
    public int spikeDamage;
    public int spikeCount;
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
        for (int i = 0; i < collision.contactCount; i++)
        {
            ZombieMovement zombie = collision.contacts[i].collider.GetComponent<ZombieMovement>();
            if (zombie != null)
            {

                zombie.TakeHit(spikeDamage);
                spikeCount--;
            }
        }
        if(spikeCount < 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            ZombieMovement zombie = collision.contacts[i].collider.GetComponent<ZombieMovement>();
            if (zombie != null)
            {
                zombie.TakeHit(spikeDamage);
                spikeCount--;
            }
        }
        if (spikeCount <= 0)
        {
            Destroy(gameObject);
        }

    }
}
