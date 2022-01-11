using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GNB_TransRates.DAL.Models
{
    public class Rates : BaseModel
    {
        [Required]
        [StringLength(3)]

        public string? FromCurr { get; set; }
        [Required]
        [StringLength(3)]

        public string? ToCurr { get; set; }
        [Required]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
    }
}