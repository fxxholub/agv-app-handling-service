using Leuze_AGV_Robot_API.Models.Handling;
using Leuze_AGV_Robot_API.StateMachine;
using MongoDB.Bson;
using Realms;
using System.Collections.Generic;
using System.Linq;

namespace Leuze_AGV_Robot_API.RealmDB
{
    public static class SessionDatabaseHandler
    {
        // Session handling methods
        public static IList<SessionModel> GetSessions(Realm realm, string mode)
        {
            return realm.All<SessionModel>().Where(s => s.Mode == mode).ToList();
        }

        public static SessionModel GetSession(Realm realm, string sessionId, string mode)
        {
            var session = realm.Find<SessionModel>(ObjectId.Parse(sessionId));
            return session != null && session.Mode == mode ? session : null;
        }

        public static void AddSession(Realm realm, SessionModel session, string mode)
        {
            if (session.Mode != mode) return;

            realm.Write(() => realm.Add(session));
        }

        public static void ChangeSessionState(Realm realm, string sessionId, string mode, SessionState state)
        {
            var session = GetSession(realm, sessionId, mode);
            if (session == null) return;

            realm.Write(() => session.State = state);
        }

        public static void RemoveSession(Realm realm, string sessionId, string mode)
        {
            var session = GetSession(realm, sessionId, mode);
            if (session == null) return;

            realm.Write(() =>
            {
                foreach (var action in session.Actions)
                {
                    realm.Remove(action);
                }
                foreach (var process in session.Processes)
                {
                    realm.Remove(process);
                }
                realm.Remove(session);
            });
        }

        // Session Action handling methods
        public static IList<ActionModel> GetSessionActions(Realm realm, string sessionId, string mode)
        {
            var session = GetSession(realm, sessionId, mode);
            return session?.Actions.AsQueryable().ToList();
        }

        public static ActionModel GetSessionAction(Realm realm, string sessionId, string actionId, string mode)
        {
            var session = GetSession(realm, sessionId, mode);
            return session?.Actions.FirstOrDefault(a => a.Id == ObjectId.Parse(actionId));
        }

        public static void AddSessionAction(Realm realm, string sessionId, ActionModel action, string mode)
        {
            var session = GetSession(realm, sessionId, mode);
            if (session == null) return;

            realm.Write(() => session.Actions.Add(action));
        }

        public static void RemoveSessionAction(Realm realm, string sessionId, string actionId, string mode)
        {
            var session = GetSession(realm, sessionId, mode);
            if (session == null) return;

            var action = session.Actions.FirstOrDefault(a => a.Id == ObjectId.Parse(actionId));
            if (action == null) return;

            realm.Write(() =>
            {
                session.Actions.Remove(action);
                realm.Remove(action);
            });
        }

        // Session Process handling methods
        public static void AddSessionProcess(Realm realm, string sessionId, ProcessModel process, string mode)
        {
            var session = GetSession(realm, sessionId, mode);
            if (session == null) return;

            realm.Write(() => session.Processes.Add(process));
        }

        public static IList<ProcessModel> GetSessionProcesses(Realm realm, string sessionId, string mode)
        {
            var session = GetSession(realm, sessionId, mode);
            return session?.Processes.AsQueryable().ToList();
        }

        public static ProcessModel GetSessionProcess(Realm realm, string sessionId, string processId, string mode)
        {
            var session = GetSession(realm, sessionId, mode);
            return session?.Processes.FirstOrDefault(p => p.Id == ObjectId.Parse(processId));
        }

        public static void RemoveSessionProcess(Realm realm, string sessionId, string processId, string mode)
        {
            var session = GetSession(realm, sessionId, mode);
            if (session == null) return;

            var process = session.Processes.FirstOrDefault(p => p.Id == ObjectId.Parse(processId));
            if (process == null) return;

            realm.Write(() =>
            {
                session.Processes.Remove(process);
                realm.Remove(process);
            });
        }

        public static void SetSessionProcessActive(Realm realm, string sessionId, string mode, string processId, bool isActive)
        {
            var session = GetSession(realm, sessionId, mode);
            if (session == null) return;

            var process = session.Processes.FirstOrDefault(p => p.Id == ObjectId.Parse(processId));
            if (process == null) return;

            realm.Write(() => process.Active = isActive);
        }
    }
}
