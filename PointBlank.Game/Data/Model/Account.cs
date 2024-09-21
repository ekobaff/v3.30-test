// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Model.Account
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Account.Title;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Sync;
using PointBlank.Game.Data.Xml;
using System;
using System.Collections.Generic;
using System.Net;

namespace PointBlank.Game.Data.Model
{
    public class Account
    {
        public bool Information;
        public bool _isOnline;
        public bool HideGMcolor;
        public bool AntiKickGM;
        public bool LoadedShop;
        public bool DebugPing;
        public bool ICafe;
        public string player_name = "";
        public string password;
        public string login;
        //public string token;
        //public string hwid;
        public string FindPlayer = "";
        public long player_id;
        public long ban_obj_id;
        public uint LastRankUpDate;
        public uint LastLoginDate;
        public IPAddress PublicIP;
        public CouponEffects effects;
        public PlayerSession Session;
        public int Sight;
        public int FindClanId;
        public int LastClanPage;
        public int LastRoomPage;
        public int LastPlayerPage;
        public int tourneyLevel;
        public int channelId = -1;
        public int clanAccess;
        public int clanDate;
        public int _exp, _gp, _money, _tag;
        public int clanId;
        public int brooch;
        public int insignia;
        public int medal;
        public int blue_order;
        public int _slotId = -1;
        public int name_color;
        public int _rank;
        public int pc_cafe;
        public int matchSlot = -1;
        public int age;
        public PlayerEquipedItems _equip = new PlayerEquipedItems();
        public PlayerInventory _inventory = new PlayerInventory();
        public List<PlayerItemTopup> _topups = new List<PlayerItemTopup>();
        public List<PlayerMakeVip> _makevip = new List<PlayerMakeVip>();
        public List<Character> Characters = new List<Character>();
        public PlayerConfig _config;
        public GameClient _connection;
        public Room _room;
        public PlayerBonus _bonus;
        public Match _match;
        public AccessLevel access;
        public PlayerDailyRecord Daily = new PlayerDailyRecord();
        public PlayerMissions _mission = new PlayerMissions();
        public PlayerStats _statistic = new PlayerStats();
        public FriendSystem FriendSystem = new FriendSystem();
        public PlayerTitles _titles = new PlayerTitles();
        public AccountStatus _status = new AccountStatus();
        public PlayerEvent _event;
        public DateTime LastLobbyEnter;
        public DateTime LastPingDebug;
        public PlayerRanked Ranked = new PlayerRanked();
        public DateTime LastFreeBonus; 
        public bool _damage = false;

        public Account() => LastLobbyEnter = DateTime.Now;

        public void SimpleClear()
        {
            _titles = new PlayerTitles();
            _mission = new PlayerMissions();
            _inventory = new PlayerInventory();
            _status = new AccountStatus();
            Characters = new List<Character>();
            Daily = new PlayerDailyRecord();
            Ranked = new PlayerRanked();
            _topups.Clear();
            _makevip.Clear();
            FriendSystem.CleanList();
            Session = null;
            _event = null;
            _match = null;
            _room = null;
            _config = null;
            _connection = null;
        }

        public void SetPublicIP(IPAddress address)
        {
            if (address == null)
                PublicIP = new IPAddress(new byte[4]);
            PublicIP = address;
        }

        public void SetPublicIP(string address) => PublicIP = IPAddress.Parse(address);

        public Channel getChannel() => ChannelsXml.getChannel(channelId);

        public void ResetPages()
        {
            LastRoomPage = 0;
            LastPlayerPage = 0;
        }

        public bool getChannel(out Channel channel)
        {
            channel = ChannelsXml.getChannel(channelId);
            return channel != null;
        }

        public void setOnlineStatus(bool online)
        {
            if (_isOnline == online || !ComDiv.updateDB("players", nameof(online), online, "player_id", player_id))
                return;
            _isOnline = online;
        }

        public void updateCacheInfo()
        {
            if (player_id == 0L)
                return;
            lock (AccountManager._accounts)
                AccountManager._accounts[player_id] = this;
        }

        public Character getCharacter(int ItemId)
        {
            for (int index = 0; index < Characters.Count; ++index)
            {
                Character character = Characters[index];
                if (character.Id == ItemId)
                    return character;
            }
            return null;
        }

        public Character getCharacterSlot(int Slot)
        {
            for (int index = 0; index < Characters.Count; ++index)
            {
                Character character = Characters[index];
                if (character.Slot == Slot)
                    return character;
            }
            return null;
        }

        public int getRank() => _bonus != null && _bonus.fakeRank != 55 ? _bonus.fakeRank : _rank;

        public void Close(int time, bool kicked = false)
        {
            if (_connection == null)
                return;
            _connection.Close(time, kicked);
        }

