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
    partial class Program
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

        static string Readline(string what)
        {
            Console.Write(what + ":");
            return Console.ReadLine();
        }
      

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("no arguments provided");
                return;
            }

            if (args[0] != "config" || args[0] != "version")
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
                    case "version":
                        ParseVersion();
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
