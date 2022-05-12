using UnityEngine;

namespace Root.Entities
{
    public class StateMachine : MonoBehaviour
    {
        internal BaseState currentState { get; private set; }

        private void Start()
        {
            currentState = GetInitialState();

            if (currentState == null) return;

            currentState.Enter();
        }

        private void Update()
        {
            if (currentState == null) return;

            currentState.UpdateLogic();
        }

        private void FixedUpdate()
        {
            if (currentState == null) return;

            currentState.UpdatePhysics();
        }

        public void ChangeState(BaseState newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }

        protected virtual BaseState GetInitialState() => null;

        private void OnGUI()
        {
            string content = currentState != null ? currentState.name : "(No current state)";

            GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        }
    }
}