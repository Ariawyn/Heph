using System.Collections;
using Heph.Scripts.Character;
using Heph.Scripts.Combat.Card;
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

                    Debug.Log("Executing round action: " + BattleSystem.currentRoundAction);

                    // TODO: Also this doesnt take into account abilities canceling each other, like if one ability is a punch during dialogue??
                    // TODO: Maybe we have to check on that before launching the coroutines..., make a check function
                    // TODO: Check for interrupt cancelation of top cards abilities
                    Coroutine playerRoutine = null;
                    Coroutine enemyRoutine = null;
                    if (!BattleSystem.player.isBusyWithDesire)
                    {
                        playerRoutine = BattleSystem.StartCoroutine(BattleSystem.player.ExecuteTopCard(BattleSystem.enemy));
                    }
                    if (!BattleSystem.enemy.isBusyWithDesire)
                    {
                        enemyRoutine = BattleSystem.StartCoroutine(BattleSystem.enemy.ExecuteTopCard(BattleSystem.player));
                    }

                    if (playerRoutine != null) yield return playerRoutine;
                    if (enemyRoutine != null) yield return enemyRoutine;
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
