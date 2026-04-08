using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpsilonWebApp.Models;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Means that SQL server will generate the GUID on insert
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? CompanyName { get; set; }

    [MaxLength(100)]
    public string? ContactName { get; set; }

    [MaxLength(200)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(50)]
    public string? Region { get; set; }

    [MaxLength(20)]
    public string? PostalCode { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    [MaxLength(30)]
    public string? Phone { get; set; }
}
