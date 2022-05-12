using UnityEngine;
using UnityEngine.Audio;

namespace Root.Systems.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField] private AudioSource audioSource;
        [Space]
        [SerializeField] private AudioMixerGroup masterMixerGroup;
        [SerializeField] private AudioMixerGroup musicMixerGroup;
        [SerializeField] private AudioMixerGroup sfxMixerGroup;
        [SerializeField] private AudioMixerGroup uiMixerGroup;

        private AudioMixer masterMixer;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            masterMixer = (AudioMixer)Resources.Load("MasterMixer");
        }

        public void PlayGlobalOneShot(AudioObject audioObject)
        {
            audioSource.outputAudioMixerGroup = GetMixerGroup(audioObject);
            audioSource.PlayOneShot(audioObject.clip, audioObject.volume);
        }

        public AudioMixerGroup GetMixerGroup(AudioObject audioObject)
        {
            var mixerGroup = masterMixerGroup;

            switch (audioObject.targetMixer)
            {
                case AudioMixers.Music:
                    mixerGroup = musicMixerGroup;
                    break;
                case AudioMixers.Sfx:
                    mixerGroup = sfxMixerGroup;
                    break;
                case AudioMixers.UI:
                    mixerGroup = uiMixerGroup;
                    break;
            }

            return mixerGroup;
        }
    }
}
