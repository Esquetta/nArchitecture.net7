using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Pipeline.Authorization
{
    public interface ISecureRequest
    {
        public string[] Roles { get;}
    }
}
