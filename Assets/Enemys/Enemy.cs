using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour, IDead
{
    public static UnityEvent<int> EnemyIsDied = new UnityEvent<int>();

    public void MoveTheEnemy(Rigidbody2D _rigidBody2D, Vector2 _direction)
    {
        _rigidBody2D.velocity = _direction;
    }

    [SerializeField]
    private GameObject _bulletPrefab;
    private float _reloadTime = 2f;
    private float elapsedTime = 0f;
    private Vector2 _spawnPos;

    public void Shoot(AudioSource _audioSource, AudioClip _ShotClip)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > _reloadTime)
        {
            _spawnPos = transform.position;
            _spawnPos += new Vector2(0, - 1.2f);
            Instantiate(_bulletPrefab, _spawnPos, Quaternion.identity);
            _audioSource.PlayOneShot(_ShotClip);
            elapsedTime = 0f; //reset bullet firing timer
        }
    }
    public void PlayAnimAndEffectsOfDeath(Animator _anim, AudioSource _audioSource, AudioClip _DeathClip)
    {
        _anim.SetBool("IsDead", true);
        _audioSource.PlayOneShot(_DeathClip);
    }

    //Check if destroed by player`s bullet
    public void DeactivateRenderAndCollision(PolygonCollider2D _polygonCollider2D)
    {
        _polygonCollider2D.enabled = false;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            var child = this.transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(false);
        }
    }

    public void DestroyAnEnemyAndBullet(Collision2D collision)
    {
        StartCoroutine(DelayBeforeDestroy());
        Destroy(collision.gameObject);
    }
    IEnumerator DelayBeforeDestroy()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}
