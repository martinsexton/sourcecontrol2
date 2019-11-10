using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace doneillspa.Data
{
    public class SeedData
    {
        private const string _adminRoleName = "Administrator";
        private const string _supervisorRoleName = "Supervisor";
        private const string _chargeHandRoleName = "ChargeHand";
        private const string _electR1RoleName = "ElectR1";
        private const string _loc1RoleName = "Loc1";
        private const string _electR2RoleName = "ElectR2";
        private const string _loc2RoleName = "Loc2";
        private const string _electR3RoleName = "ElectR3";
        private const string _loc3RoleName = "Loc3";
        private const string _tempRoleName = "Temp";
        private const string _firstYearApprenticeRoleName = "First Year Apprentice";
        private const string _secondYearApprenticeRoleName = "Second Year Apprentice";
        private const string _thirdYearApprenticeRoleName = "Third Year Apprentice";
        private const string _fourthYearApprenticeRoleName = "Fourth Year Apprentice";


        private string _adminEmail = "admin@doneill.local";
        private string _adminPassword = "Hellofido!1234";

        private string[] _defaultRoles = new string[] { _adminRoleName, _supervisorRoleName, _chargeHandRoleName,
            _electR1RoleName, _electR2RoleName, _electR3RoleName, _tempRoleName, _firstYearApprenticeRoleName,
            _secondYearApprenticeRoleName, _thirdYearApprenticeRoleName, _fourthYearApprenticeRoleName,  _loc1RoleName, _loc2RoleName, _loc3RoleName };

        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public static async Task Run(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //var instance = serviceScope.ServiceProvider.GetService<SeedData>();
                //await instance.Initialize();
                SeedData datasetup = new SeedData(serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>(), serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole<Guid>>>());
                await datasetup.Initialize();
            }
        }

        public SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task Initialize()
        {
            await EnsureRoles();
            //await EnsureDefaultUser();
        }

        protected async Task EnsureRoles()
        {
            foreach (var role in _defaultRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }

        protected async Task EnsureDefaultUser()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync(_adminRoleName);

            if (!adminUsers.Any())
            {
                var adminUser = new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    Email = _adminEmail,
                    UserName = _adminEmail
                };

                var result = await _userManager.CreateAsync(adminUser, _adminPassword);
                await _userManager.AddToRoleAsync(adminUser, _adminRoleName);
            }
        }
    }
}
