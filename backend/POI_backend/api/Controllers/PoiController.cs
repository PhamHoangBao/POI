using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using POI.repository.Entities;
using NetTopologySuite.Geometries;
using POI.service.Services;
using POI.repository.AutoMapper;
using POI.repository.ViewModels;
using POI.repository.ResultEnums;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using POI.repository.Enums;
using POI.repository.Utils;

namespace POI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoiController : ControllerBase
    {
        private readonly ILogger<PoiController> _logger;
        private readonly IPoiService _poiService;
        private int PageSize = 5;
        public PoiController(IPoiService poiService, ILogger<PoiController> logger)
        {
            _logger = logger;
            _poiService = poiService;
        }

        /// <summary>
        /// Get all pois
        /// </summary>
        /// <remarks>
        /// 
        /// Authorize : Admin , Moderator
        /// 
        /// Get all pois in POI system
        /// 
        ///     No parameter
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The pois is retrieved", typeof(List<ResponsePoiViewModel>))]
        [SwaggerResponse(404, "The pois is not found")]
        public async Task<IActionResult> Get(int? pageIndex)
        {
            _logger.LogInformation("All POIs are queried");
            PagedList<ResponsePoiViewModel> responses = await _poiService.GetPOIWithPaging(m => true, false, pageIndex ?? 1, PageSize);
            var metadata = new
            {
                responses.PageSize,
                responses.CurrentPageIndex,
                responses.TotalCount,
                responses.HasNext,
                responses.TotalPages,
                responses.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(responses);
        }

        /// <summary>
        /// Get all pois which belong to a Destination
        /// </summary>
        /// <remarks>
        ///  Authorize : Admin , Moderator
        /// Get all pois which belong to a Destination
        /// 
        /// Sample request:
        ///
        ///     GET /poi/{destinationId}
        ///     {
        ///        "destinationId": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///     }
        /// 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("destination/{destinationId}")]
        [Authorize(Roles = "User, Admin , Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The poi is retrieved", typeof(List<ResponsePoiViewModel>))]
        [SwaggerResponse(404, "The poi is not found")]
        public async Task<IActionResult> GetWithinDestination(Guid destinationId, int? pageIndex)
        {
            User currentUser = (User)HttpContext.Items["User"];
            PagedList<ResponsePoiViewModel> responses = null;
            if (currentUser.Role.RoleName.Equals("User"))
            {
                responses = await _poiService.GetPOIWithPaging(m => m.DestinationId.Equals(destinationId) && m.Status == (int)PoiEnum.Available, false, pageIndex ?? 1, PageSize);
            }
            else
            {
                responses = await _poiService.GetPOIWithPaging(m => m.DestinationId.Equals(destinationId), false, pageIndex ?? 1, PageSize);
            }
            var metadata = new
            {
                responses.PageSize,
                responses.CurrentPageIndex,
                responses.TotalCount,
                responses.HasNext,
                responses.TotalPages,
                responses.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(responses);
        }

        /// <summary>
        /// Get poi from its user 
        /// </summary>
        /// <remarks>
        /// 
        /// Authorize : User
        /// 
        /// Get all pois which belong to a Destination. User request this POI
        /// 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("post-by-user")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The poi is retrieved", typeof(List<ResponsePoiViewModel>))]
        [SwaggerResponse(404, "The poi is not found")]
        public async Task<IActionResult> GetPOIOfUser()
        {
            User currentUser = (User)HttpContext.Items["User"];
            return Ok(await _poiService.GetPoi(m => m.UserId.Equals(currentUser.UserId), false));
        }

        /// <summary>
        /// Get poi from its user 
        /// </summary>
        /// <remarks>
        /// Authorize : Admin , Moderator
        /// Sample request:
        ///
        ///     GET /poi/post-by-user/{id}
        ///     {
        ///        "id": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///     }
        /// 
        /// 
        /// Get poi from its user 
        /// 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("post-by-user/{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The poi is retrieved", typeof(List<ResponsePoiViewModel>))]
        [SwaggerResponse(404, "The poi is not found")]
        public async Task<IActionResult> GetPOIOfUserBySystem(Guid id)
        {
            return Ok(await _poiService.GetPoi(m => m.UserId.Equals(id), false));
        }


        /// <summary>
        /// Get poi by ID
        /// </summary>
        /// <remarks>
        /// Authorize : Admin, Moderator, User
        /// 
        /// Get poi in POI system with ID
        /// Sample request:
        ///
        ///     GET /poi/{id}
        ///     {
        ///        "id": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Moderator, User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The poi is retrieved", typeof(ResponsePoiViewModel))]
        [SwaggerResponse(404, "The poi is not found")]
        public async Task<IActionResult> Get(Guid id)
        {
            User currentUser = (User)HttpContext.Items["User"];
            ResponsePoiViewModel poi = null;
            if (currentUser.Role.RoleName.Equals("User"))
            {
                poi = (await _poiService.GetPoi(m => m.PoiId.Equals(id) && m.Status == (int)PoiEnum.Available, false)).First();
            }
            else
            {
                poi = (await _poiService.GetPoi(m => m.PoiId.Equals(id), false)).First();
            }

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
        /// Create new poi 
        /// </summary>
        /// <remarks>
        /// Authorize : Admin , Moderator, User
        /// 
        /// Create new poi 
        /// 
        /// Sample request: 
        /// 
        ///     POST /poi
        ///     {
        ///         "name": "string",
        ///         "poiTypeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "destinationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "description": "string",
        ///         "imageUrl": "string",
        ///         "location": {
        ///              "latitude": 0,
        ///              "longtitude": 0
        ///         }
        ///     }
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
            Tuple<CreateEnum, Guid> result = await _poiService.CreateNewPoi(poiViewModel, userID);
            if (result.Item1 == CreateEnum.Success)
            {
                return CreatedAtAction("Get", result.Item2);
            }
            else if (result.Item1 == CreateEnum.ErrorInServer)
            {
                return StatusCode(500);
            }
            else
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Update poi information 
        /// </summary>
        /// <remarks>
        /// 
        /// Authorize : Admin , Moderator
        /// 
        /// Update your poi with name
        /// 
        /// Sample request: 
        /// 
        ///     PUT /poi
        ///     {
        ///         "poiId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///         "name": "string",
        ///         "poiTypeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "destinationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "description": "string",
        ///         "imageUrl": "string",
        ///         "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "location": {
        ///              "latitude": 0,
        ///              "longtitude": 0
        ///         }
        ///     }
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
        /// Authorize : Admin , Moderator
        /// 
        /// Deactivate poi by this id 
        /// 
        /// Sample request:
        ///
        ///     DELETE /poi
        ///     {
        ///        "id": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///     }
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
        /// Authorize : Admin , Moderator
        /// 
        /// 
        /// Approve a new POI from user
        /// Sample request:
        ///
        ///     PUT /poi/approve/{id}
        ///     {
        ///        "id": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///     }
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

        /// <summary>
        /// Assign user location to POI
        /// </summary>
        /// <remarks>
        /// Authorize : User
        /// 
        /// 
        /// Assign user location to POI
        /// Sample request:
        ///
        ///     POST /poi/location
        ///     {
        ///         "latitude": 10.7788026,
        ///         "longtitude": 106.6925037
        ///     }
        /// </remarks>
        /// <returns></returns>
        [HttpPost("location")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(400, "Fail to add record")]
        [SwaggerResponse(200, "Add user location to redis data")]
        public async Task<IActionResult> CheckUserLocation(MyPoint point)
        {
            User currentUser = (User)HttpContext.Items["User"];

            CreateEnum result = await _poiService.AddUserToPoiInRedis(point, currentUser.UserId);
            if (result == CreateEnum.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Cannot find the nearest POI ");
            }

        }
    }
}
