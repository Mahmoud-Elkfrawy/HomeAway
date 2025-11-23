using HomeAway.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetAllAsync();
        Task<RoomDto> GetRoomByIdAsync(int id);
        Task<bool> CreateRoomAsync(CreateRoomDto roomDto);
    }
}
