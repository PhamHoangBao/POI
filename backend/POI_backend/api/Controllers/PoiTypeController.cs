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
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;


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
        /// Authorize : Admin , Moderator, User
        /// 
        /// Get all poiTypes in POI system
        /// 
        ///     No parameter
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Moderator, User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The POI types is retrieved", typeof(Poitype))]
        [SwaggerResponse(404, "The POI types is not found")]
        public IActionResult Get()
        {
            _logger.LogInformation("All POIs are queried");
            return Ok(_poiTypeService.GetAll());
        }

        /// <summary>
        /// Get poiType by ID
        /// </summary>
        /// <remarks>
        /// 
        /// Authorize : Admin , Moderator, User
        /// 
        /// Get poiType in POI system with ID
        /// 
        ///     GET /poiType
        ///     {
        ///        "id": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///     } 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Moderator, User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The POI type is retrieved", typeof(Poitype))]
        [SwaggerResponse(404, "The POI type is not found")]
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
        /// Create new poiType 
        /// </summary>
        /// <remarks>
        /// Authorize : Admin , Moderator
        /// 
        ///     POST /poiType
        ///     {
        ///        "name": "Vui chơi lớn",
        ///        "icon" : "Image from firebase url"
        ///     }  
        /// Create new poiType 
        /// </remarks>

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Create successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
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
        /// Authorize : Admin , Moderator
        /// 
        /// 
        /// Update your poiType with name
        /// 
        /// Sample request:
        /// 
        ///     PUT /poiType
        ///     {
        ///         "poitypeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "name": "string",
        ///         "icon": "image from firebase url"
        ///     }
        /// </remarks>
        [HttpPut]
        [Authorize(Roles = "Admin , Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "Update successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
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
        /// 
        /// Authorize : Admin , Moderator
        /// 
        /// Deactivate poiType by this id   
        /// 
        /// Sample request:
        ///
        ///     DELETE /poiType/{id}
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
