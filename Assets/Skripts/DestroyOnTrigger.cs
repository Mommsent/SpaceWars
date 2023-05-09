using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DestroyOnTrigger : MonoBehaviour
{
    public static UnityEvent EnemyPassed = new UnityEvent();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
            EnemyPassed.Invoke();

        Destroy(other.gameObject);
    }
}
