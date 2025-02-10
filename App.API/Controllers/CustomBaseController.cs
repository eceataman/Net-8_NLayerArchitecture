using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ServiceResult<T> result)
        {
            if (result.Status == HttpStatusCode.NoContent)
            {
                // return NoContent();
                return new ObjectResult(null) { StatusCode = result.Status.GetHashCode() };
                //her bir response bodysi dolacak diye bir şey yok örneğin 404 ise oradan anlaacak bad request diye
            }
            if (result.Status == HttpStatusCode.Created)
            {
                return Created(result.UrlAsCreated, result);
                //resulttan url geliyor
            }
            return new ObjectResult(result) { StatusCode = result.Status.GetHashCode() };
        }
        [NonAction]
        public IActionResult CreateActionResult(ServiceResult result)
        {
            if (result.Status == HttpStatusCode.NoContent)
            {
                return new ObjectResult(null) { StatusCode = result.Status.GetHashCode() };
                //her bir response bodysi dolacak diye bir şey yok örneğin 404 ise oradan anlaacak bad request diye
            }
            return new ObjectResult(result) { StatusCode = result.Status.GetHashCode() };
        }
    }
}
