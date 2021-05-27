using System;

namespace Finix.CsUtils
{
    public interface IState
    {
        void OnEnterState(IState? previous);

        void OnUpdate();

        void OnExitState(IState? next);
    }
}
