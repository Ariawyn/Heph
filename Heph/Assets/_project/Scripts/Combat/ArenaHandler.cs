using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Heph.Scripts.Combat
{
    public class ArenaHandler : MonoBehaviour
    {
        [SerializeField] public ArenaSpaceEntry[] arenaSpacesArray;
        private Dictionary<int, GameObject> _arenaSpaces;

        public GameObject playerGO;
        public GameObject enemyGO;

        public int playerStartSpaceIndex;
        public int enemyStartSpaceIndex;

        public int currentDistanceBetweenFighters;
        
        private void Start()
        {
            _arenaSpaces = new Dictionary<int, GameObject>();
            foreach (var entry in arenaSpacesArray)
            {
                _arenaSpaces.Add(entry.index, entry.space);
            }

            CombatEventsManager.Instance.BattleStarted += HandleBattleStart;
            CombatEventsManager.Instance.FighterMovementAction += HandleMovement;
        }

        private void HandleBattleStart()
        {
            CombatEventsManager.Instance.OnFighterMovementAction(true, playerStartSpaceIndex);
            CombatEventsManager.Instance.OnFighterMovementAction(false, enemyStartSpaceIndex);
        }

        public void HandleMovement(bool isPlayer, int newIndex)
        {
            if (isPlayer)
            {
                playerGO.transform.position = _arenaSpaces[newIndex].transform.position;
            }
            else
            {
                enemyGO.transform.position = _arenaSpaces[newIndex].transform.position;
            }
        }
    }
}