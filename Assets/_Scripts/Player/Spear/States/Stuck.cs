using Root.Entities;
using Root.Systems.Input;
using UnityEngine;

namespace Root.Player.Spear
{
    [RequireComponent(typeof(Collider2D))]
    public class Stuck : BaseState
    {
        public Stuck(StateMachine stateMachine) : base(stateMachine) { }
        private SpearStateManager Sm => (SpearStateManager)stateMachine;

        private Collider2D solidCollider;

        public override void Enter()
        {
            base.Enter();

            Sm.SetObjectActivity(activeObj: false, stuckObj: true, returnObj: false);

            Sm.rb.velocity = Vector3.zero;

            solidCollider = Sm.stuckObjects.GetComponent<Collider2D>();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            solidCollider.enabled = InputManager.instance.verticalInput == -1 ? false : Sm.PlayerCollision.spearUnderPlayer;
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
