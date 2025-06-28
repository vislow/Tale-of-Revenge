using System;
using System.Collections;
using Root.Input;
using UnityEngine;

namespace Root
{
    public enum GameState
    {
        Preload,
        Title,
        Loading,
        Gameplay,
        Paused
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public static GameState gameState { get; private set; }

        // public static bool inGameplay => gameState == GameState.Gameplay;
        // public static bool inGame => gameState == GameState.Gameplay || gameState == GameState.Paused;
        // public static bool paused => gameState == GameState.Paused;

        public static event Action<GameState> OnGameStateChanged;

        private const string gameSpeed = "GameSpeed";

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

        // Game freezing is utilized in game for
        // a little oomf when the player is hit
        public void FreezeGame(float duration = 0.05f) => StartCoroutine(Freeze(duration));

        private IEnumerator Freeze(float duration)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = PlayerPrefs.GetFloat(gameSpeed);
        }

        public static void SetState(GameState newGameState)
        {
            if (newGameState == gameState) return;

            gameState = newGameState;
            OnGameStateChanged?.Invoke(newGameState);

            // Set games speed based on game state. If Paused, the game is frozen, otherwise
            Time.timeScale = gameState == GameState.Gameplay ? 1f : PlayerPrefs.GetFloat(gameSpeed);
        }

        private void SetInitialGameState()
        {
            int sceneIndex = Utility.Utils.GetActiveSceneIndex();

            GameState newState;

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
