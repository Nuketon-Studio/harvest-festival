using HarvestFestival.Entities;
using HarvestFestival.Types;

namespace HarvestFestival.States
{
    public class PlayerStateMachine : StateMachine<PlayerStateType>
    {
        public System.Action<PlayerStateType> OnChangeState;

        public override void SetState(PlayerStateType state)
        {
            base.SetState(state);

            OnChangeState(state);
        }
    }
}