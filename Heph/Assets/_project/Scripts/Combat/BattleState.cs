using Heph.Scripts.Utils.StateMachine;

namespace Heph.Scripts.Combat
{
    public class BattleState : State
    {
        protected BattleSystem BattleSystemRef;

        protected BattleState(BattleSystem battleSystem)
        {
            BattleSystemRef = battleSystem;
        }
    }
}
