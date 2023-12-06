using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MainStAds.Core.Entities;
using MainStAds.Core.Interfaces;
using AutoMapper;
using MainStAds.Application.DTOs;
using System.IO;
using System.Diagnostics;

namespace MainStAds.Controllers
{
    public class BusinessesController : Controller
    {
        private readonly IBusinessRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<BusinessesController> _logger;

        public BusinessesController(IBusinessRepository repository, IMapper mapper, ILogger<BusinessesController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var businesses = await _repository.GetAllAsync();
                var businessDtos = _mapper.Map<IEnumerable<BusinessDto>>(businesses);
                return View(businessDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching businesses in Index.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var business = await _repository.GetByIdAsync(id.Value);
                if (business == null)
                {
                    return NotFound();
                }

                var businessDto = _mapper.Map<BusinessDto>(business);
                return View(businessDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching details of business with ID: {BusinessId}", id);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessDto businessDto)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    businessDto.ImageType = Path.GetExtension(file.FileName).Substring(1).ToUpper();

                    using var dataStream = new MemoryStream();
                    await file.CopyToAsync(dataStream);
                    businessDto.ImageData = dataStream.ToArray();
                }

                if (ModelState.IsValid)
                {
                    var business = _mapper.Map<Business>(businessDto);
                    await _repository.AddAsync(business);
                    return RedirectToAction(nameof(Index));
                }
                return View(businessDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new business.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var business = await _repository.GetByIdAsync(id.Value);
                if (business == null)
                {
                    return NotFound();
                }

                var businessDto = _mapper.Map<BusinessDto>(business);
                return View(businessDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching business for editing with ID: {BusinessId}", id);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BusinessDto businessDto)
        {
            try
            {
                if (id != businessDto.Id)
                {
                    return NotFound();
                }

                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    businessDto.ImageType = Path.GetExtension(file.FileName).Substring(1).ToUpper();

                    using var dataStream = new MemoryStream();
                    await file.CopyToAsync(dataStream);
                    businessDto.ImageData = dataStream.ToArray();
                }
                else
                {
                    var existingBusiness = await _repository.GetByIdAsync(id);
                    businessDto.ImageData = existingBusiness.ImageData;
                    businessDto.ImageType = existingBusiness.ImageType;
                }

                if (ModelState.IsValid)
                {
                    var business = _mapper.Map<Business>(businessDto);
                    await _repository.UpdateAsync(business);
                    return RedirectToAction(nameof(Index));
                }
                return View(businessDto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await _repository.BusinessExistsAsync(businessDto.Id))
                {
                    return NotFound();
                }
                _logger.LogError(ex, "Concurrency error while editing business with ID: {BusinessId}.", businessDto.Id);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing business with ID: {BusinessId}.", businessDto.Id);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var business = await _repository.GetByIdAsync(id.Value);
                if (business == null)
                {
                    return NotFound();
                }

                var businessDto = _mapper.Map<BusinessDto>(business);
                return View(businessDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching business for deletion with ID: {BusinessId}.", id);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting business with ID: {BusinessId}.", id);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
