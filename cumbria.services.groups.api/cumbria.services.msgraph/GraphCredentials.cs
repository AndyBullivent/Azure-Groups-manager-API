using System.Globalization;

namespace cumbria.services.msgraph
{
    public class GraphCredentials
    {
        public string ClientId { get; set; }
        public string Key { get; set; }
        public string URI { get; set; }
        public string Tenant { get; set; }
        public string Authority
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}", "e58cae89-8f91-4f69-8cce-51abf1d13b44");
            }
        }
    }
}