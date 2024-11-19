using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petzweb.Models.Room
{
  public class PetRoom(RegisteredRoom room) : RegisteredRoom(room)
  {
    public int RoomTemperature { get; private set; } = room.AmbientRoomTemperature;
  }
}
