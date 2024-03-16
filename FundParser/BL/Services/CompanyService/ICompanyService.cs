using BL.DTOs;

namespace BL.Services.CompanyService;

public interface ICompanyService
{
    Task<CompanyDTO?> PrepareCompanyIfNotExists(AddCompanyDTO company);
}