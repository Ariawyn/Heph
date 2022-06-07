using Heph.Scripts.Combat;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.InputSystem;

namespace Heph.Scripts.Managers.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager _instance;

        // INK RELATED STORY VARIABLES
        private Story _currentStory;
        public  bool _storyIsLoaded { get; private set; }
        public bool _dialogueIsActive { get; private set; }
        
        // INK SPECIFIC TAGS WE USE
        private const string SPEAKER_TAG = "speaker";
        private const string LAYOUT_TAG = "layout";
        
        // INPUT HANDLING VAR
        private PlayerInputs _playerInputs;
        
        // UI SPECIFIC VAR
        public GameObject dialoguePanel;
        public TextMeshProUGUI characterNameTextArea;
        public TextMeshProUGUI dialogueTextArea;
        
        // BATTLE RELATED VAR
        private BattleSystem _battleSystem;
        
        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            else if(_instance != this)
            {
                Destroy(gameObject);
            }

            _storyIsLoaded = false;
            _dialogueIsActive = false;
            
            dialoguePanel.SetActive(false);
            
            _playerInputs = new PlayerInputs();
            _playerInputs.Player.Interact.performed += HandleInteract;
            _playerInputs.Player.Interact.Enable();

            CombatEventsManager.Instance.FighterDialogueStartAction += StartupDialogueInstance;
        }

        private void Start()
        {
            _battleSystem = GetComponent<BattleSystem>();
        }
        
        public void StartStory(TextAsset inkJSON)
        {
            if (_storyIsLoaded) return;
            _currentStory = new Story(inkJSON.text);
            _storyIsLoaded = true;
        }

        public void StartupDialogueInstance(bool playerStartedDialogue)
        {
            ToggleDialogue(true);
            AdvanceDialogue();
        }

        public void ToggleDialogue(bool setActiveValue)
        {
            _dialogueIsActive = setActiveValue;
            dialoguePanel.SetActive(setActiveValue);
            _battleSystem.isWaitingOnDialogue = setActiveValue;
        }

        public void AdvanceDialogue()
        {
            if (!_storyIsLoaded) return;
            if (!_dialogueIsActive) return;
            if (_currentStory.canContinue)
            {
                dialogueTextArea.text = _currentStory.Continue();
            }
            else
            {
                ToggleDialogue(false);
            }
        }

        public void ExitStory()
        {
            _storyIsLoaded = false;
            _currentStory = null;
            
            dialoguePanel.SetActive(false);
        }
        
        private void HandleInteract(InputAction.CallbackContext obj)
        {
            if (!_dialogueIsActive) return;
            AdvanceDialogue();
        }
    }
}
