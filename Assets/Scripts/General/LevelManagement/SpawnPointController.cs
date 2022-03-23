using UnityEngine;

namespace LevelManagement {
    public class SpawnPointController : MonoBehaviour {
        public static SpawnPointController instance;

        private void Awake() {
            instance = this;
        }
    }
}