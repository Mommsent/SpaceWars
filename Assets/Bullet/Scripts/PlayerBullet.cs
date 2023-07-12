using UnityEngine;

public class PlayerBullet : MonoBehaviour, IBullet
{
    public Vector2 direction = new Vector2(0, 1);
    private float _speed = 17f;
    private Vector2 velocity;
    private void Start()
    {
        velocity = direction * _speed;
    }
    private void FixedUpdate()
    {
        Shot();
    }

    public void Shot()
    {
        Vector2 pos = transform.position;
        pos += velocity * Time.fixedDeltaTime;
        transform.position = pos;
    }
}
