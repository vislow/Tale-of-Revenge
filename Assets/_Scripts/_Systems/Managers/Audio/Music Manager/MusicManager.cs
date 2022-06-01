using System.Collections;
using UnityEngine;

namespace Root.Systems.Audio
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager instance;

        [SerializeField] private AudioSource musicSource;
        [SerializeField][Range(0, 1)] private float fadeSpeed = 0.5f;

        private void Awake()
        {
            if (instance == null) instance = this; else Destroy(this);
        }

        public void StartMusic(MusicObject nextSong) => StartCoroutine(FadeIn(nextSong));

        public void StopMusic() => StartCoroutine(FadeOut());

        private IEnumerator FadeOut()
        {
            float initialVolume = musicSource.volume;

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

            while (musicSource.volume < musicObject.volume - 0.0001)
            {
                musicSource.volume = Mathf.Lerp(musicSource.volume, musicObject.volume, fadeSpeed);

                yield return null;
            }
        }
    }
}