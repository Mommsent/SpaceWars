using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    private Animator ship_animator;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _ShotClip;
    [SerializeField] private AudioClip _DeathClip;


    private Vector2 spawnPos;


    public GameManager gameManager;

    public GameObject bulletPrefab;
    public GameObject shildPrefab;
    public GameObject gunUpPrefab;

    public float speed = 10f;
    public float xLimit;
    private float reloadTime = 0.5f;

    private bool isShilded = false;

    public float ReloadTime
    {
        get { return reloadTime = 0.25f;}
        set { reloadTime = value;}
    }

    float elapsedTime = 0f;

    private void Start()
    {
        ship_animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        transform.Translate(xInput * speed * Time.deltaTime, 0f, 0f);

        Vector2 position = transform.position;
        position.x = Mathf.Clamp(position.x, -xLimit, xLimit);
        transform.position = position;
    }

    private void Shoot()
    {
        elapsedTime += Time.deltaTime;

        if (Input.GetButton("Jump") && elapsedTime > reloadTime)
        {
            spawnPos = transform.position;
            spawnPos += new Vector2(0, 1.2f);
            Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            _audioSource.PlayOneShot(_ShotClip);

            elapsedTime = 0f; //reset bullet firing timer
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShootFasterPowerUP"))
        {
            reloadTime /= 2;
            Destroy(other.gameObject);
            gunUpPrefab.SetActive(true);
            StartCoroutine(Cooldown());
        }
        else if(other.CompareTag("ShieldPowerUp"))
        {
            isShilded = true;
            Destroy(other.gameObject);
            shildPrefab.SetActive(true);
            StartCoroutine(ShildedTime());
        }
        else if(other.CompareTag("Enemy") && isShilded == false)
        {
            ship_animator.SetBool("IsDead", true);
            gameManager.PlayerDied();
            Destroy(other.gameObject);
            _audioSource.PlayOneShot(_DeathClip);
            StartCoroutine(DelayBeforeDestroy());
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(6f);
        gunUpPrefab.SetActive(false);
        reloadTime = 0.5f;
    }
    IEnumerator ShildedTime()
    {
        yield return new WaitForSeconds(4f);
        shildPrefab.SetActive(false);
        isShilded = false;
    }

    IEnumerator DelayBeforeDestroy()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
