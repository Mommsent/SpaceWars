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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        if (_IsDead == false)
        {
            GetAxisOfPlayerInput();
            Shoot();
        }
    }

    private void GetAxisOfPlayerInput()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Move(xInput, yInput);
        Rotate(xInput);

        float mousXInput = Input.GetAxis("Mouse X") * 0.7f;
        float mousYInput = Input.GetAxis("Mouse Y") * 0.8f;
        Move(mousXInput, mousYInput);
        Rotate(mousXInput);
    }

    private float tiltAngle = 10.0f;
    private float smooth = 3.0f;
    private void Rotate(float xInput)
    {
        float tiltAroundZ = -xInput * tiltAngle;
        Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }

    private float _speed = 12f;
    private float _transformXLimit = 13f;
    private float _uppYLimit = 7f;
    private float _downYLimmit = -4.3f;
    private void Move(float xInput, float yInput)
    {
        transform.Translate(xInput * _speed * Time.deltaTime, 0f, 0f);
        transform.Translate(0f, yInput * _speed * Time.deltaTime, 0f);

        Vector2 xPosition = transform.position;
        xPosition.x = Mathf.Clamp(xPosition.x, -_transformXLimit, _transformXLimit);
        transform.position = xPosition;

        Vector2 yPosition = transform.position;
        yPosition.y = Mathf.Clamp(yPosition.y, _downYLimmit, _uppYLimit);
        transform.position = yPosition;
    }


    [SerializeField]
    private Transform _spawnPos;
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private AudioClip _ShotClip;

    private float elapsedTime = 0f;
    private float _reloadTime = 0.3f;
    Vector2 direction;

    private void Shoot()
    {
        elapsedTime += Time.deltaTime;
        direction = (transform.localRotation * Vector2.up).normalized;

        if (Input.GetButton("Jump") && elapsedTime > _reloadTime || Input.GetButton("Fire1") && elapsedTime > _reloadTime)
        {
            GameObject instanshietedBullet = Instantiate(_bulletPrefab, _spawnPos.position, transform.rotation);
            PlayerBullet bullet = instanshietedBullet.GetComponent<PlayerBullet>();
            bullet.direction = direction;

            _audioSource.PlayOneShot(_ShotClip);
            elapsedTime = 0f; //reset bullet firing timer
        }
    }


    [SerializeField]
    private GameObject _shildPrefab;
    [SerializeField]
    private AudioClip _catchUpShield;
    [SerializeField]
    private GameObject _gunUpPrefab;
    [SerializeField]
    private AudioClip _catchUpGun;
    private bool _isShilded = false;
    private bool _IsDead = false;
    private bool _isShootFasterActive = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShootFasterPowerUP"))
        {
            if (_isShootFasterActive == false)
            {
                PickUpPowerUps(other);
                StartCoroutine(BuffDuration());
            }
        }
        else if (other.CompareTag("ShieldPowerUp"))
        {
            if (_isShilded == false)
            {
                PickUpPowerUps(other);
                StartCoroutine(ShildedTime());
            }
        }
        else if (other.CompareTag("Enemy") && _isShilded == false && _IsDead == false)
        {
            PlayPlayersDeath(other);
            StartCoroutine(DelayBeforeDestroy());
        }
        else if (other.CompareTag("EnemyBullet") && _IsDead == false && _isShilded == false)
        {
            PlayPlayersDeath(other);
            StartCoroutine(DelayBeforeDestroy());
        }
    }

    private void PickUpPowerUps(Collider2D other)
    {
        Destroy(other.gameObject);
    }


    [SerializeField] private AudioClip _DeathClip;

    private void PlayPlayersDeath(Collider2D other)
    {
        _IsDead = true;
        GameOver.Invoke();
        Destroy(other.gameObject);
        _ship_animator.SetBool("IsDead", true);
        _audioSource.PlayOneShot(_DeathClip);
    }

    IEnumerator BuffDuration()
    {
        _isShootFasterActive = true;
        _reloadTime /= 2f;
        _gunUpPrefab.SetActive(true);
        _audioSource.PlayOneShot(_catchUpGun);

        yield return new WaitForSeconds(6f);

        _gunUpPrefab.SetActive(false);
        _isShootFasterActive = false;
        _reloadTime = 0.5f;

    }

    IEnumerator ShildedTime()
    {
        _isShilded = true;
        _shildPrefab.SetActive(true);
        _audioSource.PlayOneShot(_catchUpShield);

        yield return new WaitForSeconds(4f);

        _shildPrefab.SetActive(false);
        _isShilded = false;
    }

    IEnumerator DelayBeforeDestroy()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
