using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core
{
    public class MovselexClientFactory
    {
        public static IMovselexClient Create(Assembly executingAssembly, string appConfigFilePath)
        {
            return new MovselexClient(executingAssembly, appConfigFilePath);
        }
    }
}
