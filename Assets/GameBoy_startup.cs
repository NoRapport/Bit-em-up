using UnityEngine;

public class GameBoy_startup : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    // Play before everything !!
    void Awake()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
