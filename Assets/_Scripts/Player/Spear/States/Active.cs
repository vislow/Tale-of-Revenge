using Root.Entities;
using UnityEngine;

namespace Root.Player.Spear
{
    public class Active : BaseState
    {
        public Active(StateMachine stateMachine) : base(stateMachine) { }
        private SpearStateManager sm => (SpearStateManager)stateMachine;

        public override void Enter()
        {
            base.Enter();

            sm.SetObjectActivity(activeObj: true, stuckObj: false, returnObj: false);
            sm.rb.bodyType = RigidbodyType2D.Kinematic;
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

            if (sm.rb.bodyType == RigidbodyType2D.Static) return;

            sm.rb.velocity = sm.transform.right * (sm.speed * Time.deltaTime);
        }
    }
}
