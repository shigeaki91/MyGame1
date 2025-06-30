using UnityEngine;

public class RightArrow : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = Vector2.right;
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
