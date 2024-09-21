using System;
using PointBlank.Auth.Data.Model;
using PointBlank.Core;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;

namespace PointBlank.Auth.Network.ServerPacket
{
	// Token: 0x02000011 RID: 17
	public class PROTOCOL_BASE_GET_CHARA_INFO_ACK : SendPacket
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00002232 File Offset: 0x00000432
		public PROTOCOL_BASE_GET_CHARA_INFO_ACK(Account Player)
		{
			this.Player = Player;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003F70 File Offset: 0x00002170
		public override void write()
		{
			try
			{
				base.writeH(661);
				base.writeH(0);
				base.writeC((byte)this.Player.Characters.Count);
				foreach (Character character in this.Player.Characters)
				{
					if (character != null)
					{
						base.writeC((byte)character.Slot);
						if (character.Slot == 0 || character.Slot == 1)
						{
							base.writeC(20);
						}
						else
						{
							base.writeC(30);
						}
						base.writeD((int)character.ObjId);
						base.writeD(character.CreateDate);
						base.writeD(character.PlayTime);
						base.writeD(character.PlayTime);
						base.writeUnicode(character.Name, 66);
						if (this.Player._equip._primary == 0)
						{
							base.writeD(104006);
						}
						else
						{
							base.writeD(this.Player._equip._primary);
						}
						ItemsModel item = this.Player._inventory.getItem(this.Player._equip._primary);
						base.writeD((int)((item != null) ? item._objId : 0L));
						if (this.Player._equip._secondary == 0)
						{
							base.writeD(202003);
						}
						else
						{
							base.writeD(this.Player._equip._secondary);
						}
						ItemsModel item2 = this.Player._inventory.getItem(this.Player._equip._secondary);
						base.writeD((int)((item2 != null) ? item2._objId : 0L));
						if (this.Player._equip._melee == 0)
						{
							base.writeD(301001);
						}
						else
						{
							base.writeD(this.Player._equip._melee);
						}
						ItemsModel item3 = this.Player._inventory.getItem(this.Player._equip._melee);
						base.writeD((int)((item3 != null) ? item3._objId : 0L));
						if (this.Player._equip._grenade == 0)
						{
							base.writeD(407001);
						}
						else
						{
							base.writeD(this.Player._equip._grenade);
						}
						ItemsModel item4 = this.Player._inventory.getItem(this.Player._equip._grenade);
						base.writeD((int)((item4 != null) ? item4._objId : 0L));
						if (this.Player._equip._special == 0)
						{
							base.writeD(508001);
						}
						else
						{
							base.writeD(this.Player._equip._special);
						}
						ItemsModel item5 = this.Player._inventory.getItem(this.Player._equip._special);
						base.writeD((int)((item5 != null) ? item5._objId : 0L));
						base.writeD(character.Id);
						ItemsModel item6 = this.Player._inventory.getItem(character.Id);
						base.writeD((int)((item6 != null) ? item6._objId : 0L));
						base.writeD(this.Player._equip.face);
						if (this.Player._inventory.getItem(this.Player._equip.face) == null)
						{
							base.writeD(0);
						}
						else
						{
							base.writeD((int)this.Player._inventory.getItem(this.Player._equip.face)._objId);
						}
						base.writeD(this.Player._equip._helmet);
						if (this.Player._inventory.getItem(this.Player._equip._helmet) == null)
						{
							base.writeD(0);
						}
						else
						{
							base.writeD((int)this.Player._inventory.getItem(this.Player._equip._helmet)._objId);
						}
						base.writeD(this.Player._equip.jacket);
						base.writeD((int)this.Player._inventory.getItem(this.Player._equip.jacket)._objId);
						base.writeD(this.Player._equip.poket);
						base.writeD((int)this.Player._inventory.getItem(this.Player._equip.poket)._objId);
						base.writeD(this.Player._equip.glove);
						base.writeD((int)this.Player._inventory.getItem(this.Player._equip.glove)._objId);
						base.writeD(this.Player._equip.belt);
						base.writeD((int)this.Player._inventory.getItem(this.Player._equip.belt)._objId);
						base.writeD(this.Player._equip.holster);
						base.writeD((int)this.Player._inventory.getItem(this.Player._equip.holster)._objId);
						base.writeD(this.Player._equip.skin);
						base.writeD((int)this.Player._inventory.getItem(this.Player._equip.skin)._objId);
						base.writeD(this.Player._equip._beret);
						if (this.Player._inventory.getItem(this.Player._equip._beret) == null)
						{
							base.writeD(0);
						}
						else
						{
							base.writeD((int)this.Player._inventory.getItem(this.Player._equip._beret)._objId);
						}
						base.writeC(0);
						base.writeC(byte.MaxValue);
						base.writeC(byte.MaxValue);
						base.writeC(byte.MaxValue);
						base.writeC(0);
						base.writeC(0);
						base.writeC(0);
					}
				}
				base.writeC(0);
			}
			catch (Exception ex)
			{
				Logger.error("PROTOCOL_BASE_GET_CHARA_INFO_ACK: " + ex.ToString());
			}
		}

		// Token: 0x04000024 RID: 36
		private Account Player;
	}
}
