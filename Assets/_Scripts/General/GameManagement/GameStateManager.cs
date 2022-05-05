using System;
using UnityEngine;

namespace Root.GameManagement
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager instance;

        public static GameState CurrentGameState { get; private set; }

        public GameState currentGameState;

        public static event Action<GameState> OnGameStateChanged;

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

            SetInitialGameState();
        }

        private void Update()
        {
            currentGameState = CurrentGameState;
        }

        public static void SetState(GameState newGameState)
        {
            if (newGameState == CurrentGameState)
                return;

            CurrentGameState = newGameState;

            OnGameStateChanged?.Invoke(newGameState);
        }

        private void SetInitialGameState()
        {
            int sceneIndex = Utility.Utils.GetActiveSceneIndex();

            SetState(GameState.Gameplay);
        }
    }
}