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
        public string Owner {get;set;}
        
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

        public List<Goal> GetEntries(string asOwner)
        {
            var list = new List<Goal>();
            using (var db = OpenDB())
            {
                var col = db.GetCollection<Goal>("goals");
                foreach (var g in col.FindAll().Reverse())
                {
                    if ( g.Owner == asOwner)
                        list.Add(g);
                }
            }

            return list;
        }

        public void Delete(int id, string asOwner)
        {
            using (var db = OpenDB())
            {
                var col = db.GetCollection<Goal>("goals");
                if (col.FindById(id).Owner == asOwner)
                    col.Delete(id);
                else
                    throw new Exception("Access denied"); 
            }
        }

        public int Insert(Goal g, string asOwner)
        {
            if (g.Owner != asOwner)
                throw new Exception("Access denied");
            using (var db = OpenDB())
            {
                var col = db.GetCollection<Goal>("goals");
               
                return col.Insert(g);
            }
        }
    }
}