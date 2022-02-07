using System;
using UnityEngine;

namespace Heph.Scripts.Combat
{
    public sealed class CombatEventsManager : MonoBehaviour
    {
        public static CombatEventsManager Instance;

        private void Awake()
        {
            if(Instance == null)
			{
				Instance = this;
			}
			else if(Instance != this)
			{
				Destroy(gameObject);
			}
        }

        public event Action RoundTick;
        public void OnRoundTick()
        {
            RoundTick?.Invoke();
        }

        public event Action ActionTick;
        public void OnActionTick()
        {
            ActionTick?.Invoke();
        }
    }
}
