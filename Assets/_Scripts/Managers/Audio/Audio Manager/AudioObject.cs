using UnityEngine;

namespace Root.Audio
{
    [CreateAssetMenu(fileName = "NewAudioObject", menuName = "Scriptable Objects/Audio/Audio Object")]
    public class AudioObject : ScriptableObject
    {
        public AudioClip clip;
        public AudioMixers targetMixer;
        [Range(0, 1)] public float volume = 1;
    }
}
