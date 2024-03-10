using AutoMapper;
using BL.DTOs;
using DAL.Models;
using DAL.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.HoldingService
{
    public class HoldingService : IHoldingService
    {
        private readonly IUoWHolding uow;
        private readonly IMapper mapper;

        public HoldingService(IUoWHolding uow, 
            IMapper mapper) 
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<HoldingDTO>> GetHoldings()
        {
            var holdings = await uow.HoldingRepository.GetAll();
            return mapper.Map<IEnumerable<HoldingDTO>>(holdings.ToList());
        }
    }
}
