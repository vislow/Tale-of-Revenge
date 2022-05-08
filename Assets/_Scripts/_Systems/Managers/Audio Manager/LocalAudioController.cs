using UnityEngine;

namespace Root.Audio
{
    public class LocalAudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource source;

        private void Reset() => GetAudioSource();

        public void GetAudioSource() => source = GetComponent<AudioSource>();

        public void Play(AudioObject audioObject)
        {
            var mixerGroup = AudioManager.Instance.GetMixerGroup(audioObject);

            source.outputAudioMixerGroup = mixerGroup;
            source.PlayOneShot(audioObject.clip, audioObject.volume);
        }

        public void PlayRandom(AudioObject[] audioObject)
        {
            var randomIndex = Random.Range(0, audioObject.Length - 1);
            var randomObject = audioObject[randomIndex];

            Play(randomObject);
        }
    }
}