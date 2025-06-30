using UnityEngine;

namespace Root.Levels
{
    public class LocationController : MonoBehaviour
    {
        public static LocationController instance;

        public LocationObject location;

        private void Awake()
        {
            if (instance == null) instance = this; else Destroy(this);
        }
    }
}