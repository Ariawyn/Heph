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
        public int currentPlayerFacing = 1;
        public GameObject enemyGO;
        public int currentEnemyOccupiedSpaceIndex = -1;
        public int currentEnemyFacing = -1;
        
        public int playerStartSpaceIndex;
        public int enemyStartSpaceIndex;

        public int maxArenaSpaceIndex = -1;
        public int minArenaSpaceIndex = -1;
        public int currentDistanceBetweenFighters;

        public bool hasSetupFighters = false;
        
        private void Start()
        {
            minArenaSpaceIndex = maxArenaSpaceIndex = 0;
            _arenaSpaces = new List<Transform>();
            for (var i = 0; i < arena.childCount; i++)
            {
                _arenaSpaces.Add(arena.GetChild(i));
                maxArenaSpaceIndex = i;
            }

            CombatEventsManager.Instance.BattleStarted += HandleBattleStart;
            CombatEventsManager.Instance.FighterMovementAction += HandleMovement;
        }

        private void HandleBattleStart()
        {
            HandleMovementToSpecificIndex(true, playerStartSpaceIndex);
            HandleMovementToSpecificIndex(false, enemyStartSpaceIndex);
            hasSetupFighters = true;
        }

        public void HandleMovement(bool isPlayer, int spacesToMove, bool isRetreat)
        {
            for (var i = 0; i < spacesToMove; i++) { 
                var result = Move(isPlayer, isRetreat);
                if (!result) break;
            }
        }

        public void HandleMovementToSpecificIndex(bool isPlayer, int newIndex)
        {
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

        public bool IsOccupied(int index)
        {
            return index == currentPlayerOccupiedSpaceIndex || index == currentEnemyOccupiedSpaceIndex;
        }

        public bool Move(bool isPlayer, bool isRetreat)
        {
            var desiredIndex = -1;
            if (isPlayer)
            {
                desiredIndex = isRetreat
                    ? currentPlayerOccupiedSpaceIndex + (1 * currentPlayerFacing)
                    : currentPlayerOccupiedSpaceIndex - (1 * currentPlayerFacing);
                if (IsOccupied(desiredIndex)) return false; // TODO: Add Push? If Advance
                if (desiredIndex < minArenaSpaceIndex || desiredIndex > maxArenaSpaceIndex) return false;
                
                playerGO.transform.position = _arenaSpaces[desiredIndex].transform.position;
                currentPlayerOccupiedSpaceIndex = desiredIndex;
                return true;
            }
            desiredIndex = isRetreat
                ? currentEnemyOccupiedSpaceIndex + (1 * currentEnemyFacing)
                : currentEnemyOccupiedSpaceIndex - (1 * currentEnemyFacing);
            if (IsOccupied(desiredIndex)) return false; // TODO: Add Push? If Advance
            if (desiredIndex < minArenaSpaceIndex || desiredIndex > maxArenaSpaceIndex) return false;
            enemyGO.transform.position = _arenaSpaces[desiredIndex].transform.position;
            currentEnemyOccupiedSpaceIndex = desiredIndex;
            return true;
        }
    }
}
