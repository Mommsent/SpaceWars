using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 10f;

    //GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameObject.FindObjectOfType<GameManager>();
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(0f, speed); //add force to bullet and push it on Y axis
    }

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);// destroy anemy
            gameManager.AddScore();// add score for enemy
            Destroy(gameObject);// will destroy bullet itself
        }
    }
    */
}
