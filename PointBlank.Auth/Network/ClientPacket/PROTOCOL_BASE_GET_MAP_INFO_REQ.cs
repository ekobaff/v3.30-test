// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ClientPacket.PROTOCOL_BASE_GET_MAP_INFO_REQ
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Network.ServerPacket;
using PointBlank.Core.Models.Map;
using PointBlank.Core.Network;
using System.Collections.Generic;
using System.Linq;

namespace PointBlank.Auth.Network.ClientPacket
{
  public class PROTOCOL_BASE_GET_MAP_INFO_REQ : ReceivePacket
  {
    public PROTOCOL_BASE_GET_MAP_INFO_REQ(AuthClient Client, byte[] Buffer) => this.makeme(Client, Buffer);

    public override void read()
    {
    }

    public override void run()
    {
      this._client.SendPacket((SendPacket) new PROTOCOL_BASE_MAP_RULELIST_ACK());
      IEnumerable<IEnumerable<MapMatch>> mapMatches = MapModel.Matchs.Split<MapMatch>(100);
      int Total = 0;
      foreach (IEnumerable<MapMatch> source in mapMatches)
      {
        Total += 100;
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_MAP_MATCHINGLIST_ACK(source.ToList<MapMatch>(), Total));
      }
    }
  }
}
