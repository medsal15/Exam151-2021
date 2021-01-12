using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Exam1Api.Enums;

namespace Exam1Api.Models
{
    public class Webcomic
    {
        public int Id { get; set; }
        public List<AuthorWebcomic> AuthorWebcomics { get; set; }
        public string Name { get; set; }
        public State State { get; set; }
        public byte[] Picture { get; set; }

        public WebcomicResult ToResult()
        {
            return new WebcomicResult {
                Id = this.Id,
                Authors = this.AuthorWebcomics.Select(a => a.AuthorId).ToArray(),
                Name = this.Name,
                State = this.State
            };
        }
    }

    public class WebcomicInput
    {
        public int[] Authors { get; set; }
        [StringLength(60)]
        [Required]
        public string Name { get; set; }
        public State State { get; set; }

        public Webcomic ToReal()
        {
            return new Webcomic {
                Name = this.Name,
                State = this.State
            };
        }
    }

    public class WebcomicResult
    {
        public int Id { get; set; }
        public int[] Authors { get; set; }
        public string Name { get; set; }
        public State State { get; set; }
    }
}
