using System.Collections;
using System.Collections.Generic;
using Heph.Scripts.Managers.Game;
using TMPro;
using UnityEngine;

namespace Heph
{
    public class FightResultScreen : MonoBehaviour
    {
        private GameManager _gameManager;
        public TextMeshProUGUI resultText;
        
        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            resultText.text = _gameManager.lastCombatResult;
        }
    }
}
