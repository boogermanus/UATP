using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UATP.Core.Models;

public class Currency
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string Symbol { get; set; } = string.Empty;
}