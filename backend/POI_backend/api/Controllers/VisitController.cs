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
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            Visit visit = _visitService.GetByID(id);
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
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(CreateVisitViewModel visitViewModel)
        {
            _logger.LogInformation("Post request is called");
            CreateEnum resultCode = await _visitService.CreateNewVisit(visitViewModel);
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
        /// Update visit information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your visit 
        /// </remarks>
        [HttpPut]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Put(UpdateVisitViewModel visitViewModel)
        {
            _logger.LogInformation("Put request is called");
            UpdateEnum resultCode = _visitService.UpdateVisit(visitViewModel);
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
        /// Archive an visit (Delete method)
        /// </summary>
        /// <remarks>
        /// Archive hashtag by this id   
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesDefaultResponseType]
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
