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
    public class VisitController : ControllerBase
    {
        private readonly ILogger<VisitController> _logger;
        private readonly IVisitService _visitService;

        public VisitController(IVisitService visitService, ILogger<VisitController> logger)
        {
            _logger = logger;
            _visitService = visitService;
        }

        /// <summary>
        /// Get all visits
        /// </summary>
        /// <remarks>
        /// Get all visits in POI system
        /// 
        ///     No parameter
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "User, Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The visit is retrieved")]
        [SwaggerResponse(404, "The visit is not found")]
        public IActionResult Get()
        {
            _logger.LogInformation("All hashtag is queried");
            return Ok(_visitService.GetAll());
        }


        /// <summary>
        /// Get visit by ID
        /// </summary>
        /// <remarks>
        /// Get visit in POI system with ID
        /// 
        ///    ID : ID of visit 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The visit is retrieved")]
        [SwaggerResponse(404, "The visit is not found")]
        public IActionResult Get(Guid id)
        {
            ResponseVisitViewModel visit = _visitService.GetVisits(m => m.VisitId.Equals(id), false).First();
            if (visit == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(visit);
            }
        }

        /// <summary>
        /// Create new Visit (Post method)
        /// </summary>
        /// <remarks>
        /// Create new Visit 
        /// </remarks>

        [HttpPost]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Create successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
        public async Task<IActionResult> Post(CreateVisitViewModel visitViewModel)
        {
            _logger.LogInformation("Post request is called");
            User currentUser = (User)HttpContext.Items["User"];
            Tuple<CreateEnum,Guid> result = await _visitService.CreateNewVisit(visitViewModel, currentUser.UserId);
            if (result.Item1 == CreateEnum.Success)
            {
                Console.WriteLine(result.Item2);
                return Get(result.Item2);
            }
            else if (result.Item1 == CreateEnum.ErrorInServer)
            {
                return StatusCode(500);
            }
            else
            {
                return BadRequest("You already check in it in this trip");
            }
        }


        /// <summary>
        /// Update visit information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your visit 
        /// </remarks>
        [HttpPut]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "Update successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
        public async Task<IActionResult> Put(UpdateVisitViewModel visitViewModel)
        {
            _logger.LogInformation("Put request is called");
            User currentUser = (User)HttpContext.Items["User"];
            UpdateEnum resultCode = await _visitService.UpdateVisit(visitViewModel, currentUser.UserId);
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
                return BadRequest("Some thing wrong here");
            }
        }



        /// <summary>
        /// Archive an visit (Delete method)
        /// </summary>
        /// <remarks>
        /// Archive hashtag by this id   
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Delete successfully")]
        [SwaggerResponse(400, "ID is not allowed to delete")]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            DeleteEnum resultCode = _visitService.ArchiveVisit(id);
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
