using UnityEngine;

namespace Root.Audio
{
    [CreateAssetMenu(fileName = "AudioObject", menuName = "Scriptable Objects/AudioObject", order = 0)]
    public class AudioObject : ScriptableObject
    {
        public AudioClip clip;
        public AudioMixers targetMixer;
        [Range(0, 1)] public float volume = 1;
    }
}
