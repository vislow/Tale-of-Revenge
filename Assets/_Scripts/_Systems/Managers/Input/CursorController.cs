using Root.Systems.States;
using UnityEngine;

namespace Root.Systems.Input
{
    public class CursorController : MonoBehaviour
    {
        private void Awake() => GameStateManager.OnGameStateChanged += OnGameStateChanged;

        private void OnGameStateChanged(GameState state) => transform.gameObject.SetActive(state == GameState.Gameplay);

        private void Update() => MoveCursorToMouse();

        private void MoveCursorToMouse()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            if (!GameStateManager.inGameplay) return;

            transform.position = UnityEngine.Input.mousePosition;
        }

        private void OnDestroy() => GameStateManager.OnGameStateChanged -= OnGameStateChanged;
    }
}
