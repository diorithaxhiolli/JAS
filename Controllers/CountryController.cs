﻿using JAS.Areas.Identity.Data;
using JAS.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JAS.Controllers
{
    public class CountryController : Controller
    {
        private readonly JASContext jasContext;

        public CountryController(JASContext jasContext)
        {
            this.jasContext = jasContext;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var country = await jasContext.Country.ToListAsync();
            return View(country);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AddCountry()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddCountry(Country addUserRequest)
        {
            var newCountry = new Country()
            {
                countryId = 0,
                countryName = addUserRequest.countryName,
                language = addUserRequest.language
            };

            await jasContext.Country.AddAsync(newCountry);
            await jasContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ViewCountry(int ID_Country)
        {
            var country = await jasContext.Country.FirstOrDefaultAsync(x => x.countryId == ID_Country);

            if (country != null)
            {
                return View("ViewCountry", country);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ViewCountryPost(Country model)
        {
            var country = await jasContext.Country.FindAsync(model.countryId);


            if (country != null)
            {
                country.countryId = model.countryId;
                country.countryName = model.countryName;
                country.language = model.language;

                await jasContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }



        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCountry(Country model)
        {
            var country = await jasContext.Country.FindAsync(model.countryId);

            if (country != null)
            {
                jasContext.Country.Remove(country);
                await jasContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}