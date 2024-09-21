// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BATTLE_RESPAWN_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using Game.data.managers;
using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Sync;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_BATTLE_RESPAWN_REQ : ReceivePacket
  {
    private PlayerEquipedItems Equip;
    private int WeaponsFlag;

    public PROTOCOL_BATTLE_RESPAWN_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      PointBlank.Game.Data.Model.Room room = this._client._player._room;
      bool flag = this._client._player._slotId % 2 == 0;
      this.Equip = new PlayerEquipedItems();
      this.Equip._primary = this.readD();
      this.readD();
      this.Equip._secondary = this.readD();
      this.readD();
      this.Equip._melee = this.readD();
      this.readD();
      this.Equip._grenade = this.readD();
      this.readD();
      this.Equip._special = this.readD();
      this.readD();
      if (room.room_type == RoomType.Boss || room.room_type == RoomType.CrossCounter)
      {
        if (!room.swapRound)
        {
          if (flag)
          {
            this.Equip._red = this._client._player._equip._red;
            this.Equip._blue = this._client._player._equip._blue;
            this.Equip._dino = this.readD();
            this.readD();
          }
          else
          {
            this.Equip._red = this._client._player._equip._red;
            this.Equip._blue = this.readD();
            this.Equip._dino = this._client._player._equip._dino;
            this.readD();
          }
        }
        else if (flag)
        {
          this.Equip._red = this._client._player._equip._red;
          this.Equip._blue = this.readD();
          this.Equip._dino = this._client._player._equip._dino;
          this.readD();
        }
        else
        {
          this.Equip._red = this._client._player._equip._red;
          this.Equip._blue = this._client._player._equip._blue;
          this.Equip._dino = this.readD();
          this.readD();
        }
      }
      else
      {
        if (flag)
        {
          this.Equip._red = this.readD();
          this.Equip._blue = this._client._player._equip._blue;
          this.readD();
        }
        else
        {
          this.Equip._red = this._client._player._equip._red;
          this.Equip._blue = this.readD();
          this.readD();
        }
        this.Equip._dino = this._client._player._equip._dino;
      }
      this.Equip.face = this.readD();
      this.readD();
      this.Equip._helmet = this.readD();
      this.readD();
      this.Equip.jacket = this.readD();
      this.readD();
      this.Equip.poket = this.readD();
      this.readD();
      this.Equip.glove = this.readD();
      this.readD();
      this.Equip.belt = this.readD();
      this.readD();
      this.Equip.holster = this.readD();
      this.readD();
      this.Equip.skin = this.readD();
      this.readD();
      this.Equip._beret = this.readD();
      this.readD();
      this.WeaponsFlag = (int) this.readH();
    }

    public override void run()
    {
      try
      {
        PointBlank.Game.Data.Model.Account player = this._client._player;
        if (player == null)
          return;
        PointBlank.Game.Data.Model.Room room = player._room;
        if (room == null || room._state != RoomState.Battle)
          return;
        Slot slot = room.getSlot(player._slotId);
        if (slot == null || slot.state != SlotState.BATTLE)
          return;
        if (slot._deathState.HasFlag((Enum) DeadEnum.Dead) || slot._deathState.HasFlag((Enum) DeadEnum.UseChat))
          slot._deathState = DeadEnum.Alive;
        PlayerManager.CheckEquipedItems(this.Equip, player._inventory._items, true);
       this.CheckEquipment(player, room, this.Equip);
       if (this.ClassicModeCheck(room, this.Equip, player))
        this._client.SendPacket((SendPacket)new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Você não pode equipar este item devido as regras da sala."));
      
        slot._equip = this.Equip;
        if ((this.WeaponsFlag & 8) > 0)
          this.InsertItem(this.Equip._primary, slot);
        if ((this.WeaponsFlag & 4) > 0)
          this.InsertItem(this.Equip._secondary, slot);
        if ((this.WeaponsFlag & 2) > 0)
          this.InsertItem(this.Equip._melee, slot);
        if ((this.WeaponsFlag & 1) > 0)
          this.InsertItem(this.Equip._grenade, slot);
        this.InsertItem(this.Equip._special, slot);
        if (slot._team == 0)
          this.InsertItem(this.Equip._red, slot);
        else
          this.InsertItem(this.Equip._blue, slot);
        if (room.MaskActive)
        {
          this.Equip._helmet = 1000800000;
          this.InsertItem(this.Equip._helmet, slot);
        }
        else
          this.InsertItem(this.Equip._helmet, slot);
        this.InsertItem(this.Equip._beret, slot);
        this.InsertItem(this.Equip._dino, slot);
        using (PROTOCOL_BATTLE_RESPAWN_ACK battleRespawnAck = new PROTOCOL_BATTLE_RESPAWN_ACK(room, slot))
          room.SendPacketToPlayers((SendPacket) battleRespawnAck, SlotState.BATTLE, 0);
        if (slot.firstRespawn)
        {
          slot.firstRespawn = false;
          GameSync.SendUDPPlayerSync(room, slot, player.effects, 0);
        }
        else
          GameSync.SendUDPPlayerSync(room, slot, player.effects, 2);
      }
      catch (Exception ex)
      {
        Logger.warning("PROTOCOL_BATTLE_RESPAWN_REQ: " + ex.ToString());
      }
    }

    public void CheckEquipment(PointBlank.Game.Data.Model.Account Player, PointBlank.Game.Data.Model.Room Room, PlayerEquipedItems Equip)
    {
      if (Room.BarrettActive && (Equip._primary == 105032 || Equip._primary == 105082 || Equip._primary == 105232 || Equip._primary == 105292))
        Equip._primary = !Room.SniperMode ? 103004 : 105003;
      if (!Room.ShotgunActive || Equip._primary >= 107000 || Equip._primary <= 106000)
        return;
      Equip._primary = 103004;
    }

        private bool ClassicModeCheck(PointBlank.Game.Data.Model.Room room, PlayerEquipedItems equip, PointBlank.Game.Data.Model.Account player)
        {
            bool flag = false;
            if (!room.name.ToLower().Contains("@camp") && !room.name.ToLower().Contains("camp") &&
                !room.name.ToLower().Contains("@cnpb") && !room.name.ToLower().Contains("cnpb") &&
                !room.name.ToLower().Contains("@rush") && !room.name.ToLower().Contains("rush") &&
                !room.name.ToLower().Contains("@combat") && !room.name.ToLower().Contains("combat") &&
                !room.name.ToLower().Contains("@gold") && !room.name.ToLower().Contains("gold") &&
                !room.name.ToLower().Contains("@cbp") && !room.name.ToLower().Contains("cbp")
                || !GameConfig.EnableClassicRules)
                return false;
            if (room.name.ToLower().Contains("@camp") ||
                room.name.ToLower().Contains(" camp") ||
                room.name.ToLower().Contains("camp ") ||
                room.name.ToLower().Contains("camp"))
            {
                for (int index = 0; index < ClassicModeManager._camp.Count; ++index)
                {
                    int listid = ClassicModeManager._camp[index];
                    if (ClassicModeManager.IsBlocked(listid, equip._primary))
                    {
                        equip._primary = player._equip._primary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._secondary))
                    {
                        equip._secondary = player._equip._secondary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._melee))
                    {
                        equip._melee = player._equip._melee;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._grenade))
                    {
                        equip._grenade = player._equip._grenade;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._special))
                    {
                        equip._special = player._equip._special;
                        flag = true;
                    }
                }
            }

            if (room.name.ToLower().Contains("@cnpb") ||
                room.name.ToLower().Contains(" cnpb") ||
                room.name.ToLower().Contains("cnpb ") ||
                room.name.ToLower().Contains("cnpb"))
            {
                for (int index = 0; index < ClassicModeManager._cnpb.Count; ++index)
                {
                    int listid = ClassicModeManager._cnpb[index];
                    if (ClassicModeManager.IsBlocked(listid, equip._primary))
                    {
                        equip._primary = player._equip._primary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._secondary))
                    {
                        equip._secondary = player._equip._secondary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._melee))
                    {
                        equip._melee = player._equip._melee;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._grenade))
                    {
                        equip._grenade = player._equip._grenade;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._special))
                    {
                        equip._special = player._equip._special;
                        flag = true;
                    }
                }
            }

            if (room.name.ToLower().Contains("@rush") ||
                room.name.ToLower().Contains(" rush") ||
                room.name.ToLower().Contains("rush ") ||
                room.name.ToLower().Contains("rush"))
            {
                for (int index = 0; index < ClassicModeManager._rush.Count; ++index)
                {
                    int listid = ClassicModeManager._rush[index];
                    if (ClassicModeManager.IsBlocked(listid, equip._primary))
                    {
                        equip._primary = player._equip._primary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._secondary))
                    {
                        equip._secondary = player._equip._secondary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._melee))
                    {
                        equip._melee = player._equip._melee;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._grenade))
                    {
                        equip._grenade = player._equip._grenade;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._special))
                    {
                        equip._special = player._equip._special;
                        flag = true;
                    }
                }
            }

            if (room.name.ToLower().Contains("@combat") ||
                room.name.ToLower().Contains(" combat") ||
                room.name.ToLower().Contains("combat ") ||
                room.name.ToLower().Contains("combat"))
            {
                for (int index = 0; index < ClassicModeManager._combat.Count; ++index)
                {
                    int listid = ClassicModeManager._combat[index];
                    if (ClassicModeManager.IsBlocked(listid, equip._primary))
                    {
                        equip._primary = player._equip._primary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._secondary))
                    {
                        equip._secondary = player._equip._secondary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._melee))
                    {
                        equip._melee = player._equip._melee;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._grenade))
                    {
                        equip._grenade = player._equip._grenade;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._special))
                    {
                        equip._special = player._equip._special;
                        flag = true;
                    }
                }
            }
            if (room.name.ToLower().Contains("@gold") ||
                room.name.ToLower().Contains(" gold") ||
                room.name.ToLower().Contains("gold ") ||
                room.name.ToLower().Contains("gold"))
            {
                for (int index = 0; index < ClassicModeManager._gold.Count; ++index)
                {
                    int listid = ClassicModeManager._gold[index];
                    if (ClassicModeManager.IsBlocked(listid, equip._primary))
                    {
                        equip._primary = player._equip._primary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._secondary))
                    {
                        equip._secondary = player._equip._secondary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._melee))
                    {
                        equip._melee = player._equip._melee;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._grenade))
                    {
                        equip._grenade = player._equip._grenade;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._special))
                    {
                        equip._special = player._equip._special;
                        flag = true;
                    }
                }
            }

            if (room.name.ToLower().Contains("@cbp") ||
                room.name.ToLower().Contains(" cbp") ||
                room.name.ToLower().Contains("cbp ") ||
                room.name.ToLower().Contains("cbp"))
            {
                for (int index = 0; index < ClassicModeManager._cbp.Count; ++index)
                {
                    int listid = ClassicModeManager._cbp[index];
                    if (ClassicModeManager.IsBlocked(listid, equip._primary))
                    {
                        equip._primary = player._equip._primary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._secondary))
                    {
                        equip._secondary = player._equip._secondary;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._melee))
                    {
                        equip._melee = player._equip._melee;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._grenade))
                    {
                        equip._grenade = player._equip._grenade;
                        flag = true;
                    }
                    else if (ClassicModeManager.IsBlocked(listid, equip._special))
                    {
                        equip._special = player._equip._special;
                        flag = true;
                    }
                }
            }


            return flag;
        }
        private void InsertItem(int id, Slot slot)
    {
      lock (slot.armas_usadas)
      {
        if (slot.armas_usadas.Contains(id))
          return;
        slot.armas_usadas.Add(id);
      }
    }
  }
}
