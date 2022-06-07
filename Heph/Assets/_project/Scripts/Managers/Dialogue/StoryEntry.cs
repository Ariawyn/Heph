using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

namespace Heph
{
    [System.Serializable] 
    [CreateAssetMenu(fileName = "New Story Entry", menuName = "Story/LibraryEntry")]
    public class StoryEntry : ScriptableObject
    {
        [SerializeField] public string characterID;
        [SerializeField] public List<TextAsset> characterStories;
    }
}
