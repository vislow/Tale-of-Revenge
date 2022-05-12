using UnityEngine;
using UnityEngine.Audio;

namespace Root.Systems.Audio
{
    public class LocalAudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource source;

        private void Reset() => GetAudioSource();

        public void GetAudioSource() => source = GetComponent<AudioSource>();

        public void Play(AudioObject audioObject)
        {
            AudioMixerGroup mixerGroup = AudioManager.Instance.GetMixerGroup(audioObject);

            source.outputAudioMixerGroup = mixerGroup;
            source.PlayOneShot(audioObject.clip, audioObject.volume);
        }

        public void PlayRandom(AudioObject[] audioObject)
        {
            int randomIndex = Random.Range(0, audioObject.Length - 1);

            AudioObject randomObject = audioObject[randomIndex];

            Play(randomObject);
        }
    }
}