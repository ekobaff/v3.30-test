using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Sql;

namespace PointBlank.Core.Xml
{
    // Token: 0x02000007 RID: 7
    public class CafeInventoryXml
    {
        // Token: 0x06000029 RID: 41 RVA: 0x00004D8C File Offset: 0x00002F8C
        // Token: 0x04000010 RID: 16
        public static List<ItemsModel> vipbasic = new List<ItemsModel>();
        public static List<ItemsModel> vipplus = new List<ItemsModel>();
        public static List<ItemsModel> vipmaster = new List<ItemsModel>();
        public static List<ItemsModel> vipcombat = new List<ItemsModel>();
        public static List<ItemsModel> vipextreme = new List<ItemsModel>();
        public static List<ItemsModel> vipbooster = new List<ItemsModel>();




        public static void Load()
        {
            try
            {
                using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand npgsqlCommand = npgsqlConnection.CreateCommand();
                    npgsqlConnection.Open();
                    npgsqlCommand.CommandText = "SELECT * FROM server_cafe";
                    npgsqlCommand.CommandType = CommandType.Text;
                    NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader();
                    while (npgsqlDataReader.Read())
                    {
                        int @int = npgsqlDataReader.GetInt32(0);
                        ItemsModel item = new ItemsModel(npgsqlDataReader.GetInt32(1))
                        {
                            _name = npgsqlDataReader.GetString(2),
                            _count = npgsqlDataReader.GetInt64(3),
                            _equip = npgsqlDataReader.GetInt32(4)
                        };
                        if (@int == 1)
                        {
                            CafeInventoryXml.vipbasic.Add(item);
                        }
                        if(@int ==  2)
                        {
                            CafeInventoryXml.vipplus.Add(item);
                        }
                        if (@int == 3)
                        {
                            CafeInventoryXml.vipmaster.Add(item);
                        }
                        if (@int == 4)
                        {
                            CafeInventoryXml.vipcombat.Add(item);
                        }
                        if (@int == 5)
                        {
                            CafeInventoryXml.vipextreme.Add(item);
                        }
                        if (@int == 6)
                        {
                            CafeInventoryXml.vipbooster.Add(item);
                        }

                        
                    }
                    npgsqlCommand.Dispose();
                    npgsqlDataReader.Close();
                    npgsqlConnection.Dispose();
                    npgsqlConnection.Close();


                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
        }


    }
}
