using Root;
using UnityEngine;

namespace Root.Input
{
    public class CursorController : MonoBehaviour
    {
        private void Awake() => GameManager.OnGameStateChanged += OnGameStateChanged;

        private void OnGameStateChanged(GameState state) => transform.gameObject.SetActive(state == GameState.Gameplay);

        private void Update() => MoveCursorToMouse();

        private void MoveCursorToMouse()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            if (GameManager.gameState != GameState.Gameplay) return;

            transform.position = UnityEngine.Input.mousePosition;
        }

        private void OnDestroy() => GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
}
