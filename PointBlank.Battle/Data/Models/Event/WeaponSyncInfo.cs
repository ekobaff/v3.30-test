using System;

namespace PointBlank.Battle.Data.Models.Event
{
	// Token: 0x0200005D RID: 93
	public class WeaponSyncInfo
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00003176 File Offset: 0x00001376
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000317E File Offset: 0x0000137E
		public byte Extensions { get; set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00003187 File Offset: 0x00001387
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000318F File Offset: 0x0000138F
		public int WeaponId { get; set; }
	}
}
