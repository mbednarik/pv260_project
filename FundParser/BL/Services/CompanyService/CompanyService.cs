using AutoMapper;
using BL.DTOs;
using DAL.Models;
using DAL.UnitOfWork.Interface;
using Microsoft.EntityFrameworkCore;

namespace BL.Services.CompanyService;

public class CompanyService : ICompanyService
{
    private readonly IUoWCompany _uow;
    private readonly IMapper _mapper;

    public CompanyService(IUoWCompany uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CompanyDTO?> AddCompanyIfNotExists(AddCompanyDTO company)
    {
        try
        {
            // Cusip should be unique
            var existingCompany = await _uow.CompanyRepository.GetQueryable()
                .FirstOrDefaultAsync(f => f.Cusip == company.Cusip);
            if (existingCompany != null)
            {
                return _mapper.Map<CompanyDTO>(existingCompany);
            }

            var newCompany = _uow.CompanyRepository.Insert(new Company
            {
                Cusip = company.Cusip,
                Ticker = company.Ticker,
                Name = company.Name,
            });
            await _uow.CommitAsync();
            return _mapper.Map<CompanyDTO>(newCompany);     
        }
        catch (Exception e)
        {
            Console.WriteLine("AddCompanyIfNotExists error: {0}", e.Message);
            return null;
        }
    }
}