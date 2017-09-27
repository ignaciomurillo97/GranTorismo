using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gran_Torismo_API.RedisHelper
{
    public class RedisItem
    {
        public int productId { get; set; }
        public int establishmentId { get; set; }

        public RedisItem(int establishmentId, int productId)
        {
            this.productId = productId;
            this.establishmentId = establishmentId;
        }
    }
}