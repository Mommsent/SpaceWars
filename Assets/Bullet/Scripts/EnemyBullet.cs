using UnityEngine;

public class EnemyBullet : MonoBehaviour, IBullet
{
    private float _speed = -15f;
    void Start()
    {
        Shot();
    }

    public void Shot()
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(0f, _speed); //add force to bullet and push it on Y axis
    }
}
