using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoManager.Infrastructure.Options
{
    public class AuthOption
    {
        public const string ISSUER = "https://todomanager.com";
        public const string AUDIENCE = "https://todomanager.com";
        const string KEY = "lfG8l8qYQF2qiUT0WvoJvPYjimVuGNeTJOPUsRS3k3kMcMRt6okRs17qVdtj3TCj0YH5q0YwsrtmKXxUZcmT9YguiKQG2oiStZ";

        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
