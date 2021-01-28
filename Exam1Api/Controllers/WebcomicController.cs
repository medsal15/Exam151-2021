using System;
using System.IO;
using System.Linq;
using Exam1Api.Models;
using Exam1Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam1Api.Controllers
{
    [ApiController]
    public class WebcomicController : ControllerBase
    {
        private readonly IWebcomicService service;

        public WebcomicController(IWebcomicService service) : base()
        {
            this.service = service;
        }

        [HttpGet("webcomics")]
        public IActionResult GetAll([FromQuery]string name = "")
        {
            return Ok(service.GetAll(name).Select(w => w.ToResult()));
        }

        [HttpGet("webcomics/{id}")]
        public IActionResult GetSingle([FromRoute]int id)
        {
            var webcomic = service.GetSingle(id);

            if (webcomic == null)
            {
                return NotFound();
            }

            return Ok(webcomic.ToResult());
        }

        [HttpPost("webcomics/{id}")]
        public IActionResult Update([FromRoute]int id, [FromBody]WebcomicInput webcomic)
        {
            try
            {
                return Ok(service.Update(id, webcomic).ToResult());
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

        [HttpPost("webcomics")]
        public IActionResult Add([FromBody]WebcomicInput webcomic)
        {
            try
            {
                return Ok(service.Create(webcomic).ToResult());
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("webcomics/{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            service.Delete(id);
            return NoContent();
        }

        [HttpGet("webcomics/link/{webcomic}/{author}")]
        public IActionResult Link([FromRoute]int webcomic, [FromRoute]int author)
        {
            try
            {
                service.AddAuthor(webcomic, author);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("webcomics/unlink/{webcomic}/{author}")]
        public IActionResult Unlink([FromRoute]int webcomic, [FromRoute]int author)
        {
            try
            {
                service.RemoveAuthor(webcomic, author);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("webcomics/{id}/image")]
        public IActionResult GetImage([FromRoute]int id)
        {
            var webcomic = service.GetSingle(id);
            if (webcomic == null || webcomic.Picture == null)
            {
                return NotFound();
            }
            return File(webcomic.Picture, "image/jpeg");
        }

        [HttpPost("webcomics/{id}/image")]
        public IActionResult SetImage([FromRoute]int id, IFormFile file)
        {
            var webcomic = service.GetSingle(id);
            if (webcomic == null)
            {
                return NotFound("The webcomic does not exist");
            }
            if (file.Length > 0 && !file.ContentType.Contains("image"))
            {
                return BadRequest("The picture is not an image");
            }

            var ms = new MemoryStream();
            file.CopyTo(ms);
            service.SetImage(id, ms.ToArray());
            return Ok(webcomic.ToResult());
        }
    }
}
