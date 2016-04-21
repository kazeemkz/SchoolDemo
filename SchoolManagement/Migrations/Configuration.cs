//namespace eVoting.Migrations
//{
//    using SchoolManagement.DAL;
//    using System;
//    using System.Data.Entity;
//    using System.Data.Entity.Migrations;
//    using System.Linq;
//    using System.Web.Security;
//    using WebMatrix.WebData;

//    internal sealed class Configuration : DbMigrationsConfiguration<smContext>
//    {
//        public Configuration()
//        {
//            AutomaticMigrationsEnabled = true;
//            this.AutomaticMigrationDataLossAllowed = true;
//        }

//        protected override void Seed(smContext context)
//        {
//            //WebSecurity.InitializeDatabaseConnection("eBlogDatabase", "UserProfile", "UserId", "UserName", autoCreateTables: true);
//            ////  WebSecurity.InitializeDatabaseConnection(connectionStringName: "evotingDatabase", userTableName: "MySchema.User", userIdColumn: "ID", userNameColumn: "Username", autoCreateTables: true);
//            ////WebSecurity.InitializeDatabaseConnection(
//            ////   "evotingDatabase",
//            ////   "UserProfile",
//            ////   "UserId",
//            ////   "UserName", autoCreateTables: true);

//            //if (!Roles.RoleExists("SuperAdmin"))
//            //    Roles.CreateRole("SuperAdmin");

//            //if (!WebSecurity.UserExists("kazeem"))
//            //    WebSecurity.CreateUserAndAccount(
//            //        "kazeem",
//            //        "P@ssw0rd");

//            //if (!Roles.GetRolesForUser("kazeem").Contains("SuperAdmin"))
//            //    Roles.AddUsersToRoles(new[] { "kazeem" }, new[] { "SuperAdmin" });
//            ////if (!Roles.RoleExists("SuperAdmin"))
//            ////{
//            //    Roles.CreateRole("SuperAdmin");
//            //}



//            //if (Membership.GetUser("kazeemkz") == null)
//            //{
//            //    Membership.CreateUser("kazeem", "P@ssw0rd", "kazeemkz@yahoo.com");
//            //    Roles.AddUserToRole("kazeemkz", "SuperAdmin");
//            //}




//            //if (Membership.GetUser("kazeemkz") == null)
//            //{
//            //    WebSecurity.CreateUserAndAccount("kazeem", "P@ssw0rd");
//            //  //  Membership.CreateUser("kazeem", "P@ssw0rd", "kazeemkz@yahoo.com");
//            //    Roles.AddUserToRole("kazeem", "SuperAdmin");
//            //}

//            //  This method will be called after migrating to the latest version.

//            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
//            //  to avoid creating duplicate seed data. E.g.
//            //
//            //    context.People.AddOrUpdate(
//            //      p => p.FullName,
//            //      new Person { FullName = "Andrew Peters" },
//            //      new Person { FullName = "Brice Lambson" },
//            //      new Person { FullName = "Rowan Miller" }
//            //    );
//            //
//        }
//    }
//}
