using System;
using System.Linq;
using Exam1Api.Models;
using Exam1Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam1Api.Controllers
{
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService service;

        public AuthorController(IAuthorService service) : base()
        {
            this.service = service;
        }

        [HttpGet("authors")]
        public IActionResult GetAll([FromQuery]string name = "")
        {
            return Ok(service.GetAll(name).Select(a => a.ToResult()));
        }

        [HttpGet("authors/{id}")]
        public IActionResult GetSingle([FromRoute]int id)
        {
            var author = service.GetSingle(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author.ToResult());
        }

        [HttpPost("authors/{id}")]
        public IActionResult Update([FromRoute]int id, [FromBody]AuthorInput author)
        {
            try
            {
                return Ok(service.Update(id, author).ToResult());
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("authors")]
        public IActionResult Add([FromBody]AuthorInput author)
        {
            try
            {
                return Ok(service.Create(author).ToResult());
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("authors/{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            service.Delete(id);
            return NoContent();
        }

        [HttpGet("authors/link/{author}/{webcomic}")]
        public IActionResult Link([FromRoute]int author, [FromRoute]int webcomic)
        {
            try
            {
                service.AddWebcomic(author, webcomic);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("authors/unlink/{author}/{webcomic}")]
        public IActionResult Unlink([FromRoute]int author, [FromRoute]int webcomic)
        {
            try
            {
                service.RemoveWebcomic(author, webcomic);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
