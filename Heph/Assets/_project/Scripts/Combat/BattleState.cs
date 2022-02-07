using Heph.Scripts.Utils.StateMachine;

namespace Heph.Scripts.Combat
{
    public class BattleState : State
    {
        protected readonly BattleSystem BattleSystem;

        protected BattleState(BattleSystem battleSystem)
        {
            BattleSystem = battleSystem;
        }
    }
}
