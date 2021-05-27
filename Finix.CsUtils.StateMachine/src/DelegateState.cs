using System;

namespace Finix.CsUtils
{
    public sealed class DelegateState : IState
    {
        public Action<IState?> Enter { get; set; }

        public Action Update { get; set; }

        public Action<IState?> Exit { get; set; }

        public void OnEnterState(IState? previous)
        {
            Enter?.Invoke(previous);
        }

        public void OnUpdate()
        {
            Update?.Invoke();
        }

        public void OnExitState(IState? next)
        {
            Exit?.Invoke(next);
        }
    }
}
