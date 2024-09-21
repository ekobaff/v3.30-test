using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Core.Models.Account.Rank
{
    public class RankedModel
    {
        public int _onNextLevel;
        public int _id;
        public int _onGPUp;
        public int _onCHUp;
        public int _onTGUp;


        public RankedModel(int rank, int onNextLevel, int onGPUp, int onCHUp, int onTGUp)
        {
            this._id = rank;
            this._onNextLevel = onNextLevel;
            this._onGPUp = onGPUp;
            this._onCHUp = onCHUp;
            this._onTGUp = onTGUp;

        }
    }
}
