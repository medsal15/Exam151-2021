using System;
using System.Collections.Generic;
using System.Linq;
using Exam1Api.Data;
using Exam1Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam1Api.Services
{
    public class DbAuthorService : IAuthorService
    {
        private readonly Exam1ApiDataContext context;

        public DbAuthorService(Exam1ApiDataContext context)
        {
            this.context = context;
        }

        public Author Create(Author author)
        {
            if (author.Id < 1)
            {
                throw new ArgumentException("The id must be at least 1");
            }
            if (context.Authors.Any(a => a.Id == author.Id))
            {
                throw new ArgumentException("The id is already in use");
            }
            if (context.Authors.Any(a => a.Name == author.Name))
            {
                throw new ArgumentException("The author already exists");
            }

            context.Authors.Add(author);
            context.SaveChanges();

            return author;
        }

        public Author Create(AuthorInput author)
        {
            try
            {
                var new_author = author.ToReal();
                new_author.Id = 1;

                if (context.Authors.Count() > 0)
                {
                    new_author.Id = context.Authors.Select(a => a.Id).Max() + 1;
                }

                new_author = Create(new_author);

                if (author.webcomics != null)
                {
                    foreach (int id in author.webcomics)
                    {
                        AddWebcomic(new_author.Id, id);
                    }
                }

                return new_author;
            }
            catch
            {
                throw;
            }
        }

        public List<Author> GetAll(string name = "")
        {
            return context.Authors.Include(a => a.AuthorWebcomics)
                .Include(a => a.SocialLinks)
                .Where(a => a.Name.Contains(name) || name.Length == 0).ToList();
        }

        public Author GetSingle(int id)
        {
            if (context.Authors.Any(a => a.Id == id))
            {
                return context.Authors.Include(a => a.AuthorWebcomics)
                    .Include(a => a.SocialLinks)
                    .First(a => a.Id == id);
            }
            else
            {
                return null;
            }
        }

        public Author Update(int id, Author author)
        {
            if (id < 1)
            {
                throw new ArgumentException("The id must be at least 1");
            }
            if (context.Authors.Any(a => a.Id != id && a.Name == author.Name))
            {
                throw new ArgumentException("The name is already in use");
            }

            var old = GetSingle(id);

            if (old == null)
            {
                throw new NullReferenceException();
            }

            old.Name = author.Name;
            context.Authors.Update(old);
            context.SaveChanges();

            return old;
        }

        public Author Update(int id, AuthorInput author)
        {
            try
            {
                var new_author = author.ToReal();

                new_author.Id = id;

                return Update(id, new_author);
            }
            catch
            {
                throw;
            }
        }

        public void Delete(int id)
        {
            var author = GetSingle(id);
            if (author != null)
            {
                var links = context.AuthorWebcomics.Where(link => link.AuthorId == id);
                foreach (var link in links) {
                    context.AuthorWebcomics.Remove(link);
                }

                context.Authors.Remove(author);
                context.SaveChanges();
            }
        }

        public void AddWebcomic(int author, int webcomic)
        {
            if (!context.Authors.Any(a => a.Id == author))
            {
                throw new ArgumentException("Attempted to link a non existant author");
            }
            if (!context.Webcomics.Any(w => w.Id == webcomic))
            {
                throw new ArgumentException("Attempted to link a non existant webcomic");
            }

            // Only link if there is no existing link
            if (!context.AuthorWebcomics.Any(link => link.AuthorId == author && link.WebcomicId == webcomic))
            {
                var link = new AuthorWebcomic {
                    AuthorId = author,
                    WebcomicId = webcomic
                };

                context.AuthorWebcomics.Add(link);
                context.SaveChanges();
            }
        }

        public void RemoveWebcomic(int author, int webcomic)
        {
            if (!context.Authors.Any(a => a.Id == author))
            {
                throw new ArgumentException("Attempted to unlink a non existant author");
            }
            if (!context.Webcomics.Any(w => w.Id == webcomic))
            {
                throw new ArgumentException("Attempted to unlink a non existant webcomic");
            }

            // Only unlink if there is a link
            if (context.AuthorWebcomics.Any(link => link.AuthorId == author && link.WebcomicId == webcomic))
            {
                var link = context.AuthorWebcomics.First(link => link.AuthorId == author && link.WebcomicId == webcomic);

                context.AuthorWebcomics.Remove(link);
                context.SaveChanges();
            }
        }

        public void AddSocialLink(int author, SocialLink socialLink)
        {
            if (socialLink == null)
            {
                throw new ArgumentNullException();
            }
            if (GetSingle(author) == null)
            {
                throw new NullReferenceException();
            }

            socialLink.AuthorId = author;
            context.SocialLinks.Add(socialLink);
            context.SaveChanges();
        }

        public void AddSocialLink(int author, SocialLinkInput socialLink)
        {
            try
            {
                var new_socialLink = socialLink.ToReal();
                new_socialLink.Id = 1;

                if (context.SocialLinks.Count() > 0)
                {
                    new_socialLink.Id = context.SocialLinks.Select(l => l.Id).Max() + 1;
                }

                AddSocialLink(author, new_socialLink);
            }
            catch
            {
                throw;
            }
        }

        public void RemoveSocialLink(int socialLink)
        {
            if (!context.SocialLinks.Any(s => s.Id == socialLink))
            {
                throw new ArgumentException();
            }

            var old = context.SocialLinks.First(s => s.Id == socialLink);
            context.SocialLinks.Remove(old);
            context.SaveChanges();
        }
    }
}
