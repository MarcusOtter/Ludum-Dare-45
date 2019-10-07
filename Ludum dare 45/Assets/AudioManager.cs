using UnityEngine;

public class AudioManager : MonoBehaviour
{
    internal static AudioManager Instance { get; private set; }

    private void Awake()
    {
        SingletonCheck();
    }

    internal void PlaySoundEffect(SoundEffect soundEffect)
    {
        var audioSource = new GameObject(soundEffect.Clips[0].name, typeof(AudioSource)).GetComponent<AudioSource>();

        audioSource.volume = soundEffect.RandomizeVolume
            ? Random.Range(soundEffect.MinVolume, soundEffect.MaxVolume)
            : soundEffect.MinVolume;

        audioSource.pitch = soundEffect.RandomizePitch
            ? Random.Range(soundEffect.MinPitch, soundEffect.MaxPitch)
            : 1;

        var clip = soundEffect.RandomizeClip
            ? soundEffect.Clips[Random.Range(0, soundEffect.Clips.Length)]
            : soundEffect.Clips[0];


        audioSource.clip = clip;

        audioSource.Play();

        // Destroy sound effect 1 second after it completes
        Destroy(audioSource.gameObject, clip.length + 1f);
    }

    private void SingletonCheck()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
