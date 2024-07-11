using Leuze_AGV_Robot_API.Models.Handling;

namespace Leuze_AGV_Robot_API.Controllers.Handling
{
    using Realms;
    using MongoDB.Bson;
    using System.Collections.Generic;
    using System.Linq;

    public static class SessionDatabaseHandler
    {
        // Method to add a new session
        public static void AddSession(Realm realm, SessionModel session)
        {
            realm.Write(() =>
            {
                realm.Add(session);
            });
        }

        // Method to find a session by its ID
        public static SessionModel FindSession(Realm realm, string sessionId)
        {
            return realm.Find<SessionModel>(ObjectId.Parse(sessionId));
        }

        // Method to remove a session
        public static void RemoveSession(Realm realm, SessionModel session)
        {
            realm.Write(() =>
            {
                realm.Remove(session);
            });
        }

        // Method to get all sessions
        public static List<SessionModel> GetAllSessions(Realm realm)
        {
            return realm.All<SessionModel>().ToList();
        }

        // Method to add a new action to a session
        public static void AddActionToSession(Realm realm, SessionModel session, ActionModel action)
        {
            realm.Write(() =>
            {
                session.Actions.Add(action);
            });
        }

        // Method to get all actions from a session
        public static List<ActionModel> GetActionsFromSession(SessionModel session)
        {
            return session.Actions.ToList();
        }
    }

}

