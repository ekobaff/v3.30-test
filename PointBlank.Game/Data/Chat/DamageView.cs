using PointBlank.Core;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Data.Chat
{
    public class DamageView
    {

        public static string SyncDAMAGE(Account player, Room room)
        {
            try
            {
              
                if (room != null && !room.isBotMode())
                {
                   player._damage = !player._damage;
                   return "HitMarker " + string.Concat(player._damage == true ? "Ativado" : "Desativado");
                }
                else
                    return "Você precisa está em sala normal";
            }
            catch
            {
                return "Error Grave!";
            }
        }

    }
}
