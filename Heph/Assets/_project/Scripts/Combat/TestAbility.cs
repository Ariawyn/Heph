using Heph.Scripts.Character;
using UnityEngine;

namespace Heph.Scripts.Combat
{
    [CreateAssetMenu]
    public class TestAbility : Ability
    {
        public string debugText = "Test";

        public override void Activate(FighterHandler owner, FighterHandler target)
        {
            Debug.Log("Test ability activated: " + debugText);
        }
    }
}
