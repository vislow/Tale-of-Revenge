using UnityEngine;

namespace Root.Systems.Levels
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