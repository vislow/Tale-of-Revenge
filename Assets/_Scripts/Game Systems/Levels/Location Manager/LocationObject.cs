using UnityEngine;

namespace Root.Levels
{
    [CreateAssetMenu(fileName = "LocationObject", menuName = "Scriptable Objects/Location Management/Location Object")]
    public class LocationObject : ScriptableObject
    {
        public string LocationName;
    }
}