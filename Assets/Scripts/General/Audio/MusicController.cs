using UnityEngine;

namespace Audio
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private Songs song;

        private void Start()
        {
            MusicManager.instance.StartMusic(song);
        }
    }
}