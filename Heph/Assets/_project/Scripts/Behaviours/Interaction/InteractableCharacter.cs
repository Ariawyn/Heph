using Heph.Scripts.Character;
using Heph.Scripts.Managers.Game;
using UnityEngine;

namespace Heph.Scripts.Behaviours.Interaction
{
    public class InteractableCharacter : Interactable
    {
        [SerializeField] public string fighterID;

        [SerializeField] public DEVIATION_CONTROL deviationRequested;
        
        private GameManager _gameManager;

        private void Start()
        {
            // THERE ABSOLUTELY MUST BE A BETTER WAY FOR THE INTERACTABLE CHARACTERS TO COMMUNICATE TO THE GAME MANAGER I THINK?? DOES IT MATTER???
            _gameManager = FindObjectOfType<GameManager>();
        }

        public override void Interact()
        {
            _gameManager.UpdateDeviation(deviationRequested);
            _gameManager.StartBattle(fighterID);
        }
    }
}
