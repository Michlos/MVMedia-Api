namespace MVMedia.Api.DTOs;

public class CompanyAddDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CNPJ { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string? ZipCode { get; set; }
}
