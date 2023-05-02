using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour, IDead
{
    private float speed = -4f;
    private Rigidbody2D rigidBody;

    private int pointForDefeating = 1;

    private PolygonCollider2D _polygonCollider2D;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _DeathClip;

    public delegate void EnemyDied(int points);
    public static event EnemyDied EnemyIsDied;

    private Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();

        rigidBody = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        rigidBody.velocity = new Vector2(0, speed); //push the enemy downside
    }

    //Check if destroed by player`s bullet
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            DeactivateRenderAndCollision();
            anim.SetBool("IsDead", true);

            //give points value for defeating an enemy to all subscribed methods
            if (EnemyIsDied != null)
                EnemyIsDied(pointForDefeating);

            _audioSource.PlayOneShot(_DeathClip);
            StartCoroutine(DelayBeforeDestroy());
            Destroy(collision.gameObject);
        }
    }

    //Give an opportunity to play death anim.
    IEnumerator DelayBeforeDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void DeactivateRenderAndCollision()
    {
        _polygonCollider2D.enabled = false;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            var child = this.transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(false);
        }
    }
}
