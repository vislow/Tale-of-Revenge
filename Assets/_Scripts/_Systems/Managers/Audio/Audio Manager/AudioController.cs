using System.Collections.Generic;
using UnityEngine;

namespace Root.Systems.Audio
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private List<AudioObject> objectList = new List<AudioObject>();

        public void PlaySound(int audioIndex)
        {
            if (audioIndex < 0 || audioIndex > objectList.Count - 1) return;

            AudioManager.Instance.PlayGlobalOneShot(objectList[audioIndex]);
        }

        public void PlaySound(AudioObject audioObject) => AudioManager.Instance.PlayGlobalOneShot(audioObject);
    }
}