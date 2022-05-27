using System;
using UnityEngine;

namespace Root.Systems.States
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager instance;

        public static GameState CurrentGameState { get; private set; }
        public GameState currentGameState;
        public static bool inGame => CurrentGameState == GameState.Gameplay;

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

        private void Update() => currentGameState = CurrentGameState;

        public static void SetState(GameState newGameState)
        {
            if (newGameState == CurrentGameState) return;

            CurrentGameState = newGameState;
            OnGameStateChanged?.Invoke(newGameState);
        }

        private void SetInitialGameState()
        {
            int sceneIndex = Utility.Utils.GetActiveSceneIndex();

            GameState newState = default;

            switch (sceneIndex)
            {
                case 0: newState = GameState.Title; break;
                case 1: newState = GameState.Loading; break;
                default: newState = GameState.Gameplay; break;
            }

            SetState(newState);
        }
    }
}