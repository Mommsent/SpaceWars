using System.Collections;
using UnityEngine;


public class RoamingEnemy : Enemy
{
    private Vector2 _position;
    private Vector2 _direction;

    private int _pointForDefeating = 2;

    private Rigidbody2D _rigidbody2D;
    private float _speed = -1.5f;
    private float _strafeSpeed = 10f;
    private int _randomDirection;
    private float _border = 12f;
    private bool _isDead = false;

    private PolygonCollider2D _polygonCollider2D;

    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _DeathClip;
    [SerializeField]
    private AudioClip _ShotClip;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ChooseDirection();
        StartCoroutine(CanShoot());
        _canShoot = true;
    }

    private void LateUpdate()
    {
        if(_isDead == false)
        {
            MoveTheEnemy(_rigidbody2D, _direction);
            if(_canShoot == true) Shoot();

            CheckIfBorder();
        }
    }

    [SerializeField]
    private GameObject _bulletPrefab;

    private float elapsedTime = 0f;
    private Vector2 _spawnPos;
    private float _reloadTime;
    public void Shoot()
    {
        elapsedTime += Time.deltaTime;
        _reloadTime = Random.Range(0.5f, 2f);

        if (elapsedTime > _reloadTime)
        {
            _spawnPos = transform.position;
            _spawnPos += new Vector2(0, -1.2f);
            Instantiate(_bulletPrefab, _spawnPos, Quaternion.identity);
            _audioSource.PlayOneShot(_ShotClip);
            elapsedTime = 0f; //reset bullet firing timer
        }
    }

    private Vector2 ChooseDirection()
    {
        _randomDirection = Random.Range(0, 2);

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
        if (collision.gameObject.tag == "PlayerBullet")
        {
            _isDead = true;
            DeactivateRenderAndCollision(_polygonCollider2D);

            //give points value for defeating an enemy to all subscribed methods
            EnemyIsDied.Invoke(_pointForDefeating);

            PlayAnimAndEffectsOfDeath(_anim, _audioSource, _DeathClip);
            DestroyAnEnemyAndBullet(collision);
        }
    }

    private bool _canShoot;
    IEnumerator CanShoot()
    {
        yield return new WaitForSeconds(9f);
        _canShoot = false;
    }
}
