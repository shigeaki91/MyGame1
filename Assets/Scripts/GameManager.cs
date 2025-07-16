using UnityEngine;

public class GameManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip BowSoundClip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            audioSource.PlayOneShot(BowSoundClip);
        }
    }
}
