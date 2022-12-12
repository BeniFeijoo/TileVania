using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;

    [SerializeField] int pointsForCoinPickup = 100;

    bool wasCollected = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSesion>().AddToScore(pointsForCoinPickup);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
