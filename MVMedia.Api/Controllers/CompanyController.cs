using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVMedia.Api.DTOs;
using MVMedia.Api.Identity;
using MVMedia.Api.Models;
using MVMedia.Api.Services.Interfaces;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class CompanyController : Controller
{

    private readonly ICompanyService _companyService;
    private readonly IAuthtenticate _authtenticateService;
    private readonly IUserService _userService;

    public CompanyController(ICompanyService companyService, IUserService userService, IAuthtenticate authtenticateService)
    {
        _companyService = companyService;
        _userService = userService;
        _authtenticateService = authtenticateService;
    }

    [HttpGet("GetAllCompanies")]
    public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
    {



        var companies = await _companyService.GetAllCompanies();
        if (companies.Any())
        {
            var users = await _userService.GetAllUsers();
            if (users.Any())
            {
                if (!User.Identity?.IsAuthenticated ?? true)
                    return Unauthorized("Usuário não autenticado.");




                // Usuário está logado: filtra Companies pelo CompanyId do usuário
                var userId = User.GetUserId();
                var user = await _userService.GetUser(userId);
                if (user == null)
                    return Unauthorized("Usuário não encontrado.");

                if (!user.IsAdmin)
                {
                    var filteredCompanies = companies.Where(c => c.Id == user.CompanyId);
                    return Ok(filteredCompanies);
                }
                else
                {
                    // Usuário é admin: retorna todas as Companies
                    return Ok(companies);
                }
            }
        }
        return Ok(companies);
    }
        // Se não existe Company ou usuário, retorna todas

    [HttpGet("GetCompany/{id}")]
    public async Task<ActionResult<Company>> GetCompanyById(int id)
    {
        if (!await _userService.IsAdmin(User.GetUserId()))
            return Unauthorized("You are not authorized to access this resource.");
        var company = await _companyService.GetCompanyById(id);
        if (company == null)
            return NotFound("Company not found.");
        return Ok(company);
    }

    [HttpPost]
    public async Task<ActionResult> AddCompany(CompanyAddDTO company)
    {
        //EXIST COMPANY
        var existingCompany = await _companyService.GetAllCompanies();
        if (existingCompany.Any())
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized("Usuário não autenticado.");

            if (!await _userService.IsAdmin(User.GetUserId()))
                return Unauthorized("You are not authorized to access this resource.");

            var addedCompany = await _companyService.AddCompany(company);
            return CreatedAtAction(nameof(GetCompanyById), new { id = addedCompany.Id }, addedCompany);
        }
        else
        {
            var addedCompany = await _companyService.AddCompany(company);
            return CreatedAtAction(nameof(GetCompanyById), new { id = addedCompany.Id }, addedCompany);
        }
    }

    [HttpPut]
    public async Task<ActionResult<Company>> UpdateCompany([FromBody] CompanyUpdateDTO companyDTO)
    {
        if (!await _userService.IsAdmin(User.GetUserId()))
            return Unauthorized("You are not authorized to access this resource.");

        if(companyDTO.Id == 0)
            return BadRequest("Company ID is required for update.");

        var existingCompany = await _companyService.GetCompanyById(companyDTO.Id);
        if (existingCompany == null)
            return NotFound("Company not found.");

        var updatedCompany = await _companyService.UpdateCompany(companyDTO);

        if (updatedCompany == null)
            return NotFound("Company not found.");

        return Ok(updatedCompany);
    }

}
