using System.Collections;

namespace Heph.Scripts.Utils.StateMachine
{
    public abstract class State
    {
        public virtual IEnumerator Start()
        {
            yield break;
        }

        public virtual void End()
        {
            
        }
    }
}
