using System;
using System.Collections.Generic;

namespace PlcGateway.Drivers.Inovance
{
    public class BytesConverter
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

        /// <summary>
        /// 汇川结构体成员，bool 占用两个字节
        /// 例如：01 00 表示结构体 bool 成员变量值为 true
        /// 该方法正确转换普通 bool 变量和结构体 bool 变量
        /// </summary>
        public static bool ToBoolean(byte[] bytes)
        {
            // 结构体 bool 变量：01 00
            // 取首字节
            // 普通 bool 变量，只有 1 字节
            return BitConverter.ToBoolean(bytes, 0);
        }
    }
}
