namespace PlcGateway.Drivers.Inovance.Native
{
    internal enum ErrorCode : int
    {
        ERR_EIP_STOPED = -2,                              //协议栈未开启
        OTHER_ERROR = -1,                                 //其他错误   
        SUCCESS = 0,                                      //成功
        ERRI_INVALID_CONNECTION_INSTANCE_SPECIFIED = 1,   //连接的实例ID与已有的ID重复或超过最大值
        ERRI_CONN_CONFIG_FAILED_INVALID_NETWORK_PATH,     //连接的网络路径格式错误，无法检测出来目标IP离线等错误
        ERRI_CONNECTION_COUNT_LIMIT_REACHED,              //达到最大连接数量
        ERRI_OUT_OF_MEMORY,                               //内存溢出，缓冲区已满
        ERRR_CONN_CONFIG_FAILED_INVALID_NETWORK_PATH,     //连接的网络地址无效
        ERRR_CONN_CONFIG_FAILED_NO_RESPONSE,              //连接无响应
        ERRR_CONN_CONFIG_FAILED_ERROR_RESPONSE,           //连接响应错误
        ERRR_INVALID_DESTINATION,                         //目标标签不存在
        ERRR_TAGNAME_TOO_LONG,                            //标签名超过255字节
        ERRR_REQUEST_DATA_TOO_LARGE,                      //请求数据超限
        ERRR_CONN_CONNECTION_TIMED_OUT,                   //响应超时，请检查设备是否离线
        ERRR_TAGNAME_CONVERT_FAILED,                      //标签名解析错误
        ERRR_WRITE_DATASIZE_UNCONSISTENT,                 //写入数据大小与标签实际大小不一致
        ERRR_SCAN_ERROR,                                  //扫描标签信息失败
    }
}
