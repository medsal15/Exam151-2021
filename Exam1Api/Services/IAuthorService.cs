using System.Collections.Generic;
using Exam1Api.Models;

namespace Exam1Api.Services
{
    public interface IAuthorService
    {
        /// <summary>
        /// Current instance of the service
        /// </summary>
        public static IAuthorService instance;

        /// <summary>
        /// Saves an author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public Author Create(Author author);

        /// <summary>
        /// Saves an author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public Author Create(AuthorInput author);

        /// <summary>
        /// Gets the full list of authors
        /// </summary>
        /// <param name="name">String that must be included in the name</param>
        /// <returns></returns>
        public List<Author> GetAll(string name);

        /// <summary>
        /// Gets a single author
        /// </summary>
        /// <param name="id">Id of the author to get</param>
        /// <returns></returns>
        public Author GetSingle(int id);

        /// <summary>
        /// Updates an author
        /// </summary>
        /// <param name="id">ID of the author to update</param>
        /// <param name="author">New values</param>
        /// <returns></returns>
        public Author Update(int id, Author author);

        /// <summary>
        /// Updates an author
        /// </summary>
        /// <param name="id">ID of the author to update</param>
        /// <param name="author">New values</param>
        /// <returns></returns>
        public Author Update(int id, AuthorInput author);

        /// <summary>
        /// Deletes an author
        /// </summary>
        /// <param name="id">ID of the author to delete</param>
        public void Delete(int id);

        /// <summary>
        /// Links an author and a webcomic
        /// </summary>
        /// <param name="author">Id of the author to link</param>
        /// <param name="webcomic">Id of the webcomic to link</param>
        public void AddWebcomic(int author, int webcomic);

        /// <summary>
        /// Unlinks an author and a webcomic
        /// </summary>
        /// <param name="author">Id of the author to unlink</param>
        /// <param name="webcomic">Id of the webcomic to unlink</param>
        public void RemoveWebcomic(int author, int webcomic);

        /// <summary>
        /// Adds a social link to an author
        /// </summary>
        /// <param name="author">ID of the author to link</param>
        /// <param name="socialLink">Social link to add</param>
        /// <returns></returns>
        public void AddSocialLink(int author, SocialLink socialLink);

        /// <summary>
        /// Adds a social link to an author
        /// </summary>
        /// <param name="author">ID of the author to link</param>
        /// <param name="socialLink">Social link to add</param>
        /// <returns></returns>
        public void AddSocialLink(int author, SocialLinkInput socialLink);

        /// <summary>
        /// Removes a social link from an author
        /// </summary>
        /// <param name="socialLink">ID of the social link to remove</param>
        /// <returns></returns>
        public void RemoveSocialLink(int socialLink);
    }
}
