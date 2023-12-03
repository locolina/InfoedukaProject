namespace InfoedukaAPI.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime DateExpires { get; set; }
        public int UserID { get; set; }
        public int ClassID { get; set; }
    }
}
