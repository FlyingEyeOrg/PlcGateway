namespace PlcGateway.Drivers.Inovance.Native
{
    internal enum ConnectionState : int
    {
        CONNECTION_NON_EXISTENT = 0X0,    //该实例未有连接
        CONNECTION_CONFIGURING = 0X1,    //连接正在打开过程中
        CONNECTION_ESTABLISHED = 0X3,    //连接已成功建立并在活动中
        CONNECTION_TIMEDOUT = 0X4,       //连接超时
        CONNECTION_CLOSING = 0X6         //连接正在关闭中
    }
}
