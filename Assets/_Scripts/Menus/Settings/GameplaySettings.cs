using UnityEngine;

namespace Root.Systems.Settings
{
    public class GameplaySettings : MonoBehaviour
    {
        /*[SerializeField] private HorizontalSelector gameSpeedSelector;

        private float currentGameSpeed;

        private void Awake() {
            gameSpeedSelector.OnIndexChange += SetGameSpeed;
        }

        private void Start() {
            LoadSettings();
        }

        private void SetGameSpeed() => currentGameSpeed = ((float)gameSpeedSelector.CurrentIndex + 1f) / 10f;

        public void SetToDefaults() {
            currentGameSpeed = 1;
            gameSpeedSelector.CurrentIndex = gameSpeedSelector.selectionList.Count - 1;

            SaveSettings();
        }

        private void OnDisable() => SaveSettings();

        // ----- Saving and Loading -----

        private const string gameSpeed = "GameSpeed";

        private void SaveSettings() {
            PlayerPrefs.SetFloat(gameSpeed, currentGameSpeed);

            PlayerPrefs.Save();
        }

        private void LoadSettings() {
            currentGameSpeed = PlayerPrefs.GetFloat(gameSpeed, 1);
            gameSpeedSelector.CurrentIndex = (int)((currentGameSpeed * 10) - 1);
        }*/
    }
}