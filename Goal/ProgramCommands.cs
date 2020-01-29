using System;
using System.Reflection;

namespace GoalCmd
{
    partial class Program
    {
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

        static void ParseVersion()
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine($"goal {v.ToString()}");
        }

        static void ParseDelete(string[] args)
        {
            var api = CreateAPI();
            var id = int.Parse(Readline("id"));
            api.Api1GoalsDelete(id);
        }
    }
}