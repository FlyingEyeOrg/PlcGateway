using System;
using System.Collections.Generic;

namespace PlcGateway.Drivers.Inovance
{
    internal class InovanceByteArrayConverter
    {
        /// <summary>
        /// 汇川结构体成员，bool 占用两个字节
        /// 例如：01 00 表示结构体 bool 成员变量值为 true
        /// </summary>
        public static byte[] ToMemberBytes(bool value)
        {
            var result = new List<byte>();
            var bytes = BitConverter.GetBytes(value);
            result.AddRange(bytes);
            result.Add(0);
            return result.ToArray();
        }
    }
}
