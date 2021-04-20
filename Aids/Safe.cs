using System;
namespace Contoso.Aids
{
    public static class Safe
    {
        public static T Run<T>(Func<T> function, T valueOnException)
        {
            try
            {
                return function();
            }
            catch (Exception e)
            {
                logException(e);
                return valueOnException;
            }
        }

        public static void Run(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                logException(e);
            }
        }

        private static void logException(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}