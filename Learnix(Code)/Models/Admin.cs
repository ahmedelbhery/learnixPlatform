using Learnix.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class Admin
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; } 
        public ApplicationUser User { get; set; }
        public decimal Balance { get; set; } = 0;

    }
}
