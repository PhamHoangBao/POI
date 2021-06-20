using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using POI.repository.Entities;
using POI.service.Services;
using POI.repository.AutoMapper;
using POI.repository.ViewModels;
using POI.repository.ResultEnums;

namespace POI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly ILogger<ProvinceController> _logger;
        private readonly IProvinceService _provinceService;

        public ProvinceController(IProvinceService provinceService, ILogger<ProvinceController> logger)
        {
            _logger = logger;
            _provinceService = provinceService;
        }

        /// <summary>
        /// Get all provinces
        /// </summary>
        /// <remarks>
        /// Get all provinces in POI system
        /// 
        ///     No parameter
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            _logger.LogInformation("All provinces is queried");
            return Ok(_provinceService.GetAll());
        }

        /// <summary>
        /// Get province by ID
        /// </summary>
        /// <remarks>
        /// Get province in POI system with ID
        /// 
        ///    ID : ID of province 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            Province province = _provinceService.GetByID(id);
            if (province == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(province);
            }
        }


        /// <summary>
        /// Create new province (Post method)
        /// </summary>
        /// <remarks>
        /// Create new province 
        /// </remarks>

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(CreateProvinceViewModel provinceViewModel)
        {
            _logger.LogInformation("Post request is called");
            CreateEnum resultCode = await _provinceService.CreateNewProvince(provinceViewModel);
            if (resultCode == CreateEnum.Success)
            {
                return CreatedAtAction("Get", null);
            }
            else if (resultCode == CreateEnum.ErrorInServer)
            {
                return StatusCode(500);
            }
            else
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Update province information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your province with name
        /// </remarks>
        [HttpPut]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Put(UpdateProvinceViewModel provinceViewModel)
        {
            _logger.LogInformation("Put request is called");
            UpdateEnum resultCode = _provinceService.UpdateProvince(provinceViewModel);
            if (resultCode == UpdateEnum.Success)
            {
                return Ok();
            }
            else if (resultCode == UpdateEnum.ErrorInServer)
            {
                return StatusCode(500);
            }
            else
            {
                return StatusCode(405);
            }
        }

        /// <summary>
        /// Deactivate an province (Delete method)
        /// </summary>
        /// <remarks>
        /// Deactivate province by this id   
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesDefaultResponseType]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            DeleteEnum resultCode = _provinceService.DeactivateProvince(id);
            if (resultCode == DeleteEnum.Success)
            {
                return Ok();
            }
            else if (resultCode == DeleteEnum.ErrorInServer)
            {
                return StatusCode(500);
            }
            else
            {
                return StatusCode(405);
            }
        }

    }
}
