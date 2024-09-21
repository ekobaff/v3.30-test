using Npgsql;
using PointBlank.Core.Models.Account.Rank;
using PointBlank.Core.Models.Gift;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointBlank.Core.Sql;

namespace PointBlank.Core.Xml
{
    public class WeaponExpXml
    {
        private static List<WeaponExp> ExpWeapon = new List<WeaponExp>();
        public static void Load()
        {
            try
            {
                using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = npgsqlConnection.CreateCommand();
                    ((DbConnection)npgsqlConnection).Open();
                    ((DbCommand)command).CommandText = "SELECT * FROM server_weapon;";
                    ((DbCommand)command).CommandType = CommandType.Text;
                    NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
                    while (((DbDataReader)npgsqlDataReader).Read())
                        WeaponExpXml.ExpWeapon.Add(new WeaponExp(((DbDataReader)npgsqlDataReader).GetInt32(0), ((DbDataReader)npgsqlDataReader).GetInt32(1)));
                    ((Component)command).Dispose();
                    ((DbDataReader)npgsqlDataReader).Close();
                    ((Component)npgsqlConnection).Dispose();
                    ((DbConnection)npgsqlConnection).Close();
                }
               
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
        }
        public static WeaponExp GetWeaponExp(int rp)
        {
            lock (WeaponExpXml.ExpWeapon)
            {
                for (int index = 0; index < WeaponExpXml.ExpWeapon.Count; ++index)
                {
                    WeaponExp weap = WeaponExpXml.ExpWeapon[index];
                    if (weap._Weapon == rp)
                        return weap;
                }
                return (WeaponExp)null;
            }
        }
    }
}
