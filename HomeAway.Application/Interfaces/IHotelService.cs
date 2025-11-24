using HomeAway.Application.DTOs;
using HomeAway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.Interfaces
{
    public interface IHotelService
    {
        Task<List<HotelDto>> GetAllAsync();

        Task<HotelDto> GetByIdAsync(int id);

        Task<int> CreateAsync(HotelDto dto);

        Task<bool> UpdateAsync(HotelDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
