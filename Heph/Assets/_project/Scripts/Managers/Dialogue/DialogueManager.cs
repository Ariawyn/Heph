using System.Collections.Generic;
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
        private const string DIALOGUE_CHOICE_TYPE_TAG = "choice_type";
        
        // INK TAG SPECIFIC VAR FOR CHECKING
        private bool currentDialogueCardChoiceType = false;
        
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
            CombatEventsManager.Instance.FighterDialogueChoiceAction += HandleChoiceMade;
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
                HandleTags(_currentStory.currentTags);
                if (_currentStory.currentChoices.Count > 0)
                {
                    // Handle dialogue choice tags
                    SetupChoices();
                }
            }
            else
            {
                ToggleDialogue(false);
            }
        }

        private bool HandleTags(List<string> currentTags)
        {
            if (currentTags.Count <= 0) return false;
            foreach (var tag in currentTags)
            {
                var splitTag = tag.Split(':');
                if (splitTag.Length != 2)
                {
                    Debug.LogError("Dialogue tag could not be parsed: " + tag);
                }
                var tagKey = splitTag[0].Trim();
                var tagValue = splitTag[1].Trim();

                switch (tagKey)
                {
                    case DIALOGUE_CHOICE_TYPE_TAG:
                        Debug.Log("Updating dialogue card choice type thingy!!!");
                        currentDialogueCardChoiceType = tagValue == "flirt_card";
                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        public List<Choice> GetCurrentChoices()
        {
            return _currentStory.currentChoices;
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

        private void HandleChoiceMade(bool isPlayer, int choiceIndex, bool isDialogueCard)
        {
            CombatEventsManager.Instance.OnToggleChoicesUI(false, isDialogueCard, isDialogueCard);
            _currentStory.ChooseChoiceIndex(choiceIndex);
            AdvanceDialogue();
        }

        private void SetupChoices()
        {
            CombatEventsManager.Instance.OnToggleChoicesUI(true, currentDialogueCardChoiceType, currentDialogueCardChoiceType);
        }
    }
}
