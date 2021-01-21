using System;
using System.Linq;
using Exam1Api.Models;
using Exam1Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam1Api.Controllers
{
    [ApiController]
    public class SocialLinkController : ControllerBase
    {
        private readonly ISocialLinkService service;

        public SocialLinkController(ISocialLinkService service) : base()
        {
            this.service = service;
        }

        [HttpGet("sociallinks/author/{author}")]
        public IActionResult GetAll([FromRoute]int author)
        {
            return Ok(service.GetAll(author).Select(link => link.ToResult()));
        }

        [HttpGet("sociallinks/{id}")]
        public IActionResult GetSingle([FromRoute]int id)
        {
            return Ok(service.GetSingle(id).ToResult());
        }

        [HttpPost("sociallinks/{id}")]
        public IActionResult Update([FromRoute]int id, [FromBody]SocialLinkInput socialLink)
        {
            try
            {
                return Ok(service.Update(id, socialLink).ToResult());
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("sociallinks")]
        public IActionResult Add([FromBody]SocialLinkInput socialLink)
        {
            try
            {
                return Ok(service.Create(socialLink).ToResult());
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("sociallinks/{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            service.Delete(id);
            return NoContent();
        }
    }
}
