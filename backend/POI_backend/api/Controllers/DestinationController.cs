using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POI.repository.Entities;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.service.IServices;

namespace POI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly ILogger<DestinationController> _logger;
        private readonly IDestinationService _destinationService;

        public DestinationController(IDestinationService destinationService, ILogger<DestinationController> logger)
        {
            _logger = logger;
            _destinationService = destinationService;
        }


        /// <summary>
        /// Get all destinations
        /// </summary>
        /// <remarks>
        /// Get all destinations in POI system
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
            _logger.LogInformation("All destination is queried");
            return Ok(_destinationService.GetAll());
        }


        /// <summary>
        /// Get destination by ID
        /// </summary>
        /// <remarks>
        /// Get destination in POI system with ID
        /// 
        ///    ID : ID of destination 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            Destination destination = _destinationService.GetByID(id);
            if (destination == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(destination);
            }
        }

        /// <summary>
        /// Create new destination (Post method)
        /// </summary>
        /// <remarks>
        /// Create new destination 
        /// </remarks>

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(CreateDestinationViewModel destinationViewModel)
        {
            _logger.LogInformation("Post request is called");
            CreateEnum resultCode = await _destinationService.CreateNewDestination(destinationViewModel);
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
        /// Update destination information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your destination with name and short name  
        /// </remarks>
        [HttpPut]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Put(UpdateDestinationViewModel destinationViewModel)
        {
            _logger.LogInformation("Put request is called");
            UpdateEnum resultCode = _destinationService.UpdateDestination(destinationViewModel);
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
        /// Deactivate an destination (Delete method)
        /// </summary>
        /// <remarks>
        /// Deactivate destination by this id   
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesDefaultResponseType]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            DeleteEnum resultCode = _destinationService.DeactivateDestination(id);
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
