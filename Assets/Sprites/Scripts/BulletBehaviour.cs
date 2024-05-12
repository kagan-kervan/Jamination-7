using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float HorizontalOffset;
    public float VerticalOffset;
    public float moveSpeed;
    public int damage;
    [SerializeField]
    private bool isHorizontal;
    public Rigidbody2D rb2d;
    public void SetDirection(float angle)
    {
        // Convert angle to direction
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        rb2d.velocity = direction * moveSpeed; // Set bullet velocity
    }
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check for out of bounds
        if (transform.position.x < -100f || transform.position.x > 100f ||
            transform.position.y < -50 || transform.position.y > 50)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ZombieMovement zombie = collision.collider.gameObject.GetComponent<ZombieMovement>();
        if(zombie != null)
        {
            zombie.TakeHit(damage);
            Destroy(this.gameObject);
        }
    }

}
