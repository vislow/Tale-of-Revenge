using UnityEngine;
using Root.Player.Components;
using TMPro;

namespace Root
{
    public class DeathCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI deathCounterText, highestDeathText;

        private int currentDeaths;
        private int CurrentDeaths
        {
            get => currentDeaths;
            set
            {
                if (value == currentDeaths) return;
                currentDeaths = value;
                deathCounterText.text = "Current Deaths: " + value.ToString();
            }
        }

        private int highestDeaths;
        private int HighestDeaths
        {
            get => highestDeaths;
            set
            {
                if (value == highestDeaths) return;
                highestDeaths = value;
                highestDeathText.text = "Highest Deaths: " + value.ToString();
            }
        }

        private bool hasSaved;

        private void Awake()
        {
            HighestDeaths = PlayerPrefs.GetInt("HighestDeaths");
            PlayerDeathManager.OnDeathStageChanged += UpdateDeathCounter;
        }

        private void OnDestroy() => PlayerDeathManager.OnDeathStageChanged -= UpdateDeathCounter;

        private void UpdateDeathCounter(DeathStages deathStages)
        {
            switch (deathStages)
            {
                case DeathStages.Dying:
                    if (hasSaved)
                        break;

                    hasSaved = true;

                    CurrentDeaths++;

                    if (currentDeaths > HighestDeaths)
                        SaveHighestDeaths();

                    break;
                case DeathStages.Respawning:
                    hasSaved = false;
                    break;
            }
        }

        private void ResetDeathCounter() => CurrentDeaths = 0;

        private void SaveHighestDeaths()
        {
            HighestDeaths = CurrentDeaths;

            PlayerPrefs.SetInt("HighestDeaths", HighestDeaths);
            PlayerPrefs.Save();
        }

        public void OnReset()
        {
            hasSaved = false;
            CurrentDeaths = 0;
        }
    }
}