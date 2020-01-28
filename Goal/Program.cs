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
        static Config Config = null;

        static GoalsApi CreateAPI()
        {
            var c = new GoalsApi(Config.Uri);
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

        static void ParseList(string[] args)
        {
            var api = CreateAPI();
            var goals = api.Api1GoalsGet();
            foreach (var g in goals)
            {
                Console.WriteLine($"{g.Id}\t{g.Description}");
            }
        }

        static void ParseAdd(string[] args)
        {
            var api = CreateAPI();
            var description = Readline("Description");
            var g = new Org.OpenAPITools.Model.PostGoal(description);
            api.Api1GoalsPost(g);
        }

        static void ParseDelete(string[] args)
        {
            var api = CreateAPI();
            var id = int.Parse(Readline("id"));
            api.Api1GoalsDelete(id);
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
                    case "list":
                        ParseList(rest);
                        break;
                    case "add":
                        ParseAdd(rest);
                        break;
                    case "delete":
                        ParseDelete(rest);
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
        }
    }
}
