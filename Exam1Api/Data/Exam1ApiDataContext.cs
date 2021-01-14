using Exam1Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam1Api.Data
{
    public class Exam1ApiDataContext : DbContext
    {
        public DbSet<Webcomic> Webcomics { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorWebcomic> AuthorWebcomics { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }

        public Exam1ApiDataContext(DbContextOptions<Exam1ApiDataContext> options) : base(options)
        {
        }
    }
}
