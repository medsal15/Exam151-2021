namespace Exam1Api.Models
{
    public class AuthorWebcomic
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int WebcomicId { get; set; }
        public Webcomic Webcomic { get; set; }
    }
}
