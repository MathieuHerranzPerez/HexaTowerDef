using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    // Use this for initialization
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play(AudioClip audioClip, float volume)
    {
        audioSource.PlayOneShot(audioClip, volume);
    }
}