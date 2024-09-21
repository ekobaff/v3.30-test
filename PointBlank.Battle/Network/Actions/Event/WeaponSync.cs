using System;
using PointBlank.Battle.Data.Models;
using PointBlank.Battle.Data.Models.Event;

namespace PointBlank.Battle.Network.Actions.Event
{
	// Token: 0x02000027 RID: 39
	public class WeaponSync
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00008DA0 File Offset: 0x00006FA0
		public static WeaponSyncInfo ReadInfo(ActionModel ac, ReceivePacket p, bool genLog, bool OnlyBytes = false)
		{
			return WeaponSync.BaseReadInfo(ac, p, OnlyBytes, genLog);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00008DB8 File Offset: 0x00006FB8
		private static WeaponSyncInfo BaseReadInfo(ActionModel ac, ReceivePacket p, bool OnlyBytes, bool genLog)
		{
			return new WeaponSyncInfo
			{
				Extensions = p.readC(),
				WeaponId = p.readD()
			};
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00008DE4 File Offset: 0x00006FE4
		public static void WriteInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
		{
			WeaponSyncInfo info = WeaponSync.ReadInfo(ac, p, genLog, true);
			WeaponSync.WriteInfo(s, info);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00002367 File Offset: 0x00000567
		public static void WriteInfo(SendPacket s, WeaponSyncInfo info)
		{
			s.writeC(info.Extensions);
			s.writeD(info.WeaponId);
			info = null;
		}
	}
}
