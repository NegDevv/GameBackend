using System;

namespace GameBackend
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args != null || args.Length < 1)
            {
                throw new Exception("Must enter argument");
            }
            Console.WriteLine("Hello World! " + args[0]);

        }

     
    }


}
