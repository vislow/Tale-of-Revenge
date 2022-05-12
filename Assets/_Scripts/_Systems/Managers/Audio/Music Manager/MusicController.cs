using UnityEngine;

namespace Root.Systems.Audio
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private MusicObject song;

        private void Start() => MusicManager.instance.StartMusic(song);
    }
}