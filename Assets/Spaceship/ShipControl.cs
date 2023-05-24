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
<<<<<<< Updated upstream
        Cursor.visible = false;
=======
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        transform.Translate(xInput * _speed * Time.deltaTime, 0f, 0f);

        Vector2 position = transform.position;
        position.x = Mathf.Clamp(position.x, -_xLimit, _xLimit);
        transform.position = position;
    }

=======
        float yInput = Input.GetAxis("Vertical");
        Move(xInput, yInput);
        Rotate(xInput);
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
        CheckIfBorderOfTheMap();
    }
    private void CheckIfBorderOfTheMap()
    {
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

>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
=======

    [SerializeField]
    private GameObject _shieldPrefab;
    [SerializeField]
    private AudioClip _catchUpShield;
    [SerializeField]
    private GameObject _gunUpPrefab;
    [SerializeField]
    private AudioClip _catchUpGun;
    private bool _isShilded = false;
    private bool _IsDead = false;
    private bool _isShootFasterActive = false;

>>>>>>> Stashed changes
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShootFasterPowerUP"))
        {
<<<<<<< Updated upstream
            _reloadTime /= 2;
            Destroy(other.gameObject);
            _gunUpPrefab.SetActive(true);
            StartCoroutine(Cooldown());
=======
            if (_isShootFasterActive == false)
            {
                PickUpPowerUps(other);
                StartCoroutine(GunUpBuffDuration());
            }
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        yield return new WaitForSeconds(6f);
        _gunUpPrefab.SetActive(false);
        _reloadTime = 0.5f;
=======
        Destroy(other.gameObject);
    }


    [SerializeField] private AudioClip _DeathClip;

    private float _gunUpDuration = 10f;
    private float _shildDuration = 7f;
    private float _timeForPlayAnim = 3f;
    [SerializeField]
    private Animator _gunPoverUpAnimator;
    [SerializeField]
    private Animator _shieldPoverUpAnimator;
    [SerializeField]
    private AudioClip _endOfPowerUp;

    private void PlayPlayersDeath(Collider2D other)
    {
        _IsDead = true;
        GameOver.Invoke();
        Destroy(other.gameObject);
        _ship_animator.SetBool("IsDead", true);
        _audioSource.PlayOneShot(_DeathClip);
    }

    IEnumerator GunUpBuffDuration()
    {
        _isShootFasterActive = true;
        _reloadTime /= 2f;
        _gunUpPrefab.SetActive(true);
        _audioSource.PlayOneShot(_catchUpGun);

        yield return new WaitForSeconds(_gunUpDuration - _timeForPlayAnim);

        PlayCloseToEndOfPowerUpAnim(_gunPoverUpAnimator, "GunCloseToEnd");

        yield return new WaitForSeconds(_timeForPlayAnim);

        EndOfPowerUpAnim(_gunPoverUpAnimator, "ToIdle", _gunUpPrefab, ref _isShootFasterActive);
        _reloadTime = 0.5f;
        Debug.Log(_isShootFasterActive == true);
>>>>>>> Stashed changes
    }
    IEnumerator ShildedTime()
    {
<<<<<<< Updated upstream
        yield return new WaitForSeconds(4f);
        _shildPrefab.SetActive(false);
        _isShilded = false;
=======
        _isShilded = true;
        _shieldPrefab.SetActive(true);
        _audioSource.PlayOneShot(_catchUpShield);
        
        yield return new WaitForSeconds(_shildDuration - _timeForPlayAnim);

        PlayCloseToEndOfPowerUpAnim(_shieldPoverUpAnimator, "ShieldCloseToEnd");

        yield return new WaitForSeconds(_timeForPlayAnim);

        EndOfPowerUpAnim(_shieldPoverUpAnimator, "ToIdle", _shieldPrefab, ref _isShilded);
        Debug.Log(_isShilded == true);
    }

    private void PlayCloseToEndOfPowerUpAnim(Animator _PoverUpAnimator, string AnimName)
    {
        _PoverUpAnimator.SetBool(AnimName, true);
        _audioSource.PlayOneShot(_endOfPowerUp);
    }

    private void EndOfPowerUpAnim(Animator _PoverUpAnimator, string AnimName, GameObject prefub, ref bool IsActive)
    {
        _PoverUpAnimator.SetBool(AnimName, true);
        prefub.SetActive(false);
        IsActive = false;
>>>>>>> Stashed changes
    }

    IEnumerator DelayBeforeDestroy()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
