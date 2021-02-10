using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TicketingSystem.Common.Enums;
using TicketingSystem.Web.Client.Controllers;
using TicketingSystem.Web.Client.Models;
using WebConnector;

namespace TicketingSystem.Web.Client.Helper
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
            return (T)obj;
        }

    }
}