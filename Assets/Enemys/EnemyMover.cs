using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : Enemy
{
    private float _speed = -3f;
    private Vector2 _direction;

    private Rigidbody2D _rigidBody2D;

    private PolygonCollider2D _polygonCollider2D;
    private Animator _anim;

    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _DeathClip;
    [SerializeField]
    private AudioClip _ShotClip;

    //private bool isDead = false;

    private int _pointForDefeating = 1;

    // Start is called before the first frame update
    void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Start()
    {
        _direction = new Vector2(0, _speed);
        MoveTheEnemy(_rigidBody2D , _direction);
    }

    private void Update()
    {
        //if (isDead == false)
        //{
        //    Shoot(_audioSource, _ShotClip);
        //}
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            //isDead = true;
            DeactivateRenderAndCollision(_polygonCollider2D);
            PlayAnimAndEffectsOfDeath(_anim, _audioSource, _DeathClip);
            DestroyAnEnemyAndBullet(collision);
            //give points value for defeating an enemy to all subscribed methods
            EnemyIsDied.Invoke(_pointForDefeating);
        }
    }
}
