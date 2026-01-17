using System;

namespace PlcGateway.Abstractions
{
    /// <summary>
    /// 设备驱动
    /// </summary>
    internal interface IPLCDriver : IDisposable
    {
        /// <summary>
        /// 连接到设备
        /// </summary>
        void Connect();

        /// <summary>
        /// 断开设备连接
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 写入 PLC 数据
        /// </summary>
        /// <typeparam name="TValue">需要写入的 PLC 数据类型</typeparam>
        /// <param name="address">需要写入的地址</param>
        /// <param name="value">需要写入的值</param>
        void Write<TValue>(string address, TValue value);

        /// <summary>
        /// 读取 PLC 数据
        /// </summary>
        /// <typeparam name="TValue">需要读取的 PLC 数据类型</typeparam>
        /// <param name="address">需要读取的地址</param>
        /// <returns>返回读取的数据</returns>
        TValue Read<TValue>(string address);
    }
}
