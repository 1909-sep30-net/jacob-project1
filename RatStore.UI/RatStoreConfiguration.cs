using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RatStore.Data;
using RatStore.Logic;

namespace RatStore.UI
{
    public class RatStoreConfiguration
    {
        public static IDataStore GetDataStore()
        {
            IDataStore dataStore = new DatabaseStore(new DbContextOptionsBuilder<Data.Entities.jacobproject0Context>()
                /*.UseSqlServer(SecretCode.Sauce)*/
                .EnableSensitiveDataLogging()
            );
            if (!dataStore.Connected())
                dataStore = new TextStore();

            return dataStore;
        }
    }
}
