using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public void Shot(float _speed)
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(0f, _speed); //add force to bullet and push it on Y axis
    }
        
}
