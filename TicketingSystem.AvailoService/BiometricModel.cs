using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.AvailoService
{
   public class BiometricModel
    {
        public byte[] PrimaryFileBytes { get; set; }
        public byte[] SecondaryFileBytes { get; set; }
        public string AccountID { get; set; }
        public int FileType { get; set; } 
        public double MatchingVoiceThreshold { get; set; } 
        public bool MatchVoiceWithWords { get; set; } 
        public int MatchVoiceScoreRatio { get; set; } 
        public bool VoicesExtractTextDependentFeatures { get; set; }
        public bool CheckFromImageFaceSize { get; set; } 
        public double MinimumImageFaceSizeRatio { get; set; } 
    }
}
