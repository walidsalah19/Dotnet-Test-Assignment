﻿namespace Dotnet_Test_Assignment.Models
{
    public class LogsAttemps
    {
        public string IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public string CountryCode { get; set; } 
        public bool IsBlocked { get; set; }
        public string UserAgent { get; set; }
    }
}
