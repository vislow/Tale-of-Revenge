using TMPro;
using UnityEngine;

namespace Root.LevelManagement.Locations
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private Animator popupAnim;
        [SerializeField] private TextMeshProUGUI popupText;

        private LocationObject currentLocation;
        private LocationObject lastLocation;

        private void Awake()
        {
            //LevelLoader.OnLevelLoaded += CheckLocation;
        }

        private void OnDestroy()
        {
            //LevelLoader.OnLevelLoaded += CheckLocation;
        }

        private void CheckLocation()
        {
            /*
            if (LocationController.instance == null)
                return;

            lastLocation = currentLocation;
            currentLocation = LocationController.instance.location;

            if (lastLocation != currentLocation)
            {
                popupText.SetText(currentLocation.LocationName);
                popupAnim.SetTrigger("Popup");
            }*/
        }
    }
}