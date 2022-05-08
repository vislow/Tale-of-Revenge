using System;
using System.Collections.Generic;
using UnityEngine;

namespace Root
{
    public class CheckpointManager : MonoBehaviour
    {
        [SerializeField] internal SpeedrunManager speedrunManager;
        [SerializeField] internal Timer timer;

        [SerializeField] private List<CheckpointController> checkpoints = new List<CheckpointController>();
        public List<float> checkpointTimes = new List<float>();

        public static Action<int> OnCheckpointSaved;
        public static Action OnFinalCheckpointTriggered;

        private int currentCheckpoint;

        private void Awake()
        {
            CheckpointController.OnCheckpointTrigger += UpdateCurrentCheckpoint;

            for (int i = 0; i < checkpoints.Count - 1; i++)
            {
                checkpoints[i].id = i;
            }

            for (int i = 0; i < checkpoints.Count - 1; i++)
            {
                checkpointTimes.Add(PlayerPrefs.GetFloat("Checkpoint" + i + "Time"));
            }
        }

        private void OnDestroy()
        {
            CheckpointController.OnCheckpointTrigger -= UpdateCurrentCheckpoint;
        }

        private void UpdateCurrentCheckpoint(int checkpointIndex)
        {
            if (checkpointIndex == checkpoints.Count - 2)
            {
                OnFinalCheckpointTriggered?.Invoke();
            }

            if (checkpointTimes[checkpointIndex] == 0 || timer.CurrentTime < checkpointTimes[checkpointIndex])
            {
                checkpointTimes[checkpointIndex] = timer.CurrentTime;

                PlayerPrefs.SetFloat("Checkpoint" + checkpointIndex + "Time", checkpointTimes[checkpointIndex]);
                PlayerPrefs.Save();

                OnCheckpointSaved?.Invoke(checkpointIndex);
            }

            currentCheckpoint = checkpointIndex;
        }

        public void GoToCheckpoint(int checkpointIndex)
        {
            CheckpointController checkpoint = checkpoints[checkpointIndex];
            speedrunManager.player.transform.position = checkpoint.spawnPoint.position;
        }

        public void OnRespawn()
        {
            GoToCheckpoint(currentCheckpoint);
        }

        public void OnReset()
        {
            currentCheckpoint = 0;

            GoToCheckpoint(0);
        }
    }
}