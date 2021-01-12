using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Exam1Api.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AuthorWebcomic> AuthorWebcomics { get; set; }

        public AuthorResult ToResult()
        {
            var result = new AuthorResult {
                Id = this.Id,
                Name = this.Name,
                Webcomics = new int[] {}
            };

            if (AuthorWebcomics?.Count > 0) {
                result.Webcomics = AuthorWebcomics.Select(w => w.WebcomicId).ToArray();
            }

            return result;
        }
    }

    public class AuthorInput
    {
        [StringLength(60)]
        [Required]
        public string Name { get; set; }
        public int[] Webcomics { get; set; }

        public Author ToReal()
        {
            return new Author{
                Name = this.Name
            };
        }
    }

    public class AuthorResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] Webcomics { get; set; }
    }
}
