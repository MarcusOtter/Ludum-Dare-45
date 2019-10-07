using UnityEngine;

public class PlayAudioFromAnimation : MonoBehaviour
{
    [SerializeField] private SoundEffect _soundEffectToPlay;

    public void PlaySoundEffect()
    {
        AudioManager.Instance.PlaySoundEffect(_soundEffectToPlay);
    }
}
