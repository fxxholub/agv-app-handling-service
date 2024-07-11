using Leuze_AGV_Robot_API.Models.Handling;
using MongoDB.Bson;
using Realms;

namespace Leuze_AGV_Robot_API.RealmDB
{
    public static class SessionDatabaseHandler
    {
        // Session handling methods
        public static IList<SessionModel> GetSessions(Realm realm, string mode)
        {
            return realm.All<SessionModel>().Where(s => s.Mode == mode).ToList();
        }

        public static SessionModel GetSession(Realm realm, string sessionId)
        {
            return realm.Find<SessionModel>(ObjectId.Parse(sessionId));
        }
        public static void AddSession(Realm realm, SessionModel session)
        {
            realm.Write(() => realm.Add(session));
        }

        public static void RemoveSession(Realm realm, SessionModel session)
        {
            realm.Write(() =>
            {
                foreach (var action in session.Actions)
                {
                    realm.Remove(action);
                }
                realm.Remove(session);
            });
        }

        // Session Action handling methods
        public static IList<ActionModel> GetSessionActions(SessionModel session)
        {
            return session.Actions.AsQueryable().ToList();
        }

        public static ActionModel GetSessionAction(SessionModel session, string actionId)
        {
            return session.Actions.FirstOrDefault(a => a.Id == ObjectId.Parse(actionId));
        }
        public static void AddSessionAction(Realm realm, SessionModel session, ActionModel action)
        {
            realm.Write(() => session.Actions.Add(action));
        }

        public static void RemoveSessionAction(Realm realm, SessionModel session, ActionModel action)
        {
            realm.Write(() =>
            {
                session.Actions.Remove(action);
                realm.Remove(action);
            });
        }
    }
}
