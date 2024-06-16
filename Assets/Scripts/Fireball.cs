using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HealthPickup"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            FindObjectOfType<AudioManager>().Play("FireballHit");
        }
    }
}
