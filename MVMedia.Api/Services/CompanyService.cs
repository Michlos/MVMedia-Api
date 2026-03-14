using AutoMapper;
using MVMedia.Api.DTOs;
using MVMedia.Api.Models;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Services.Interfaces;

namespace MVMedia.Api.Services;

public class CompanyService : ICompanyService
{
    public readonly ICompanyRepository _companyRepository;
    public readonly IMapper _mapper;

    public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<CompanyAddDTO> AddCompany(CompanyAddDTO companyAddDTO)
    {
        var company = _mapper.Map<Company>(companyAddDTO);
        var companyAdded = await _companyRepository.AddCompany(company);
        return _mapper.Map<CompanyAddDTO>(companyAdded);
    }

    public async Task<Company> CompanyIsActive(CompanyIsActiveDTO company)
    {
        var existingCompany = _companyRepository.GetCompanyById(company.Id).Result;


        if (existingCompany == null)
        {
            throw new Exception("Company not found.");
        }
        else
        {

            CompanyIsActiveDTO companyDTO = _mapper.Map<CompanyIsActiveDTO>(existingCompany);
            companyDTO.IsActive = company.IsActive;
            await _companyRepository.CompanyIsActive(companyDTO);
        }

        return existingCompany;
    }

    public async Task<IEnumerable<Company>> GetAllCompanies()
    {
        var companies = await _companyRepository.GetAllCompanies();
        return companies;
    }

    public async Task<Company> GetCompanyById(int id)
    {
        var company = await _companyRepository.GetCompanyById(id);
        return company;
    }

    public async Task<CompanyUpdateDTO> UpdateCompany(CompanyUpdateDTO companyUpdateDTO)
    {
        var existingCompany = await _companyRepository.GetCompanyById(companyUpdateDTO.Id);

        var companyUpdated = await _companyRepository.UpdateCompany(companyUpdateDTO);

        return _mapper.Map<CompanyUpdateDTO>(companyUpdated);
    }


}
