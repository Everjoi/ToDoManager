using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoManager.Domain.Exceptions
{
    public class PasswordNotMatchExceptions : Exception
    {
        public PasswordNotMatchExceptions(): base("Password do not match")
        {
            
        }
    }
}
