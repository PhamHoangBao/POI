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
    public class PoiController : ControllerBase
    {
        private readonly ILogger<PoiController> _logger;
        private readonly IPoiService _poiService;

        public PoiController(IPoiService poiService, ILogger<PoiController> logger)
        {
            _logger = logger;
            _poiService = poiService;
        }

        /// <summary>
        /// Get all pois
        /// </summary>
        /// <remarks>
        /// Get all pois in POI system
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
            return Ok(_poiService.GetAll());
        }

        /// <summary>
        /// Get poi by ID
        /// </summary>
        /// <remarks>
        /// Get poi in POI system with ID
        /// 
        ///    ID : ID of poi 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            Poi poi = _poiService.GetByID(id);
            if (poi == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(poi);
            }
        }


        /// <summary>
        /// Create new poi (Post method)
        /// </summary>
        /// <remarks>
        /// Create new poi 
        /// </remarks>

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(CreatePoiViewModel poiViewModel)
        {
            _logger.LogInformation("Post request is called");
            CreateEnum resultCode = await _poiService.CreateNewPoi(poiViewModel);
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
        /// Update poi information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your poi with name
        /// </remarks>
        [HttpPut]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Put(UpdatePoiViewModel poiViewModel)
        {
            _logger.LogInformation("Put request is called");
            UpdateEnum resultCode = _poiService.UpdatePoi(poiViewModel);
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
        /// Deactivate a poi (Delete method)
        /// </summary>
        /// <remarks>
        /// Deactivate poi by this id   
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesDefaultResponseType]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            DeleteEnum resultCode = _poiService.DeactivatePoi(id);
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
