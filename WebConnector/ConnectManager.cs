using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Script.Serialization;
using TicketingSystem.Logging;

namespace WebConnector
{
    public class ConnectManager
    {
        private HttpClient client;
        private readonly string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        public HttpClient GetHttpClient()
        {
            try
            {
                client = new HttpClient { BaseAddress = new Uri(apiUrl) };
                return client;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        public StringContent GetStringOfObject(Object obj)
        {
            try
            {
                var stringContent = new StringContent(
                    JsonConvert.SerializeObject(obj),
                    Encoding.UTF8,
                    "application/json");
                return stringContent;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        public Dictionary<string, object> Get(int id, string url)
        {
            try
            {
                //url will be something like this -> Ticket/get || user/get
                var client = GetHttpClient();
                var response = client.GetAsync($"{url}/{id}").Result;
                var httpResponseContent = response.Content.ReadAsStringAsync().Result;

                return new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(httpResponseContent);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        public string GetAttachment(int id, string url)
        {
            try
            {
                //url will be something like this -> Ticket/get || user/get
                var client = GetHttpClient();
                var response = client.GetAsync($"{url}/{id}").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
    }
}
