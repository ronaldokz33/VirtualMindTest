using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace VirtualMind.NetTest.Arquitetura.Library.Util.Security
{
    public class SigningConfigurations
    {
        public Microsoft.IdentityModel.Tokens.SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
