using BL.GenericResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Base
{
    [ApiController]
    public class AppControllerBase : ControllerBase
    {
        protected string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        protected Guid GuidUserId =>
       Guid.TryParse(UserId, out var guid) ? guid : Guid.NewGuid();

        #region Actions
        //public ObjectResult NewResult<T>(Response<T> response)
        //{
        //    //switch (response.StatusCode)
        //    //{
        //    //    case HttpStatusCode.OK:
        //    //        return new OkObjectResult(response);
        //    //    case HttpStatusCode.Created:
        //    //        return new CreatedResult(string.Empty, response);
        //    //    case HttpStatusCode.Unauthorized:
        //    //        return new UnauthorizedObjectResult(response);
        //    //    case HttpStatusCode.BadRequest:
        //    //        return new BadRequestObjectResult(response);
        //    //    case HttpStatusCode.NotFound:
        //    //        return new NotFoundObjectResult(response);
        //    //    case HttpStatusCode.Accepted:
        //    //        return new AcceptedResult(string.Empty, response);
        //    //    case HttpStatusCode.UnprocessableEntity:
        //    //        return new UnprocessableEntityObjectResult(response);
        //    //    default:
        //    //        return new BadRequestObjectResult(response);
        //    //}


        //    switch (response.Type)
        //    {
        //        case ResponseType.Success:
        //            return Ok(response);
        //        case ResponseType.Created:
        //            return new CreatedResult(string.Empty, response);
        //        case ResponseType.Accepted:
        //            return new AcceptedResult(string.Empty, response);
        //        case ResponseType.NotFound:
        //            return NotFound(response);
        //        case ResponseType.Unauthorized:
        //            return Unauthorized(response);
        //        case ResponseType.Unprocessable:
        //            return UnprocessableEntity(response);
        //        default:
        //            return BadRequest(response);
        //    }

        //}

        #endregion

        #region Actions

        public ObjectResult NewResult<T>(Response<T> response)
        {
            var statusCode = GetStatusCode(response.Type);
            response.StatusCode = (HttpStatusCode)statusCode;

            return new ObjectResult(response)
            {
                StatusCode = statusCode
            };
        }

        private int GetStatusCode(ResponseType type)
        {
            return type switch
            {
                ResponseType.Success => StatusCodes.Status200OK,
                ResponseType.Created => StatusCodes.Status201Created,
                ResponseType.Accepted => StatusCodes.Status202Accepted,
                ResponseType.NotFound => StatusCodes.Status404NotFound,
                ResponseType.Unauthorized => StatusCodes.Status401Unauthorized,
                ResponseType.BadRequest => StatusCodes.Status400BadRequest,
                ResponseType.Unprocessable => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status400BadRequest
            };
        }

        #endregion



    }
}
