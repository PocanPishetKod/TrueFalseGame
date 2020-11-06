using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueFalse.Domain.Utils
{
    public static class EnumUtils
    {
        public static int MaxValue<TEnum>()
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"Тип аргумента ожидался enum. Пришло - {enumType.Name}");
            }

            return Enum.GetValues(enumType).Cast<int>().Max();
        }

        public static int MinValue<TEnum>()
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"Тип аргумента ожидался enum. Пришло - {enumType.Name}");
            }

            return Enum.GetValues(enumType).Cast<int>().Min();
        }
    }
}
