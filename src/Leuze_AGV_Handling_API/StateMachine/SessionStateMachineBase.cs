using SignalR_API.Models.Handling;
using SignalR_API.ProcessHandler;
using SignalR_API.RealmDB;
using Realms;
using System.Security.Cryptography;

namespace SignalR_API.StateMachine
{
    public abstract class SessionStateMachineBase(string sessionId, Realm realm, string handlingMode)
    {
        protected readonly string sessionId = sessionId;
        protected readonly Realm realm = realm;
        protected readonly string handlingMode = handlingMode;

        public abstract SessionState ChangeState(SessionState lastState, ActionCommand command);

        protected static SessionState Transition(SessionState nextState, Action transitionHandle)
        {
            transitionHandle();
            return nextState;
        }
    }
}
