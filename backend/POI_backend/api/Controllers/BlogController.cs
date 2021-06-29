using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using POI.repository.Entities;
using POI.service.Services;
using POI.repository.ViewModels;
using POI.repository.ResultEnums;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using POI.repository.Enums;

namespace POI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ILogger<HashtagController> _logger;
        private readonly IBlogService _blogService;
        private readonly IVoteService _voteService;

        public BlogController(IVoteService voteService, IBlogService blogService, ILogger<HashtagController> logger)
        {
            _logger = logger;
            _blogService = blogService;
            _voteService = voteService;
        }

        /// <summary>
        /// Create a new blog of the trip 
        /// </summary>
        /// <remarks>
        /// Authorize : User
        /// 
        /// Create Blog from User. Blog related with Trip, list of POI
        /// 
        /// Sample request
        /// 
        ///     POST /blog
        ///     {
        ///        "tripID": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///        "title" : "String",
        ///        "content" : "String",
        ///        "pois" : [
        ///                 "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///                 "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18"
        ///                 ]
        ///     } 
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "The Blog is created")]
        public async Task<IActionResult> Post(CreateBlogViewModel createBlogViewModel)
        {
            User currentUser = (User)HttpContext.Items["User"];
            _logger.LogInformation("Post request is called");
            CreateEnum result = await _blogService.CreateNewBlog(createBlogViewModel, currentUser.UserId);
            if (result == CreateEnum.Success)
            {
                return StatusCode(201);
            }
            else if (result == CreateEnum.Error)
            {
                return BadRequest("Trip is not belong to you");
            }
            else
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Update the blog of the trip 
        /// </summary>
        /// <remarks>
        /// Authorize : User
        /// 
        /// Update Blog from User. Blog related with Trip, list of POI
        /// 
        /// Sample request
        /// 
        ///     PUT /blog
        ///     {
        ///        "blogId": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///        "tripId": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///        "title" : "String",
        ///        "content" : "String",
        ///        "pois" : [
        ///                 "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///                 "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18"
        ///                 ]
        ///     } 
        /// </remarks>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The Blog is updated")]
        public IActionResult Put(UpdateBlogViewModel model)
        {
            User currentUser = (User)HttpContext.Items["User"];
            _logger.LogInformation("Put request is called");
            UpdateEnum result = _blogService.UpdateBlog(model, currentUser.UserId);
            if (result == UpdateEnum.Success)
            {
                return Ok();
            }
            else if (result == UpdateEnum.Error)
            {
                return BadRequest();
            }
            else
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Deactivate an blog
        /// </summary>
        /// <remarks>
        /// Authorize : User
        /// 
        /// Deactivate blog by this id   
        /// 
        /// Sample request:
        ///
        ///     DELETE /blog
        ///     {
        ///        "id": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///     }
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "Delete successfully")]
        [SwaggerResponse(400, "ID is not allowed to delete")]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            User currentUser = (User)HttpContext.Items["User"];
            DeleteEnum resultCode = _blogService.ArchiveBlog(id, currentUser.UserId);
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
                return BadRequest("You are not ownner of this blog");
            }
            else
            {
                return BadRequest("This blog is not created or is already archived");
            }
        }

        /// <summary>
        /// Accept Blog by admin 
        /// </summary>
        /// <remarks>
        /// Authorize : Admin, Moderator
        /// 
        /// Accept Blog by admin 
        /// 
        /// Sampe request: 
        /// 
        ///     POST /trip/accept/{id}
        ///     {
        ///        "id": "387fcbaf-34c6-4b97-8578-fd1fb5b0fc18",
        ///     } 
        /// </remarks>
        [HttpPost("accept/{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "Accept blog successfully")]
        public IActionResult AcceptBlog(Guid id)
        {
            _logger.LogInformation("Accept Request is called");
            UpdateEnum resultCode = _blogService.ApproveBlog(id);
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
                return BadRequest("Blog is available or disable");
            }
        }

        /// <summary>
        /// Get all blogs
        /// </summary>
        /// <remarks>
        /// 
        /// Authorize : Admin , Moderator, User
        /// 
        /// Get all blogs in POI system. Will be used in Mobile blog view or admin mobile blog view.
        /// 
        ///     No parameter
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Moderator, User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The blogs is retrieved", typeof(List<ResponseBlogViewModel>))]
        [SwaggerResponse(404, "The blogs is not found")]
        public IActionResult GetAllBlogs()
        {
            _logger.LogInformation("Get Request is called");
            User currentUser = (User)HttpContext.Items["User"];
            List<ResponseBlogViewModel> result = null;
            if (currentUser.Role.RoleName.Equals("User"))
            {
                result = _blogService.GetBlogs(m => m.Status == (int)BlogEnum.Available, false);
            }
            else
            {
                result = _blogService.GetBlogs(m => true, false);
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

        /// <summary>
        /// Get all blogs of user (who send this request)
        /// </summary>
        /// <remarks>
        /// 
        /// Authorize : User
        /// 
        /// Get all blogs of current user in POI system. Will be used in Mobile blog view 
        ///  
        ///     No parameter
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("user")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The blogs is retrieved", typeof(List<ResponseBlogViewModel>))]
        [SwaggerResponse(404, "The blogs is not found")]
        public IActionResult GetAllBlogsOfUser()
        {
            _logger.LogInformation("Get Request is called");
            User currentUser = (User)HttpContext.Items["User"];
            List<ResponseBlogViewModel> result = null;
            if (currentUser.Role.RoleName.Equals("User"))
            {
                result = _blogService.GetBlogs(m => m.UserId.Equals(currentUser.UserId), false);
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

        /// <summary>
        /// Get blog of that user own it or admin, moderator want to read it
        /// </summary>
        /// <remarks>
        /// 
        /// Authorize : Admin , Moderator, User
        /// 
        ///  Get blog of that user own it or admin, moderator want to read it. Will be used in Mobile blog view 
        ///  Blog can be pending or available or disable
        ///  
        ///     
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("user/{id}")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The blogs is retrieved", typeof(ResponseBlogViewModel))]
        [SwaggerResponse(404, "The blogs is not found")]
        public IActionResult GetBlogOfUser(Guid id)
        {
            _logger.LogInformation("Get Request is called");
            User currentUser = (User)HttpContext.Items["User"];
            ResponseBlogViewModel result = null;
            if (currentUser.Role.RoleName.Equals("User"))
            {
                result = _blogService.GetBlogs(m => m.UserId.Equals(currentUser.UserId) && m.BlogId.Equals(id), false).FirstOrDefault();
            }
            else
            {
                result = _blogService.GetBlogs(m => m.BlogId.Equals(id), false).FirstOrDefault();
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

        /// <summary>
        /// Get a blog for user to reading 
        /// </summary>
        /// <remarks>
        /// 
        /// Authorize : User
        /// 
        ///  Get a blog for user to reading . Will be used in Mobile blog view. It can be their blog or another user's blog
        ///  
        ///     
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The blogs is retrieved", typeof(ResponseBlogViewModel))]
        [SwaggerResponse(404, "The blogs is not found")]
        public IActionResult GetBlog(Guid id)
        {
            _logger.LogInformation("Get Request is called");
            ResponseBlogViewModel result = null;

            result = _blogService.GetBlogs(m => m.BlogId.Equals(id) && m.Status == (int)BlogEnum.Available, false).FirstOrDefault();

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Vote a blog
        /// </summary>
        /// <remarks>
        /// 
        /// Authorize : User
        /// 
        /// React to blog. It must be like, dislike or NoReaction (This was like, dislike but now user unlike or undislike it)
        /// 
        ///     No parameter
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("vote")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(201, "The vote is create")]
        [SwaggerResponse(404, "The blog or user is not found")]
        public IActionResult Vote(VoteViewModel vote)
        {
            _logger.LogInformation("Vote Request is called");
            User currentUser = (User)HttpContext.Items["User"];
            CreateEnum result = _voteService.CreateVote(vote, currentUser.UserId);
            switch (result)
            {
                case CreateEnum.Success:
                    return StatusCode(201);
                case CreateEnum.Error:
                    return BadRequest("Blog is not exist");
                case CreateEnum.ErrorInServer:
                    return StatusCode(500);
                default:
                    return StatusCode(500);
            }
        }
    }
}
