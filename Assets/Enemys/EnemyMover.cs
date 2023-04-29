using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    private float speed = -4f;
    Rigidbody2D rigidBody;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _DeathClip;

    GameManager gameManager;

    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        rigidBody.velocity = new Vector2(0, speed); //push the enemy downside
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
