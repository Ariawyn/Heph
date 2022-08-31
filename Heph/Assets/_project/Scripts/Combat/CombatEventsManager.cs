using System;
using Heph.Scripts.Character;
using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph.Scripts.Combat
{
    public sealed class CombatEventsManager : MonoBehaviour
    {
        public static CombatEventsManager Instance;

        public BattleSystem battleSystemRef;
        
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
        public void OnFighterHealthDamagedAction(bool isPlayer, int damageAmount)
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
            FighterDialogueStartAction?.Invoke(isPlayer);
        }

        public event Action<bool, int, bool> FighterDialogueChoiceAction;

        public void OnFighterDialogueChoiceAction(bool isPlayer, int choiceIndex, bool isDialogueCard)
        {
            FighterDialogueChoiceAction?.Invoke(isPlayer, choiceIndex, isDialogueCard);
        }

        public event Action<bool, BaseCard> CardBeingResolvedAction;

        public void OnCardBeingResolvedAction(bool isPlayer, BaseCard card)
        {
            CardBeingResolvedAction?.Invoke(isPlayer, card);
        }

        public event Action<bool, bool, bool> ToggleChoicesUI;

        public void OnToggleChoicesUI(bool isPlayer, bool isCardSelection, bool isDialogueCard)
        {
            ToggleChoicesUI?.Invoke(isPlayer, isCardSelection, isDialogueCard);
        }

        public event Action<bool> ShouldDraftCardAction;

        public void OnShouldDraftCardAction(bool isPlayer)
        {
            ShouldDraftCardAction?.Invoke(isPlayer);
        }

        public event Action<bool> FinishedDraftingCardAction;

        public void OnFinishedDraftingCardAction(bool isPlayer)
        {
            FinishedDraftingCardAction?.Invoke(isPlayer);
        }

        public event Action<CARD_TYPE> UpdatedSocietalExpectationAction;
        public void OnUpdatedSocietalExpectationAction(CARD_TYPE cardTypeExpectation)
        {
            UpdatedSocietalExpectationAction?.Invoke(cardTypeExpectation);
        }
    }
}
