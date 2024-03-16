using BL.DTOs;

namespace BL.Services.CompanyService;

public interface ICompanyService
{
    Task<CompanyDTO?> AddCompanyIfNotExists(AddCompanyDTO company);
}