using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeAway.Domain.Entities;
using HomeAway.Domain.Enums;
using HomeAway.Domain.Interfaces;


namespace HomeAway.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<bool> CreateRoomAsync(RoomDto roomDto)
        {
            var room = new Room
            {
                Number = roomDto.Number,
                Capacity = roomDto.Capacity,
                Type = Enum.Parse<RoomType>(roomDto.Type),
                IsAvailable = roomDto.IsAvailable
            };

            await _roomRepository.AddAsync(room);
            return true;
        }

        public async Task<List<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            return rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                Number = r.Number,
                Type = r.Type.ToString(),
                Capacity = r.Capacity,
                IsAvailable = r.IsAvailable,
                HotelName = r.Hotel.Name
            }).ToList();
        }

        public async Task<RoomDto> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return null;

            return new RoomDto
            {
                Id = room.Id,
                Number = room.Number,
                Type = room.Type.ToString(),
                Capacity = room.Capacity,
                IsAvailable = room.IsAvailable,
                HotelName = room.Hotel.Name
            };
        }
    }
}
