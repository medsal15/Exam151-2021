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
        public List<SocialLink> SocialLinks { get; set; }

        public AuthorResult ToResult()
        {
            var result = new AuthorResult {
                id = this.Id,
                name = this.Name,
                webcomics = new int[] {},
                socialLinks = new SocialLinkResult[] {},
            };

            if (AuthorWebcomics != null)
            {
                result.webcomics = AuthorWebcomics.Select(w => w.WebcomicId).ToArray();
            }
            if (SocialLinks != null)
            {
                result.socialLinks = SocialLinks.Select(l => l.ToResult()).ToArray();
            }

            return result;
        }
    }

    public class AuthorInput
    {
        [StringLength(60)]
        [Required]
        public string name { get; set; }
        public int[] webcomics { get; set; }

        public Author ToReal()
        {
            return new Author{
                Name = this.name
            };
        }
    }

    public class AuthorResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public int[] webcomics { get; set; }
        public SocialLinkResult[] socialLinks { get; set; }
    }
}
