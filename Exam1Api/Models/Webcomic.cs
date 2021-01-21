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
        public string Url { get; set; }

        public WebcomicResult ToResult()
        {
            var result = new WebcomicResult {
                id = this.Id,
                name = this.Name,
                state = this.State,
                picture = this.Picture,
                url = this.Url,
                authors = new int[]{}
            };

            if (AuthorWebcomics != null)
            {
                result.authors = AuthorWebcomics.Select(a => a.AuthorId).ToArray();
            }

            return result;
        }
    }

    public class WebcomicInput
    {
        public int[] authors { get; set; }
        [StringLength(60)]
        [Required]
        public string name { get; set; }
        public State state { get; set; }
        public string url { get; set; }

        public Webcomic ToReal()
        {
            return new Webcomic {
                Name = this.name,
                State = this.state,
                Url = this.url
            };
        }
    }

    public class WebcomicResult
    {
        public int id { get; set; }
        public int[] authors { get; set; }
        public string name { get; set; }
        public State state { get; set; }
        public byte[] picture { get; set; }
        public string url { get; set; }
    }
}
