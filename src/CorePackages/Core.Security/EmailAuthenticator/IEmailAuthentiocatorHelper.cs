using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.EmailAuthenticator
{
    public interface IEmailAuthentiocatorHelper
    {
        Task<string> CreateEmailActivationToken();
        Task<string> CreateEmailActivationCode();
    }
}
