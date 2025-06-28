using UnityEngine;

namespace Root.Audio
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private MusicObject song;

        private void Start() => MusicManager.instance.StartMusic(song);
    }
}