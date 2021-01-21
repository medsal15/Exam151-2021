using System.Collections.Generic;
using Exam1Api.Models;

namespace Exam1Api.Services
{
    public interface ISocialLinkService
    {
        public SocialLink Create(SocialLink socialLink);

        public SocialLink Create(SocialLinkInput socialLink);

        public List<SocialLink> GetAll(int author);

        public SocialLink GetSingle(int id);

        public SocialLink Update(int id, SocialLink socialLink);

        public SocialLink Update(int id, SocialLinkInput socialLink);

        public void Delete(int id);

        public void SetAuthor(int socialLink, int author);
    }
}
