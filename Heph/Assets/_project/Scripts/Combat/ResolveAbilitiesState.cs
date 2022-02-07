using System.Collections;
using UnityEngine;

namespace Heph.Scripts.Combat
{
    public class ResolveAbilitiesState : BattleState
    {
        public ResolveAbilitiesState(BattleSystem battleSystem) : base(battleSystem) {}

        public override IEnumerator Start()
        {
            Debug.Log("Resolve Abilities State started");

            BattleSystem.StartCoroutine(ResolveCurrentBattleAction());

            yield break;
        }

        public override void End()
        {
            Debug.Log("Resolve Abilities State ended");
        }

        private IEnumerator ResolveCurrentBattleAction()
        {
            Debug.Log("Resolving abilities in actions for combat round: " + BattleSystem.currentCombatRound);

            for(;;)
            {
                if(BattleSystem.currentRoundAction < BattleSystem.highestFighterDesire)
                {
                    BattleSystem.currentRoundAction++;
                    CombatEventsManager.Instance.OnActionTick();
                    
                    // TODO: Get current abilities on the round action from battle system.
                    
                    
                    Debug.Log("Executing round action: " + BattleSystem.currentRoundAction);

                    // TODO: Wait for 1 second, or wait for action to resolve, whichever is longest
                    // Really the longest something could be is a dialogue ability perhaps, in which case we have to wait
                    // for our story manager for the inkle integration when i make that

                    yield return new WaitForSeconds(1);
                }
                else
                {
                    break;
                }
            }

            Debug.Log("Finished resolving abilities in actions for combat round: " + BattleSystem.currentCombatRound);
        
            BattleSystem.MoveToNextRound();
        }
    }
}
