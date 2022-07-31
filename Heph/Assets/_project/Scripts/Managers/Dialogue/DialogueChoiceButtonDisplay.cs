using TMPro;
using UnityEngine;
using UnityEngine.UI;


using Ink.Runtime;

namespace Heph.Scripts.Combat.Card
{
    public class DialogueChoiceButtonDisplay : MonoBehaviour
    {
        public Choice dialogueChoice;
        
        public TextMeshProUGUI buttonText;
        private void Start()
        {
            if (buttonText != null) buttonText.text = dialogueChoice.text;
        }

        public void OnButtonClick()
        {
            var battleSystem = FindObjectOfType<BattleSystem>();
           battleSystem.player.AttemptToRespondToDialogueChoice(dialogueChoice.index, false);
        }
        
    }
}
