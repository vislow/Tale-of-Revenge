using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class TransitionManager : MonoBehaviour
    {
        /// <Description> Variables </Description>

        public static TransitionManager instance;

        [SerializeField] private Animator anim;

        internal TransitionStates currentState;

        public static Action<TransitionStates> OnTransitionStateChanged;

        /// <Description> Mathods </Description>
        /// <Description> Unity Methods </Description>

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

        /// <Description> Custom Methods </Description>

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