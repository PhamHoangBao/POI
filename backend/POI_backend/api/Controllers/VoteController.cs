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
    public class VoteController : ControllerBase
    {
        private readonly ILogger<VoteController> _logger;
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService, ILogger<VoteController> logger)
        {
            _voteService = voteService;
            _logger = logger;
        }

        /// <summary>
        /// Get all votes
        /// </summary>
        /// <remarks>
        /// Get all votes in POI system
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
            _logger.LogInformation("All votes is queried");
            return Ok(_voteService.GetAll());
        }

        /// <summary>
        /// Get votes by ID
        /// </summary>
        /// <remarks>
        /// Get votes in POI system with ID
        /// 
        ///    ID : ID of votes 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            Vote vote = _voteService.GetByID(id);
            if (vote == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(vote);
            }
        }


        /// <summary>
        /// Create new vote (Post method)
        /// </summary>
        /// <remarks>
        /// Create new vote 
        /// </remarks>

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(CreateVoteViewModel createVoteViewModel)
        {
            _logger.LogInformation("Post request is called");
            CreateEnum resultCode = await _voteService.CreateNewVote(createVoteViewModel);
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
        /// Update vote information (Put method)
        /// </summary>
        /// <remarks>
        /// Update your vote with its vote Value 
        /// </remarks>
        [HttpPut]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Put(UpdateVoteViewModel updateVoteViewModel)
        {
            _logger.LogInformation("Put request is called");
            UpdateEnum resultCode = _voteService.UpdateVote(updateVoteViewModel);
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

    }
}
