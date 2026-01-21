using System;
using System.Runtime.InteropServices;
using PlcGateway.Drivers.Inovance.Native;

namespace Inovance.EtherNetIP.Native
{
    /// <summary>
    /// 标签返回值结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ReadDataResult
    {
        /// <summary>
        /// 数据指针
        /// </summary>
        public IntPtr pData;

        /// <summary>
        /// 数据类型
        /// </summary>
        public TagType eType;

        /// <summary>
        /// 数据长度
        /// </summary>
        public int iDataLength;

        /// <summary>
        /// 从指针读取数据到字节数组
        /// </summary>
        public byte[] GetData()
        {
            if (pData == IntPtr.Zero || iDataLength <= 0)
                return Array.Empty<byte>();

            byte[] data = new byte[iDataLength];
            Marshal.Copy(pData, data, 0, iDataLength);
            return data;
        }
    }
}