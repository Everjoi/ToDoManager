using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoManager.Domain.Exceptions
{
    public class ExistUserException : Exception
    {

        public ExistUserException(string email) : base($"User: {email} already exist")
        {
            
        }
    }
}
