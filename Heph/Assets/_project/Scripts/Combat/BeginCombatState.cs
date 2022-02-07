using System.Collections;
using UnityEngine;

namespace Heph.Scripts.Combat
{
    public class BeginCombatState : BattleState
    {
        public BeginCombatState(BattleSystem battleSystem) : base(battleSystem) { }

        public override IEnumerator Start()
        {
            Debug.Log("Begin Combat State started");

            yield return new WaitForSeconds(2);

            BattleSystem.SetState(new SelectAbilitiesState(BattleSystem));
        }

        public override void End()
        {
            Debug.Log("Begin Combat State ended");
        }
    }
}
