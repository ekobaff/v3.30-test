// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.ActionState
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Enums;
using PointBlank.Battle.Data.Models;
using PointBlank.Battle.Data.Models.Event;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class ActionState
  {
    public static ActionStateInfo ReadInfo(
      ReceivePacket p,
      ActionModel ac,
      bool genLog)
    {
      ActionStateInfo actionStateInfo = new ActionStateInfo()
      {
        Action = (ACTION_STATE) p.readUH(),
        Value = p.readC(),
        Flag = (WEAPON_SYNC_TYPE) p.readC()
      };
      if (!genLog)
        ;
      return actionStateInfo;
    }

    public static void WriteInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
    {
      ActionStateInfo info = ActionState.ReadInfo(p, ac, genLog);
      ActionState.WriteInfo(s, info);
    }

    public static void WriteInfo(SendPacket s, ActionStateInfo info)
    {
      s.writeH((ushort) info.Action);
      s.writeC(info.Value);
      s.writeC((byte) info.Flag);
    }
  }
}
