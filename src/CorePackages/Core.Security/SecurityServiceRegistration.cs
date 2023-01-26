using Core.Security.JWT;
using Core.Security.OTPAuthenticator.OtpNet;
using Core.Security.OTPAuthenticator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Security.EmailAuthenticator;

namespace Core.Security
{
    public static class SecurityServiceRegistration
    {

        public static IServiceCollection services(this IServiceCollection services)
        {
            services.AddScoped<ITokenHelper, JwtHelper>();
            services.AddScoped<IEmailAuthentiocatorHelper, EmailAuthentiocatorHelper>();
            services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();
            return services;
        }   
    }
}
