// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ServerPacket.PROTOCOL_BASE_USER_GIFTLIST_ACK
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Core.Models.Account;
using PointBlank.Core.Network;
using System;
using System.Collections.Generic;

namespace PointBlank.Auth.Network.ServerPacket
{
  public class PROTOCOL_BASE_USER_GIFTLIST_ACK : SendPacket
  {
    private int erro;
    private List<Message> gifts;

    public PROTOCOL_BASE_USER_GIFTLIST_ACK(int erro, List<Message> gifts)
    {
      this.erro = erro;
      this.gifts = gifts;
    }

    public override void write()
    {
      this.writeH((short) 684);
      this.writeH((short) 0);
      this.writeC((byte) this.gifts.Count);
      for (int index = 0; index < this.gifts.Count; ++index)
      {
        Message gift = this.gifts[index];
      }
      this.writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
    }
  }
}
