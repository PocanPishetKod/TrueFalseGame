using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Client.Domain.Interfaces
{
    /// <summary>
    /// Сервис, предоставляющий функционал блокировки UI
    /// </summary>
    public interface IBlockUIService
    {
        /// <summary>
        /// Начинает блокирование UI
        /// </summary>
        void StartBlocking();

        /// <summary>
        /// Останавливает блокирование UI
        /// </summary>
        void StopBlocking();
    }
}
