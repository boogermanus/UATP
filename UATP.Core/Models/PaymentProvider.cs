using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UATP.Core.Models;

public class PaymentProvider
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [MaxLength(30)]
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
}