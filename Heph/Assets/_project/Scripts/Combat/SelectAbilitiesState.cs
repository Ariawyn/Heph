using System.Collections;
using UnityEngine;

namespace Heph.Scripts.Combat
{
    public class SelectAbilitiesState : BattleState
    {
        public SelectAbilitiesState(BattleSystem battleSystem) : base(battleSystem) { }

        public override IEnumerator Start()
        {
            Debug.Log("Select Abilities State started");

            yield return new WaitForSeconds(2);

            BattleSystem.SetState(new ResolveAbilitiesState(BattleSystem));
        }

        public override void End()
        {
            Debug.Log("Select Abilities State ended");
        }
    }
}
