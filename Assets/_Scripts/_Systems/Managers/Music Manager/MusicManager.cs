using System.Collections;
using UnityEngine;

namespace Root.Audio
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager instance;

        [SerializeField] private AudioSource musicSource;
        [SerializeField][Range(0, 1)] private float fadeSpeed = 0.5f;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void StartMusic(MusicObject nextSong)
        {
            if (instance == null) return;

            StartCoroutine(FadeIn(nextSong));
        }

        public void StopMusic() => StartCoroutine(FadeOut());

        private IEnumerator FadeOut()
        {
            var initialVolume = musicSource.volume;

            while (musicSource.volume > 0.0001)
            {
                musicSource.volume = Mathf.Lerp(musicSource.volume, 0f, fadeSpeed);

                yield return null;
            }

            musicSource.Stop();
        }

        private IEnumerator FadeIn(MusicObject musicObject)
        {
            musicSource.clip = musicObject.clip;

            musicSource.Play();

            var targetVolume = musicObject.volume;

            while (musicSource.volume < targetVolume - 0.0001)
            {
                musicSource.volume = Mathf.Lerp(musicSource.volume, targetVolume, fadeSpeed);

                yield return null;
            }
        }
    }
}