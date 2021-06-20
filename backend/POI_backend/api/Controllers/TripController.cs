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

namespace POI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ILogger<TripController> _logger;
        private readonly ITripService _tripService;
        private readonly ITripDestinationService _tripDestinationService;

        public TripController(ITripService tripService,
            ILogger<TripController> logger,
            ITripDestinationService tripDestinationService)
        {
            _tripService = tripService;
            _logger = logger;
            _tripDestinationService = tripDestinationService;
        }

        /// <summary>
        /// Get all trips
        /// </summary>
        /// <remarks>
        /// Get all trips in POI system
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
            _logger.LogInformation("All destination is queried");
            return Ok(_tripService.GetAll());
        }

        /// <summary>
        /// Get trip by ID
        /// </summary>
        /// <remarks>
        /// Get trip in POI system with ID
        /// 
        ///    ID : ID of trip 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            Trip trip = _tripService.GetByID(id);
            if (trip == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(trip);
            }
        }

        /// <summary>
        /// Create new Trip (Post method)
        /// </summary>
        /// <remarks>
        /// Create new Trip 
        /// </remarks>

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(CreateTripViewModel createTripViewModel)
        {
            _logger.LogInformation("Post request is called");
            CreateEnum resultCode = await _tripService.CreateNewTrip(createTripViewModel);
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
        /// Update trip information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your trip with name 
        /// </remarks>
        [HttpPut]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Put(UpdateTripViewModel updateTripViewModel)
        {
            _logger.LogInformation("Put request is called");
            UpdateEnum resultCode = _tripService.UpdateTrip(updateTripViewModel);
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
        /// Finish trip  (Put method)
        /// </summary>
        /// <remarks>
        /// Finish trip ưhen user end their trip and go home
        /// </remarks>
        [HttpPut("finish-trip/{id}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult FinishTrip(Guid id)
        {
            _logger.LogInformation("Put request is called");
            UpdateEnum resultCode = _tripService.FinishTrip(id);
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
                return BadRequest("Trip is finished or archived");
            }
        }


        /// <summary>
        /// Deactivate an trip (Delete method)
        /// </summary>
        /// <remarks>
        /// Deactivate trip by this id   
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesDefaultResponseType]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            DeleteEnum resultCode = _tripService.ArchiveTrip(id);
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
                return BadRequest("This trip is not created or is already archived");
            }
        }

        /// <summary>
        /// Get All Destinations in trips
        /// </summary>
        /// <remarks>
        /// Get trip destination entities on a trip
        /// </remarks>
        [HttpGet("{tripID}/destinations")]
        [ProducesDefaultResponseType]
        public IActionResult GetTripDestination(Guid tripID)
        {
            return Ok(_tripDestinationService.GetAllTripDetinationsWithDestination(tripID));
        }


        /// <summary>
        /// Create tripDestination 
        /// </summary>
        /// <remarks>
        /// Get trip destination entities when moving to new destination during a trip
        /// </remarks>
        [HttpPost("{tripID}/destinations")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PostTripDestination(Guid tripID, Guid destinationID)
        {
            CreateEnum resultCode = await _tripDestinationService.CreateNewTripDestination(tripID, destinationID);
            switch (resultCode)
            {
                case CreateEnum.Success:
                    return Created(nameof(GetTripDestination), tripID);
                case CreateEnum.Duplicate:
                    return BadRequest("You are in this destination");
                case CreateEnum.Error:
                    return BadRequest("Trip are not created or already finished or archived");
                case CreateEnum.ErrorInServer:
                    return StatusCode(500);
                default:
                    return StatusCode(500);
            }
        }

    }
}
