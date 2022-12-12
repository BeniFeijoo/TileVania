using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D myRigidbody;

    [SerializeField] float bulletSpeed = 5f;
    PlayerMovement player;

    float xSpeed;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Touch");
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        Destroy(gameObject);
    }
}
