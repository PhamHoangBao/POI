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
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

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
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The pois is retrieved", typeof(List<ResponsePoiViewModel>))]
        [SwaggerResponse(404, "The pois is not found")]
        public IActionResult Get()
        {
            _logger.LogInformation("All POIs are queried");
            return Ok(_poiService.GetPoi(m => true, false));
        }

        /// <summary>
        /// Get all pois which belong to a Destination
        /// </summary>
        /// <remarks>
        /// Get all pois which belong to a Destination
        /// 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("destination/{destinationId}")]
        [Authorize(Roles = "User, Admin , Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The poi is retrieved", typeof(List<ResponsePoiViewModel>))]
        [SwaggerResponse(404, "The poi is not found")]
        public IActionResult GetWithinDestination(Guid destinationId)
        {
            return Ok(_poiService.GetPoi(m => m.DestinationId.Equals(destinationId), false));
        }

        /// <summary>
        /// Get poi from its user 
        /// </summary>
        /// <remarks>
        /// Get all pois which belong to a Destination
        /// 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("post-by-user")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The poi is retrieved", typeof(List<ResponsePoiViewModel>))]
        [SwaggerResponse(404, "The poi is not found")]
        public IActionResult GetPOIOfUser()
        {
            User currentUser = (User)HttpContext.Items["User"];
            return Ok(_poiService.GetPoi(m => m.UserId.Equals(currentUser.UserId), false));
        }

        /// <summary>
        /// Get poi from its user 
        /// </summary>
        /// <remarks>
        /// Get all pois which belong to a Destination
        /// 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("post-by-user/{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The poi is retrieved", typeof(List<ResponsePoiViewModel>))]
        [SwaggerResponse(404, "The poi is not found")]
        public IActionResult GetPOIOfUserBySystem(Guid id)
        {
            return Ok(_poiService.GetPoi(m => m.UserId.Equals(id), false));
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
        [Authorize(Roles = "Admin, Moderator, User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The poi is retrieved", typeof(ResponsePoiViewModel))]
        [SwaggerResponse(404, "The poi is not found")]
        public IActionResult Get(Guid id)
        {
            ResponsePoiViewModel poi = _poiService.GetPoi(m => m.PoiId.Equals(id), false).First();
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
        [Authorize(Roles = "Admin, Moderator, User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Create successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
        public async Task<IActionResult> Post(CreatePoiViewModel poiViewModel)
        {
            User currentUser = (User)HttpContext.Items["User"];
            Guid userID = Guid.Empty;
            if (currentUser.Role.RoleName.Equals("User"))
            {
                userID = currentUser.UserId;
            }
            _logger.LogInformation("Post request is called");
            CreateEnum resultCode = await _poiService.CreateNewPoi(poiViewModel, userID);
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
        [Authorize(Roles = "Admin , Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "Update successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
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
                return BadRequest("No POI found");
            }
        }

        /// <summary>
        /// Deactivate a poi (Delete method)
        /// </summary>
        /// <remarks>
        /// Deactivate poi by this id   
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Delete successfully")]
        [SwaggerResponse(400, "ID is not allowed to delete")]
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

        /// <summary>
        /// Approve a new POI from user
        /// </summary>
        /// <remarks>
        /// Approve a new POI from user
        /// 
        /// </remarks>
        /// <returns></returns>
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The pois is retrieved", typeof(List<ResponsePoiViewModel>))]
        [SwaggerResponse(404, "The pois is not found")]
        public IActionResult ApprovePOI(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            UpdateEnum resultCode = _poiService.ApprovePOI(id);
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
                return BadRequest("No POI found ");
            }
        }
    }
}
