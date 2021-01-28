using Exam1Api.Data;
using Exam1Api.Enums;
using Exam1Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Exam1Api.Tests.Controllers
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                // Set database to an in-memory one
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<Exam1ApiDataContext>));
                if (descriptor == null) return;

                services.Remove(descriptor);

                services.AddDbContext<Exam1ApiDataContext>(options => {
                    options.UseInMemoryDatabase("Exam1Api");
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<Exam1ApiDataContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        ResetInMemoryDatabase(db);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, e.Message);
                    }
                }
            });
        }

        private static void ResetInMemoryDatabase(Exam1ApiDataContext db)
        {
            // Remove existing data
            db.Remove(db.AuthorWebcomics);
            db.Remove(db.Webcomics);
            db.Remove(db.SocialLinks);
            db.Remove(db.Authors);
            db.SaveChanges();

            // Add dummy data
            db.AddRange(new List<Webcomic> {
                new Webcomic { Id = 1, Name = "Homestuck", State = State.Finished, Url = "http://www.homestuck.com" },
                new Webcomic { Id = 2, Name = "Fortuna", State = State.Hiatus, Url = "http://cosmosdex.com" },
                new Webcomic { Id = 3, Name = "Inverted Fate", State = State.Finished, Url = "http://invertedfate.com" },
            });
            db.AddRange(new List<Author> {
                new Author { Id = 1, Name = "Andrew Hussie" },
                new Author { Id = 2, Name = "o" },
            });
            db.AddRange(new List<AuthorWebcomic> {
                new AuthorWebcomic { Id = 1, AuthorId = 1, WebcomicId = 1 },
                new AuthorWebcomic { Id = 2, AuthorId = 1, WebcomicId = 2 },
            });
            db.AddRange(new List<SocialLink> {
                new SocialLink { Id = 1, Url = "https://twitter.com/andrewhussie", AuthorId = 1 },
            });
            db.SaveChanges();
        }
    }

    public class ApiControllerTestBase : CustomWebApplicationFactory<Startup>
    {
        private CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        [TestInitialize]
        public void SetupTest()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        protected async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await _client.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(body);
        }

        protected async Task<HttpResponseMessage> PostBasicAsync<T>(string url, T body)
        {
            return await _client.PostAsJsonAsync(url, body);
        }

        protected async Task<HttpResponseMessage> PostFileAsync(string url, byte[] file)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(file), "file", "filename");
            return await _client.PostAsync(url, content);
        }

        protected async Task<T> PostAsync<T>(string url, T body)
        {
            return await PostAsync<T, T>(url, body);
        }

        protected async Task<U> PostAsync<T, U>(string url, T body)
        {
            var response = await _client.PostAsJsonAsync(url, body);

            return await response.Content.ReadFromJsonAsync<U>();
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await _client.DeleteAsync(url);

            return response.EnsureSuccessStatusCode();
        }
    }
}
