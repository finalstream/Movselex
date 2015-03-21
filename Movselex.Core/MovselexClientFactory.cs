using System.Reflection;

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