using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour, IDeadEnemy
{
    public static UnityEvent<int> EnemyIsDied = new UnityEvent<int>();

    public void MoveTheEnemy(Rigidbody2D _rigidBody2D, Vector2 _direction)
    {
        _rigidBody2D.velocity = _direction;
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
