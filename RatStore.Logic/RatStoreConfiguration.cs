using System;
using System.Collections.Generic;
using System.Text;
using RatStore.Data;

namespace RatStore.Logic
{
    public class RatStoreConfiguration
    {
        public static IDataStore GetDataStore()
        {
            IDataStore dataStore = new DatabaseStore();
            if (!dataStore.Connected())
                dataStore = new TextStore();

            return dataStore;
        }
    }
}
