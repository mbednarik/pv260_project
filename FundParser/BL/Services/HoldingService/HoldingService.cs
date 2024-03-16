using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork.Interface;

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
