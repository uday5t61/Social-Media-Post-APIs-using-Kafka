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
    public class RestoreReadDbController(ILogger<RestoreReadDbController> logger,ICommandDispatcher dispatcher) : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult> RestoreReadDb()
        {
          
            try
            {
                var command = new RestoreReadDbCommand();

                await dispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewPostResponse
                {                   
                    Message = "Read Database successfully restored!"
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
                const string SAFE_ERROR_MESSAGE = "Error while processing request to restore read database";
                logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);
                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }        
    }
}