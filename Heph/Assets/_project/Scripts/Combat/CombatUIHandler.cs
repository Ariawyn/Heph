using System;
using System.Collections;
using System.Collections.Generic;
using Heph.Scripts.Combat;
using Heph.Scripts.Combat.Card;
using Heph.Scripts.Managers.Dialogue;
using Ink.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Heph
{
    public class CombatUIHandler : MonoBehaviour
    {
        public BattleSystem battleSystemRef;
        public DialogueManager dialogueManagerRef;
        
        public TextMeshProUGUI battleTitle;
        
        public GameObject cardDisplayPrefab;
        public GameObject cardFlirtButtonDisplayPrefab;
        public GameObject cardDraftButtonDisplayPrefab;

        public GameObject choiceButtonDisplayPrefab;
        
        public GameObject playerCardArea;
        public GameObject playerCardDropArea;
        public Button confirmButton;

        public HealthBar playerHealthBar;
        public HealthBar enemyHealthBar;
        public TextMeshProUGUI playerShieldAmount;
        public TextMeshProUGUI enemyShieldAmount;

        public CardDisplay playerResolvingCardDisplay;
        public CardDisplay enemyResolvingCardDisplay;

        public GameObject playerCardSelectionOptionArea;
        public GameObject playerButtonSelectionOptionArea;

        public GameObject societalExpectationArea;
        public TextMeshProUGUI societalExpectationValue;

        public void Start()
        {
            CombatEventsManager.Instance.ActionTick += EndSelection;
            CombatEventsManager.Instance.DrawAction += AddCardToHand;
            CombatEventsManager.Instance.SelectAbilitiesStateEntered += StartSelection;
            CombatEventsManager.Instance.BattleStarted += HandleBattleStart;

            CombatEventsManager.Instance.FighterShieldDamagedAction += UpdateShieldValue;
            CombatEventsManager.Instance.FighterHealthDamagedAction += UpdateHealthbar;

            CombatEventsManager.Instance.CardBeingResolvedAction += UpdateCardResolvedUI;
            CombatEventsManager.Instance.ToggleChoicesUI += TogglePlayerSelectionOptions;

            CombatEventsManager.Instance.ShouldDraftCardAction += StartDraftingUIActions;
            CombatEventsManager.Instance.UpdatedSocietalExpectationAction += UpdateSocietalExpectationText;
        }

        private void StartDraftingUIActions(bool isPlayer)
        {
            TogglePlayerSelectionOptions(isPlayer, true, false);
        }

        private void TogglePlayerSelectionOptions(bool isPlayer, bool isCardSelection, bool isDialogueCard)
        {
            var isSettingUp = true;
            if (isCardSelection)
            {
                isSettingUp = !playerCardSelectionOptionArea.activeInHierarchy;
                playerCardSelectionOptionArea.SetActive(isSettingUp);
            }
            else
            {
                isSettingUp = !playerButtonSelectionOptionArea.activeInHierarchy;
                playerButtonSelectionOptionArea.SetActive(isSettingUp);
            }

            if (isSettingUp)
            {
                if (!isCardSelection)
                {
                    Debug.Log("We are attempting dialogue choice buttons!");
                    var currentChoices = new List<Choice>(dialogueManagerRef.GetCurrentChoices());
                    foreach (var choice in currentChoices)
                    {
                        Debug.Log("Choice: " + choice.text);
                        var choiceButtonDisplayObj = Instantiate(choiceButtonDisplayPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        choiceButtonDisplayObj.GetComponent<DialogueChoiceButtonDisplay>().dialogueChoice = choice;
                        choiceButtonDisplayObj.transform.SetParent(playerButtonSelectionOptionArea.transform, false);
                    }
                }
                else
                { 
                    switch (battleSystemRef.currentDeviationControl)
                    {
                        case DEVIATION_CONTROL.FIRST: // HERE WE NEED TO HAVE FLIRT OPTIONS
                            battleSystemRef.player.flirtAttemptDeckHandler.DrawCards(2);
                            foreach (var flirtOption in battleSystemRef.player.flirtAttemptDeckHandler.Hand)
                            {
                                var cardButtonDisplayObj = Instantiate(cardFlirtButtonDisplayPrefab,
                                    new Vector3(0, 0, 0), Quaternion.identity);
                                cardButtonDisplayObj.GetComponent<CardDisplay>().card = flirtOption;
                                cardButtonDisplayObj.transform.SetParent(playerCardSelectionOptionArea.transform,
                                    false);
                            }
                            break;
                        case DEVIATION_CONTROL.SECOND:
                            battleSystemRef.societalFailurePotentialAdditionDeckHandler.DrawCards(3);
                            foreach (var draftOption in battleSystemRef.societalFailurePotentialAdditionDeckHandler.Hand)
                            {
                                var cardButtonDisplayObj = Instantiate(cardDraftButtonDisplayPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                                cardButtonDisplayObj.GetComponent<CardButtonDisplay>().card = draftOption;
                                cardButtonDisplayObj.transform.SetParent(playerCardSelectionOptionArea.transform, false);
                            }
                            break;
                        case DEVIATION_CONTROL.BOTH:
                            if (isDialogueCard)
                            {
                                battleSystemRef.player.flirtAttemptDeckHandler.DrawCards(2);
                                foreach (var flirtOption in battleSystemRef.player.flirtAttemptDeckHandler.Hand)
                                {
                                    var cardButtonDisplayObj = Instantiate(cardFlirtButtonDisplayPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                                    cardButtonDisplayObj.GetComponent<CardDisplay>().card = flirtOption;
                                    cardButtonDisplayObj.transform.SetParent(playerCardSelectionOptionArea.transform, false);
                                }
                            }
                            else
                            {
                                battleSystemRef.societalFailurePotentialAdditionDeckHandler.DrawCards(3);
                                foreach (var draftOption in battleSystemRef.societalFailurePotentialAdditionDeckHandler.Hand)
                                {
                                    var cardButtonDisplayObj = Instantiate(cardDraftButtonDisplayPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                                    cardButtonDisplayObj.GetComponent<CardButtonDisplay>().card = draftOption;
                                    cardButtonDisplayObj.transform.SetParent(playerCardSelectionOptionArea.transform, false);
                                }
                            }
                            break;
                        case DEVIATION_CONTROL.NONE: // THIS IS HANDLED BY THE IF STATEMENT
                            break;
                        default:    // I GUESS WE SHOULD HAVE THIS EVEN THOUGH IT CANT POSSIBLY HAPPEN??
                            break;
                    }
                }
            }
            else
            {
                if (battleSystemRef.currentDeviationControl is DEVIATION_CONTROL.FIRST or DEVIATION_CONTROL.BOTH)
                {
                    battleSystemRef.player.flirtAttemptDeckHandler.TotalReset();
                }

                if (battleSystemRef.currentDeviationControl is DEVIATION_CONTROL.SECOND or DEVIATION_CONTROL.BOTH)
                {
                    battleSystemRef.societalFailurePotentialAdditionDeckHandler.TotalReset();   
                }
                
                foreach(Transform child in playerButtonSelectionOptionArea.transform)
                {
                    Destroy(child.gameObject);
                }
                
                foreach(Transform child in playerCardSelectionOptionArea.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        private void HandleBattleStart()
        {
            battleTitle.text = "Battle - Heph vs " + battleSystemRef.enemy.ID;
            
            playerHealthBar.SetMaxHealth(battleSystemRef.player.maximumHealth.baseValue);
            enemyHealthBar.SetMaxHealth(battleSystemRef.enemy.maximumHealth.baseValue);
            playerShieldAmount.text = battleSystemRef.player.currentShield.Value.ToString();
            enemyShieldAmount.text = battleSystemRef.enemy.currentShield.Value.ToString();

            societalExpectationArea.SetActive(
                battleSystemRef.currentDeviationControl is DEVIATION_CONTROL.SECOND or DEVIATION_CONTROL.BOTH);
        }

        private void UpdateSocietalExpectationText(CARD_TYPE cardType)
        {
            societalExpectationValue.text = cardType.ToString();
        }
        
        private void AddCardToHand(bool isPlayer, BaseCard card)
        {
            if (!isPlayer) return;
            if (card.type == CARD_TYPE.FLIRT) return; // IGNORE FIRST DEVIATION FLIRT CARDS, THEY DONT EXIST IN THIS DECK CONTEXT
            var cardDisplayObj = Instantiate(cardDisplayPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            cardDisplayObj.GetComponent<CardDisplay>().card = card;
            cardDisplayObj.transform.SetParent(playerCardArea.transform, false);
        }
        
        public void ClearBoard()
        {
            foreach(Transform child in playerCardArea.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in playerCardDropArea.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void StartSelection()
        {
            Debug.Log("Selection should now be available.");
            
            // Remove UI elements we dont need in select state
            playerResolvingCardDisplay.gameObject.SetActive(false);
            enemyResolvingCardDisplay.gameObject.SetActive(false);
            
            // Allow for the switching to resolve state through button confirmation
            confirmButton.gameObject.SetActive(true);

        }

        public void EndSelection()
        {
            CombatEventsManager.Instance.OnSelectionConfirmButtonPressed();
            confirmButton.gameObject.SetActive(false);
        }

        public void UpdateHealthbar(bool isPlayer, int damageAmount)
        {
            if (isPlayer)
            {
                playerHealthBar.SetHealth(battleSystemRef.player.currentHealth.Value);
            }
            else
            {
                enemyHealthBar.SetHealth(battleSystemRef.enemy.currentHealth.Value);
            }
        }

        public void UpdateShieldValue(bool isPlayer, int damageAmount)
        {
            if (isPlayer)
            {
                playerShieldAmount.text = battleSystemRef.player.currentShield.Value.ToString();
            }
            else
            {
                enemyShieldAmount.text = battleSystemRef.enemy.currentShield.Value.ToString();
            }
        }

        public void UpdateCardResolvedUI(bool isPlayer, BaseCard card)
        {
            var noCardQueuedForAction = card == null;
            if (isPlayer)
            {
                if (noCardQueuedForAction)
                {
                    playerResolvingCardDisplay.gameObject.SetActive(false);
                }
                else
                {
                    if(playerResolvingCardDisplay.gameObject.activeInHierarchy != true) 
                        playerResolvingCardDisplay.gameObject.SetActive(true);
                    playerResolvingCardDisplay.Refresh(card);
                }
            }
            else
            {
                if (noCardQueuedForAction)
                {
                    enemyResolvingCardDisplay.gameObject.SetActive(false);
                }
                else
                {
                    if (enemyResolvingCardDisplay.gameObject.activeInHierarchy != true)
                        enemyResolvingCardDisplay.gameObject.SetActive(true);
                    enemyResolvingCardDisplay.Refresh(card);
                }
            }
        }
    }
}
