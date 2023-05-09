using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipControl : MonoBehaviour
{
    public static UnityEvent GameOver = new UnityEvent();

    private Animator _ship_animator;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        _ship_animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        Cursor.visible = false;
    }

    private void Update()
    {
        if (_IsDead == false)
        {
            Move();
            Shoot();
        }
    }


    private float _speed = 10f;
    private float _xLimit = 13f;
    private float _uppYLimit = 7f;
    private float _downYLimmit = -4.3f;
    private void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        transform.Translate(xInput * _speed * Time.deltaTime, 0f, 0f);
        transform.Translate(0f, yInput * _speed * Time.deltaTime, 0f);

        Vector2 xPosition = transform.position;
        xPosition.x = Mathf.Clamp(xPosition.x, -_xLimit, _xLimit);
        transform.position = xPosition;
        Vector2 Yposition = transform.position;
        Yposition.y = Mathf.Clamp(Yposition.y, _downYLimmit, _uppYLimit);
        transform.position = Yposition;
    }


    private Vector2 _spawnPos;
    private float elapsedTime = 0f;
    private float _reloadTime = 0.5f;
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField] private AudioClip _ShotClip;

    private void Shoot()
    {
        elapsedTime += Time.deltaTime;

        if (Input.GetButton("Jump") && elapsedTime > _reloadTime)
        {
            _spawnPos = transform.position;
            _spawnPos += new Vector2(0, 1.2f);
            Instantiate(_bulletPrefab, _spawnPos, Quaternion.identity);
            _audioSource.PlayOneShot(_ShotClip);
            elapsedTime = 0f; //reset bullet firing timer
        }
    }


    [SerializeField]
    private GameObject _shildPrefab;
    [SerializeField]
    private GameObject _gunUpPrefab;
    private bool _isShilded = false;
    private bool _IsDead = false;
    private bool _isShootFasterActive = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShootFasterPowerUP"))
        {
            _isShootFasterActive = true;
            _reloadTime /= 2f;

            if(_isShootFasterActive == true) 
                durationTime += 6f;

            Destroy(other.gameObject);
            _gunUpPrefab.SetActive(true);
            StartCoroutine(BuffDuration());
        }
        else if (other.CompareTag("ShieldPowerUp"))
        {
            _isShilded = true;

            if(_isShilded == true)
                shildedTime += 4f;

            Destroy(other.gameObject);
            _shildPrefab.SetActive(true);
            StartCoroutine(ShildedTime());
        }
        else if (other.CompareTag("Enemy") && _isShilded == false && _IsDead == false)
        {
            _IsDead = true;
            Destroy(other.gameObject);
            PlayAnimOfDeath();

            GameOver.Invoke();

            StartCoroutine(DelayBeforeDestroy());
        }
        else if (other.CompareTag("EnemyBullet") && _IsDead == false && _isShilded == false)
        {
            _IsDead = true;
            Destroy(other.gameObject);
            PlayAnimOfDeath();

            GameOver.Invoke();

            StartCoroutine(DelayBeforeDestroy());
        }
    }


    [SerializeField] private AudioClip _DeathClip;

    private void PlayAnimOfDeath()
    {
        _ship_animator.SetBool("IsDead", true);
        _audioSource.PlayOneShot(_DeathClip);
    }

    private float durationTime = 6f;
    IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(durationTime);
        _gunUpPrefab.SetActive(false);
        _isShootFasterActive = false;
        _reloadTime = 0.5f;
        durationTime = 6f;
    }

    private float shildedTime = 4f;
    IEnumerator ShildedTime()
    {
        yield return new WaitForSeconds(shildedTime);
        _shildPrefab.SetActive(false);
        _isShilded = false;
        shildedTime = 4f;
    }

    IEnumerator DelayBeforeDestroy()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
