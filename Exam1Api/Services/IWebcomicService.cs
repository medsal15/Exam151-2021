using System.Collections.Generic;
using Exam1Api.Models;

namespace Exam1Api.Services
{
    public interface IWebcomicService
    {
        public Webcomic Create(Webcomic webcomic);

        public Webcomic Create(WebcomicInput webcomic);

        public List<Webcomic> GetAll(string name);

        public Webcomic GetSingle(int id);

        public byte[] GetImage(int id);

        public Webcomic Update(int id, Webcomic webcomic);

        public Webcomic Update(int id, WebcomicInput webcomic);

        public void SetImage(int id, byte[] image);

        public void Delete(int id);

        public void AddAuthor(int webcomic, int author);

        public void RemoveAuthor(int webcomic, int author);
    }
}
