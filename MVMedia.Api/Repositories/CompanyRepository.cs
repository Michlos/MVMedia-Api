using Microsoft.EntityFrameworkCore;
using MVMedia.Api.Context;
using MVMedia.Api.DTOs;
using MVMedia.Api.Models;
using MVMedia.Api.Repositories.Interfaces;

namespace MVMedia.Api.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApiDbContext _context;

    public CompanyRepository(ApiDbContext context)
    {
        _context = context;
    }
    public async Task<Company> AddCompany(Company company)
    {
        company.CreatedAt = DateTime.UtcNow;
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<IEnumerable<Company>> GetAllCompanies()
    {
        return await _context.Companies.ToListAsync();
    }

    public async Task<Company> GetCompanyById(int id)
    {
        return await _context.Companies.Where(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Company> CompanyIsActive(CompanyIsActiveDTO company)
    {
        var existsCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Id == company.Id);
        if (existsCompany == null)
        {
            throw new ArgumentException($"Company with id {company.Id} not found");
        }
        else
        {
            if (company.IsActive)
            {
                existsCompany.UpdatedAt = DateTime.UtcNow;
                company.IsActive = false;
                
            }
            else
            {
                //_context.Companies.Remove(existsCompany);
                company.IsActive = true;
                
            }
        }
        await _context.SaveChangesAsync();
        return existsCompany;
    }

    public async Task<Company> UpdateCompany(CompanyUpdateDTO company)
    {
        var existsCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Id == company.Id);
        if (existsCompany == null)
            throw new ArgumentException($"Company with id {company.Id} not found");

        //Atualize as propriedades diretamente
        if (company.Name is not null && company.Name != existsCompany.Name)
            existsCompany.Name = company.Name;
        if (company.CNPJ is not null && company.CNPJ != existsCompany.CNPJ)
            existsCompany.CNPJ = company.CNPJ;
        if (company.Email is not null && company.Email != existsCompany.Email)
            existsCompany.Email = company.Email;
        if (company.Phone is not null && company.Phone != existsCompany.Phone)
            existsCompany.Phone = company.Phone;
        if (company.Address is not null && company.Address != existsCompany.Address)
            existsCompany.Address = company.Address;
        if (company.City is not null && company.City != existsCompany.City)
            existsCompany.City = company.City;
        if (company.State is not null && company.State != existsCompany.State)
            existsCompany.State = company.State;
        if (company.ZipCode is not null && company.ZipCode != existsCompany.ZipCode)
            existsCompany.ZipCode = company.ZipCode;


        existsCompany.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existsCompany;
    }
}
