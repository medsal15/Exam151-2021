using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Exam1Api.Models
{
    public class SocialLink
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public SocialLinkResult ToResult()
        {
            return new SocialLinkResult {
                id = Id,
                url = Url,
                authorId = AuthorId
            };
        }
    }

    public class SocialLinkInput
    {
        [Required]
        [StringLength(100)]
        public string url { get; set; }
        [Required]
        public int authorId { get; set; }
        public SocialLink ToReal()
        {
            return new SocialLink {
                Url = url,
                AuthorId = authorId
            };
        }
    }

    public class SocialLinkResult
    {
        public int id { get; set; }
        public string url { get; set; }
        public int authorId { get; set; }
    }
}
