using System;
using System.Collections.Generic;
using System.Linq;
using Exam1Api.Data;
using Exam1Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam1Api.Services
{
    public class DbSocialLinkService : ISocialLinkService
    {
        private readonly Exam1ApiDataContext context;

        public DbSocialLinkService(Exam1ApiDataContext context)
        {
            this.context = context;
        }

        public SocialLink Create(SocialLink socialLink)
        {
            if (socialLink.Id < 1)
            {
                throw new ArgumentException("The id must be at least 1");
            }
            if (context.SocialLinks.Any(l => l.Url == socialLink.Url && l.AuthorId == socialLink.AuthorId))
            {
                throw new ArgumentException("The url is already in use by the same author");
            }

            context.SocialLinks.Add(socialLink);
            context.SaveChanges();

            return socialLink;
        }

        public SocialLink Create(SocialLinkInput socialLink)
        {
            try
            {
                var link = socialLink.ToReal();
                link.Id = 1;

                if (context.SocialLinks.Count() > 0)
                {
                    link.Id = context.SocialLinks.Select(l => l.Id).Max() + 1;
                }

                return Create(link);
            }
            catch
            {
                throw;
            }
        }

        public List<SocialLink> GetAll(int author)
        {
            return context.SocialLinks.Where(l => l.AuthorId == author).ToList();
        }

        public SocialLink GetSingle(int id)
        {
            if (context.SocialLinks.Any(l => l.Id == id))
            {
                return context.SocialLinks.First(link => link.Id == id);
            }
            else
            {
                return null;
            }
        }

        public SocialLink Update(int id, SocialLink socialLink)
        {
            if (id < 1)
            {
                throw new ArgumentException("The id must be at least 1");
            }
            if (context.SocialLinks.Any(l => l.Id != id && l.Url == socialLink.Url && l.AuthorId == socialLink.AuthorId))
            {
                throw new ArgumentException("The url is already in use by the same author");
            }

            var old = GetSingle(id);

            if (old == null)
            {
                throw new NullReferenceException();
            }

            old.Url = socialLink.Url;
            old.AuthorId = socialLink.AuthorId;
            context.SocialLinks.Update(old);
            context.SaveChanges();

            return old;
        }

        public SocialLink Update(int id, SocialLinkInput socialLink)
        {
            try
            {
                return Update(id, socialLink.ToReal());
            }
            catch
            {
                throw;
            }
        }

        public void Delete(int id)
        {
            var socialLink = GetSingle(id);
            if (socialLink != null)
            {
                context.SocialLinks.Remove(socialLink);
                context.SaveChanges();
            }
        }

        public void SetAuthor(int socialLink, int author)
        {
            var realSocialLink = GetSingle(socialLink);
            if (realSocialLink == null || !context.Authors.Any(a => a.Id == author))
            {
                throw new NullReferenceException();
            }

            realSocialLink.AuthorId = author;
            context.SocialLinks.Update(realSocialLink);
            context.SaveChanges();
        }
    }
}
