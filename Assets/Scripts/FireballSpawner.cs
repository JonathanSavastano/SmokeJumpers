using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpawn : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float spawnRate = 2f;
    public float spawnRateIncrease = 0.1f;
    public float maxSpawnDistance = 20f;

    private float nextSpawnTime;
    private Transform playerTransform; // ref to player's transform

    public void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found"); 
        }
    }

    private void Update()
    {
        // Increase spawn rate over time
        spawnRate -= spawnRateIncrease * Time.deltaTime;
        spawnRate = Mathf.Clamp(spawnRate, 0.1f, 10f);

        // Spawn fireballs
        if (Time.time > nextSpawnTime && playerTransform != null && Vector3.Distance(transform.position,
            playerTransform.position) > maxSpawnDistance)
        {
            SpawnFireball();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnFireball()
    {
        // Randomly determine the X position within the screen bounds
        float randomX = Random.Range(-8f, 8f);

        // Instantiate a fireball at the random position
        GameObject fireball = Instantiate(fireballPrefab, new Vector3(randomX, transform.position.y, 0f), Quaternion.identity);

        // Assuming the fireball has a Rigidbody2D component
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        // Set up the initial velocity to move the fireball upward
        if (rb != null)
        {
            // Set the velocity directly
            rb.velocity = new Vector2(0f, 10f); // Adjust the speed as needed
        }       
    }
}
