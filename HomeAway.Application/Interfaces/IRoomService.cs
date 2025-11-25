using HomeAway.Application.DTOs;
using HomeAway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.Interfaces
{
    public interface IRoomService
    {
        Task<bool> CreateRoomAsync(RoomDto roomDto);

        Task<List<Room>> GetAllAsync();

        Task<RoomDto> GetRoomByIdAsync(int id);
        Task<RoomDto> UpdateAsync(RoomDto roomDto);
        Task<bool> DeleteAsync(int Id);
    }
}
