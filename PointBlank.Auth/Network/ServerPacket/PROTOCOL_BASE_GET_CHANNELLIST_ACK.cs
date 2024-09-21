// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ServerPacket.PROTOCOL_BASE_GET_CHANNELLIST_ACK
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Data.Configs;
using PointBlank.Auth.Data.Model;
using PointBlank.Core.Network;
using System;
using System.Collections.Generic;

namespace PointBlank.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_CHANNELLIST_ACK : SendPacket
    {
        private List<Channel> Channels;
        private int ServerId;

        public PROTOCOL_BASE_GET_CHANNELLIST_ACK(List<Channel> channels, int serverId)
        {
            Channels = channels;
            ServerId = serverId;
        }

        public override void write()
        {
            try
            {
                // Paket başlığı ve meta bilgilerini yaz
                writeH(541); // Paket ID
                writeH(0);   // Rezerv veya ekstra başlık bilgisi
                writeC(0);   // Olası olay veya durum kodu

                // Kanal sayısını yaz
                writeC((byte)Channels.Count);

                // Her kanal için oyuncu sayısını yaz
                foreach (var channel in Channels)
                {
                    writeH((ushort)channel._players); // Her kanaldaki oyuncu sayısı
                }

                // Maksimum oyuncu limitlerini yaz
                writeH((ushort)AuthConfig.maxChannelPlayers); // Kanal başına maksimum oyuncu sayısı
                writeH((ushort)AuthConfig.maxChannelPlayers); // Belki başka bir limit veya aynı
                writeC((byte)Channels.Count); // Olası bir tekrar veya başka bir değer

                // Ek veri gerekiyorsa buraya ekleyebilirsiniz
                // Örneğin, ServerId gibi ek bilgileri yazabilirsiniz
                writeD(ServerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Paket yazma hatası: {ex}");
            }
        }
    }
}