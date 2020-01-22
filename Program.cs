using System;
using System.Linq;
using System.Text;

namespace goal
{
    class Program
    {
        static void Add(string s)
        {
            // add to DB
            Console.WriteLine(s);
        }
        static void Main(string[] args)
        {
           try
           {
               var command = args[0];
                switch(command)
                {
                    case "add":
                    {
                        var s = args.TakeLast(args.Length-1).Aggregate((a,b)=>
                        {
                            return a + " " + b;
                        });
                        

                        Add(s);
                        break;
                    }
                    default:
                    throw new Exception();
                }               
           }
           catch(Exception)
           {
               Console.WriteLine("Input failure!");
           }
        }
    }
}
 