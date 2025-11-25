using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using HomeAway.Domain.Entities;
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
        public async Task<List<HotelDto>> GetAllAsync()
        {
            var hotels = await _hotelRepository.GetAllAsync();
            return hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address
            }).ToList();
        }

        public async Task<HotelDto> GetByIdAsync(int id)
        {
            var h = await _hotelRepository.GetByIdAsync(id);
            if (h == null) return null;

            return new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address

            };
        }

        public async Task<int> CreateAsync(HotelDto dto)
        {
            var hotel = new Hotel
            {
                Name = dto.Name,
                Description = dto.Description,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                images = dto.images

            };

            await _hotelRepository.AddAsync(hotel);
            return hotel.Id;
        }

        public async Task<bool> UpdateAsync(HotelDto dto)
        {
            var hotel = await _hotelRepository.GetByIdAsync(dto.Id);
            if (hotel == null) return false;

            hotel.Name = dto.Name;
            hotel.Address = dto.Address;
            hotel.Description = dto.Description;
            hotel.Email = dto.Email;
            hotel.PhoneNumber = dto.PhoneNumber;
            hotel.images = dto.images;

            await _hotelRepository.UpdateAsync(hotel);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null) return false;

            await _hotelRepository.DeleteAsync(hotel);
            return true;
        }
    }
}
