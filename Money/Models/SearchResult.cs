using System.ComponentModel.DataAnnotations;

namespace Money.Models
{
    public class SearchResult
    {
        [Key]
        public int RecordID { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
    }
}
