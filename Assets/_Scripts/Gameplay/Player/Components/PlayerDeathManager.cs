using System;
using UnityEngine;
using Root.LevelManagement;

namespace Root.Player.Components
{
    public class PlayerDeathManager : PlayerComponent
    {
        [SerializeField] private bool useSpawnPoint;

        internal bool dead;

        private DeathStages currentDeathStage;
        internal DeathStages CurrentDeathStage
        {
            get => currentDeathStage;
            set
            {
                if (value == currentDeathStage) return;

                currentDeathStage = value;

                OnDeathStageChanged?.Invoke(value);
            }
        }

        public static Action<DeathStages> OnDeathStageChanged;

        private void Start()
        {
            OnDeathStageChanged += DeathEvents;
            //TransitionManager.OnTransitionMiddleStarted += Reset;
            //TransitionManager.OnTransitionMiddleFinished += StartRespawn;
        }

        private void OnDestroy()
        {
            OnDeathStageChanged -= DeathEvents;
            //TransitionManager.OnTransitionMiddleStarted -= Reset;
            //TransitionManager.OnTransitionMiddleFinished -= StartRespawn;
        }

        private void DeathEvents(DeathStages deathStage)
        {
            switch (deathStage)
            {
                case DeathStages.Dying:
                    dead = true;
                    break;
            }
        }

        private void StartTransitionToReset()
        {
            //TransitionManager.instance.TriggerTransition(1f);
        }

        private void Reset()
        {
            if (!dead) return;

            CurrentDeathStage = DeathStages.Resetting;

            SpawnPointController spawnPoint = SpawnPointController.instance;

            if (spawnPoint != null && !useSpawnPoint)
            {
                transform.position = spawnPoint.transform.position;
            }
        }

        private void StartRespawn()
        {
            if (!dead) return;

            CurrentDeathStage = DeathStages.Respawning;
        }

        private void FinishRespawn()
        {
            CurrentDeathStage = DeathStages.Done;

            dead = false;
        }
    }

    public enum DeathStages
    {
        None,
        Dying,
        Resetting,
        Respawning,
        Done,
    }
}
