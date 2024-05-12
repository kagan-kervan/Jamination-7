using System.Linq;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab; // Mermi prefabı
    public Transform firePoint; // Ateş noktası
    public float fireRate = 1f; // Ateş hızı (saniyede kaç mermi ateş edilecek)
    [SerializeField]
    private float fireCountdown = 0f; // Ateş geri sayım süresi

     private GameObject target; // Hedef nesnenin transformu
    public Rigidbody2D rigidbody2d;



    public void Seek()
    {
        target = GetClosestZombie();
    }


    void Update()
    {
        Seek();
        if(target != null)
        {
            // Hedefe doğru yönelme
            Vector2 direction = target.transform.position - transform.position;

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
        }
        fireCountdown -= Time.deltaTime;


    }
    public GameObject GetClosestZombie()
    {
        GameObject[] zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");
        if (!zombieObjects.Any())
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
    private float FindAngle()
    {
        float angle = 0;
        GameObject closest_zombie = GetClosestZombie();
        Vector2 lookDir = (Vector2)closest_zombie.transform.position - rigidbody2d.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 180f;
        return angle;
    }

    void Shoot()
    {
        float angle = FindAngle();
        rigidbody2d.rotation = angle;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        bullet.GetComponent<BulletBehaviour>().SetDirection(angle+180f);
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
}