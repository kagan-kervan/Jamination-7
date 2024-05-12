using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Mermi hızı
    public int damage = 1; // Mermi hasarı

    private GameObject target; // Hedef nesnenin transformu
    private Vector2 initialTargetPosition; // Hedefin ilk belirlendiği pozisyon
    public Rigidbody2D rb2d;

    public void Seek()
    {
            target = GetClosestZombie();
             if( target != null )
                initialTargetPosition = target.transform.position; // Hedefin ilk pozisyonunu kaydet
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
    void Update()
    {
        Seek();
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        

        // Hedefe doğru hareket etme
        Vector2 direction = initialTargetPosition - (Vector2)transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ZombieMovement zombie = collision.collider.GetComponent<ZombieMovement>();
        if(zombie != null )
        {
            zombie.TakeHit(damage);
        }
        Destroy(this.gameObject);
    }
}
