using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GNB_TransRates.DAL.Models
{
    public class Transactions : BaseModel
    {
        [Required]
        [StringLength(5)]
        public string? Sku { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal amount { get; set; }
        [Required]
        [StringLength(3)]
        public string? Currency { get; set; }
    }
}

    