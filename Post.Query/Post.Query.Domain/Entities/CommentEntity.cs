using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Post.Query.Domain.Entities
{
    [Table("Comment",Schema ="dbo")]
    public class CommentEntity
    {
        [Key]
        public Guid CommentId { get; set; }
        public required string Username { get; set; }
        public DateTime CommentData { get; set; }
        public required string Comment { get; set; }
        public bool IsEdited { get; set; }
        public Guid PostId { get; set; }

        [JsonIgnore]
        public virtual PostEntity Post { get; set; }
    }
}