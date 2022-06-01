using Root.Systems.Input;
using Root.Systems.States;
using UnityEngine;

namespace Root.Systems
{
    public enum UITypes
    {
        None,
        Pause,
        Dialogue,
        Location,
        Transition,
        Popup,
    }

    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject inGameObjects;

        private UITypes currentActiveUI;
        private PlayerControls input => InputManager.instance.input;

        private void Awake() => GameStateManager.OnGameStateChanged += OnGameStateChanged;

        private void OnGameStateChanged(GameState state)
        {
            if (state != GameState.Gameplay && state != GameState.Paused)
            {
                PauseManager.instance.UnPause();
            }

            inGameObjects.SetActive(GameStateManager.inGame);
        }

        private void OnDestroy() => GameStateManager.OnGameStateChanged -= OnGameStateChanged;
    }
}
