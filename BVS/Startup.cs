using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BVS.Models;

[assembly: OwinStartupAttribute( typeof( BVS.Startup ) )]

namespace BVS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth( app );
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>( new RoleStore<IdentityRole>( context ) );
            var UserManager = new UserManager<ApplicationUser>( new UserStore<ApplicationUser>( context ) );

            // In Startup iam creating first Admin Role and creating a default Admin User
            if ( !roleManager.RoleExists( "Admin" ) )
            {
                // first we create Admin rool
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create( role );

                //Here we create a Admin super user who will maintain the website

                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "peteslavich@gmail.com";

                string userPWD = "PDS1973*";

                var chkUser = UserManager.Create( user, userPWD );

                //Add default User to Role Admin
                if ( chkUser.Succeeded )
                {
                    var result1 = UserManager.AddToRole( user.Id, "Admin" );
                }
            }

            // creating Creating Manager role
            if ( !roleManager.RoleExists( "Doctor" ) )
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Doctor";
                roleManager.Create( role );
            }

            // creating Creating Employee role
            if ( !roleManager.RoleExists( "Patient" ) )
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Patient";
                roleManager.Create( role );
            }
        }
    }
}