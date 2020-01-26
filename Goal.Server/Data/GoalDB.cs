using System;
using System.IO;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Goal.Server.Data
{
    public class GoalEntry
    {
        public int Id { get; set; }
        
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
        public string Description { get; set; } = "";

        public override string ToString()
        {
            return string.Format("{0}\t{1}", this.Id, this.Description);
        }
    }

    public class GoalDB
    {
        private readonly IConfiguration configuration; 
        public GoalDB(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private LiteDatabase OpenDB()
        {
            var path = this.configuration.GetValue<string>("GOALDB_CONNECTIONSTRING");
            var info = new FileInfo(path);
            if (!Directory.Exists(info.DirectoryName))
                Directory.CreateDirectory(info.DirectoryName);
            return new LiteDatabase(info.FullName);
        }

        public List<GoalEntry> GetEntries()
        {
            var list = new List<GoalEntry>();
            using (var db = OpenDB())
            {
                var col = db.GetCollection<GoalEntry>("goals");
                foreach (var g in col.FindAll().Reverse())
                {
                   list.Add(g);
                }
            }

            return list;
        }

        public void Delete(int id)
        {
            using (var db = OpenDB())
            {
                var col = db.GetCollection<GoalEntry>("goals");
                col.Delete(id);
            }
        }

        public int Insert(GoalEntry g)
        {
            using (var db = OpenDB())
            {
                var col = db.GetCollection<GoalEntry>("goals");
               
                return col.Insert(g);
            }
        }
    }
}