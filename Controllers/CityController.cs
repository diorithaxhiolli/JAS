using JAS.Areas.Identity.Data;
using JAS.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace Travista.Controllers
{
    public class CityController : Controller
    {
        private readonly JASContext _dBContext;

        public CityController(JASContext _dBContext)
        {
            this._dBContext = _dBContext;
        }

        public async Task<IActionResult> Index(int ID_Country)
        {
            // Retrieve the cities for the specified country
            var cities = _dBContext.City.Where(c => c.countryId == ID_Country).ToList();

            // Pass the list of cities to the view
            return View(cities);
        }

        public IActionResult AddCity()
        {
            return View();
        }

        // get create city view
        [HttpGet]
        public async Task<IActionResult> AddCity(int ID_Country)
        {
            var viewModel = new City
            {
                countryId = ID_Country
            };

            return View(viewModel);
        }

        // create city
        [HttpPost]
        public async Task<IActionResult> AddCityPost(City addUserRequest)
        {

            var city = new City()
            {
                cityId = 0,
                cityName = addUserRequest.cityName,
                countryId = addUserRequest.countryId,
            };
            await _dBContext.City.AddAsync(city);
            await _dBContext.SaveChangesAsync();
            return Redirect(Url.Action("Index", new { ID_Country = addUserRequest.countryId }));
        }

        // view city
        [HttpGet]
        public async Task<IActionResult> ViewCity(int ID_City)
        {
            var city = await _dBContext.City.FirstOrDefaultAsync(x => x.cityId == ID_City);

            if (city != null)
            {
                var viewModel = new City()
                {
                    cityId = city.cityId,
                    cityName = city.cityName,
                    countryId = city.countryId,
                };

                return View("ViewCity", viewModel);
            }
            return RedirectToAction("Index");
        }


        // update city
        [HttpPost]
        public async Task<IActionResult> ViewCityPost(City model)
        {
            var city = await _dBContext.City.FindAsync(model.cityId);


            if (city != null)
            {
                city.cityId = model.cityId;
                city.cityName = model.cityName;
                city.countryId = model.countryId;

                await _dBContext.SaveChangesAsync();

                return RedirectToAction("Index", new { ID_Country = model.countryId });
            }

            return RedirectToAction("Index");
        }

        // delete city
        public async Task<IActionResult> DeleteCity(City model)
        {
            var city = await _dBContext.City.FindAsync(model.cityId);

            if (city != null)
            {
                _dBContext.City.Remove(city);
                await _dBContext.SaveChangesAsync();

        return RedirectToAction("Index", new { ID_Country = model.countryId });
            }

            return Redirect(Url.Action("Index", new { ID_Country = model.countryId }));
        }
    }
}