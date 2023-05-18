using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private float _speed = -15f;
    void Start()
    {
        Shot( _speed);
    }
}
