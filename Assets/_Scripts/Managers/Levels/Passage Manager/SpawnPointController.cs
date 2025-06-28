using UnityEngine;

namespace Root.Levels
{
    public class SpawnPointController : MonoBehaviour
    {
        public static SpawnPointController instance;

        private void Awake()
        {
            if (instance == null) instance = this; else Destroy(this);
        }
    }
}