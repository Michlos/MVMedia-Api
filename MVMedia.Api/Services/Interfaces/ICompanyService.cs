using MVMedia.Api.DTOs;
using MVMedia.Api.Models;

namespace MVMedia.Api.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanyAddDTO> AddCompany(CompanyAddDTO company);
    Task<CompanyUpdateDTO> UpdateCompany(CompanyUpdateDTO company);
    Task<Company> CompanyIsActive(CompanyIsActiveDTO company);
    Task<IEnumerable<Company>> GetAllCompanies();
    Task<Company> GetCompanyById(int id);

}
