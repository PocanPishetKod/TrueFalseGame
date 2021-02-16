using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Domain.Models.Moves;

namespace TrueFalse.Application.Dtos.Results
{
    public enum MoveType
    {
        FirstMove,
        BeliveMove,
        DontBeliveMove
    }

    public static class MoveTypesUtils
    {
        public static MoveType GetMoveType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.Name == typeof(FirstMove).Name)
            {
                return MoveType.FirstMove;
            }

            if (type.Name == typeof(BelieveMove).Name)
            {
                return MoveType.BeliveMove;
            }

            if (type.Name == typeof(DontBelieveMove).Name)
            {
                return MoveType.DontBeliveMove;
            }

            throw new Exception("Нет подходящего типа хода");
        }
    }
}
