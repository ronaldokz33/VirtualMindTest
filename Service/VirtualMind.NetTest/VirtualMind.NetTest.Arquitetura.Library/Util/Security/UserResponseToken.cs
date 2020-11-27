using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualMind.NetTest.Arquitetura.Library.Util.Security
{
    public class UserResponseToken
    {
        [JsonProperty("extra")]
        public string Extra { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("otp_seed")]
        public string OtpSeed { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("auth_status")]
        public string AuthStatus { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("ad_domain")]
        public string AdDomain { get; set; }
    }
}
