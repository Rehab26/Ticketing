using System;
using System.Collections.Generic;
using WebConnector;

namespace TicketingSystem.Web.Employee.Helpers
{
    public class HelperMethod
    {
        public ConnectManager Connector = new ConnectManager();

        //BaseController Base = new BaseController();
        public static T GetObject<T>(Dictionary<string, object> dict)
        {
            Type type = typeof(T);
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict)
            {
                type.GetProperty(kv.Key).SetValue(obj, kv.Value);
            }

            return (T) obj;
        }
    }
}