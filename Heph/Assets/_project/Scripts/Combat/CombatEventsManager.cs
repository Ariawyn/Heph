using System;
using Heph.Scripts.Character;
using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph.Scripts.Combat
{
    public sealed class CombatEventsManager : MonoBehaviour
    {
        public static CombatEventsManager Instance;
        private void Awake()
        {
            if(Instance == null)
			{
				Instance = this;
			}
			else if(Instance != this)
			{
				Destroy(gameObject);
			}
        }

        public event Action BattleStarted;
        public void OnBattleStarted()
        {
            BattleStarted?.Invoke();
        }
        

        public event Action RoundTick;
        public void OnRoundTick()
        {
            RoundTick?.Invoke();
        }

        public event Action ActionTick;
        public void OnActionTick()
        {
            ActionTick?.Invoke();
        }

        public event Action SelectAbilitiesStateEntered;
        public void OnSelectAbilitiesStateEntered()
        {
            SelectAbilitiesStateEntered?.Invoke();
        }

        public event Action ResolveAbilitiesStateEntered;
        public void OnResolveAbilitiesStateEntered()
        {
            ResolveAbilitiesStateEntered?.Invoke();
        }

        public event Action SelectionConfirmButtonEvent;
        public void OnSelectionConfirmButtonPressed()
        {
            SelectionConfirmButtonEvent?.Invoke();
        }

        public event Action<bool, BaseCard> DrawAction;
        public void OnDrawAction(bool isPlayer, BaseCard cardDrawn)
        {
            DrawAction?.Invoke(isPlayer, cardDrawn);
        }

        public event Action<BaseCard> QueueCardAction;
        public void OnPlayerQueueCardAction(BaseCard cardQueued)
        {
            QueueCardAction?.Invoke(cardQueued);
        }

        public event Action<bool, int> FighterHealthDamagedAction;
        public void OnFighterDamagedAction(bool isPlayer, int damageAmount)
        {
            FighterHealthDamagedAction?.Invoke(isPlayer, damageAmount);
        }
        
        public event Action<bool, int> FighterShieldDamagedAction;
        public void OnFighterShieldDamagedAction(bool isPlayer, int damageAmount)
        {
            FighterShieldDamagedAction?.Invoke(isPlayer, damageAmount);
        }
        
        public event Action<string> FighterDefeatedAction;
        public void OnFighterDefeatedAction(string fighterID)
        {
            FighterDefeatedAction?.Invoke(fighterID);
        }

        public event Action<bool, int, bool> FighterMovementAction;
        public void OnFighterMovementAction(bool isPlayer, int spacesToMove, bool isRetreat)
        {
            Debug.Log("Fighter should move, isPlayer: " + isPlayer + ", spacesToMove: " + spacesToMove + ", isRetreat: " + isRetreat);
            FighterMovementAction?.Invoke(isPlayer, spacesToMove, isRetreat);
        }

        public event Action<bool> FighterDialogueStartAction;

        public void OnFighterDialogueStartAction(bool isPlayer)
        {
            Debug.Log("Fighter should start dialogue, isPlayer: " + isPlayer);
            FighterDialogueStartAction.Invoke(isPlayer);
        }
    }
}
