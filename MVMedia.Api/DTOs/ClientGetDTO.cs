using System.ComponentModel.DataAnnotations;

namespace MVMedia.Api.DTOs;

public class ClientGetDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(200)]

    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string? CPF { get; set; }
    public string? CNPJ { get; set; }

    [Required]
    [MaxLength(200)]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Required]
    [MaxLength(15)]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }
    [MaxLength(200)]
    public string? Address { get; set; }
    [MaxLength(200)]
    public string? City { get; set; }
    [MaxLength(2)]
    public string? State { get; set; }
    [MaxLength(10)]
    [DataType(DataType.PostalCode)]
    [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "Invalid Zip Code format. Use XXXXX-XXX.")]
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
