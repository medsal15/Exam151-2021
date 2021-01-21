using System.Collections.Generic;
using Exam1Api.Models;

namespace Exam1Api.Services
{
    public interface IAuthorService
    {
        public Author Create(Author author);

        public Author Create(AuthorInput author);

        public List<Author> GetAll(string name);

        public Author GetSingle(int id);

        public Author Update(int id, Author author);

        public Author Update(int id, AuthorInput author);

        public void Delete(int id);

        public void AddWebcomic(int author, int webcomic);

        public void RemoveWebcomic(int author, int webcomic);

        public void AddSocialLink(int author, SocialLink socialLink);

        public void AddSocialLink(int author, SocialLinkInput socialLink);

        public void RemoveSocialLink(int socialLink);
    }
}
