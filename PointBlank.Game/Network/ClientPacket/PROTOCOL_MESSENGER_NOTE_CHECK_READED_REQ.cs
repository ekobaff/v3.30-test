using System;
using System.Collections.Generic;
using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Network.ClientPacket
{
	// Token: 0x0200012B RID: 299
	public class PROTOCOL_MESSENGER_NOTE_CHECK_READED_REQ : ReceivePacket
	{
		// Token: 0x06000304 RID: 772 RVA: 0x000176AC File Offset: 0x000158AC
		public PROTOCOL_MESSENGER_NOTE_CHECK_READED_REQ(GameClient client, byte[] data)
		{
			base.makeme(client, data);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000176C8 File Offset: 0x000158C8
		public override void read()
		{
			this.msgsCount = (int)base.readC();
			for (int i = 0; i < this.msgsCount; i++)
			{
				this.messages.Add(base.readD());
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00017704 File Offset: 0x00015904
		public override void run()
		{
			try
			{
				if (this._client != null && this._client._player != null && this.msgsCount != 0)
				{
					ComDiv.updateDB("player_messages", "object_id", this.messages.ToArray(), "owner_id", this._client.player_id, new string[]
					{
						"expire",
						"state"
					}, new object[]
					{
						int.Parse(DateTime.Now.AddYears(-10).AddDays(7.0).ToString("yyMMddHHmm")),
						0
					});
					this._client.SendPacket(new PROTOCOL_MESSENGER_NOTE_CHECK_READED_ACK(this.messages));
				}
			}
			catch (Exception ex)
			{
				Logger.info("PROTOCOL_MESSENGER_NOTE_CHECK_READED_REQ: " + ex.ToString());
			}
		}

		// Token: 0x040001FD RID: 509
		private int msgsCount;

		// Token: 0x040001FE RID: 510
		private List<int> messages = new List<int>();
	}
}
