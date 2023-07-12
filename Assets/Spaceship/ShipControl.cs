using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class ShipControl : MonoBehaviour, ICanShoot
{
    public static UnityEvent GameOver = new UnityEvent();

    public PlayerInput _input;

    private Animator _ship_animator;
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Start()
    {
        _ship_animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
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
        Vector2 moveDirection = _input.Player.Move.ReadValue<Vector2>();

        Move(moveDirection);
        Rotate(moveDirection);
    }

    private float tiltAngle = 10.0f;
    private float smooth = 3.0f;
    private void Rotate(Vector2 rotation)
    {
        float tiltAroundZ = -rotation.x * tiltAngle;
        Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }

    private float _speed = 12f;
    private float _transformXLimit = 13f;
    private float _uppYLimit = 7f;
    private float _downYLimmit = -4.3f;
    private void Move(Vector2 direction)
    {
        transform.Translate(direction.x * _speed * Time.deltaTime, 0f, 0f);
        transform.Translate(0f, direction.y * _speed * Time.deltaTime, 0f);
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
    private Vector2 direction;

    public void Shoot()
    {
        elapsedTime += Time.deltaTime;
        direction = (transform.localRotation * Vector2.up).normalized;
        bool isShootPressed = _input.Player.Shoot.ReadValue<float>() > 0.1f;

        if (isShootPressed  && elapsedTime > _reloadTime)
        {
            GameObject instanshietedBullet = Instantiate(_bulletPrefab, _spawnPos.position, transform.rotation);
            PlayerBullet bullet = instanshietedBullet.GetComponent<PlayerBullet>();
            bullet.direction = direction;

            _audioSource.PlayOneShot(_ShotClip);
            elapsedTime = 0f; //reset bullet firing timer
        }
    }


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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShootFasterPowerUP"))
        {
            PickUpPowerUps(other);
            StartCoroutine(GunUpBuffDuration());
        }
        else if (other.CompareTag("ShieldPowerUp"))
        {
            PickUpPowerUps(other);
            StartCoroutine(ShildedTime());
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

    private float _gunUpDuration;
    private float _shildDuration;
    private float _timeForPlayAnim;

    [SerializeField] private Animator _gunPoverUpAnimator;
    [SerializeField] private Animator _shieldPoverUpAnimator;
    [SerializeField] private AudioClip _endOfPowerUp;

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
        _gunUpDuration = 10f;
        _timeForPlayAnim = 3f;
        _gunUpPrefab.SetActive(true);
        _audioSource.PlayOneShot(_catchUpGun);

        yield return new WaitForSeconds(_gunUpDuration - _timeForPlayAnim);

        PlayCloseToEndOfPowerUpAnim(_gunPoverUpAnimator, "GunCloseToEnd");

        yield return new WaitForSeconds(_timeForPlayAnim);

        EndOfPowerUpAnim(_gunPoverUpAnimator, "ToIdle", _gunUpPrefab, ref _isShootFasterActive);
        _reloadTime = 0.5f;
    }

    IEnumerator ShildedTime()
    {
        _isShilded = true;
        _shieldPrefab.SetActive(true);
        _audioSource.PlayOneShot(_catchUpShield);
        _shildDuration = 7f;
        _timeForPlayAnim = 3f;

        yield return new WaitForSeconds(_shildDuration - _timeForPlayAnim);

        PlayCloseToEndOfPowerUpAnim(_shieldPoverUpAnimator, "ShieldCloseToEnd");

        yield return new WaitForSeconds(_timeForPlayAnim);

        EndOfPowerUpAnim(_shieldPoverUpAnimator, "ToIdle", _shieldPrefab, ref _isShilded);
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
    }

    IEnumerator DelayBeforeDestroy()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
