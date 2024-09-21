// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.UseObject
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Enums;
using PointBlank.Battle.Data.Models;
using PointBlank.Battle.Data.Models.Event;
using System.Collections.Generic;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class UseObject
  {
    public static List<UseObjectInfo> ReadSyncInfo(
      ActionModel ac,
      ReceivePacket p,
      bool genLog)
    {
      List<UseObjectInfo> useObjectInfoList = new List<UseObjectInfo>();
      int num = (int) p.readC();
      for (int index = 0; index < num; ++index)
      {
        UseObjectInfo useObjectInfo = new UseObjectInfo()
        {
          Use = p.readC(),
          SpaceFlags = (CHARA_MOVES) p.readC(),
          ObjectId = p.readUH()
        };
        if (genLog)
          Logger.warning("Slot: " + ac.Slot.ToString() + " UseObject: Flag: " + useObjectInfo.SpaceFlags.ToString() + " ObjectId: " + useObjectInfo.ObjectId.ToString());
        useObjectInfoList.Add(useObjectInfo);
      }
      return useObjectInfoList;
    }

    public static void WriteInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
    {
      List<UseObjectInfo> Infos = UseObject.ReadSyncInfo(ac, p, genLog);
      UseObject.WriteInfo(s, Infos);
    }

    public static void WriteInfo(SendPacket s, List<UseObjectInfo> Infos)
    {
      s.writeC((byte) Infos.Count);
      for (int index = 0; index < Infos.Count; ++index)
      {
        UseObjectInfo info = Infos[index];
        s.writeC(info.Use);
        s.writeC((byte) info.SpaceFlags);
        s.writeH(info.ObjectId);
      }
    }
  }
}
