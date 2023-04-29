using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingEnemy : MonoBehaviour
{
    private Vector2 position;
    private Vector2 direction;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _DeathClip;

    Animator anim;

    GameManager gameManager;

    Rigidbody2D rb;
    private float speed = -1f;
    private float strafeSpeed = 6f;

    private int randomDirection;

    private float border = 5.9f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        randomDirection = Random.Range(0, 2);
        _audioSource = GetComponent<AudioSource>();
        ChooseDirection();
    }
    private void LateUpdate()
    {
        MoveTheEnemy();
        CheckIfBorder();
    }

    private void MoveTheEnemy()
    {
        rb.velocity = direction;
    }
    private Vector2 ChooseDirection()
    {
        if (randomDirection == 0)
        {
            rb.velocity = new Vector2(-strafeSpeed, speed);
            direction = rb.velocity;
        }
        if (randomDirection == 1)
        {
            rb.velocity = new Vector2(strafeSpeed, speed);
            direction = rb.velocity;
        }
        return direction;
    }

    private void CheckIfBorder()
    {
        position = this.transform.position;
        if (position.x >= border)
        {
            rb.velocity = new Vector2(-strafeSpeed, speed);
            direction = rb.velocity;
        }
        else if (position.x <= -border)
        {
            rb.velocity = new Vector2(strafeSpeed, speed);
            direction = rb.velocity;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            anim.SetBool("IsDead", true);
            gameManager.AddScore();// add score for enemy
            _audioSource.PlayOneShot(_DeathClip);
            StartCoroutine(DelayBeforeDestroy());
            Destroy(collision.gameObject);
        }
    }

    IEnumerator DelayBeforeDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
