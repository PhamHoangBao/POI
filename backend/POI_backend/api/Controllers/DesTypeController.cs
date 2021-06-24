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
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;


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
        /// Get all destination types in POI system (Admin)
        /// 
        ///     No parameter
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The destination types is retrieved", typeof(IEnumerable<DestinationType>))]
        [SwaggerResponse(404, "No destination type is found")]
        public IActionResult Get()
        {
            _logger.LogInformation("All desType is queried");
            return Ok(_desTypeService.GetAll());
        }


        /// <summary>
        /// Get destination type by ID
        /// </summary>
        /// <remarks>
        /// Get destination type in POI system with ID (Admin)
        /// 
        ///    ID : ID of destination type 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The destination is retrieved", typeof(Destination))]
        [SwaggerResponse(404, "The destination is not found")]
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
        /// Create new destination type  (Admin)
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(201, "Create dest type successfully")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(400, "The dest type is not created")]
        [SwaggerResponse(409, "The dest type id is conflicted")]
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
        /// Update your destination type with name and short name   (Admin)
        /// </remarks>
        [HttpPut]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "Update successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
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
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Delete successfully")]
        [SwaggerResponse(400, "ID is not allowed to delete")]
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
