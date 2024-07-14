using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Entities
{
    public class UserPortfolio
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<Investment> Investments { get; set; }
    }
}