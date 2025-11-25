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
using HomeAway.Domain.ValueObjects;


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
                Quantity = roomDto.Quantity,
                Type = roomDto.Type,
                Price = roomDto.Price,
                HotelId = roomDto.HotelId,

                //IsAvailable = roomDto.IsAvailable,
                //HotelId = _roomRepository.GetByNameAsync(roomDto.HotelName).Result.Id
            };

            await _roomRepository.AddAsync(room);
            return true;
        }

        public async Task<List<Room>> GetAllAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            return rooms;
        }

        public async Task<RoomDto> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return null;

            return new RoomDto
            {
                Id = room.Id,
                //Number = room.Number,
                Type = room.Type,
                Quantity = room.Quantity,
                IsAvailable = room.IsAvailable,
                HotelId = room.HotelId,
                //HotelName = room.Hotel.Name
            };
        }
        public async Task<RoomDto> UpdateAsync(RoomDto roomDto)
        {
            var room = await _roomRepository.GetByIdAsync(roomDto.Id);

            if (room != null)
            {
                room.Quantity = roomDto.Quantity;
                //room.Type = roomDto.Type;
                //room.IsAvailable = roomDto.IsAvailable;
                await _roomRepository.UpdateAsync(room);
                return roomDto;
            }
            return null;
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            var room = await _roomRepository.GetByIdAsync(Id);

            if (room != null)
            {
                await _roomRepository.DeleteAsync(room);
                return true;
            }
            return false;
        }

    }
}
