using UnityEngine;

namespace Heph.Scripts.Utils.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State _state;

        public void SetState(State state)
        {
            _state?.End();
            _state = state;
            StartCoroutine(_state.Start());
        }

        public State GetState()
        {
            return _state;
        }
    }
}
