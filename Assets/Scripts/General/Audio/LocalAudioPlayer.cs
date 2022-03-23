using UnityEngine;

namespace Audio
{
    public class LocalAudioPlayer : MonoBehaviour
    {
        /// <Description> Variables </Description>

        [SerializeField] private AudioSource source;

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private void Reset()
            => GetAudioSource();

        /// <Description> Custom Methods </Description>

        public void GetAudioSource()
            => source = GetComponent<AudioSource>();

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