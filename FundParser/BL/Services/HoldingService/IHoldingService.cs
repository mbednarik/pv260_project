﻿using BL.DTOs;
namespace BL.Services.HoldingService
{
    public interface IHoldingService
    {
        Task<IEnumerable<HoldingDTO>> GetHoldings();

        Task<HoldingDTO?> AddHolding(AddHoldingDTO holding);
    }
}
