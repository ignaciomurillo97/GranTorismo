using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gran_Torismo_API.NeoHelper
{
    public class NeoProduct
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public bool active { get; set; }
    }
}