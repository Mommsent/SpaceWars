using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoamingEnemy : MonoBehaviour, IDead
{
    private Vector2 _position;
    private Vector2 _direction;

    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _DeathClip;

    private Animator _anim;

    private PolygonCollider2D _polygonCollider2D;

    private int _pointForDefeating = 2;

    public static UnityEvent<int> EnemyIsDied = new UnityEvent<int>();

    private Rigidbody2D _rigidbody2D;
    private float _speed = -1f;
    private float _strafeSpeed = 6f;

    private int _randomDirection;

    private float _border = 5.9f;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _randomDirection = Random.Range(0, 2);
        _audioSource = GetComponent<AudioSource>();
<<<<<<< Updated upstream
=======
    }

    private void Start()
    {
>>>>>>> Stashed changes
        ChooseDirection();
        StartCoroutine(CanShoot());
        _canShoot = true;
    }
    private void LateUpdate()
    {
<<<<<<< Updated upstream
        MoveTheEnemy();
        CheckIfBorder();
=======
        if(_isDead == false)
        {
            MoveTheEnemy(_rigidbody2D, _direction);
            if(_canShoot == true) Shoot(_audioSource, _ShotClip);

            CheckIfBorder();
        }
>>>>>>> Stashed changes
    }

    private void MoveTheEnemy()
    {
        _rigidbody2D.velocity = _direction;
    }
    private Vector2 ChooseDirection()
    {
        if (_randomDirection == 0)
        {
            _rigidbody2D.velocity = new Vector2(-_strafeSpeed, _speed);
            _direction = _rigidbody2D.velocity;
        }
        if (_randomDirection == 1)
        {
            _rigidbody2D.velocity = new Vector2(_strafeSpeed, _speed);
            _direction = _rigidbody2D.velocity;
        }
        return _direction;
    }

    private void CheckIfBorder()
    {
        _position = this.transform.position;
        if (_position.x >= _border)
        {
            _rigidbody2D.velocity = new Vector2(-_strafeSpeed, _speed);
            _direction = _rigidbody2D.velocity;
        }
        else if (_position.x <= -_border)
        {
            _rigidbody2D.velocity = new Vector2(_strafeSpeed, _speed);
            _direction = _rigidbody2D.velocity;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {

            //give points value for defeating an enemy to all subscribed methods 
            EnemyIsDied.Invoke(_pointForDefeating);

            _anim.SetBool("IsDead", true);
            DeactivateRenderAndCollision();
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

    private bool _canShoot;
    IEnumerator CanShoot()
    {
        yield return new WaitForSeconds(9f);
        _canShoot = false;
    }
}
