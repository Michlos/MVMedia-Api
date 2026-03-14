using MVMedia.Api.DTOs;
using MVMedia.Api.Models;

namespace MVMedia.Api.Repositories.Interfaces;

public interface ICompanyRepository
{
    Task<Company> AddCompany(Company company);
    Task<Company> UpdateCompany(CompanyUpdateDTO company);
    Task<Company> CompanyIsActive(CompanyIsActiveDTO company);
    Task<IEnumerable<Company>> GetAllCompanies();
    Task<Company> GetCompanyById(int id);

    Task<bool> SaveAllAsync();
}
