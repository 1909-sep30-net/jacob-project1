using System;
using System.Collections.Generic;
using System.Text;
using RatStore.Data;

namespace RatStore.Logic
{
    public class RatStore : Location
    {
        public RatStore()
        {

        }
        public RatStore(IDataStore newDataStore)
        {
            DataStore = newDataStore;

            DataStore.Initialize();

            try
            {
                ChangeLocation(1);
            }
            catch
            {
                // Add test location if none available
                Address = "123 Test St, Everett, WA 98203";
                LocationId = 1;
                DataStore.AddLocation(this);
                DataStore.Save();

                ChangeLocation(1);
            }
        }
    }
}
