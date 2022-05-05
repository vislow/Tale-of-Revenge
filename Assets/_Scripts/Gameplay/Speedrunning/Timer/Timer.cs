using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Root
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private SpeedrunManager speedrunManager;
        [Space]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI fastestTimeText;
        [Space]
        [SerializeField] private CheckpointManager checkpointManager;
        [SerializeField] private Transform checkpointTimerParent;
        [SerializeField] private GameObject checkpointTimerPrefab;

        private List<TextMeshProUGUI> checkpointTextList = new List<TextMeshProUGUI>();

        private float currentTime;
        internal float CurrentTime
        {
            get => currentTime;
            set
            {
                if (value == currentTime) return;
                currentTime = value;
                timerText.text = ScoreToText(value);
            }
        }

        private float fastestTime;
        private float FastestTime
        {
            get => fastestTime;
            set
            {
                if (value == fastestTime) return;
                fastestTime = value;
                fastestTimeText.text = ScoreToText(value);
            }
        }

        private bool timerStarted;
        private bool runTimer = true;

        private void Awake()
        {
            CheckpointManager.OnCheckpointSaved += UpdateCheckpointTimer;
            CheckpointManager.OnFinalCheckpointTriggered += StopTimer;

            FastestTime = PlayerPrefs.GetFloat("FastestTime");
        }

        private void OnDestroy()
        {
            CheckpointManager.OnCheckpointSaved -= UpdateCheckpointTimer;
            CheckpointManager.OnFinalCheckpointTriggered -= StopTimer;
        }

        private void Start()
        {
            foreach (Transform child in checkpointTimerParent)
            {
                Destroy(child);
            }

            for (int i = 0; i < checkpointManager.checkpointTimes.Count; i++)
            {
                GameObject timerObject = Instantiate(checkpointTimerPrefab, Vector3.zero, Quaternion.identity, checkpointTimerParent);

                checkpointTextList.Add(timerObject.GetComponent<TextMeshProUGUI>());

                UpdateCheckpointTimer(i);
            }
        }

        private void Update()
        {
            if (!timerStarted && speedrunManager.player.components.controller.horizontalInput != 0)
            {
                timerStarted = true;
            }

            if (!timerStarted || !runTimer) return;

            CurrentTime += Time.deltaTime;
            timerText.text = ScoreToText(CurrentTime);
        }

        private void UpdateCheckpointTimer(int index)
        {
            float timeValue = PlayerPrefs.GetFloat("Checkpoint" + index + "Time");
            string timeText = ScoreToText(timeValue);
            checkpointTextList[index].text = $"C{index}: {timeText}";
        }

        public void UpdateFastestTime()
        {
            runTimer = false;

            if (FastestTime < CurrentTime && FastestTime != 0) return;

            FastestTime = CurrentTime;
        }

        private void SaveFastestTime()
        {
            PlayerPrefs.SetFloat("FastestTime", CurrentTime);
            PlayerPrefs.Save();
        }

        private string ScoreToText(float time)
        {
            var minutes = Mathf.FloorToInt(time / 60);
            var seconds = Mathf.FloorToInt(time % 60);
            var milliseconds = (time % 1) * 1000;

            return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }

        private void StopTimer()
        {
            runTimer = false;
            SaveFastestTime();
        }

        public void OnReset()
        {
            timerStarted = false;
            runTimer = true;

            CurrentTime = 0;
        }
    }
}