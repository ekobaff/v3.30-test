// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.FireData
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models;
using PointBlank.Battle.Data.Models.Event;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class FireData
  {
    public static FireDataInfo ReadInfo(
      ActionModel Action,
      ReceivePacket Packet,
      bool Log)
    {
      FireDataInfo fireDataInfo = new FireDataInfo()
      {
        Effect = Packet.readC(),
        Part = Packet.readC(),
        Index = Packet.readH(),
        X = Packet.readUH(),
        Y = Packet.readUH(),
        Z = Packet.readUH(),
        Extensions = Packet.readC(),
        WeaponId = Packet.readD()
      };
      if (Log)
      {
        Logger.warning("[1] Effect: " + ((int) fireDataInfo.Effect >> 4).ToString() + "; Slot: " + ((int) fireDataInfo.Effect & 15).ToString());
        Logger.warning("[2] Slot: " + Action.Slot.ToString() + " FireData: " + fireDataInfo.Effect.ToString() + ";" + fireDataInfo.Part.ToString());
      }
      return fireDataInfo;
    }

    public static void ReadInfo(ReceivePacket Packet) => Packet.Advance(15);

    public static void WriteInfo(
      SendPacket Send,
      ActionModel Action,
      ReceivePacket Packet,
      bool Log)
    {
      FireDataInfo Info = FireData.ReadInfo(Action, Packet, Log);
      FireData.WriteInfo(Send, Info);
    }

    public static void WriteInfo(SendPacket Send, FireDataInfo Info)
    {
      Send.writeC(Info.Effect);
      Send.writeC(Info.Part);
      Send.writeH(Info.Index);
      Send.writeH(Info.X);
      Send.writeH(Info.Y);
      Send.writeH(Info.Z);
      Send.writeC(Info.Extensions);
      Send.writeD(Info.WeaponId);
    }
  }
}
