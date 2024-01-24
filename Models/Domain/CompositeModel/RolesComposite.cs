using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JAS.Models.Domain;

namespace JAS.Models.Domain.CompositeModel
{
    public class RolesComposite
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesComposite(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IdentityRole? Role { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
