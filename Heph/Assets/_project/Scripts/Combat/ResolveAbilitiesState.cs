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

            if (BattleSystemRef.player == null) { Debug.Log("In start of resolve abilities state, player is null in battlesystem for some reason."); }
            if (BattleSystemRef.enemy == null) { Debug.Log("In start of resolve abilities state, enemy is null in battlesystem for some reason."); }
            
            BattleSystemRef.StartCoroutine(ResolveCurrentBattleAction());

            CombatEventsManager.Instance.OnResolveAbilitiesStateEntered();
            
            yield break;
        }

        public override void End()
        {
            Debug.Log("Resolve Abilities State ended");
        }

        private IEnumerator ResolveCurrentBattleAction()
        {
            Debug.Log("Resolving abilities in actions for combat round: " + BattleSystemRef.currentCombatRound);

            for(;;)
            {
                if (BattleSystemRef.player == null) { Debug.Log("In resolve battle action tick, player is null in battlesystem for some reason."); }
                if (BattleSystemRef.enemy == null) { Debug.Log("In resolve battle action tick, enemy is null in battlesystem for some reason."); }

                if(BattleSystemRef.currentRoundAction < BattleSystemRef.highestFighterDesire)
                {
                    BattleSystemRef.currentRoundAction++;
                    CombatEventsManager.Instance.OnActionTick();

                    Debug.Log("Executing round action: " + BattleSystemRef.currentRoundAction);

                    // TODO: Also this doesnt take into account abilities canceling each other, like if one ability is a punch during dialogue??
                    // TODO: Maybe we have to check on that before launching the coroutines..., make a check function
                    // TODO: Check for interrupt cancelation of top cards abilities
                    Coroutine playerRoutine = null;
                    Coroutine enemyRoutine = null;
                    if (!BattleSystemRef.player.isBusyWithDesire)
                    {
                        playerRoutine = BattleSystemRef.StartCoroutine(BattleSystemRef.player.ExecuteTopCard(BattleSystemRef.enemy));
                    }
                    if (!BattleSystemRef.enemy.isBusyWithDesire)
                    {
                        enemyRoutine = BattleSystemRef.StartCoroutine(BattleSystemRef.enemy.ExecuteTopCard(BattleSystemRef.player));
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

            Debug.Log("Finished resolving abilities in actions for combat round: " + BattleSystemRef.currentCombatRound);
        
            BattleSystemRef.MoveToNextRound();
        }
    }
}
