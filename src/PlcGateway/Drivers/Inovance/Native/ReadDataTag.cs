using System.Runtime.InteropServices;

namespace Inovance.EtherNetIP.Native
{
    /// <summary>
    /// 标签读取基础数据结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct ReadDataTag
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string pName;

        /// <summary>
        /// 元素个数
        /// </summary>
        public int iElementCount;

        /// <summary>
        /// 创建 ReadDataTag
        /// </summary>
        public ReadDataTag(string tagName, int elementCount = 1)
        {
            pName = tagName;
            iElementCount = elementCount;
        }
    }
}