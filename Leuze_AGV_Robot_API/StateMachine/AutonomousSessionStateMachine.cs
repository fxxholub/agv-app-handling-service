namespace Leuze_AGV_Robot_API.StateMachine
{
    public static class AutonomousSessionStateMachine
    {
        public static SessionState ChangeState(SessionState lastState, ActionCommand command)
        {
            SessionState nextState = (lastState, command) switch
            {
                (SessionState.IDLING, ActionCommand.INIT) => Transition(SessionState.RUNNING, () => { }),
                (SessionState.RUNNING, ActionCommand.RUN) => Transition(SessionState.RUNNING, () => { }),
                (SessionState.RUNNING, ActionCommand.STOP) => Transition(SessionState.STOPPED, () => { }),
                (SessionState.STOPPED, ActionCommand.RUN) => Transition(SessionState.RUNNING, () => { }),
                (SessionState.STOPPED, ActionCommand.END) => Transition(SessionState.ENDED, () => { }),

                _ => throw new NotSupportedException($"State '{lastState}' has no transition on '{command}' command")
            };

            return nextState;
        }

        private static SessionState Transition(SessionState nextState, Action action)
        {
            action();
            return nextState;
        }
    }
}
