using System.Collections;
using System.Collections.Generic;
using Heph.Scripts.Combat;
using Heph.Scripts.Combat.Card;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Heph
{
    public class CombatUIHandler : MonoBehaviour
    {
        public GameObject cardDisplayPrefab;
        
        public GameObject playerCardArea;
        public Button confirmButton;

        public void Start()
        {
            CombatEventsManager.Instance.ActionTick += EndSelection;
        }
        
        public void DisplayHand(List<BaseCard> cards)
        {
            foreach (var card in cards)
            {
                var cardDisplayObj = Instantiate(cardDisplayPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                cardDisplayObj.GetComponent<CardDisplay>().card = card;
                cardDisplayObj.transform.SetParent(playerCardArea.transform, false);
            }
            confirmButton.gameObject.SetActive(true);
        }

        public void ClearHand()
        {
            foreach(Transform child in playerCardArea.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void EndSelection()
        {
            CombatEventsManager.Instance.OnSelectionConfirmButtonPressed();
            confirmButton.gameObject.SetActive(false);
        }
    }
}
