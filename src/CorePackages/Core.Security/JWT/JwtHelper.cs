using Core.Security.Encryption;
using Core.Security.Entities;
using Core.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private readonly TokenOptions tokenOptions;
        private DateTime accessTokenExpiration;
        public JwtHelper(IConfiguration configuration)
        {
            this.Configuration = configuration;
            tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public AccessToken CreateToken(User user, IList<OperationClaim> operationClaims)
        {
            accessTokenExpiration = DateTime.Now.AddMinutes(tokenOptions.AccessTokenExpiration);
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = SigningCreadentialsHelper.CreateSigningCredentials(securityKey);
            JwtSecurityToken jwt = CreateJwtSecurityToken(tokenOptions, user, signingCredentials, operationClaims);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            string? token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken { Token = token, Expiration = accessTokenExpiration };
        }

        public RefreshToken CreateRefreshToken(User user, string ipAddress)
        {
            throw new NotImplementedException();
        }
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
                                                   SigningCredentials signingCredentials,
                                                   IList<OperationClaim> operationClaims)
        {

            JwtSecurityToken jwt = new(tokenOptions.Issuer, tokenOptions.Audience, expires: accessTokenExpiration,
                notBefore: DateTime.Now, claims: SetClaims(user, operationClaims), signingCredentials: signingCredentials);
            return jwt;

        }
        private IEnumerable<Claim> SetClaims(User user, IList<OperationClaim> operationClaims)
        {
            List<Claim> claims = new();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());
            return claims;
        }
    }
}
