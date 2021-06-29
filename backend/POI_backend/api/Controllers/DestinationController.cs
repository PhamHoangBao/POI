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
        /// Authorize : User, Admin , Moderator
        /// Sample request:
        ///
        ///     GET /destination
        ///     {
        ///        "id": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///     }
        ///
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User, Moderator")]
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
        /// Create new destination 
        /// </summary>
        /// <remarks>
        /// Create new destination 
        /// 
        /// Authorize : Admin , Moderator
        /// 
        /// Sample request:
        ///
        ///     POST /destination
        ///     {
        ///         "destinationName": "Vườn dâu tây Đà Lạt",
        ///         "location": {
        ///              "latitude": 0,
        ///              "longtitude": 0
        ///         },
        ///         "provinceId": "a589b039-a021-435a-8328-71f824ce9a30",
        ///         "imageUrl": "string of Firebase image url",
        ///         "destinationTypeId": "3be4e631-8c25-4eff-af3b-d3ad882e362a"
        ///     }
        ///
        ///  
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
        /// Update destination information 
        /// </summary>
        /// <remarks>
        /// Update your destination with new location, image url, province or destination type.
        /// 
        /// Authorize : Admin , Moderator
        /// 
        /// Sample request:
        ///
        ///     PUT /destination
        ///     {
        ///         "destinationId" : "2298e97d-c8eb-4e48-8ddd-b37d3ffec2bf"
        ///         "destinationName": "Vườn dâu tây Đà Lạt",
        ///         "location": {
        ///              "latitude": 0,
        ///              "longtitude": 0
        ///         },
        ///         "provinceId": "a589b039-a021-435a-8328-71f824ce9a30",
        ///         "imageUrl": "string of Firebase image url",
        ///         "destinationTypeId": "3be4e631-8c25-4eff-af3b-d3ad882e362a"
        ///     }
        ///
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
        /// Deactivate an destination
        /// </summary>
        /// <remarks>
        /// Deactivate destination by this id   
        /// 
        /// Authorize : Admin , Moderator
        /// 
        /// Sample request:
        ///
        ///     DELETE /destination
        ///     {
        ///        "id": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///     }
        ///
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
        /// Authorize : Admin , Moderator
        /// 
        /// Assign hashtag to destination  
        /// 
        /// Sample request:
        ///
        ///     POST /destination/{destinationID}/hashtag
        ///     {
        ///        "destinationID": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///        "hashtagID": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///     }
        ///
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
        /// Get Destination with Province ,hashtag and name
        /// </summary>
        /// <remarks>
        /// Authorize : Admin , Moderator, User
        /// 
        /// Get Destination within Province , contain hashtag or have name that satify condition. This api can search all options or one only
        /// Sample request:
        ///
        ///     GET /destination
        ///     {
        ///        "provinceId": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///        "hashtagId": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///        "destinationName : "a"
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
        public IActionResult Get(Guid? provinceID, Guid? hashtagId, string? destinationName)
        {
            List<ResponseDestinationViewModel> result = null;

            result = _destinationService.GetDestination(m =>
                        (provinceID == null || m.ProvinceId.Equals(provinceID))
                     && (hashtagId == null || m.DesHashtags.Where(d => d.HashtagId.Equals(hashtagId)).Any())
                     && (destinationName == null || m.DestinationName.Contains(destinationName))
            , false);
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
