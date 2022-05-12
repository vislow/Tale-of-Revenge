using UnityEngine;

namespace Root.Systems.Levels
{
    public class LocationController : MonoBehaviour
    {
        public static LocationController instance;

        public LocationObject location;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
    }
}