﻿using Microsoft.AspNetCore.Http;
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
        /// Get all trips in POI system (Admin)
        /// 
        ///     No parameter
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The trip is retrieved")]
        [SwaggerResponse(404, "The trip is not found")]
        public IActionResult GetTrip(Guid? userID)
        {
            List<ResponseTripViewModel> result = null;
            if (userID != null)
            {
                result = _tripService.GetTrips(m => m.User.UserId.Equals(userID), false);
            }
            else
            {
                result = _tripService.GetTrips(m => true, false);
            }
            _logger.LogInformation("All destination is queried");
            return Ok(result);
        }

        /// <summary>
        /// Get trip by ID
        /// </summary>
        /// <remarks>
        /// Get trip in POI system with ID (Admin , User)
        /// 
        ///    ID : ID of trip 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The destination is retrieved", typeof(ResponseTripViewModel))]
        [SwaggerResponse(404, "The destination is not found")]
        public IActionResult Get(Guid id)
        {
            ResponseTripViewModel trip = _tripService.GetTrips(m => m.TripId.Equals(id), false).First();
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
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Create successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
        public async Task<IActionResult> Post(CreateTripViewModel createTripViewModel)
        {
            User currentUser = (User)HttpContext.Items["User"];
            _logger.LogInformation("Post request is called");
            Tuple<CreateEnum, Guid> result = await _tripService.CreateNewTrip(createTripViewModel, currentUser.UserId);
            if (result.Item1 == CreateEnum.Success)
            {
                return Get(result.Item2);
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
        /// Update trip information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your trip with name 
        /// </remarks>
        [HttpPut]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "Update successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
        public IActionResult Put(UpdateTripViewModel updateTripViewModel)
        {
            _logger.LogInformation("Put request is called");
            User currentUser = (User)HttpContext.Items["User"];
            UpdateEnum resultCode = _tripService.UpdateTrip(updateTripViewModel, currentUser.UserId);
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
        /// Finish trip when user end their trip and go home
        /// </remarks>
        [HttpPut("finish-trip/{id}")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Finish trip successfully")]
        [SwaggerResponse(400, "Trip is finished or archived")]
        public IActionResult FinishTrip(Guid id)
        {
            _logger.LogInformation("Put request is called");
            User currentUser = (User)HttpContext.Items["User"];
            UpdateEnum resultCode = _tripService.FinishTrip(id, currentUser.UserId);
            if (resultCode == UpdateEnum.Success)
            {
                return Ok();
            }
            else if (resultCode == UpdateEnum.ErrorInServer)
            {
                return StatusCode(500);
            }
            else if (resultCode == UpdateEnum.NotOwner)
            {
                return BadRequest("You are not ownner of this trip");
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
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Delete successfully")]
        [SwaggerResponse(400, "ID is not allowed to delete")]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            User currentUser = (User)HttpContext.Items["User"];
            DeleteEnum resultCode = _tripService.ArchiveTrip(id, currentUser.UserId);
            if (resultCode == DeleteEnum.Success)
            {
                return Ok();
            }
            else if (resultCode == DeleteEnum.ErrorInServer)
            {
                return StatusCode(500);
            }
            else if (resultCode == DeleteEnum.NotOwner)
            {
                return BadRequest("You are not ownner of this trip");
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
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "Create new trip-destination successfully")]
        [SwaggerResponse(400, "ID is not allowed to create")]
        public async Task<IActionResult> PostTripDestination(Guid tripID, Guid destinationID)
        {
            User currentUser = (User)HttpContext.Items["User"];
            CreateEnum resultCode = await _tripDestinationService.CreateNewTripDestination(tripID, destinationID, currentUser.UserId);
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
                case CreateEnum.NotOwner:
                    return BadRequest("This trip is not belong to yours");
                default:
                    return StatusCode(500);
            }
        }

    }
}
