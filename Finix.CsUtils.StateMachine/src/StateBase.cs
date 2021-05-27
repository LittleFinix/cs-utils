using System;

namespace Finix.CsUtils
{
    public abstract class StateBase : IState
    {
        public virtual void OnEnterState(IState? previous)
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnExitState(IState? next)
        {
        }
    }
}
