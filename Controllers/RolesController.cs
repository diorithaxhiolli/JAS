using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JAS.Models.Domain;
using JAS.Models.Domain.CompositeModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using JAS.Areas.Identity.Data;

namespace JAS.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JASContext _dBContext;

        public RolesController(RoleManager<IdentityRole> roleManager, JASContext _dBContext)
        {
            _roleManager = roleManager;
            this._dBContext = _dBContext;
        }
        public IActionResult Index()
        {
            RolesComposite rolesComposite = new RolesComposite(_roleManager)
            {
                Roles = _roleManager.Roles,
                Role = null
            };
            return View(rolesComposite);
        }


        [HttpPost]
        public async Task<IActionResult> AddRoles(IdentityRole model)
        {
            if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewRoles(string rolesId)
        {
            var role = await _roleManager.FindByIdAsync(rolesId);

            if (role != null)
            {
                var viewModel = new IdentityRole()
                {
                    Id = role.Id,
                    Name = role.Name,
                };

                return View("ViewRole", role);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ViewRoleOnPost(IdentityRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role != null)
            {
                role.Name = model.Name;

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteRole(IdentityRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role != null)
            {
                await _roleManager.DeleteAsync(role);

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}