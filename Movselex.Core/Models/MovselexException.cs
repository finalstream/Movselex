using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{
    public class MovselexException : Exception
    {
        public MovselexException(string message) : base(message)
        {
        }
    }
}
