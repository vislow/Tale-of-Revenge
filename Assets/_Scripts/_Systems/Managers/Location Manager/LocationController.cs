using UnityEngine;

namespace Root.LevelManagement.Locations
{
    public class LocationController : MonoBehaviour
    {
        public static LocationController instance;

        public LocationObject location;

        private void Awake()
        {
            instance = this;
        }
    }
}