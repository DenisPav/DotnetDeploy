using System;

namespace DotnetDeploy.Extensions
{
    public static class GenericExtension
    {
        public static void TryCatchExecute<T>(this T target, Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            { }
        }
    }
}
