using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipControl : MonoBehaviour
{
    private Animator _ship_animator;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _ShotClip;
    [SerializeField] private AudioClip _DeathClip;

    public static UnityEvent GameOver = new UnityEvent();

    private Vector2 _spawnPos;

    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private GameObject _shildPrefab;
    [SerializeField]
    private GameObject _gunUpPrefab;

    [SerializeField]
    public float _speed = 10f;
    [SerializeField]
    public float _xLimit;
    private float _reloadTime = 0.5f;

    private bool _isShilded = false;
    private bool _IsDead = false;

    private float ReloadTime
    {
        get { return _reloadTime = 0.25f;}
        set { _reloadTime = value;}
    }

    private float elapsedTime = 0f;

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

    private void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        transform.Translate(xInput * _speed * Time.deltaTime, 0f, 0f);

        Vector2 position = transform.position;
        position.x = Mathf.Clamp(position.x, -_xLimit, _xLimit);
        transform.position = position;
    }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShootFasterPowerUP"))
        {
            _reloadTime /= 2;
            Destroy(other.gameObject);
            _gunUpPrefab.SetActive(true);
            StartCoroutine(Cooldown());
        }
        else if(other.CompareTag("ShieldPowerUp"))
        {
            _isShilded = true;
            Destroy(other.gameObject);
            _shildPrefab.SetActive(true);
            StartCoroutine(ShildedTime());
        }
        else if(other.CompareTag("Enemy") && _isShilded == false)
        {
            _ship_animator.SetBool("IsDead", true);
            _IsDead = true;

            GameOver.Invoke();

            Destroy(other.gameObject);
            _audioSource.PlayOneShot(_DeathClip);
            StartCoroutine(DelayBeforeDestroy());
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(6f);
        _gunUpPrefab.SetActive(false);
        _reloadTime = 0.5f;
    }
    IEnumerator ShildedTime()
    {
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
