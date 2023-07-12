using UnityEngine;

public class DestroyOnTriggerBullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}
