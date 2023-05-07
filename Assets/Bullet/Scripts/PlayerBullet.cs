using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private float _speed = 10f;

    void Start()
    {
        Shot(_speed);
    }
}
