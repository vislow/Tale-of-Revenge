using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LevelManagement.Locations
{
    public class LocationManager : MonoBehaviour
    {
        /// <Description> Variables </Description>

        [SerializeField] private Animator popupAnim;
        [SerializeField] private TextMeshProUGUI popupText;

        private LocationObject currentLocation;
        private LocationObject lastLocation;

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private void Awake()
        {
            //LevelLoader.OnLevelLoaded += CheckLocation;
        }

        private void OnDestroy()
        {
            //LevelLoader.OnLevelLoaded += CheckLocation;
        }

        /// <Description> Custom Methods </Description>

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