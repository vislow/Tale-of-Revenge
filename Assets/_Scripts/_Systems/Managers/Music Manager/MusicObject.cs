using UnityEngine;

namespace Root.Audio
{
    [CreateAssetMenu(fileName = "NewMusicObject", menuName = "Scriptable Objects/Audio/Music Object")]
    public class MusicObject : ScriptableObject
    {
        public AudioClip clip;
        [Range(0, 1)] public float volume = 1;
    }
}
