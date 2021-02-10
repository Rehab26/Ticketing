using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using TicketingSystem.Logging;
using NAudio.Wave;

namespace TicketingSystem.AvailoService
{
    public class AvailoAuthManager
    {
        private readonly string AvailoUrl = ConfigurationManager.AppSettings["AvailoURL"];
        public bool AuthenticateUser(string primaryFile, string secondaryFile)
        {
            try
            {
                int fileType = 0;
                var primaryfileExtention = primaryFile.Split('.');
                var secondaryfileExtention = primaryFile.Split('.');

                if (primaryfileExtention[1] == "jpg" && secondaryfileExtention[1] == "jpg") {
                    fileType = 1;
                }

                byte[] primaryFileBytes = System.IO.File.ReadAllBytes(primaryFile);
                byte[] secondaryFileBytes = System.IO.File.ReadAllBytes(secondaryFile);

                // send to api 
                BiometricModel biometricModel = new BiometricModel
                {
                    PrimaryFileBytes = primaryFileBytes,
                    SecondaryFileBytes = secondaryFileBytes,
                    AccountID = "40c9ef0a-c25e-4b7d-929e-1b9571b74ced",
                    FileType = fileType,
                    MatchingVoiceThreshold = 2.1,
                    MatchVoiceWithWords = true,
                    MatchVoiceScoreRatio = 4,
                    VoicesExtractTextDependentFeatures = true,
                    CheckFromImageFaceSize = true,
                    MinimumImageFaceSizeRatio = 7.1
                };
                HttpClient client = new HttpClient { BaseAddress = new Uri(AvailoUrl) };
                client.DefaultRequestHeaders.Add("ServiceKey", "5415D7B5-592C-47CE-8C2A-001A96EA12B8");
                var response = client.PostAsJsonAsync("api/MMA/RecognizeBytes", biometricModel).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    var httpResponseContent = response.Content.ReadAsStringAsync().Result;
                    var data = new JavaScriptSerializer().Deserialize<AvailoResponseModel>(httpResponseContent);
                    if (data.data.Status == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return false;
            }
        }

       

    }
}