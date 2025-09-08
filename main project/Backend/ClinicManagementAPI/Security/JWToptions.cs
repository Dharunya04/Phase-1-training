namespace ClinicManagementAPI.Security
{
    public class JWToptions
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; }="";
        public string Key { get; set; } = "";
        public int ExpiryInMinutes { get; set; }=60;
    }
}
