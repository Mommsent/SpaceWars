using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public Vector2 direction = new Vector2(0, 1);
    private float _speed = 17f;
    private Vector2 velocity;
    private void Update()
    {
        velocity = direction * _speed;
    }
    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos += velocity * Time.fixedDeltaTime;
        transform.position = pos;
    }
}
