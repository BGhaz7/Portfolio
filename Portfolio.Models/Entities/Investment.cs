using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Entities
{
    public class Investment
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public Guid ProjectId { get; set; }
        public decimal Amount { get; set; }
    }
}