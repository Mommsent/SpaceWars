using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightEnemy : Enemy
{
    private float _speed = -4f;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            DeactivateRenderAndCollision(_polygonCollider2D);
            PlayAnimAndEffectsOfDeath(_anim, _audioSource, _DeathClip);
            DestroyAnEnemyAndBullet(collision);
            //give points value for defeating an enemy to all subscribed methods
            EnemyIsDied.Invoke(_pointForDefeating);
        }
    }
}
