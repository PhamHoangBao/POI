using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using POI.repository.Entities;
using POI.service.IServices;
using POI.repository.AutoMapper;
using POI.repository.ViewModels;
using POI.repository.ResultEnums;

namespace POI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoiTypeController : ControllerBase
    {
        private readonly ILogger<PoiTypeController> _logger;
        private readonly IPoiTypeService _poiTypeService;

        public PoiTypeController(IPoiTypeService poiTypeService, ILogger<PoiTypeController> logger)
        {
            _logger = logger;
            _poiTypeService = poiTypeService;
        }

        /// <summary>
        /// Get all poiTypes
        /// </summary>
        /// <remarks>
        /// Get all poiTypes in POI system
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
            _logger.LogInformation("All POIs are queried");
            return Ok(_poiTypeService.GetAll());
        }

        /// <summary>
        /// Get poiType by ID
        /// </summary>
        /// <remarks>
        /// Get poiType in POI system with ID
        /// 
        ///    ID : ID of poiType 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            Poitype poiType = _poiTypeService.GetByID(id);
            if (poiType == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(poiType);
            }
        }


        /// <summary>
        /// Create new poiType (Post method)
        /// </summary>
        /// <remarks>
        /// Create new poiType 
        /// </remarks>

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(CreatePoiTypeViewModel poiTypeViewModel)
        {
            _logger.LogInformation("Post request is called");
            CreateEnum resultCode = await _poiTypeService.CreateNewPoiType(poiTypeViewModel);
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
        /// Update poiType information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your poiType with name
        /// </remarks>
        [HttpPut]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Put(UpdatePoiTypeViewModel poiTypeViewModel)
        {
            _logger.LogInformation("Put request is called");
            UpdateEnum resultCode = _poiTypeService.UpdatePoiType(poiTypeViewModel);
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
        /// Deactivate a poiType (Delete method)
        /// </summary>
        /// <remarks>
        /// Deactivate poiType by this id   
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesDefaultResponseType]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            DeleteEnum resultCode = _poiTypeService.DeactivatePoiType(id);
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
