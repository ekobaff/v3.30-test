// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.ActionForObjectSync
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models;
using PointBlank.Battle.Data.Models.Event.Event;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class ActionForObjectSync
  {
    public static ActionObjectInfo ReadSyncInfo(
      ActionModel ac,
      ReceivePacket p,
      bool genLog)
    {
      ActionObjectInfo actionObjectInfo = new ActionObjectInfo()
      {
        Unk1 = p.readC(),
        Unk2 = p.readC()
      };
      if (genLog)
        Logger.warning("Slot: " + ac.Slot.ToString() + " ActionForObjectSync: Unk (" + actionObjectInfo.Unk1.ToString() + ";" + actionObjectInfo.Unk2.ToString() + ")");
      return actionObjectInfo;
    }

    public static void WriteInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
    {
      ActionObjectInfo actionObjectInfo = ActionForObjectSync.ReadSyncInfo(ac, p, genLog);
      s.writeC(actionObjectInfo.Unk1);
      s.writeC(actionObjectInfo.Unk2);
    }
  }
}
