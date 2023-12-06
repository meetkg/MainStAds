using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MainStAds.Core.Entities;
using MainStAds.Core.Interfaces;
using AutoMapper;
using MainStAds.Application.DTOs;

namespace MainStAds.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBusinessRepository _businessRepository;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IBusinessRepository businessRepository, IMapper mapper)
        {
            _logger = logger;
            _businessRepository = businessRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetBusinesses()
        {
            try
            {
                var businesses = await _businessRepository.GetAllAsync();
                var businessesDto = _mapper.Map<IEnumerable<BusinessDto>>(businesses);

                var businessesViewModel = businessesDto
                    .Select(b => new
                    {
                        b.Id,
                        b.Name,
                        b.Category,
                        b.Description,
                        b.Address,
                        b.Link,
                        ImageType = b.ImageType,
                        ImageData = (b.ImageData != null) ? Convert.ToBase64String(b.ImageData) : null,
                        Latitude = b.Latitude,
                        Longitude = b.Longitude,
                    })
                    .ToList();

                return Json(businessesViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching businesses.");
                return Json(new { error = "An error occurred while fetching businesses. Please try again later." });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult StaticError()
        {
            // Serve the static HTML error page
            return File("~/StaticError.html", "text/html");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                var errorViewModelDto = new ErrorViewModelDto { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
                return View(_mapper.Map<ErrorViewModel>(errorViewModelDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the error request. RequestId: {RequestId}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
                return RedirectToAction("StaticError", "Home");
            }
        }
    }
}
