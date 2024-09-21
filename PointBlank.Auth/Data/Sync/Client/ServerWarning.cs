// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Data.Sync.Client.ServerWarning
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Data.Managers;
using PointBlank.Auth.Data.Model;
using PointBlank.Auth.Network.ServerPacket;
using PointBlank.Core;
using PointBlank.Core.Network;
using System;

namespace PointBlank.Auth.Data.Sync.Client
{
  public static class ServerWarning
  {
    public static void LoadGMWarning(ReceiveGPacket p)
    {
      string str1 = p.readS((int) p.readC());
      string text = p.readS((int) p.readC());
      string msg = p.readS((int) p.readH());
      string str2 = ComDiv.gen5(text);
      Account accountDb = AccountManager.getInstance().getAccountDB((object) str1, (object) str2, 2, 0);
      if (accountDb == null || accountDb.access <= 3)
        return;
      int num = 0;
      using (new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(msg))
        ;
      string[] strArray1 = new string[9]
      {
        "[SM] Mensagem enviada a ",
        num.ToString(),
        " jogadores: ",
        msg,
        "; by Login: '",
        str1,
        "'; Date: '",
        null,
        null
      };
      DateTime now = DateTime.Now;
      strArray1[7] = now.ToString("dd/MM/yy HH:mm");
      strArray1[8] = "'";
      Logger.warning(string.Concat(strArray1));
      string[] strArray2 = new string[9]
      {
        "[Via SM] Mensagem enviada a ",
        num.ToString(),
        " jogadores: ",
        msg,
        "; by Login: '",
        str1,
        "'; Date: '",
        null,
        null
      };
      now = DateTime.Now;
      strArray2[7] = now.ToString("dd/MM/yy HH:mm");
      strArray2[8] = "'";
      Logger.LogCMD(string.Concat(strArray2));
    }

    public static void LoadShopRestart(ReceiveGPacket p)
    {
    }

    public static void LoadServerUpdate(ReceiveGPacket p)
    {
    }

    public static void LoadShutdown(ReceiveGPacket p)
    {
    }
  }
}
