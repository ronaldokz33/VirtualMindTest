using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;

namespace VirtualMind.NetTest.Arquitetura.Library.Util.Security
{
    public class Token
    {
        public static UserResponseToken GerarToken(Dictionary<string, string> claims, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations)
        {
            ClaimsIdentity identity = new ClaimsIdentity();

            if (claims != null)
            {
                foreach (var item in claims)
                {
                    identity.AddClaim(new Claim(item.Key, item.Value));
                }
            }

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            return new UserResponseToken()
            {
                AccessToken = token,
                ExpiresIn = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                AuthStatus = "AUTHENTICATION_SUCCESS",
                TokenType = "Bearer"
            };
        }

        public static JwtSecurityToken DecodeToken(string protectedText, SigningConfigurations signingConfiguration, TokenConfigurations tokenConfigurations)
        {
            protectedText = protectedText.Replace("Bearer ", "");

            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = tokenConfigurations.Issuer,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = true,
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                IssuerSigningKey = signingConfiguration.Key,
                ValidateIssuerSigningKey = true
            };

            var handler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;

            ClaimsPrincipal principal = handler.ValidateToken(protectedText, validationParameters, out securityToken);

            return (JwtSecurityToken)securityToken;
        }
    }
}
