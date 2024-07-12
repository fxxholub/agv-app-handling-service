namespace Leuze_AGV_Robot_API.StateMachine
{
    public static class ManualSessionStateMachine
    {
        public static SessionState ChangeState(SessionState lastState, ActionCommand command)
        {
            SessionState nextState = (lastState, command) switch
            {
                (SessionState.IDLING, ActionCommand.RUN) => Transition(SessionState.RUNNING, () => { }),
                (SessionState.RUNNING, ActionCommand.STOP) => Transition(SessionState.STOPPED, () => { }),
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
