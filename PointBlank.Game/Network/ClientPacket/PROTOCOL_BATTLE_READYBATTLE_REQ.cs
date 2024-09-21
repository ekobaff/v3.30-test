// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BATTLE_READYBATTLE_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using Game.data.managers;
using PointBlank.Core;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Map;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Core.Progress;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_BATTLE_READYBATTLE_REQ : ReceivePacket
  {
    private int Error;

    public PROTOCOL_BATTLE_READYBATTLE_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      int num = (int) this.readC();
      this.Error = this.readD();
    }

      private bool ClassicModeCheck(Account p, Room room)
        {
            if (!room.name.ToLower().Contains("@camp") && !room.name.ToLower().Contains("camp") &&
                !room.name.ToLower().Contains("@cnpb") && !room.name.ToLower().Contains("cnpb") &&
                !room.name.ToLower().Contains("@rush") && !room.name.ToLower().Contains("rush") &&
                !room.name.ToLower().Contains("@combat") && !room.name.ToLower().Contains("combat") &&
                !room.name.ToLower().Contains("@gold") && !room.name.ToLower().Contains("gold") &&
                !room.name.ToLower().Contains("@cbp") && !room.name.ToLower().Contains("cbp"))
                return false;
            List<string> list = new List<string>();
            PlayerEquipedItems equip = p._equip;
            if (room.name.ToLower().Contains("@camp") ||
                room.name.ToLower().Contains(" @camp") ||
                room.name.ToLower().Contains("@camp ") ||
                room.name.ToLower().Contains("camp"))
            {
                for (int index = 0; index < ClassicModeManager._camp.Count; ++index)
                {
                    int listid = ClassicModeManager._camp[index];
                    if (!ClassicModeManager.IsBlocked(listid, equip._primary, ref list, Translation.GetLabel("ClassicCategory1")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._secondary, ref list, Translation.GetLabel("ClassicCategory2")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._melee, ref list, Translation.GetLabel("ClassicCategory3")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._grenade, ref list, Translation.GetLabel("ClassicCategory4")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._special, ref list, Translation.GetLabel("ClassicCategory5")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._red, ref list, Translation.GetLabel("ClassicCategory6")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._blue, ref list, Translation.GetLabel("ClassicCategory7")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._helmet, ref list, Translation.GetLabel("ClassicCategory8")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._dino, ref list, Translation.GetLabel("ClassicCategory9")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._beret, ref list, Translation.GetLabel("ClassicCategory10")))
                        ClassicModeManager.IsBlocked(listid, equip.face, ref list, Translation.GetLabel("ClassicCategory11"));
               
                }
            }
            if (room.name.ToLower().Contains("@cnpb") || 
                room.name.ToLower().Contains(" @cnpb") || 
                room.name.ToLower().Contains("@cnpb ") ||
                room.name.ToLower().Contains("cnpb"))
            {
                for (int index = 0; index < ClassicModeManager._cnpb.Count; ++index)
                {
                    int listid = ClassicModeManager._cnpb[index];
                    if (!ClassicModeManager.IsBlocked(listid, equip._primary, ref list, Translation.GetLabel("ClassicCategory1")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._secondary, ref list, Translation.GetLabel("ClassicCategory2")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._melee, ref list, Translation.GetLabel("ClassicCategory3")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._grenade, ref list, Translation.GetLabel("ClassicCategory4")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._special, ref list, Translation.GetLabel("ClassicCategory5")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._red, ref list, Translation.GetLabel("ClassicCategory6")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._blue, ref list, Translation.GetLabel("ClassicCategory7")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._helmet, ref list, Translation.GetLabel("ClassicCategory8")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._dino, ref list, Translation.GetLabel("ClassicCategory9")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._beret, ref list, Translation.GetLabel("ClassicCategory10")))
                        ClassicModeManager.IsBlocked(listid, equip.face, ref list, Translation.GetLabel("ClassicCategory11"));
                }
            }
            if (room.name.ToLower().Contains("@rush") ||
                room.name.ToLower().Contains(" @rush") ||
                room.name.ToLower().Contains("@rush ") ||
                room.name.ToLower().Contains("rush"))
            {
                for (int index = 0; index < ClassicModeManager._rush.Count; ++index)
                {
                    int listid = ClassicModeManager._rush[index];
                    if (!ClassicModeManager.IsBlocked(listid, equip._primary, ref list, Translation.GetLabel("ClassicCategory1")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._secondary, ref list, Translation.GetLabel("ClassicCategory2")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._melee, ref list, Translation.GetLabel("ClassicCategory3")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._grenade, ref list, Translation.GetLabel("ClassicCategory4")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._special, ref list, Translation.GetLabel("ClassicCategory5")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._red, ref list, Translation.GetLabel("ClassicCategory6")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._blue, ref list, Translation.GetLabel("ClassicCategory7")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._helmet, ref list, Translation.GetLabel("ClassicCategory8")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._dino, ref list, Translation.GetLabel("ClassicCategory9")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._beret, ref list, Translation.GetLabel("ClassicCategory10")))
                        ClassicModeManager.IsBlocked(listid, equip.face, ref list, Translation.GetLabel("ClassicCategory11"));
                }
            }
            if (room.name.ToLower().Contains("@combat") ||
                room.name.ToLower().Contains(" @combat") ||
                room.name.ToLower().Contains("@combat ") ||
                room.name.ToLower().Contains("combat"))
            {
                for (int index = 0; index < ClassicModeManager._combat.Count; ++index)
                {
                    int listid = ClassicModeManager._combat[index];
                    if (!ClassicModeManager.IsBlocked(listid, equip._primary, ref list, Translation.GetLabel("ClassicCategory1")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._secondary, ref list, Translation.GetLabel("ClassicCategory2")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._melee, ref list, Translation.GetLabel("ClassicCategory3")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._grenade, ref list, Translation.GetLabel("ClassicCategory4")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._special, ref list, Translation.GetLabel("ClassicCategory5")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._red, ref list, Translation.GetLabel("ClassicCategory6")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._blue, ref list, Translation.GetLabel("ClassicCategory7")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._helmet, ref list, Translation.GetLabel("ClassicCategory8")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._dino, ref list, Translation.GetLabel("ClassicCategory9")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._beret, ref list, Translation.GetLabel("ClassicCategory10")))
                        ClassicModeManager.IsBlocked(listid, equip.face, ref list, Translation.GetLabel("ClassicCategory11"));
                }
            }

            if (room.name.ToLower().Contains("@gold") ||
               room.name.ToLower().Contains(" @gold") ||
               room.name.ToLower().Contains("@gold ") ||
               room.name.ToLower().Contains("gold"))
            {
                for (int index = 0; index < ClassicModeManager._gold.Count; ++index)
                {
                    int listid = ClassicModeManager._gold[index];
                    if (!ClassicModeManager.IsBlocked(listid, equip._primary, ref list, Translation.GetLabel("ClassicCategory1")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._secondary, ref list, Translation.GetLabel("ClassicCategory2")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._melee, ref list, Translation.GetLabel("ClassicCategory3")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._grenade, ref list, Translation.GetLabel("ClassicCategory4")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._special, ref list, Translation.GetLabel("ClassicCategory5")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._red, ref list, Translation.GetLabel("ClassicCategory6")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._blue, ref list, Translation.GetLabel("ClassicCategory7")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._helmet, ref list, Translation.GetLabel("ClassicCategory8")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._dino, ref list, Translation.GetLabel("ClassicCategory9")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._beret, ref list, Translation.GetLabel("ClassicCategory10")))
                        ClassicModeManager.IsBlocked(listid, equip.face, ref list, Translation.GetLabel("ClassicCategory11"));
                }
            }

            if (room.name.ToLower().Contains("@cbp") ||
              room.name.ToLower().Contains(" @cbp") ||
              room.name.ToLower().Contains("@cbp ") ||
              room.name.ToLower().Contains("cbp"))
            {
                for (int index = 0; index < ClassicModeManager._cbp.Count; ++index)
                {
                    int listid = ClassicModeManager._cbp[index];
                    if (!ClassicModeManager.IsBlocked(listid, equip._primary, ref list, Translation.GetLabel("ClassicCategory1")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._secondary, ref list, Translation.GetLabel("ClassicCategory2")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._melee, ref list, Translation.GetLabel("ClassicCategory3")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._grenade, ref list, Translation.GetLabel("ClassicCategory4")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._special, ref list, Translation.GetLabel("ClassicCategory5")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._red, ref list, Translation.GetLabel("ClassicCategory6")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._blue, ref list, Translation.GetLabel("ClassicCategory7")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._helmet, ref list, Translation.GetLabel("ClassicCategory8")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._dino, ref list, Translation.GetLabel("ClassicCategory9")) &&
                        !ClassicModeManager.IsBlocked(listid, equip._beret, ref list, Translation.GetLabel("ClassicCategory10")))
                        ClassicModeManager.IsBlocked(listid, equip.face, ref list, Translation.GetLabel("ClassicCategory11"));
                }
            }

            if (list.Count <= 0)
                return false;
            this._client.SendPacket((SendPacket)new PROTOCOL_BATTLE_READYBATTLE_ACK(1));// Erro 2147487915U caso bug
            p.SendPacket((SendPacket)new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Translation.GetLabel("ClassicModeWarn", (object)string.Join(", ", list.ToArray()))));
            return true;
        }

        public  bool RankedCheck(Account p, Room room)
        {
            
            Account Lider = room.getLeader();           
            try
            {
                if (room.name.ToLower().Contains("@ranked") && room.room_type == RoomType.Bomb && GameManager.Config.RankedEnable)
                {
                    if (p._rank < 0)
                    {
                        this._client.SendPacket((SendPacket)new PROTOCOL_BATTLE_READYBATTLE_ACK(1)); // Erro 2147487915U caso bug
                        SendErros(p, $"Para jogar Ranqueado, você precisa atingir a patente necessária {Functions.NameRank(0)}.");
                        return  false;

                    }else if(room.getAllPlayers().Count < 4)
                    {
                        this._client.SendPacket((SendPacket)new PROTOCOL_BATTLE_READYBATTLE_ACK(1));// Erro 2147487915U caso bug
                        SendErros(p, $"Modo ranqueado só começa com 4 jogadores");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                return true;
            }
            catch
            {
                string Texto = "Não e possivel iniciar a sala!";
                this._client.SendPacket((SendPacket)new PROTOCOL_BATTLE_READYBATTLE_ACK(1));// Erro 2147487915U caso bug
                SendErros(Lider, Texto);
                return false;
            }

            //return true;
        }
        public static void SendErros(Account p, string texto)
        {
            p.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(texto));
        }

        public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        PointBlank.Game.Data.Model.Room room = player._room;
        Channel ch;
        Slot slot;
        if (room == null || room.getLeader() == null || !room.getChannel(out ch) || !room.getSlot(player._slotId, out slot))
          return;
        if (slot._equip == null)
        {
          this._client.SendPacket((SendPacket) new PROTOCOL_BATTLE_READYBATTLE_ACK(2147487915U));
        }
        else
        {
          bool isBotMode = room.isBotMode();
         // slot.specGM = this.Error == 1 && player.IsGM();
          if (player.IsGM() && Error == 1)
                    {
                        slot.specGM = true;
                    }
                    else
                    {
                        slot.specGM = false;
                    }
          player.DebugPing = false;
          if (GameConfig.EnableClassicRules && this.ClassicModeCheck(player, room))
            return;
          if(!RankedCheck(player, room))
            return;
          if (room._leader == player._slotId)
          {
            if (room._state != RoomState.Ready && room._state != RoomState.CountDown)
              return;
            int TotalEnemys = 0;
            int redPlayers = 0;
            int bluePlayers = 0;
            this.GetReadyPlayers(room, ref redPlayers, ref bluePlayers, ref TotalEnemys);
            if (GameConfig.isTestMode && GameConfig.udpType == UdpState.RELAY)
              TotalEnemys = 1;
            int num = 0;
            MapMatch mapMatch = MapModel.Matchs.Find((Predicate<MapMatch>) (x => (MapIdEnum) x.Id == room.mapId && MapModel.getRule(x.Mode).Rule == room.rule));
            if (mapMatch != null)
              num = mapMatch.Limit;
            if (num == 8 && (redPlayers >= 4 || bluePlayers >= 4) && ch._type != 4)
            {
              this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_UNREADY_4VS4_ACK());
            }
            else
            {
              if (this.ClanMatchCheck(room, ch._type, TotalEnemys))
                return;
              if (isBotMode || room.room_type == RoomType.Tutorial || TotalEnemys > 0 && !isBotMode)
              {
                room.changeSlotState(slot, SlotState.READY, false);
                if (room._state != RoomState.CountDown)
                  this.TryBalanceTeams(room, isBotMode);
                if (room.thisModeHaveCD())
                {
                  if (room._state == RoomState.Ready)
                  {
                    room._state = RoomState.CountDown;
                    room.updateRoomInfo();
                    room.StartCountDown();
                  }
                  else if (room._state == RoomState.CountDown)
                  {
                    room.changeSlotState(room._leader, SlotState.NORMAL, false);
                    room.StopCountDown(CountDownEnum.StopByHost);
                  }
                }
                else
                  room.StartBattle(false);
                room.updateSlotsInfo();
              }
              else
              {
                if (TotalEnemys != 0 || isBotMode)
                  return;
                this._client.SendPacket((SendPacket) new PROTOCOL_BATTLE_READYBATTLE_ACK(2147487753U));
              }
            }
          }
          else if (room._slots[room._leader].state >= SlotState.LOAD)
          {
            if (slot.state != SlotState.NORMAL)
              return;
            if (room.BalanceType == (short) 1 && !isBotMode)
              AllUtils.TryBalancePlayer(room, player, true, ref slot);
            room.changeSlotState(slot, SlotState.LOAD, true);
            slot.SetMissionsClone(player._mission);
            this._client.SendPacket((SendPacket) new PROTOCOL_BATTLE_READYBATTLE_ACK((uint) slot.state));
            this._client.SendPacket((SendPacket) new PROTOCOL_BATTLE_START_GAME_ACK(room));
            using (PROTOCOL_BATTLE_START_GAME_TRANS_ACK startGameTransAck = new PROTOCOL_BATTLE_START_GAME_TRANS_ACK(room, slot, player._titles))
              room.SendPacketToPlayers((SendPacket) startGameTransAck, SlotState.READY, 1, slot._id);
          }
          else if (slot.state == SlotState.NORMAL)
          {
            room.changeSlotState(slot, SlotState.READY, true);
            if (room._state != RoomState.CountDown)
              return;
            this.TryBalanceTeams(room, isBotMode);
          }
          else
          {
            if (slot.state != SlotState.READY)
              return;
            room.changeSlotState(slot, SlotState.NORMAL, false);
            if (room._state == RoomState.CountDown && room.getPlayingPlayers(room._leader % 2 == 0 ? 1 : 0, SlotState.READY, 0) == 0)
            {
              room.changeSlotState(room._leader, SlotState.NORMAL, false);
              room.StopCountDown(CountDownEnum.StopByPlayer);
            }
            room.updateSlotsInfo();
          }
        }
      }
      catch (Exception ex)
      {
        Logger.info("PROTOCOL_BATTLE_READYBATTLE_REQ: " + ex.ToString());
      }
    }

    private void GetReadyPlayers(
      PointBlank.Game.Data.Model.Room room,
      ref int redPlayers,
      ref int bluePlayers,
      ref int TotalEnemys)
    {
      for (int index = 0; index < 16; ++index)
      {
        Slot slot = room._slots[index];
        if (slot.state == SlotState.READY)
        {
          if (slot._team == 0)
            ++redPlayers;
          else
            ++bluePlayers;
        }
      }
      if (room._leader % 2 == 0)
        TotalEnemys = bluePlayers;
      else
        TotalEnemys = redPlayers;
    }

    private bool ClanMatchCheck(PointBlank.Game.Data.Model.Room room, int type, int TotalEnemys)
    {
      if (GameConfig.isTestMode || type != 4)
        return false;
      if (!AllUtils.Have2ClansToClanMatch(room))
      {
        this._client.SendPacket((SendPacket) new PROTOCOL_BATTLE_READYBATTLE_ACK(2147487857U));
        return true;
      }
      if (TotalEnemys <= 0 || AllUtils.HavePlayersToClanMatch(room))
        return false;
      this._client.SendPacket((SendPacket) new PROTOCOL_BATTLE_READYBATTLE_ACK(2147487858U));
      return true;
    }

        private void TryBalanceTeams(Room room, bool isBotMode)
        {
            if (room.BalanceType != 1 || isBotMode)
            {
                return;
            }
            int TeamIdx = AllUtils.getBalanceTeamIdx(room, false, -1);
            if (TeamIdx == -1)
            {
                return;
            }
            int[] teamArray = TeamIdx == 1 ? room.RED_TEAM : room.BLUE_TEAM;
            Slot LastSlot = null;
            for (int i = teamArray.Length - 1; i >= 0; i--)
            {
                Slot slot = room._slots[teamArray[i]];
                if ((int)slot.state == 9 && room._leader != slot._id)
                {
                    LastSlot = slot;
                    break;
                }
            }
            Account player;
            if (LastSlot != null && room.getPlayerBySlot(LastSlot, out player))
            {
                AllUtils.TryBalancePlayer(room, player, false, ref LastSlot);
            }
        }
    }
}
