using System;
using System.IO;
using LiteDB;
using Goal.Common;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Goal.Server.Data
{
    public class GoalDB
    {
        private readonly IConfiguration configuration; 
        public GoalDB(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private LiteDatabase OpenDB()
        {
            var path = this.configuration.GetValue<string>("GoalDB:ConnectionString");
            var info = new FileInfo(path);
            if (!Directory.Exists(info.DirectoryName))
                Directory.CreateDirectory(info.DirectoryName);
            return new LiteDatabase(info.FullName);
        }

        public List<GoalEntry> ListEntries()
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
    }
}