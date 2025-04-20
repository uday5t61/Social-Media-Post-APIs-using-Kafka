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
    public class CommentController(ILogger<CommentController> logger,ICommandDispatcher dispatcher) : ControllerBase
    {

        [HttpPut("{id}")]
        public async Task<ActionResult> AddCommentAsync(Guid id,AddCommentCommand command)
        {           
            try
            {
                command.Id = id;

                await dispatcher.SendAsync(command);

                return Ok(new BaseResponse
                {
                    Message = "Add comment request completed successfully!"
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
                const string SAFE_ERROR_MESSAGE = "Error while processing request to add comment!";
                logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);
                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Id = id,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult> EditComment(Guid id, EditCommentCommand command)
        {
            try
            {
                command.Id = id;
                await dispatcher.SendAsync(command);
                return Ok(new BaseResponse
                {
                    Message = "Edit comment request is processed successfully!"
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
                const string SAFE_ERROR_MESSAGE = "Error while processing request to edit the comment!";
                logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
        [HttpPut("remove/{id}")]
        public async Task<ActionResult> RemoveComent(Guid id,RemoveCommentCommand command)
        {
            try
            {
                command.Id = id;
                await dispatcher.SendAsync(command);
                return Ok(new BaseResponse
                {
                    Message = "Remove comment request is processed successfully!"
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
                const string SAFE_ERROR_MESSAGE = "Error while processing request to remove the comment!";
                logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}