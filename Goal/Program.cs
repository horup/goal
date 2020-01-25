using System;
using System.Linq;
using System.Text;
using System.IO;
using LiteDB;
using System.Reflection;

namespace goal
{
    class GoalEntry
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
        public string Description { get; set; } = "";

        public override string ToString()
        {
            return string.Format("{0}\t{1}", this.Id, this.Description);
        }
    }

    class Program
    {
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
                var col = db.GetCollection<GoalEntry>("goals");
                var g = new GoalEntry()
                {
                    Description = s
                };

                col.Insert(g);
            }
        }
        static void List(string[] args)
        {
            using (var db = OpenDB())
            {
                var col = db.GetCollection<GoalEntry>("goals");
                foreach (var g in col.FindAll().Reverse())
                {
                    Console.WriteLine(g.ToString());
                }
            }
        }
        static void Delete(string[] args)
        {
            using (var db = OpenDB())
            {
                var id = int.Parse(args[0]);
                var col = db.GetCollection<GoalEntry>("goals");
                col.Delete(id);
            }
        }
        static void Version()
        {
            var asm = Assembly.GetEntryAssembly();
            var v = asm.GetName().Version;
            Console.WriteLine("goal v" + v.ToString());
        }
        static void Main(string[] args)
        {
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
            }
        }
    }
}
