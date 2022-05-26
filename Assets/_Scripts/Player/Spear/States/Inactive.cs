using System;
using Root.Entities;
using UnityEngine;

namespace Root.Player.Spear
{
    public class Inactive : BaseState
    {
        public Inactive(StateMachine stateMachine) : base(stateMachine) { }
        private SpearStateManager sm => (SpearStateManager)stateMachine;

        public override void Enter()
        {
            base.Enter();

            sm.rb.bodyType = RigidbodyType2D.Static;
            sm.returnObjects.GetComponent<SpriteRenderer>().enabled = false;

            var emission = sm.returnObjects.GetComponent<ParticleSystem>().emission;
            emission.rateOverTime = 0f;

            sm.DestroySelf(0.5f);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
