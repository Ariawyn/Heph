using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Heph.Scripts.Combat
{
    public class ArenaHandler : MonoBehaviour
    {
        [SerializeField] public Transform arena;
        private List<Transform> _arenaSpaces;

        public GameObject playerGO;
        public int currentPlayerOccupiedSpaceIndex = -1;
        public GameObject enemyGO;
        public int currentEnemyOccupiedSpaceIndex = -1;
        
        public int playerStartSpaceIndex;
        public int enemyStartSpaceIndex;

        public int currentDistanceBetweenFighters;
        
        private void Start()
        {
            _arenaSpaces = new List<Transform>();
            for (var i = 0; i < arena.childCount; i++)
            {
                _arenaSpaces.Add(arena.GetChild(i));
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
            var currentCharacterGO = isPlayer ? playerGO : enemyGO;
            var currentOpposingCharacterGO = isPlayer ? enemyGO : playerGO;

            var facingPositive = true;
            if ((currentPlayerOccupiedSpaceIndex != -1) && (currentEnemyOccupiedSpaceIndex != -1))
            {
                facingPositive = currentPlayerOccupiedSpaceIndex <= currentEnemyOccupiedSpaceIndex;
            }
            
            if (isPlayer)
            {
                playerGO.transform.position = _arenaSpaces[newIndex].transform.position;
                currentPlayerOccupiedSpaceIndex = newIndex;
            }
            else
            {
                enemyGO.transform.position = _arenaSpaces[newIndex].transform.position;
                currentEnemyOccupiedSpaceIndex = newIndex;
            }
        }
    }
}
