// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using System;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK : SendPacket
    {
        private uint erro;
        private ItemsModel item;

        public PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK(uint erro, ItemsModel item = null, PointBlank.Game.Data.Model.Account p = null)
        {
            this.erro = erro;
            if (erro != 1U)
                return;
            if (item != null)
            {
                switch (ComDiv.getIdStatics(item._id, 1))
                {
                    case 17:
                    case 18:
                    case 20:
                    case 28:
                        if (item._count > 1L && item._equip == 1)
                        {
                            ComDiv.updateDB("player_items", "count", (object)--item._count, "object_id", (object)item._objId, "owner_id", (object)p.player_id);
                            break;
                        }
                        PlayerManager.DeleteItem(item._objId, p.player_id);
                        p._inventory.RemoveItem(item);
                        item._id = 0;
                        item._count = 0L;
                        break;
                    default:
                        item._equip = 2;
                        break;
                }
                this.item = item;
            }
            else
                this.erro = 2147483648U;
        }

        public override void write()
        {
            writeH((short)1048);
            writeD(erro);
            if (erro != 1U)
                return;
            writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
            writeD((int)item._objId);
            if (item._category == 3 && item._equip == 2)
            {
                writeD(0);
                writeC((byte)1);
                writeD(0);
            }
            else
            {
                writeD(item._id);
                writeC((byte)item._equip);
                writeD((int)item._count);
            }
        }
    }
}
