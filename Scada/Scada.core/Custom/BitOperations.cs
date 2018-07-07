using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.core.Custom
{
    public static class BitOperations
    {
        /// <summary>
        /// Toggle bit of byte
        /// </summary>
        /// <param name="InByte"></param>
        /// <param name="BitNo"></param>
        /// <returns></returns>
        public static byte ToogleBit(byte InByte, byte BitNo)
        {
            int buff = InByte;
            buff ^= 1 << BitNo;
            return (byte)buff;
        }

        /// <summary>
        /// Is bit set? 
        /// </summary>
        /// <param name="InByte"></param>
        /// <param name="BitNo"></param>
        /// <returns>true or false</returns>
        public static bool IsBitSet(byte InByte, byte BitNo)
        {
            return (InByte & (1 << BitNo)) != 0;
        }
    }
}
