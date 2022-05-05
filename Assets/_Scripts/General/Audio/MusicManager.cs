using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Root.Audio
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager instance;

        [SerializeField] private AudioSource musicSource;
        [SerializeField][Range(0, 1)] private float fadeSpeed = 0.5f;
        [SerializeField] private List<SongData> songDataList = new List<SongData>();

        private Hashtable songTable = new Hashtable();

        [System.Serializable]
        public class SongData
        {
            public AudioObject audioObject;
            public Songs song;
        }

        private void Awake()
        {
            instance = this;

            foreach (var data in songDataList)
            {
                songTable.Add(data.song, data.audioObject);
            }
        }

        public void StartMusic(Songs nextSong)
        {
            if (instance == null) return;
            StartCoroutine(FadeIn(GetSongData(nextSong)));
        }

        public void StopMusic()
        {
            StartCoroutine(FadeOut());
        }

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

        private IEnumerator FadeIn(AudioObject audioObject)
        {
            musicSource.clip = audioObject.clip;

            musicSource.Play();

            var targetVolume = audioObject.volume;

            while (musicSource.volume < targetVolume - 0.0001)
            {
                musicSource.volume = Mathf.Lerp(musicSource.volume, targetVolume, fadeSpeed);

                yield return null;
            }
        }

        private AudioObject GetSongData(Songs song)
        {
            return (AudioObject)(songTable[song]);
        }
    }

    public enum Songs
    {
        TitleScreen,
        Hollows,
        PlatformingDemo,
    }
}