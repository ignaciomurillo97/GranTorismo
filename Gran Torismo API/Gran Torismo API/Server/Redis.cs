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

        public static void AddToCart(int userId, int serviceId)
        {
            IDatabase db = redis.GetDatabase();
            db.ListLeftPush(cartKey(userId), serviceId);
        }

        public static List<int> GetCart(int userId)
        {
            IDatabase db = redis.GetDatabase();
            List<RedisValue> result = db.ListRange(cartKey(userId), 0, -1).ToList();
            List<int> cart = new List<int>();
            foreach (RedisValue r in result)
            {
                cart.Add((int)r);
            }
            return cart;
        }

        public static void DeleteFromCart(int userId, int serviceId)
        {
            IDatabase db = redis.GetDatabase();
            db.ListRemove(cartKey(userId), serviceId, 1);
        }

        private static string cartKey(int userId)
        {
            return userId.ToString() + "_cart";
        }

    }
}