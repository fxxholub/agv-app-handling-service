using Leuze_AGV_Robot_API.Models.Handling;
using Leuze_AGV_Robot_API.ProcessHandler;
using Leuze_AGV_Robot_API.RealmDB;
using Realms;
using System.Security.Cryptography;

namespace Leuze_AGV_Robot_API.StateMachine
{
    public class ManualSessionStateMachine(string sessionId, Realm realm, string handlingMode) : SessionStateMachineBase(sessionId, realm, handlingMode)
    {

        public override SessionState ChangeState(SessionState lastState, ActionCommand command)
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
    }
}
