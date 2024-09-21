// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Data.Models.PacketModel
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using System;

namespace PointBlank.Battle.Data.Models
{
  public class PacketModel
  {
    public int Opcode;
    public int Slot;
    public int Round;
    public int Length;
    public int AccountId;
    public int Unk;
    public int Respawn;
    public int RoundNumber;
    public float Time;
    public byte[] Data;
    public byte[] withEndData;
    public byte[] noEndData;
    public DateTime ReceiveDate;
  }
}
