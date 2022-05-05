using System;
using UnityEngine;

namespace Root.LevelManagement
{
    public class TransitionManager : MonoBehaviour
    {
        public static TransitionManager instance;

        [SerializeField] private Animator anim;

        internal TransitionStates currentState;

        public static Action<TransitionStates> OnTransitionStateChanged;

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
        }

        public void FadeIn()
            => anim.SetTrigger("FadeIn");

        public void FadeOut()
            => anim.SetTrigger("FadeOut");

        private void SetTransitionState(TransitionStates state)
        {
            currentState = state;
            OnTransitionStateChanged?.Invoke(state);
        }
    }

    public enum TransitionStates
    {
        FadeOutStarted,
        FadeOutFinished,
        FadeInStarted,
        FadeInFinished,
    }
}