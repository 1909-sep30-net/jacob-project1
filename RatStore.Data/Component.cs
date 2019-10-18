using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public struct Component
    {
        public int ComponentId { get; set; }
        public decimal? Cost { get; set; }
        public string Name { get; set; }
    }
}
