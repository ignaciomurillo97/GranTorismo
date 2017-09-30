using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gran_Torismo_API.RedisHelper
{
    public class RedisItem
    {
        public int serviceId { get; set; }
        public int establishmentId { get; set; }

        public RedisItem(int establishmentId, int serviceId)
        {
            this.serviceId = serviceId;
            this.establishmentId = establishmentId;
        }
    }
}