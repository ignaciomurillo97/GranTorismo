using Gran_Torismo_API.RedisHelper;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedisConnect
{
    public class Redis
    {
        static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

        public static void AddToCart(int userId, int establishmentId, int serviceId)
        {
            IDatabase db = redis.GetDatabase();
            db.ListLeftPush(cartKey(userId), serializeItem(establishmentId, serviceId));
        }

        public static List<RedisItem> GetCart(int userId)
        {
            IDatabase db = redis.GetDatabase();
            List<RedisValue> result = db.ListRange(cartKey(userId), 0, -1).ToList();
            List<RedisItem> cart = new List<RedisItem>();
            foreach (RedisValue r in result)
            {
                RedisItem item = deserializeItem(r);
                cart.Add(item);
            }
            return cart;
        }

        public static void DeleteFromCart(int userId, int establishmentId, int serviceId)
        {
            IDatabase db = redis.GetDatabase();
            db.ListRemove(cartKey(userId), serializeItem(establishmentId, serviceId), 1);
        }

        private static string cartKey(int userId)
        {
            return userId.ToString() + "_cart";
        }

        private static string serializeItem(int establishmentId, int serviceId)
        {
            var item = new RedisItem(establishmentId, serviceId);
            string json = JsonConvert.SerializeObject(item);
            return json;
        }

        private static RedisItem deserializeItem(string json)
        {
            RedisItem item = JsonConvert.DeserializeObject<RedisItem>(json);
            return item;
        }
    }
}