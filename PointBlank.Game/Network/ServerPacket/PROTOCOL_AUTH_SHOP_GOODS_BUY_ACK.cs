// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Shop;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK : SendPacket
    {
        /* private List<GoodItem> Items = new List<GoodItem>();
         private Account Player;
         private uint Error;

         public PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(uint Error, List<GoodItem> Item = null, Account Player = null)
         {
           this.Error = Error;
           if (this.Error != 1U)
             return;
           this.Player = Player;
           this.AddItem(Item);
         }

         public override void write()
         {
           this.writeH((short) 1044);
           this.writeH((short) 0);
           if (this.Error == 1U)
           {
             this.writeC((byte) this.Items.Count);
             for (int index = 0; index < this.Items.Count; ++index)
             {
               GoodItem goodItem = this.Items[index];
               this.writeD(0);
               this.writeD(goodItem.id);
               this.writeC((byte) 0);
             }
             this.writeD(this.Player._money);
             this.writeD(this.Player._gp);
             this.writeD(uint.Parse(DateTime.Now.AddYears(-10).ToString("yyMMddHHmm")));
           }
           else
             this.writeD(this.Error);
         }

         private void AddItem(List<GoodItem> Goods)
         {
           try
           {
             for (int index = 0; index < Goods.Count; ++index)
               this.Items.Add(Goods[index]);
           }
           catch (Exception ex)
           {
             Logger.warning("PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK: " + ex.ToString());
           }
         }*/
        private List<GoodItem> Items = new List<GoodItem>();
        private Account Player;
        private uint Error;

        public PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(uint Error, List<GoodItem> Item = null, Account Player = null)
        {
            this.Error = Error;
            if (this.Error != 1U)
                return;
            this.Player = Player;
            this.AddItem(Item);
        }

        public override void write()
        {
            this.writeH((short)1044);
            this.writeH((short)0);
            if (this.Error == 1U)
            {
                this.writeC((byte)this.Items.Count);
                for (int index = 0; index < this.Items.Count; ++index)
                {
                    GoodItem goodItem = this.Items[index];
                    this.writeD(0);
                    this.writeD(goodItem.id);
                    this.writeC((byte)0);
                }
                this.writeD(this.Player._money);
                this.writeD(this.Player._gp);
                this.writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
            }
            else
                this.writeD(this.Error);
        }

        private void AddItem(List<GoodItem> Goods)
        {
            try
            {
                for (int index = 0; index < Goods.Count; ++index)
                    this.Items.Add(Goods[index]);
            }
            catch (Exception ex)
            {
                Logger.warning("PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK: " + ex.ToString());
            }
        }

    }
}
