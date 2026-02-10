using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Objects
{
    public enum UnitOfWorkState
    {
        Open,
        Comitted,
        RolledBack
    }
}
