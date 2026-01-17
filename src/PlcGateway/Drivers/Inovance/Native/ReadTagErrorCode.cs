namespace PlcGateway.Drivers.Inovance.Native
{
    internal enum ReadTagErrorCode : int
    {
        ERRR_SCAN_ERROR = -8,                             // 扫描标签信息失败
        ERRR_TAGNAME_CONVERT_FAILED = -7,                 // 标签名解析错误
        ERRR_CONN_CONNECTION_TIMED_OUT = -6,              // 响应超时，请检查设备是否离线
        ERRR_INVALID_DESTINATION = -5,                    // 目标标签不存在
        ERRR_TAGNAME_TOO_LONG = -4,                       // 标签名长度大于 255 字节
        ERRI_INVALID_CONNECTION_INSTANCE_SPECIFIED = -3,  // 实例 ID 小于或等于 0
        ERR_EIP_STOPED = -2,                              // 协议栈未开启
        OTHER_ERROR = -1,                                 // 其他错误   
    }
}
