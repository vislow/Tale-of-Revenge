using Root.Entities;
using UnityEngine;

namespace Root.Player.Spear
{
    public class Returning : BaseState
    {
        public Returning(StateMachine stateMachine) : base(stateMachine) { }
        private SpearStateManager sm => (SpearStateManager)stateMachine;

        public override void Enter()
        {
            base.Enter();

            sm.SetObjectActivity(activeObj: false, stuckObj: false, returnObj: true);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            sm.RotateTowardsPlayer();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            if (Vector2.Distance(sm.transform.position, sm.PlayerPos) < sm.playerDestroyDistance)
            {
                sm.ChangeState(sm.inactiveState);
                return;
            }

            if (sm.rb.bodyType == RigidbodyType2D.Static) return;

            sm.rb.velocity = sm.transform.right * (-sm.speed * Time.deltaTime);
        }
    }
}
