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

            if (BattleSystemRef.player == null) { Debug.Log("In begin combat state, player is null in battlesystem for some reason."); }
            if (BattleSystemRef.enemy == null) { Debug.Log("In begin combat state, enemy is null in battlesystem for some reason."); }
            
            yield return new WaitForSeconds(2);

            BattleSystemRef.SetState(new SelectAbilitiesState(BattleSystemRef));
        }

        public override void End()
        {
            Debug.Log("Begin Combat State ended");
        }
    }
}
