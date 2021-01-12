using System;
using System.Collections.Generic;
using System.Linq;
using Exam1Api.Data;
using Exam1Api.Models;

namespace Exam1Api.Services
{
    public class DbWebcomicService : IWebcomicService
    {
        private readonly Exam1ApiDataContext context;

        public DbWebcomicService(Exam1ApiDataContext context)
        {
            this.context = context;
            IWebcomicService.instance = this;
        }

        public Webcomic Create(Webcomic webcomic)
        {
            if (webcomic.Id < 1)
            {
                throw new ArgumentException("The id must be at least 1");
            }
            if (context.Webcomics.Any(w => w.Id == webcomic.Id))
            {
                throw new ArgumentException("The id is already in use");
            }
            if (context.Webcomics.Any(w => w.Name == webcomic.Name))
            {
                throw new ArgumentException("The name is already in use");
            }

            context.Webcomics.Add(webcomic);
            context.SaveChanges();

            return webcomic;
        }

        public Webcomic Create(WebcomicInput webcomic)
        {
            try
            {
                var new_webcomic = webcomic.ToReal();
                new_webcomic.Id = 1;

                if (context.Webcomics.Count() > 0)
                {
                    new_webcomic.Id = context.Webcomics.Select(w => w.Id).Max() + 1;
                }

                new_webcomic = Create(new_webcomic);

                if (webcomic?.Authors.Length > 0)
                {
                    foreach (int id in webcomic.Authors)
                    {
                        AddAuthor(new_webcomic.Id, id);
                    }
                }

                return new_webcomic;
            }
            catch
            {
                throw;
            }
        }

        public List<Webcomic> GetAll(string name = "")
        {
            return context.Webcomics.Where(w => w.Name.Contains(name)).ToList();
        }

        public Webcomic GetSingle(int id)
        {
            if (context.Webcomics.Any(w => w.Id == id))
            {
                return context.Webcomics.First(w => w.Id == id);
            }
            else
            {
                return null;
            }
        }

        public byte[] GetImage(int id)
        {
            return GetSingle(id).Picture;
        }

        public Webcomic Update(int id, Webcomic webcomic)
        {
            if (id < 1)
            {
                throw new ArgumentException("The id must be at least 1");
            }
            if (context.Webcomics.Any(w => w.Id != id && w.Name == webcomic.Name))
            {
                throw new ArgumentException("The name is already in use");
            }

            var old = GetSingle(id);

            if (old == null)
            {
                throw new NullReferenceException();
            }

            old.Name = webcomic.Name;
            old.State = webcomic.State;

            context.Webcomics.Update(old);
            context.SaveChanges();

            return old;
        }

        public Webcomic Update(int id, WebcomicInput webcomic)
        {
            try
            {
                return Update(id, webcomic.ToReal());
            }
            catch
            {
                throw;
            }
        }

        public void SetImage(int id, byte[] image)
        {
            var webcomic = GetSingle(id);
            if (webcomic == null)
            {
                throw new NullReferenceException();
            }

            webcomic.Picture = image;

            context.Webcomics.Update(webcomic);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var webcomic = GetSingle(id);
            if (webcomic != null)
            {
                context.Webcomics.Remove(webcomic);
                context.SaveChanges();
            }
        }

        public void AddAuthor(int webcomic, int author)
        {
            if (!context.Webcomics.Any(w => w.Id == webcomic))
            {
                throw new ArgumentException("Attempted to link a non existant webcomic");
            }
            if (!context.Authors.Any(a => a.Id == author))
            {
                throw new ArgumentException("Attempted to link a non existant author");
            }

            // Only link if there is no existing link
            if (!context.AuthorWebcomics.Any(link => link.WebcomicId == webcomic && link.AuthorId == author))
            {
                var link = new AuthorWebcomic {
                    WebcomicId = webcomic,
                    AuthorId = author,
                };

                context.AuthorWebcomics.Add(link);
                context.SaveChanges();
            }
        }

        public void RemoveAuthor(int webcomic, int author)
        {
            if (!context.Webcomics.Any(w => w.Id == webcomic))
            {
                throw new ArgumentException("Attempted to unlink a non existant webcomic");
            }
            if (!context.Authors.Any(a => a.Id == author))
            {
                throw new ArgumentException("Attempted to unlink a non existant author");
            }

            // Only unlink if there is a link
            if (context.AuthorWebcomics.Any(link => link.WebcomicId == webcomic && link.AuthorId == author))
            {
                var link = context.AuthorWebcomics.First(link => link.AuthorId == author && link.WebcomicId == webcomic);

                context.AuthorWebcomics.Remove(link);
                context.SaveChanges();
            }
        }
    }
}
