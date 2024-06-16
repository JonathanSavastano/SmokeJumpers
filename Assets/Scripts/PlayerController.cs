using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Health healthScript; // ref to health
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float fallSpeed = 3f;
    public float respawnThreshold = -40f; // how far we go before tele to top
    public float fireballDestroyThreshold = 10f; 

    private Rigidbody2D rb;
    private Vector3 lastPosition;

    private bool isMovingRight = true;

    void Start()
    {
        healthScript = GetComponent<Health>();

        if (healthScript == null)
        {
            Debug.LogError("Health script not found on the same GameObject as PlayerController.");
        }

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // disable default gravity

        lastPosition = transform.position;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMovingRight = !isMovingRight;
        }

        float horInput = isMovingRight ? 1f : -1f;
        Vector2 movement = new Vector2(horInput * moveSpeed, -fallSpeed);
        rb.velocity = movement;

        // Rotate the sprite based on the movement direction
        RotateSprite(horInput);

        // check if player is below respawn threshold
        if (transform.position.y < respawnThreshold) 
        {
            Respawn();
        }

        CheckAndDestroyFireballs();
    }

    private void RotateSprite(float direction)
    {
        if (direction != 0)
        {
            // Apply a fixed 30-degree angle towards the movement direction
            float targetAngle = Mathf.Sign(direction) * 30f;

            // Smoothly interpolate between the current rotation and the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), rotationSpeed * Time.deltaTime);
        }
    }

    void Respawn()
    {
        // Calculate the difference in Y position between the player's current position and the respawn threshold
        float yOffset = respawnThreshold - transform.position.y;

        // Move the player to the top of the level
        transform.position = new Vector3(lastPosition.x, lastPosition.y + 200f, lastPosition.z);

        // Find all game objects with the "Fireball" tag
        GameObject[] fireballs = GameObject.FindGameObjectsWithTag("Fireball");

        // Dictionary to store the relative positions of fireballs
        Dictionary<GameObject, Vector3> fireballRelativePositions = new Dictionary<GameObject, Vector3>();

        // Store the relative positions of fireballs
        foreach (var fireball in fireballs)
        {
            fireballRelativePositions.Add(fireball, fireball.transform.position - lastPosition);
        }

        // Move each fireball along with the player on the Y axis
        foreach (var fireball in fireballs)
        {
            // Get the relative position from the dictionary
            Vector3 relativePosition = fireballRelativePositions[fireball];

            // Apply the offset to the fireball's position
            fireball.transform.position = new Vector3(transform.position.x + relativePosition.x, transform.position.y + relativePosition.y + yOffset, transform.position.z + relativePosition.z);
        }
    }

    private void LateUpdate()
    {
        lastPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            Destroy (collision.gameObject); // destroy fireball
            GameManager.Instance.IncrementFireballCollisions();

            FindObjectOfType<AudioManager>().Play("PlayerHurt");
            FindObjectOfType<AudioManager>().Play("FireballHit");

            healthScript.DecrementHealth();
        }
        // change movement direction after boundary hit
        if (collision.gameObject.CompareTag("Boundary"))
        {
            if (isMovingRight)
            {
                isMovingRight = false;
            }
            else {
                isMovingRight = true;
            }
        }
        // decrement fireball hits if collide with health pickup
        if (collision.gameObject.CompareTag("HealthPickup"))
        {
            Destroy (collision.gameObject); // destroy health pickup
            GameManager.Instance.DecrementFireballCollisions();

            healthScript.IncrementHealth();
        }
    }

    private void CheckAndDestroyFireballs()
    {
        GameObject[] fireballs = GameObject.FindGameObjectsWithTag("Fireball");

        foreach (var fireball in fireballs)
        {
            float distanceToPlayer = fireball.transform.position.y - transform.position.y;

            if (distanceToPlayer > fireballDestroyThreshold)
            {
                Destroy(fireball);
            }
        }
    }
}
