using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models.Entities
{
    public class Investment
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public Guid ProjectId { get; set; }
        public decimal Amount { get; set; }
        
        public UserPortfolio UserPortfolio { get; set; }
    }
}