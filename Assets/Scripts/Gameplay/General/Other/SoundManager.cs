using PT.Tools.EventListener;
using PT.Tools.Helper;
using UnityEngine;

namespace Gameplay.General.Other
{
    public class SoundManager : MonoBehaviourEventListener
    {
        public enum SoundEventEnum
        {
            none,
            FinishReached,
        }
        
        [Header("Sounds")]
        [SerializeField] private SerializableKeyValue<SoundEventEnum, AudioClip> kvSounds;
        
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
        }

        public void PlaySound(SoundEventEnum soundEvent)
        {
            if (PlayerPrefs.GetInt("SoundEnabled", 1) == 1)
            {
                audioSource.PlayOneShot(kvSounds.Dictionary[soundEvent]);
            }
        }
    }
}