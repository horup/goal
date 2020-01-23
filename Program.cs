using System;
using System.Linq;
using System.Text;
using LiteDB;

namespace goal
{
    class GoalEntry
    {
        public int Id{get;set;}
        public DateTimeOffset Timestamp {get;set;} = DateTimeOffset.Now;
        public string Description {get;set;} = "";

        public override string ToString()
        {
            return string.Format("{0}\t{1}", this.Id, this.Description);
        }
    }

    class Program
    {
        static LiteDatabase OpenDB()
        {
            return new LiteDatabase(@"goal.db");
        }
        static void Add(string s)
        {
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
                    case "list":
                    {
                        List(args.TakeLast(args.Length-1).ToArray());
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
 