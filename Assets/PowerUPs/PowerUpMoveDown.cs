using UnityEngine;

public class PowerUpMoveDown : MonoBehaviour
{
    Rigidbody2D rb;
    private float speed = -2f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, speed);
    }

}
