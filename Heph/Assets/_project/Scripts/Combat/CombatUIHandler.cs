using System.Collections;
using System.Collections.Generic;
using Heph.Scripts.Combat;
using Heph.Scripts.Combat.Card;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Heph
{
    public class CombatUIHandler : MonoBehaviour
    {
        public BattleSystem battleSystemRef;
        
        public TextMeshProUGUI battleTitle;
        
        public GameObject cardDisplayPrefab;
        
        public GameObject playerCardArea;
        public GameObject playerCardDropArea;
        public Button confirmButton;

        public Slider playerHealthBar;
        public Slider enemyHealthBar;

        public void Start()
        {
            CombatEventsManager.Instance.ActionTick += EndSelection;
            CombatEventsManager.Instance.DrawAction += AddCardToHand;
            CombatEventsManager.Instance.SelectAbilitiesStateEntered += StartSelection;
            CombatEventsManager.Instance.BattleStarted += HandleBattleStart;
            
        }

        private void HandleBattleStart()
        {
            battleTitle.text = "Battle - Heph vs " + battleSystemRef.enemy.ID;
        }
        
        private void AddCardToHand(bool isPlayer, BaseCard card)
        {
            if (!isPlayer) return;
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
            confirmButton.gameObject.SetActive(true);
        }

        public void EndSelection()
        {
            CombatEventsManager.Instance.OnSelectionConfirmButtonPressed();
            confirmButton.gameObject.SetActive(false);
        }
    }
}
