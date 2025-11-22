using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using HomeAway.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<bool> CreateHotelAsync(HotelDto hotelDto)
        {
            var hotel = new Domain.Entities.Hotel
            {
                Name = hotelDto.Name,
                Address = hotelDto.Address
            };

            await _hotelRepository.AddAsync(hotel);
            return true;
        }

        public async Task<List<HotelDto>> GetAllHotelsAsync()
        {
            var hotels = await _hotelRepository.GetAllAsync();
            return hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address
            }).ToList();
        }

        public async Task<HotelDto> GetHotelByIdAsync(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null) return null;

            return new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address
            };
        }
    }
}
