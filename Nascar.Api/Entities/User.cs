#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nascar.Api.Entities;

[Table("User", Schema = "dbo")]
public class User
{
    [Key]
    public int ID { get; set; }
    public string Username { get; set; }
    public string Type { get; set; }
    public int Avatar {  get; set; }
}
