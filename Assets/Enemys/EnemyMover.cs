using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMover : MonoBehaviour, IDead
{
    private float _speed = -4f;
    private Rigidbody2D _rigidBody;

    private int _pointForDefeating = 1;

    private PolygonCollider2D _polygonCollider2D;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _DeathClip;

    public static UnityEvent<int> EnemyIsDied = new UnityEvent<int>();

    private Animator _anim;

    // Start is called before the first frame update
    void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();

        _rigidBody = GetComponent<Rigidbody2D>();

        _anim = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        _rigidBody.velocity = new Vector2(0, _speed); //push the enemy downside
    }

    //Check if destroed by player`s bullet
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            DeactivateRenderAndCollision();
            _anim.SetBool("IsDead", true);

            //give points value for defeating an enemy to all subscribed methods
            EnemyIsDied.Invoke(_pointForDefeating);

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
