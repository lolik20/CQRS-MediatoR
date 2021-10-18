using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS_MediatoR.Api.Validation
{
    public class TokenPair
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

}
