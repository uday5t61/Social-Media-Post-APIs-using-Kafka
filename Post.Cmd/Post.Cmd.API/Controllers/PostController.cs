using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.API.Commands;
using Post.Cmd.API.DTOs;
using Post.Common.DTOs;
using Post.Common.Events;

namespace Post.Cmd.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostController(ILogger<PostController> logger,ICommandDispatcher dispatcher) : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult> NewPostAsync(NewPostCommand command)
        {
            var id = Guid.NewGuid();

            try
            {
                command.Id = id;

                await dispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewPostResponse
                {
                    Id = id,
                    Message = "New post creation request completed successfully!"
                });
            }
            catch(InvalidOperationException ex)
            {
                logger.Log(LogLevel.Warning, ex, "Client made a bad request");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch(Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error whiel processing request to create new post!";
                logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);
                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Id = id,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditMessage(Guid id,EditMessageCommand command)
        {            
            try
            {
                command.Id = id;
                await dispatcher.SendAsync(command);
                return Ok(new BaseResponse
                {
                    Message = "Edit message request is processed successfully!"                    
                });
            }
            catch(AggregateNotFoundException ex)
            {
                logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate.Client passed incorrect post Id targetting the aggregate");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                logger.Log(LogLevel.Warning, ex, "Client made a bad request");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to edit the post!";
                logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
        [HttpPut("like/{id}")]
        public async Task<ActionResult> LikePost(Guid id)
        {
            try
            {
                var command = new LikePostCommand
                {
                    Id = id
                };
                await dispatcher.SendAsync(command);
                return Ok(new BaseResponse
                {
                    Message = "Like post request is processed successfully!"
                });
            }
            catch (AggregateNotFoundException ex)
            {
                logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate.Client passed incorrect post Id targetting the aggregate");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                logger.Log(LogLevel.Warning, ex, "Client made a bad request");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to like the post!";
                logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(Guid id,DeletePostCommand command)
        {
            try
            {
                command.Id = id;
                await dispatcher.SendAsync(command);
                return Ok(new BaseResponse
                {
                    Message = "Delete post request is processed successfully!"
                });
            }
            catch (AggregateNotFoundException ex)
            {
                logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate.Client passed incorrect post Id targetting the aggregate");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                logger.Log(LogLevel.Warning, ex, "Client made a bad request");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to delete the post!";
                logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}