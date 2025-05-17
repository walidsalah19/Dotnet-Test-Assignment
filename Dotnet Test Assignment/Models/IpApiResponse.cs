namespace Dotnet_Test_Assignment.Models
{
    public class IpApiResponse
    {
         public string ip { get; set; }
        public string network { get; set; }
        public string version { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string region_code { get; set; }
        public string country { get; set; }
        public string country_name { get; set; }
        public string country_code { get; set; }
        public string country_code_iso3 { get; set; }
        public string country_capital { get; set; }
        public string country_tld { get; set; }
        public string continent_code { get; set; }
        public bool in_eu { get; set; }
        public string? postal { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string timezone { get; set; }
        public string utcOffset { get; set; }
        public string country_calling_code { get; set; }
        public string currency { get; set; }
        public string currency_name { get; set; }
        public string languages { get; set; }
        public double country_area { get; set; }
        public long country_population { get; set; }
        public string asn { get; set; }
        public string org { get; set; }
    }
}
