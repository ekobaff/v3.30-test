// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.RoomsManager
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data;
using PointBlank.Battle.Data.Enums;
using PointBlank.Battle.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PointBlank.Battle.Network
{
  public class RoomsManager
  {
        public static List<AssistModel> Assists = new List<AssistModel>();
        private static List<Room> list = new List<Room>();

    public static Room CreateOrGetRoom(uint UniqueRoomId, uint Seed)
    {
      lock (RoomsManager.list)
      {
        for (int index = 0; index < RoomsManager.list.Count; ++index)
        {
          Room room = RoomsManager.list[index];
          if ((int) room.UniqueRoomId == (int) UniqueRoomId)
            return room;
        }
        int roomInfo1 = AllUtils.GetRoomInfo(UniqueRoomId, 2);
        int roomInfo2 = AllUtils.GetRoomInfo(UniqueRoomId, 1);
        int roomInfo3 = AllUtils.GetRoomInfo(UniqueRoomId, 0);
        Room room1 = new Room(roomInfo1)
        {
          UniqueRoomId = UniqueRoomId,
          Seed = Seed,
          RoomId = roomInfo3,
          ChannelId = roomInfo2,
          MapId = (MAP_STATE_ID) AllUtils.GetSeedInfo(Seed, 2),
          RoomType = (ROOM_STATE_TYPE) AllUtils.GetSeedInfo(Seed, 0),
          Rule = AllUtils.GetSeedInfo(Seed, 1)
        };
        RoomsManager.list.Add(room1);
        return room1;
      }
    }

    public static Room getRoom(uint UniqueRoomId)
    {
      lock (RoomsManager.list)
      {
        for (int index = 0; index < RoomsManager.list.Count; ++index)
        {
          Room room = RoomsManager.list[index];
          if (room != null && (int) room.UniqueRoomId == (int) UniqueRoomId)
            return room;
        }
        return (Room) null;
      }
    }

    public static Room getRoom(uint UniqueRoomId, uint Seed)
    {
      lock (RoomsManager.list)
      {
                lock (RoomsManager.list)
                {
                    return RoomsManager.list.FirstOrDefault(x => x.UniqueRoomId == UniqueRoomId && x.Seed == Seed);

                    //for (int index = 0; index < RoomsManager.list.Count; ++index)
                    //{
                    //  Room room = RoomsManager.list[index];
                    //  if (room != null && (int) room.UniqueRoomId == (int) UniqueRoomId && (int) room.Seed == (int) Seed)
                    //    return room;
                    //}
                    //return (Room) null;
                }
            }
    }

    public static bool getRoom(uint UniqueRoomId, out Room room)
    {
      room = (Room) null;
      lock (RoomsManager.list)
      {
        for (int index = 0; index < RoomsManager.list.Count; ++index)
        {
          Room room1 = RoomsManager.list[index];
          if (room1 != null && (int) room1.UniqueRoomId == (int) UniqueRoomId)
          {
            room = room1;
            return true;
          }
        }
      }
      return false;
    }

    public static void RemoveRoom(uint UniqueRoomId)
    {
      try
      {
                lock (RoomsManager.Assists)
                {
                    AssistModel assistModel1 = RoomsManager.Assists.Find((Predicate<AssistModel>)(x => (long)x.RoomId == (long)UniqueRoomId));
                    RoomsManager.Assists.Remove(assistModel1);
                    foreach (AssistModel assistModel2 in RoomsManager.Assists.FindAll((Predicate<AssistModel>)(x => (long)x.RoomId == (long)UniqueRoomId)))
                        RoomsManager.Assists.Remove(assistModel2);
                }
                lock (RoomsManager.list)
        {
          for (int index = 0; index < RoomsManager.list.Count; ++index)
          {
            if ((int) RoomsManager.list[index].UniqueRoomId == (int) UniqueRoomId)
            {
              RoomsManager.list.RemoveAt(index);
              break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
      }
    }
  }
}
