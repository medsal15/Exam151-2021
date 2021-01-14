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
                Id = this.Id,
                Url = this.Url,
                AuthorId = this.AuthorId
            };
        }
    }

    public class SocialLinkInput
    {
        [Required]
        [StringLength(100)]
        public string Url { get; set; }
        public SocialLink ToReal()
        {
            return new SocialLink {
                Url = this.Url
            };
        }
    }

    public class SocialLinkResult
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int AuthorId { get; set; }
    }
}
