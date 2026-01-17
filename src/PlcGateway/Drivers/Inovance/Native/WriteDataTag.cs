using System;
using System.Runtime.InteropServices;
using PlcGateway.Drivers.Inovance.Native;

namespace Inovance.EtherNetIP.Native
{
    /// <summary>
    /// 标签写入数据结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct WriteDataTag
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string pName;

        /// <summary>
        /// 数据指针
        /// </summary>
        public IntPtr pData;

        /// <summary>
        /// 数据类型
        /// </summary>
        public TagType eType;  // 对应 C++ 的 TAG_TYPE，用 int 接收

        /// <summary>
        /// 数据长度
        /// </summary>
        public int iDataLength;

        /// <summary>
        /// 元素个数
        /// </summary>
        public int iElementCount;

        /// <summary>
        /// 创建 WriteDataTag
        /// </summary>
        public WriteDataTag(string tagName, byte[] data, TagType eType, int elementCount = 1)
        {
            pName = tagName;
            pData = IntPtr.Zero;
            this.eType = eType;
            iDataLength = data?.Length ?? 0;
            iElementCount = elementCount;

            if (data != null && data.Length > 0)
            {
                pData = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, pData, data.Length);
            }
        }

        public void Free()
        {
            if (pData != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pData);
                pData = IntPtr.Zero;
            }
        }
    }
}