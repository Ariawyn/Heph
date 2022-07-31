using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Heph.Scripts.Combat.Card
{
    public class CardDisplay : MonoBehaviour
    {
        public BaseCard card;

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;

        public TextMeshProUGUI desireCostText;

        private void Start()
        {
            if(nameText != null) nameText.text = card.name;
            if(desireCostText != null) desireCostText.text = card.desireCost.ToString();
            if(descriptionText != null) descriptionText.text = card.description;
        }

        public void Refresh()
        {
            if(nameText != null) nameText.text = card.name;
            if(desireCostText != null) desireCostText.text = card.desireCost.ToString();
            if(descriptionText != null) descriptionText.text = card.description;
        }

        public void Refresh(BaseCard cardToReplace)
        {
            card = cardToReplace;
            Refresh();
        }

        public void TryActivateCard()
        {
            var battleSystem = FindObjectOfType<BattleSystem>();
            StartCoroutine(card.Activate(battleSystem.player, battleSystem.enemy));
        }
    }
}
