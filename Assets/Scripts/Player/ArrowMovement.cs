using System.Collections;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = Vector2.right;
    private const float pie = 3.14159265358979323846f;
    public float timer = 0f;
    void Start()
    {
        direction *= transform.localScale.x;
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= 0.3f)
        {
            direction += Vector2.down * Time.deltaTime * 0.5f;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Arrow hit the ground");
            direction = new Vector2(Mathf.Sin(pie / 4), Mathf.Cos(pie / 4) * transform.localScale.x);
            speed *= 0.1f;
            StartCoroutine(DestroyArrow());
        }
    }
    private IEnumerator DestroyArrow()
    {
        Vector3 angles = transform.rotation.eulerAngles;
        angles.z += 10f;
        transform.rotation = Quaternion.Euler(angles);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
