using UnityEngine;

namespace Root.LevelManagement
{
    public class SpawnPointController : MonoBehaviour
    {
        public static SpawnPointController instance;

        private void Awake()
        {
            instance = this;
        }
    }
}