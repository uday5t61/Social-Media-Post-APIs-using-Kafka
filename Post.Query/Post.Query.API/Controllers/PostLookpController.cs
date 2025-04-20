
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.API.DTOs;
using Post.Query.API.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostLookpController(ILogger<PostLookpController> logger,IQueryDispatcher<PostEntity> dispatcher) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAllPostsAsync()
        {
            try
            {
                var posts = await dispatcher.SendAsync(new FindAllPostsQuery());

                if (posts == null || posts.Count == 0) return NoContent();

                var count = posts.Count;

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"Successfully returned  {count} post{(count > 1 ? "s" : string.Empty)}"
                });
            }
            catch(Exception ex)
            {
                const string SAFE_ERROR = "Error while processing to retrieve all posts";
                return ErrorResponse(ex, SAFE_ERROR);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPostByIdAsync(Guid id)
        {
            try
            {
                var posts = await dispatcher.SendAsync(new FindPostByIdQuery
                {
                    Id = id
                });

                if (posts == null || posts.Count == 0) return NoContent();

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"Successfully returned post"
                });
            }
            catch (Exception ex)
            {
                string SAFE_ERROR = $"Error while processing to retrieve post by id {id}";
                return ErrorResponse(ex, SAFE_ERROR);
            }
        }

        private ActionResult ErrorResponse(Exception ex, string SAFE_ERROR)
        {
            logger.LogError(ex, SAFE_ERROR);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR
            });
        }

        [HttpGet("byauthor/{author}")]
        public async Task<ActionResult> GetPostByAuthorAsync(string author)
        {
            try
            {
                var posts = await dispatcher.SendAsync(new FindPostByAuthorQuery
                {
                    Author = author
                });

                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                string SAFE_ERROR = $"Error while processing to retrieve post by author {author}";
                return ErrorResponse(ex, SAFE_ERROR);
            }
        }
        [HttpGet("withcomments")]
        public async Task<ActionResult> GetPostByCommentsAsync()
        {
            try
            {
                var posts = await dispatcher.SendAsync(new FindPostWithCommentsQuery());

                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                string SAFE_ERROR = $"Error while processing to retrieve post having comments";
                return ErrorResponse(ex, SAFE_ERROR);
            }
        }
        [HttpGet("withlikes/{numberOfLikes}")]
        public async Task<ActionResult> GetPostWithLikesAsync(int numberOfLikes)
        {
            try
            {
                var posts = await dispatcher.SendAsync(new FindPostWithLikesQuery()
                {
                    NumberOfLikes = numberOfLikes
                });

                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                string SAFE_ERROR = $"Error while processing to retrieve post with likes";
                return ErrorResponse(ex, SAFE_ERROR);
            }
        }

        private ActionResult NormalResponse(List<PostEntity> posts)
        {
            if (posts == null || posts.Count == 0) return NoContent();

            var count = posts.Count;

            return Ok(new PostLookupResponse
            {
                Posts = posts,
                Message = $"Successfully returned  {count} post{(count > 1 ? "s" : string.Empty)}"
            });
        }
    }
}