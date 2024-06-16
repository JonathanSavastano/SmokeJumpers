using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public GameObject healthPickupPrefab; 
    public float spawnRate = 10f;
    private Transform playerTransform;
    private float nextSpawnTime;
    public float maxSpawnDistance = 20f;

    private Rigidbody2D rb;
    private Health playerHealth; // Reference to the Health script

    // Start is called before the first frame update
    public void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<Health>(); // Get the Health component from the player
            if (playerHealth == null)
            {
                Debug.LogError("Health component not found on the player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player not found"); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnRate = Mathf.Clamp(spawnRate, 1f, 10f);

        if (Time.time > nextSpawnTime && playerTransform != null && Vector3.Distance(transform.position,
            playerTransform.position) > maxSpawnDistance)
            {
                spawnHealth();
                nextSpawnTime = Time.time + spawnRate;
            }
    }

    void spawnHealth()
    {
        if (playerHealth != null && playerHealth.health < 3)
        {
            // Randomly determine the X and Y position within the screen bounds
            float randomX = Random.Range(-8f, 8f);
         
            // Instantiate a health pickup at the random position
            GameObject healthPickup = Instantiate(healthPickupPrefab, new Vector3(randomX, transform.position.y, 0f), Quaternion.identity);

            // Assuming the fireball has a Rigidbody2D component
            Rigidbody2D rb = healthPickupPrefab.GetComponent<Rigidbody2D>();
        }
    }
}
