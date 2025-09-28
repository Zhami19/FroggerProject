using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource _audi;

    public AudioClip[] soundFX;
    public AudioClip[] bgMusic;
    void Start()
    {
        _audi = GetComponent<AudioSource>();
    }
}
