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
using POI.service.Services;

namespace POI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesTypeController : ControllerBase
    {
        private readonly ILogger<DesTypeController> _logger;
        private readonly IDesTypeService _desTypeService;

        public DesTypeController(IDesTypeService desTypeService, ILogger<DesTypeController> logger)
        {
            _logger = logger;
            _desTypeService = desTypeService;
        }


        /// <summary>
        /// Get all destination types
        /// </summary>
        /// <remarks>
        /// Get all destination types in POI system
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
            _logger.LogInformation("All desType is queried");
            return Ok(_desTypeService.GetAll());
        }


        /// <summary>
        /// Get destination type by ID
        /// </summary>
        /// <remarks>
        /// Get destination type in POI system with ID
        /// 
        ///    ID : ID of destination type 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            DestinationType desType = _desTypeService.GetByID(id);
            if (desType == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(desType);
            }
        }

        /// <summary>
        /// Create new destination type (Post method)
        /// </summary>
        /// <remarks>
        /// Create new destination type 
        /// </remarks>

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(CreateDesTypeViewModel desTypeViewModel)
        {
            _logger.LogInformation("Post request is called");
            CreateEnum resultCode = await _desTypeService.CreateNewDesType(desTypeViewModel);
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
        /// Update destination type information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your destination type with name and short name  
        /// </remarks>
        [HttpPut]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Put(UpdateDesTypeViewModel desTypeViewModel)
        {
            _logger.LogInformation("Put request is called");
            UpdateEnum resultCode = _desTypeService.UpdateDesType(desTypeViewModel);
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
        /// Deactivate an destination type (Delete method)
        /// </summary>
        /// <remarks>
        /// Deactivate destination type by this id   
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesDefaultResponseType]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            DeleteEnum resultCode = _desTypeService.DeactivateDesType(id);
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
