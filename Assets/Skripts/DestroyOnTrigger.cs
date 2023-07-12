using UnityEngine;
using UnityEngine.Events;
public class DestroyOnTrigger : MonoBehaviour
{
    public static UnityEvent EnemyPassed = new UnityEvent();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyPassed.Invoke();
            Destroy(other.gameObject);
        }
        
        if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }
}
