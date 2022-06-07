using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Heph.Scripts.Managers.Game;
using UnityEngine;

namespace Heph
{
    [Serializable]
    public class StoryLibrary : MonoBehaviour
    {
        [SerializeField] public List<StoryEntry> stories;

        private static StoryLibrary _instance;
        
        private Dictionary<string, int> storyProgress;

        private GameManager _gameManager;
        
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
        
        public void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            // LOAD STORY PROGRESS
            storyProgress = new Dictionary<string, int>();
            foreach (var characterData in _gameManager.characterDataArray)
            {
                storyProgress.Add(characterData.ID, 0);
            }
        }

        public TextAsset GetCurrentStoryForEnemy(string enemyID)
        {
            var enemyStoryIndex = storyProgress[enemyID];

            var enemyStoryEntry = GetStoryEntryForEnemy(enemyID);
            return enemyStoryEntry == null ? null : GetStoryFromEntry(enemyStoryEntry, enemyStoryIndex);
        }

        public StoryEntry GetStoryEntryForEnemy(string enemyID)
        {
            return stories.FirstOrDefault(entry => entry.characterID == enemyID);
        }

        public TextAsset GetStoryFromEntry(StoryEntry entry, int index)
        {
            return entry.characterStories.Count < index ? null : entry.characterStories[index];
        }
    }
}
