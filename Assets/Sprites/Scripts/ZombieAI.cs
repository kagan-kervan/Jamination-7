using UnityEngine;
using UnityEngine.XR;

public class ZombieMovement : MonoBehaviour
{
    public string targetTag; // Hedef objenin etiketi
    public float moveSpeed = 5f; // Zombie'nin hareket hýzý
    public int life;

    private Rigidbody2D rb;
    private Transform target; // Hedef objenin referansý

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Hedefi etiketi kullanarak bul
        target = GameObject.FindGameObjectWithTag(targetTag)?.transform;
    }

    void Update()
    {
        if (target != null)
        {
            // Hedefe doðru dönme
            Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
            transform.up = direction;

            // Hedefin pozisyonuna doðru hareket etme
            Vector2 moveDirection = direction * moveSpeed;
            rb.velocity = moveDirection;
        }
        else
        {
            // Hedef yoksa Zombie durur
            rb.velocity = Vector2.zero;
        }
    }

    public void TakeHit(int damage)
    {
        life = life - damage;
        if (life < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetComponent<WallBehaviour>() != null)
        {
            collision.collider.GetComponent<WallBehaviour>().GetHit();
            if (rb.velocity.x > 0) // Moving left
            {
                // Move slightly to the left to separate from the wall
                transform.position += Vector3.left * 0.2f;
            }
            else // Moving right
            {
                // Move slightly to the right to separate from the wall
                transform.position += Vector3.right * 0.2f;
            }

            // Reset velocity to allow for subsequent collisions
            rb.velocity = Vector2.zero;
        }
    }
}