        public void SendPacket(SendPacket sp)
        {
            if (_connection == null)
                return;
            _connection.SendPacket(sp);
        }

        public void SendPacket(SendPacket sp, bool OnlyInServer)
        {
            if (_connection != null)
            {
                _connection.SendPacket(sp);
            }
            else
            {
                if (OnlyInServer || _status.serverId == byte.MaxValue || _status.serverId == GameConfig.serverId)
                    return;
                GameSync.SendBytes(player_id, sp, _status.serverId);
            }
        }

        public void SendPacket(byte[] data)
        {
            if (_connection == null)
                return;
            _connection.SendPacket(data);
        }

        public void SendPacket(byte[] data, bool OnlyInServer)
        {
            if (_connection != null)
            {
                _connection.SendPacket(data);
            }
            else
            {
                if (OnlyInServer || _status.serverId == byte.MaxValue || _status.serverId == GameConfig.serverId)
                    return;
                GameSync.SendBytes(player_id, data, _status.serverId);
            }
        }

        public void SendCompletePacket(byte[] data)
        {
            if (_connection == null)
                return;
            _connection.SendCompletePacket(data);
        }

        public void SendCompletePacket(byte[] data, bool OnlyInServer)
        {
            if (_connection != null)
            {
                _connection.SendCompletePacket(data);
            }
            else
            {
                if (OnlyInServer || _status.serverId == byte.MaxValue || _status.serverId == GameConfig.serverId)
                    return;
                GameSync.SendCompleteBytes(player_id, data, _status.serverId);
            }
        }

        public void LoadInventory()
        {
            lock (_inventory._items)
                _inventory._items.AddRange(PlayerManager.getInventoryItems(player_id));
        }

        public void LoadMissionList()
        {
            PlayerMissions mission = MissionManager.getInstance().getMission(player_id, _mission.mission1, _mission.mission2, _mission.mission3, _mission.mission4);
            if (mission == null)
                MissionManager.getInstance().addMissionDB(player_id);
            else
                _mission = mission;
        }

        public void LoadPlayerBonus()
        {
            PlayerBonus playerBonusDb = PlayerManager.getPlayerBonusDB(player_id);
            if (playerBonusDb.ownerId == 0L)
            {
                PlayerManager.CreatePlayerBonusDB(player_id);
                playerBonusDb.ownerId = player_id;
            }
            _bonus = playerBonusDb;
        }

        public uint getSessionId() => Session == null ? 0U : Session._sessionId;

        public void SetPlayerId(long id)
        {
            player_id = id;
            GetAccountInfos(35);
        }

        public void SetPlayerId(long id, int LoadType)
        {
            player_id = id;
            GetAccountInfos(LoadType);
        }

        public void GetAccountInfos(int LoadType)
        {
            if (LoadType <= 0 || player_id <= 0L)
                return;
            if ((LoadType & 1) == 1)
            {
                Characters = CharacterManager.getCharacters(player_id);
                _topups = PlayerManager.getPlayerTopups(player_id);
                _makevip = PlayerManager.getPlayerMakeVip(player_id);
                _titles = TitleManager.getInstance().getTitleDB(player_id);
                ICafe = ICafeManager.GetCafe(_connection.GetIPAddress());
                Daily = PlayerManager.getPlayerDailyRecord(player_id);
                Ranked = PlayerManager.getPlayerRanked(player_id);
                if (ICafe)
                {
                    pc_cafe = 1;
                    pc_cafe = 2;
                    pc_cafe = 3;
                    pc_cafe = 4;
                    pc_cafe = 5;
                    pc_cafe = 6;
                }
            }
            if ((LoadType & 2) == 2)
                _bonus = PlayerManager.getPlayerBonusDB(player_id);
            if ((LoadType & 4) == 4)
            {
                List<Friend> friendList = PlayerManager.getFriendList(player_id);
                if (friendList.Count > 0)
                {
                    FriendSystem._friends = friendList;
                    AccountManager.getFriendlyAccounts(FriendSystem);
                }
            }
            if ((LoadType & 8) == 8)
                _event = PlayerManager.getPlayerEventDB(player_id);
            if ((LoadType & 16) == 16)
                _config = PlayerManager.getConfigDB(player_id);
            if ((LoadType & 32) != 32)
                return;
            List<Friend> friendList1 = PlayerManager.getFriendList(player_id);
            if (friendList1.Count <= 0)
                return;
            FriendSystem._friends = friendList1;
        }

        public bool UseChatGM()
        {
            if (HideGMcolor)
                return false;
            return _rank == 53 || _rank == 54;
        }

        public bool IsGM() => _rank == 53 || _rank == 54 || HaveGMLevel();

        public bool HaveGMLevel() => access > AccessLevel.Streamer;

        public bool HaveAcessLevel() => access > AccessLevel.Normal;
    }
}
