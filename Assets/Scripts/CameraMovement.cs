using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        transform.localPosition = new Vector3(player.transform.localPosition.x, 0f, -10f);
        if (transform.localPosition.x >= 0)
        {
            transform.localPosition = new Vector3(0f, 0f, -10f);
        }
    }
}
