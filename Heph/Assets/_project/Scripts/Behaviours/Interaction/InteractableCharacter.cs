using Heph.Scripts.Character;
using Heph.Scripts.Managers.Game;
using UnityEngine;

namespace Heph.Scripts.Behaviours.Interaction
{
    public class InteractableCharacter : Interactable
    {
        [SerializeField] private FighterHandler fighterData; 

        private GameManager _gameManager;

        private void Start()
        {
            // THERE ABSOLUTELY MUST BE A BETTER WAY FOR THE INTERACTABLE CHARACTERS TO COMMUNICATE TO THE GAME MANAGER I THINK?? DOES IT MATTER???
            _gameManager = FindObjectOfType<GameManager>();
        }

        public override void Interact()
        {
            Debug.Log("Hello");
            
            // IS THIS THE BEST WAY??????????????
            var playerGameObject = GameObject.FindWithTag("Player");
        
            playerGameObject.TryGetComponent<FighterHandler>(out var playerFighterData);
            if(playerFighterData == null) return;
        
            _gameManager.StartBattle(playerFighterData, fighterData);
        }
    }
}
