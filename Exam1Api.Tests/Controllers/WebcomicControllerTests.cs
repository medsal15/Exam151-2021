using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Exam1Api.Enums;
using Exam1Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exam1Api.Tests.Controllers
{
    [TestClass]
    public class WebcomicControllerTests : ApiControllerTestBase
    {
        [TestMethod, TestCategory("webcomics")]
        public async Task WebcomicsGetAll()
        {
            // Act
            var response = await GetAsync<List<WebcomicResult>>("/webcomics");

            // Assert
            Assert.AreEqual(3, response.Count);
        }

        [TestMethod, TestCategory("webcomics")]
        [DataRow("uck", 1)]
        [DataRow("", 3)]
        [DataRow("t", 3)]
        public async Task WebcomicNameOk(string name, int expected)
        {
            // Act
            var response = await GetAsync<List<Webcomic>>($"/webcomics?name={name}");

            // Assert
            Assert.AreEqual(expected, response.Count);
        }

        [TestMethod, TestCategory("webcomics")]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task GetSingleOk(int id)
        {
            // Act
            var response = await GetAsync<WebcomicResult>($"/webcomics/{id}");

            // Assert
            Assert.AreEqual(id, response.id);
        }

        [TestMethod, TestCategory("webcomics")]
        [DataRow("Oceanfalls", "https://oceanfalls.net", State.Ongoing)]
        public async Task WebcomicCreate(string name, string url, State state)
        {
            // Arrange
            var webcomic = new WebcomicInput { name = name, state = state, url = url };

            // Act
            var response = await PostAsync<WebcomicInput, WebcomicResult>("/webcomics", webcomic);

            // Assert
            Assert.AreEqual(name, response.name);
            Assert.AreEqual(url, response.url);
            Assert.AreEqual(state, response.state);
        }

        [TestMethod, TestCategory("webcomics")]
        [DataRow(3, "Inverted Fate", "http://invertedfate.com", State.Ongoing)]
        public async Task WebcomicUpdate(int id, string name, string url, State state)
        {
            // Arrange
            var webcomic = new WebcomicInput { name = name, state = state, url = url };

            // Act
            var response = await PostAsync<WebcomicInput, WebcomicResult>($"/webcomics/{id}", webcomic);

            // Assert
            Assert.AreEqual(name, response.name);
            Assert.AreEqual(url, response.url);
            Assert.AreEqual(state, response.state);
        }

        [TestMethod, TestCategory("webcomics")]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task WebcomicDelete(int id)
        {
            // Act
            var response = await DeleteAsync($"/webcomics/{id}");

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("webcomics")]
        [DataRow(5)]
        [DataRow(-1)]
        public async Task WebcomicNotExist(int id)
        {
            // Act
            var response = await GetAsync($"/webcomics/{id}");

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("webcomics")]
        [DataRow(3, 2)]
        public async Task WebcomicLinkAuthor(int webcomic, int author)
        {
            // Act
            var response = await GetAsync($"/webcomics/link/{webcomic}/{author}");

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, (int)response.StatusCode);

            var comic = await GetAsync<WebcomicResult>($"/webcomics/{webcomic}");
            Assert.IsTrue(comic.authors.Any(a => a == author));
        }

        [TestMethod, TestCategory("webcomics")]
        [DataRow(2, 1)]
        public async Task WebcomicUnlinkAuthor(int webcomic, int author)
        {
            // Act
            var response = await GetAsync($"/webcomics/unlink/{webcomic}/{author}");
            var comic = await GetAsync<WebcomicResult>($"/webcomics/{webcomic}");

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, (int)response.StatusCode);
            Assert.IsFalse(comic.authors.Any(a => a == author));
        }

        [TestMethod, TestCategory("webcomics")]
        public async Task WebcomicSetPicture()
        {
            // Arrange
            var file = File.ReadAllBytes("picture.png");

            // Act
            var response = await PostFileAsync($"/webcomics/{1}/image", file);

            // Assert
            var webcomic = await GetAsync<WebcomicResult>($"/webcomics/{1}");
            Assert.IsNotNull(webcomic.picture);
            Assert.IsTrue(file.SequenceEqual(webcomic.picture));
        }
    }
}
