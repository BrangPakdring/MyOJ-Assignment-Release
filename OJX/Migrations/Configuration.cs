
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace OJX.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<OJX.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    } 
}