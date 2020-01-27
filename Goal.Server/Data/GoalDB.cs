using System;
using System.IO;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Goal.Server.Data
{
    public class Goal
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

        public List<Goal> GetEntries()
        {
            var list = new List<Goal>();
            using (var db = OpenDB())
            {
                var col = db.GetCollection<Goal>("goals");
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
                var col = db.GetCollection<Goal>("goals");
                col.Delete(id);
            }
        }

        public int Insert(Goal g)
        {
            using (var db = OpenDB())
            {
                var col = db.GetCollection<Goal>("goals");
               
                return col.Insert(g);
            }
        }
    }
}