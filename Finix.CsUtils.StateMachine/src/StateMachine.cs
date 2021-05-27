using System.Collections.Generic;
using System;

namespace Finix.CsUtils
{
    public class StateMachine
    {
        public IState? CurrentState { get; protected set; }

        public Stack<IState> StateStack { get; } = new Stack<IState>();

        public virtual void Move(IState nextState)
        {
            var oldState = CurrentState;

            if (oldState != null)
                oldState.OnExitState(nextState);

            CurrentState = nextState;
            CurrentState.OnEnterState(oldState);
        }

        public virtual void Update()
        {
            CurrentState?.OnUpdate();
        }

        public virtual void Push(IState nextState)
        {
            StateStack.Push(CurrentState);
            Move(nextState);
        }

        public virtual void Pop()
        {
            var state = StateStack.Pop();
            Move(state);
        }
    }
}
