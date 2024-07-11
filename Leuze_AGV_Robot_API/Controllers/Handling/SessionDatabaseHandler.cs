using Leuze_AGV_Robot_API.Models.Handling;
using MongoDB.Bson;
using Realms;

namespace Leuze_AGV_Robot_API.RealmDB
{
    public static class SessionDatabaseHandler
    {
        public static IList<SessionModel> GetAllSessions(Realm realm)
        {
            return realm.All<SessionModel>().ToList();
        }

        public static SessionModel FindSession(Realm realm, string sessionId)
        {
            return realm.Find<SessionModel>(ObjectId.Parse(sessionId));
        }

        public static ActionModel FindActionById(SessionModel session, string actionId)
        {
            return session.Actions.FirstOrDefault(a => a.Id == ObjectId.Parse(actionId));
        }

        public static void RemoveActionFromSession(Realm realm, SessionModel session, ActionModel action)
        {
            realm.Write(() =>
            {
                session.Actions.Remove(action);
                realm.Remove(action);
            });
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

        public static void AddActionToSession(Realm realm, SessionModel session, ActionModel action)
        {
            realm.Write(() => session.Actions.Add(action));
        }

        public static IList<ActionModel> GetActionsFromSession(SessionModel session)
        {
            return session.Actions.AsQueryable().ToList();
        }
    }
}
