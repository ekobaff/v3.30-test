using PointBlank.Core.Managers;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Network;
using PointBlank.Core.Xml;
using PointBlank.Auth.Data.Model;
using PointBlank.Auth.Data.Xml;
using System;
using System.Collections.Generic;

namespace PointBlank.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_SYSTEM_INFO_ACK : SendPacket
    {
        public PROTOCOL_BASE_GET_SYSTEM_INFO_ACK()
        {

        }

        public override void write()
        {
            writeH(523);
            writeH(0);
            writeC(0);
            writeC(5);
            writeC(10);
            writeC(32);
            writeC(4);
            writeC(0);
            writeC(1);
            writeC(2);
            writeC(5);
            writeC(3);
            writeC(6);
            //writeB(new byte[25]);

            writeC(7);
            writeB(new byte[206]);

            short Mask1 = 0;
            if (AuthManager.Config.ClanEnable) Mask1 += 4096;

            writeH(Mask1); // playtime 256, clan 4096
            writeH(0); // gift 256, tags 1024

            writeC(3); // Count Event Play Time
            writeD(600); // 600
            writeD(2400); // 2400
            writeD(6000); // 6000

            writeC(1);
            writeH((ushort)MissionsXml._missionPage1);
            writeH((ushort)MissionsXml._missionPage2);
            writeH(29890);
            writeC((byte)ServersXml._servers.Count);
            for (int i = 0; i < ServersXml._servers.Count; i++)
            {
                GameServerModel server = ServersXml._servers[i];
                writeD(server._state);
                writeIP(server.Connection.Address);
                writeH(server._port);
                writeC((byte)server._type);
                writeH((ushort)server._maxPlayers);
                writeD(server._LastCount);
                if (i == 0)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        writeC(1);
                    }
                }
                else
                {
                    for (int Id = 0; Id < ChannelsXml.getChannels(i).Count; Id++)
                    {
                        Channel channel = ChannelsXml._channels[Id];
                        writeC((byte)channel._type);
                    }
                    writeC(1);
                }
            }
            writeH((ushort)AuthManager.Config.ExitURL.Length);
            writeS(AuthManager.Config.ExitURL, AuthManager.Config.ExitURL.Length);
            writeC(51);
            for (int Id = 0; Id < 51; Id++)
            {
                List<ItemsModel> Items = RankXml.getAwards(Id);
                writeC((byte)(Id));
                for (int Idx = 0; Idx < Items.Count; Idx++)
                {
                    ItemsModel Item = Items[Idx];
                    if (ShopManager.getItemId(Item._id) == null)
                    {
                        writeD(0);
                    }
                    else
                    {
                        writeD(ShopManager.getItemId(Item._id).id);
                    }
                }
                for (int i = Items.Count; (4 - i) > 0; i++)
                {
                    writeD(0);
                }
            }
            writeC(1);
        }
    }
}