using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class ArrowMovement : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = new Vector2(0f, 0f);
    private const float pie = 3.14159265358979323846f;
    public float timer = 0f;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isColliding = false;
    public float zAngle;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        zAngle = transform.rotation.eulerAngles.z % 360f;
        if (zAngle == 0f)
        {
            zAngle = 360f;
        }
        if (!isColliding)
            {
                direction = new Vector2(Mathf.Cos(zAngle * Mathf.Deg2Rad),
                                        Mathf.Sin(zAngle * Mathf.Deg2Rad));
                timer += Time.deltaTime;
                if (timer >= 0.4f)
                {
                    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + (-zAngle + 270f) * Time.deltaTime);
                }
            }
        transform.localPosition += (Vector3)direction * speed * Time.deltaTime;
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
            if(direction.x < 0f)
            {
                direction = new Vector2(Mathf.Cos(pie / 4f * 3f), Mathf.Sin(pie / 4f * 3f));
            }
            else
            {
                direction = new Vector2(Mathf.Cos(pie / 4f), Mathf.Sin(pie / 4f));
            }
            speed *= 0.5f;
            StartCoroutine(DestroyArrow());
        }
    }
    private IEnumerator DestroyArrow()
    {
        isColliding = true;
        float timer = 0f;
        bc.enabled = false;
        rb.gravityScale = 1f;
        Color color = sr.color;
        while (timer < 0.5f)
        {
            Vector3 angles = transform.rotation.eulerAngles;
            if (direction.x < 0f)
            {
                angles.z += 720f * Time.deltaTime;
            }
            else
            {
                angles.z -= 720f * Time.deltaTime;
            }
            transform.rotation = Quaternion.Euler(angles);
            color.a = Mathf.Lerp(1f, 0f, timer / 0.5f);
            sr.color = color;
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
