﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ServerPacket.PROTOCOL_BASE_MAP_RULELIST_ACK
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Core.Models.Map;
using PointBlank.Core.Network;

namespace PointBlank.Auth.Network.ServerPacket
{
  public class PROTOCOL_BASE_MAP_RULELIST_ACK : SendPacket
  {
    public override void write()
    {
      this.writeH((short) 670);
      this.writeH((short) 0);
      this.writeH((short) MapModel.Rules.Count);
      for (int index = 0; index < MapModel.Rules.Count; ++index)
      {
        MapRule rule = MapModel.Rules[index];
        this.writeD(rule.Id);
        this.writeC((byte) 0);
        this.writeC((byte) rule.Rule);
        this.writeC((byte) rule.StageOptions);
        this.writeC((byte) rule.Conditions);
        this.writeC((byte) 0);
        this.writeS(rule.Name, 67);
      }
    }
  }
}
