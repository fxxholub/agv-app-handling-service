using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using Realms;

namespace Leuze_AGV_Robot_API.RealmDB
{
    public abstract class RealmControllerBase : ControllerBase
    {
        private readonly RealmConfiguration _realmConfig;

        public RealmControllerBase(Realm realm)
        {
            _realmConfig = (RealmConfiguration?)realm.Config;
        }

        protected Realm GetRealmInstance()
        {
            return Realm.GetInstance(_realmConfig);
        }
    }
}
