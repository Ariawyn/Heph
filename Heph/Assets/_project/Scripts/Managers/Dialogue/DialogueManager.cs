using UnityEngine;
using Ink.Runtime;

namespace Heph.Scripts.Managers.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager _instance;

        // INK RELATED STORY VARIABLES
        private Story _currentStory;
        private bool _storyIsLoaded = false;
        private bool _dialogueIsActive = false;
        
        // INK SPECIFIC TAGS WE USE
        private const string SPEAKER_TAG = "speaker";
        private const string LAYOUT_TAG = "layout";
        
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
        }

        public void StartStory(TextAsset inkJSON)
        {
            _currentStory = new Story(inkJSON.text);
            _storyIsLoaded = true;
        }

        public void ToggleDialogue()
        {
            //TODO: Setup active dialogue UI
        }
        
        
        
    }
}
