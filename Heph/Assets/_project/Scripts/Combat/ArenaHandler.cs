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
                var result = Move(isPlayer, isRetreat, false);
                
                if(result == -1) break;
                
                if (isPlayer) currentPlayerOccupiedSpaceIndex = result;
                else currentEnemyOccupiedSpaceIndex = result;
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

        public int Move(bool isPlayer, bool isRetreat, bool isPush)
        {
            var desiredIndex = -1;

            desiredIndex = isPlayer
                ? CalculateDesiredIndex(isRetreat, currentPlayerOccupiedSpaceIndex, currentPlayerFacing)
                : CalculateDesiredIndex(isRetreat, currentEnemyOccupiedSpaceIndex, currentEnemyFacing);

            
            if (!isPush)
            {
                var shouldPush = IsOccupied(desiredIndex);
                var pushResult = -1;
                if (shouldPush) pushResult = Move(!isPlayer, true, true);
                if (shouldPush && (pushResult == -1)) return -1;
            }
            
            if (desiredIndex < minArenaSpaceIndex || desiredIndex > maxArenaSpaceIndex) return -1;

            var currentCharacterGO = isPlayer ? playerGO : enemyGO;
            currentCharacterGO.transform.position = _arenaSpaces[desiredIndex].transform.position;

            RefreshCharactersFacing();
            
            return desiredIndex;
        }

        private static int CalculateDesiredIndex(bool isRetreat, int currentOccupiedIndex, int currentFacing)
        {
            return isRetreat
                ? currentOccupiedIndex - (1 * currentFacing)
                : currentOccupiedIndex + (1 * currentFacing);
        }

        private void RefreshCharactersFacing()
        {
            if (currentPlayerOccupiedSpaceIndex < currentEnemyOccupiedSpaceIndex)
            {
                currentPlayerFacing = 1;
                currentEnemyFacing = -1;
            }
            else
            {
                currentPlayerFacing = -1;
                currentEnemyFacing = 1;
            }
        }
    }
}
