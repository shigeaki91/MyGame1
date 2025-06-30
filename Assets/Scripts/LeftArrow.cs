using UnityEngine;

public class LeftArrow : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = Vector2.left;
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
