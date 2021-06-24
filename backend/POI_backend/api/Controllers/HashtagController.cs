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
    public class HashtagController : ControllerBase
    {
        private readonly ILogger<HashtagController> _logger;
        private readonly IHashtagService _hashtagService;

        public HashtagController(IHashtagService hashtagService, ILogger<HashtagController> logger)
        {
            _logger = logger;
            _hashtagService = hashtagService;
        }


        /// <summary>
        /// Get all hashtags
        /// </summary>
        /// <remarks>
        /// Get all hashtags in POI system (Admin)
        /// 
        ///     No parameter
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(200, "All hashtag is retrieved", typeof(IEnumerable<Hashtag>))]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(404, "The destination is not found")]
        public IActionResult Get()
        {
            _logger.LogInformation("All hashtag is queried");
            return Ok(_hashtagService.GetAll());
        }


        /// <summary>
        /// Get hashtag by ID
        /// </summary>
        /// <remarks>
        /// Get hastag in POI system with ID (Admin ,User)
        /// 
        ///    ID : ID of hashtag 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The hashtag is retrieved", typeof(Hashtag))]
        [SwaggerResponse(404, "The hashtag is not found")]
        public IActionResult Get(Guid id)
        {
            Hashtag hastag = _hashtagService.GetByID(id);
            if (hastag == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(hastag);
            }
        }

        /// <summary>
        /// Create new hastag (Post method)
        /// </summary>
        /// <remarks>
        /// Create new hastag  (Admin)
        /// </remarks>

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Create successfully")]
        [SwaggerResponse(400, "Hashtag is not allowed to update")]
        [SwaggerResponse(409, "Hashtag is conflict")]
        public async Task<IActionResult> Post(CreateHashtagViewModel hashtagViewModel)
        {
            _logger.LogInformation("Post request is called");
            CreateEnum resultCode = await _hashtagService.CreateNewHashtag(hashtagViewModel);
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
        /// Update hashtag information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your hashtag with name and short name (Admin)
        /// </remarks>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "Update successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
        public IActionResult Put(UpdateHashtagViewModel hashtagViewModel)
        {
            _logger.LogInformation("Put request is called");
            UpdateEnum resultCode = _hashtagService.UpdateHashtag(hashtagViewModel);
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
        /// Deactivate an hashtag (Delete method)
        /// </summary>
        /// <remarks>
        /// Deactivate hashtag by this id   
        /// </remarks>
        [HttpDelete("{id}")]
        [SwaggerResponse(201, "Delete successfully")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(400, "ID is not allowed to delete")]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            DeleteEnum resultCode = _hashtagService.DeactivateHashtag(id);
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

    }
}
