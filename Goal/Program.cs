using System;
using System.Linq;
using System.Text;
using System.IO;
using LiteDB;
using System.Reflection;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Api;

namespace GoalCmd
{
    class Program
    {
        static GoalsApi CreateAPI(string uri = "http://localhost:5000")
        {
            var c = new GoalsApi(uri);
            return c;
        }

        static LiteDatabase OpenDB()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                + Path.DirectorySeparatorChar + "goal"
                + Path.DirectorySeparatorChar + "goal.db";
            var info = new FileInfo(path);
            if (!Directory.Exists(info.DirectoryName))
                Directory.CreateDirectory(info.DirectoryName);
            return new LiteDatabase(info.FullName);
        }

        static void Add(string[] rest)
        {
            var s = rest.Aggregate((a, b) =>
            {
                return a + " " + b;
            });

            // add to DB
            using (var db = OpenDB())
            {
                
            }
        }
        static void List(string[] args)
        {
           
        }
        static void Delete(string[] args)
        {
            
        }
        static void Version()
        {
            var asm = Assembly.GetEntryAssembly();
            var v = asm.GetName().Version;
            Console.WriteLine("goal v" + v.ToString());
        }

        static string Readline(string what)
        {
            Console.Write(what + ":");
            return Console.ReadLine();
        }
        static void Init()
        {
            using (var db = OpenDB())
            {
                var c = db.GetCollection<Config>();
                
                var config = new Config()
                {
                    Uri = Readline("Uri")
                };
               
                c.Upsert(config);
            }
        }

        static Config Config = null;
        static void ParseConfig(string[] args)
        {
            using (var db = OpenDB())
            {
                var c = db.GetCollection<Config>();
                
                var config = new Config()
                {
                    Uri = Readline("Uri")
                };
               
                c.Upsert(config);
            }

            Console.WriteLine("goal configuration completed");
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("no arguments provided");
                return;
            }

            if (args[0] != "config")
            {
                using (var db = OpenDB())
                {
                    var col = db.GetCollection<Config>();
                    Config = col.FindById(1);
                    if (Config == null)
                    {
                        Console.WriteLine("goal not configured, please run 'goal config' to configure goal");
                        return;
                    }
                }
            }



            try
            {
                var first = args[0].ToLower();
                var rest = args.TakeLast(args.Length - 1).ToArray();
                switch (first)
                {
                    case "config":
                        ParseConfig(rest);
                        break;

                    default:
                        Console.WriteLine("unknown command");
                        break;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            /*
                        try
                        {
                            var command = args[0];
                            var rest = args.TakeLast(args.Length - 1).ToArray();
                            switch (command)
                            {
                                case "delete":
                                    {
                                        Delete(rest);
                                        break;
                                    }
                                case "version":
                                    {
                                        Version();
                                        break;
                                    }
                                case "add":
                                    {
                                        Add(rest);
                                        break;
                                    }
                                case "list":
                                    {
                                        List(rest);
                                        break;
                                    }
                                default:
                                    throw new Exception();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }*/
        }
    }
}
