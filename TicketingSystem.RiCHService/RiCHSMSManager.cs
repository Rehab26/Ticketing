using System;
using System.Net.Http;
using System.Configuration;

namespace TicketingSystem.RiCHService
{

    public class RiCHSMSManager
    {

        private readonly string RichUrl = ConfigurationManager.AppSettings["RiCHURL"];
        private readonly string RichSender = ConfigurationManager.AppSettings["RiCHSender"];
        private readonly string RichPassword = ConfigurationManager.AppSettings["RiCHPassword"];
        private readonly string RichUsername = ConfigurationManager.AppSettings["RiCHUsername"];

        public OTPResponseModel SendSmsSignUp(string number, int referenceid = 0)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri(RichUrl) };
            string[] AllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string RandomOTP = GenerateRandomOTP(4, AllowedCharacters);
            var response = client.GetAsync($"/RiCHClientServiceREST.svc/SendSmsLoginGet?username={RichUsername}" +
                $"&password={RichPassword}&Sender={RichSender}&Text={RandomOTP}&number={number}&referenceid={referenceid} ").Result;
            return new OTPResponseModel() { OTPassword = RandomOTP, IsSent = response.IsSuccessStatusCode };
        }

        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string OTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                OTP += sTempChars;
            }
            return OTP;
        }
    }
}
