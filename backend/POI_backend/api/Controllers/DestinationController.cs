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
using POI.repository.Enums;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace POI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly ILogger<DestinationController> _logger;
        private readonly IDestinationService _destinationService;
        private readonly IDesHashtagService _desHashtagService;

        public DestinationController(IDestinationService destinationService,
            ILogger<DestinationController> logger,
            IDesHashtagService desHashtagService)
        {
            _logger = logger;
            _destinationService = destinationService;
            _desHashtagService = desHashtagService;
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
        [Authorize(Roles = "Admin, User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The destination is retrieved", typeof(ResponseDestinationViewModel))]
        [SwaggerResponse(404, "The destination is not found")]
        public IActionResult Get(Guid id)
        {
            ResponseDestinationViewModel destination = _destinationService.GetDestination(m => m.DestinationId.Equals(id), false).First();
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
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Create successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
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
                return BadRequest();
            }
        }


        /// <summary>
        /// Update destination information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your destination with name and short name  
        /// </remarks>
        [HttpPut]
        [Authorize(Roles = "Admin , Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "Update successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
        public IActionResult Put(UpdateDestinationViewModel destinationViewModel)
        {
            _logger.LogInformation("Put request is called");
            UpdateEnum resultCode = _destinationService.UpdateDestination(destinationViewModel);
            if (resultCode == UpdateEnum.Success)
            {
                return Get(destinationViewModel.DestinationId);
            }
            else if (resultCode == UpdateEnum.ErrorInServer)
            {
                return StatusCode(500);
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Deactivate an destination (Delete method)
        /// </summary>
        /// <remarks>
        /// Deactivate destination by this id   
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Delete successfully")]
        [SwaggerResponse(400, "ID is not allowed to delete")]
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
                return BadRequest();
            }
        }

        /// <summary>
        /// Assign hashtag to destination
        /// </summary>
        /// <remarks>
        /// Assign hashtag to destination  
        /// </remarks>
        [HttpPost("{destinationID}/hashtag")]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "The destination was updated with new hashtag", typeof(ResponseDestinationViewModel))]
        [SwaggerResponse(400, "The destination or hashtag data is invalid")]
        public async Task<IActionResult> AssignHashtagToDestination(Guid destinationID, Guid hashtagID)
        {
            CreateEnum resultCode = await _desHashtagService.CreateNewDesHashtag(destinationID, hashtagID);
            switch (resultCode)
            {
                case CreateEnum.Success:
                    return Get(destinationID);
                case CreateEnum.Duplicate:
                    return BadRequest("This destination and hashtag is already assigned");
                case CreateEnum.Error:
                    return BadRequest("Destination or hashtag is not existed or disable");
                case CreateEnum.ErrorInServer:
                    return StatusCode(500);
                default:
                    return StatusCode(500);
            }
        }

        /// <summary>
        /// Get Destination within Province and have hashtag
        /// </summary>
        /// <remarks>
        /// Get Destination within Province and have hashtag. This api can search both option or one only
        /// Sample request:
        ///
        ///     GET /user/find
        ///     {
        ///        "provinceId": "",
        ///        "hashtagId": "",
        ///     }
        ///
        /// </remarks>
        /// <returns>Return list of destinations that satisfy conditions</returns>
        /// <response code="200">Returns list of destinations that satisfy condition</response>
        /// <response code="404">If the list is empty</response> 
        [HttpGet]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin, User, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The list of destination is retrieved", typeof(List<ResponseDestinationViewModel>))]
        [SwaggerResponse(404, "The list of destination is not found")]
        public IActionResult Get(Guid? provinceID, Guid? hashtagId)
        {
            List<ResponseDestinationViewModel> result = null;
            if (provinceID != null && hashtagId != null)
            {
                result = _destinationService.GetDestination(m =>
                          m.ProvinceId.Equals(provinceID) && m.DesHashtags.Where(d => d.HashtagId.Equals(hashtagId)).Any()
                          , false);
            }
            else if (provinceID != null && hashtagId == null)
            {
                result = _destinationService.GetDestination(m => m.ProvinceId.Equals(provinceID), false);
            }
            else if (provinceID == null && hashtagId != null)
            {
                result = _destinationService.GetDestination(m => m.DesHashtags.Where(d => d.HashtagId.Equals(hashtagId)).Any(), false);
            }
            else
            {
                result = _destinationService.GetDestination(m => true, false);
            }
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
