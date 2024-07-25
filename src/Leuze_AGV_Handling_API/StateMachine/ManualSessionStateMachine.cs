using SignalR_API.Models.Handling;
using SignalR_API.ProcessHandler;
using SignalR_API.RealmDB;
using Realms;
using System.Security.Cryptography;

namespace SignalR_API.StateMachine
{
    public class ManualSessionStateMachine(string sessionId, Realm realm, string handlingMode) : SessionStateMachineBase(sessionId, realm, handlingMode)
    {

        public override SessionState ChangeState(SessionState lastState, ActionCommand command)
        {
            SessionState nextState = (lastState, command) switch
            {
                _ => throw new NotSupportedException($"State '{lastState}' has no transition on '{command}' command")
            };

            return nextState;
        }
    }
}
