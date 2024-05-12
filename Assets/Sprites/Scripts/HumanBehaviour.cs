using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HumanBehaviour : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    [SerializeField]
    private char[] PatrolRoute;
    private int patrolMovementIndex;
    public float patrolDelay;
    [SerializeField]
    private float patrolWait;
    public float verticalMoveOffset;
    public float horizontalMoveOffset;
    public float meleeRange;
    public float attackWait;
    [SerializeField]
    private float attackTime;
    public float rangedRange;
    public float rangedWait;
    [SerializeField]
    private float rangedWaitDelay;

    public Transform firePoint;
    public GameObject bulletObject;
    public GameObject[] zombieObjects;
    public enum EnemyBehaviour
    {
        IDLE,
        PATROL,
        RANGED,
        MELEE,
        DEATH
    }
    private EnemyBehaviour behaviour;
    // Start is called before the first frame update
    void Start()
    {
        patrolMovementIndex = 0;
        patrolWait = 0;
        attackTime = 0;
        rangedWaitDelay = 0;
        zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");
    }

    // Update is called once per frame
    void Update()
    {
        patrolWait = Timer(patrolWait);
        attackTime = Timer(attackTime);
        rangedWaitDelay = Timer(rangedWaitDelay);
        BehaviourCheck();
        if(behaviour == EnemyBehaviour.RANGED && rangedWaitDelay < 0)
        {
            RangedAttack();
            rangedWaitDelay = rangedWait;
        }
        if(behaviour == EnemyBehaviour.MELEE && attackTime < 0)
        {

        }
        if (behaviour == EnemyBehaviour.PATROL && patrolWait < 0)
        {
            Patrol();
        }
    }

    public void RangedAttack()
    {
        float angle = FindAngle();
        rigidbody2d.rotation = angle;
        GameObject bullet = Instantiate(bulletObject,firePoint.position, Quaternion.Euler(0, 0, angle));
        bullet.GetComponent<BulletBehaviour>().SetDirection(angle);
        // Ignore collision between bullet and walls
        GameObject[] wallObjs = GameObject.FindGameObjectsWithTag("Wall"); 
        Collider2D[] wallColliders = new Collider2D[wallObjs.Length];
        for (int i = 0; i < wallObjs.Length; i++)
        {
            wallColliders[i] = wallObjs[i].GetComponent<Collider2D>();
        }
        foreach (Collider2D wallCollider in wallColliders)
        {
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), wallCollider);
        }
    }
    
    private float FindAngle()
    {
        float angle = 0;
        GameObject closest_zombie = GetClosestZombie();
        Vector2 lookDir = (Vector2)closest_zombie.transform.position - rigidbody2d.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x)*Mathf.Rad2Deg;
        return angle;
    }

    public void BehaviourCheck()
    {
        GameObject closestZombie = GetClosestZombie();
        if(closestZombie != null)
        {
            float dist = Vector2.Distance(closestZombie.transform.position, this.transform.position);
            /* Dismissed for easing the process
            if (dist < meleeRange)
            {
                Debug.Log("Melee Range");
                behaviour = EnemyBehaviour.MELEE;
            }
            */
            if (dist < rangedRange)
            {
                //Debug.Log("Ranged Mode");
                behaviour = EnemyBehaviour.RANGED;
            }
            else
            {
                behaviour = EnemyBehaviour.PATROL;
            }
        }
        else
        {
            behaviour = EnemyBehaviour.PATROL;
        }
    }


    public void Patrol()
    {
        //Make the movement. When finishes takes back.
        rigidbody2d.rotation = 0f;
        Movement(PatrolRoute[patrolMovementIndex]);
        patrolMovementIndex++;
        patrolMovementIndex %= PatrolRoute.Length;
        patrolWait = patrolDelay;
    }
    private float Timer(float time)
    {
        return time - Time.deltaTime;
    }
    public void Movement(char moveChar)
    {
        var pos = transform.position;
        switch (moveChar)
        {
            case 'S':
                pos.y = pos.y - verticalMoveOffset;
                break;
            case 'N':
                pos.y += verticalMoveOffset;
                break;
            case 'W':
                pos.x = pos.x - horizontalMoveOffset;
                break;
            case 'E':
                pos.x += horizontalMoveOffset;
                break;
            default:
                break;
        }
        transform.position = pos;
    }

    public GameObject GetClosestZombie()
    {
        zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");
        if(!zombieObjects.Any()) 
            return null;
        int index = 0;
        float distance = float.MaxValue;
        for (int i = 0; i < zombieObjects.Length; i++)
        {
            float dist = GetDistance(zombieObjects[index]);
            if (dist < distance)
            {
                distance = dist;
                index = i;
            }
        }
        return zombieObjects[index];
    }

    public float GetDistance(GameObject obj)
    {
        var obj_pos = obj.transform.position;
        var distance = Vector2.Distance(obj_pos, transform.position);
        return distance;
    }
}
