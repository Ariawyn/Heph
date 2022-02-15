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
            nameText.text = card.name;
            desireCostText.text = card.desireCost.ToString();
            descriptionText.text = card.description;
        }
    }
}
