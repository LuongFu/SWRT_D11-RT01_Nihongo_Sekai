namespace NihongoSekaiWebApplication_D11_RT01.Data.Services
{
    public class MyService
    {
        private readonly string _conn;
        private readonly string _smtpUser;
        private readonly string _googleClientId;

        public MyService(IConfiguration config)
        {
            _conn = config["DB_CONN_STRING"];
            _smtpUser = config["SMTP_USERNAME"];
            _googleClientId = config["GOOGLE_CLIENT_ID"];
        }
    }

}
