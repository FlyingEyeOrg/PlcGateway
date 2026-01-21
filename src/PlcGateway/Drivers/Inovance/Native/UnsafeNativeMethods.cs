using System;
using System.Runtime.InteropServices;
using PlcGateway.Drivers.Inovance.Native;

namespace Inovance.EtherNetIP.Native
{
    internal static class UnsafeNativeMethods
    {
        /// <summary>
        /// 序号：1
        /// extern "C" __declspec(dllexport) bool DeleteTagListStru(TagRetValue* pRetValue, int iNumOfTags);
        /// 删除标签列表结构
        /// </summary>
        /// <param name="pResult">标签返回值结构数组指针</param>
        /// <param name="iNumOfTags">标签数量</param>
        /// <returns>成功返回 true，失败返回 false</returns>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]  // bool 在 C++ 中是 1 字节
        public static extern bool DeleteTagListStru(
            [In] ReadDataResult[] pResult,
            int iNumOfTags);

        /*
         * 这两个不需要导入
         * 2    1 000020C0 EipClaimAllHostIPAddress
         * 3    2 00002010 EipClaimHostIPAddress
         */

        /// <summary>
        /// 序号：4
        /// extern "C" __declspec(dllexport) ERROR_NO EipCloseConnection(int iInstanceID);
        /// 关闭一个 EIP 连接
        /// </summary>
        /// <param name="iInstanceID">需关闭的实例ID</param>
        /// <returns>成功返回SUCCESS，失败返回ERROR_NO类型对应的错误码</returns>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ErrorCode EipCloseConnection(int iInstanceID);

        /// <summary>
        /// 序号：5
        /// extern "C" __declspec(dllexport) EtIPConnectionState EipGetConnectionState(int iInstanceID);
        /// 获取连接的状态
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ConnectionState EipGetConnectionState(int iInstanceID);

        /// <summary>
        /// 序号：6
        /// 创建连接
        /// </summary>
        /// <param name="pIpAddress">目标设备的 IP 地址</param>
        /// <param name="pInstanceID">指令类型返回自动分配的实例 ID（指针类型传参）</param>
        /// <returns>
        /// 成功：SUCCESS
        /// 失败：返回 ErrorCode 类型对应的错误码
        /// </returns>
        /// <remarks>
        /// 调用后阻塞等待协议栈创建连接完成，连接完成后返回。
        /// </remarks>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipOpenConnection(
            [MarshalAs(UnmanagedType.LPStr)] string pIpAddress,
            out int pInstanceID
        );


        /// <summary>
        /// 序号：7
        /// extern "C" __declspec(dllexport) int EipReadTag(
        /// int iInstanceID, 
        /// const char* pTagName, 
        /// TAG_TYPE* pType, 
        /// unsigned char* pDest, 
        /// int iDestLength, 
        /// unsigned short iElementCount = 1, 
        /// int iArrayPos = INVALID_MEMBER);
        /// 返回值：
        /// 成功：返回实际读取数据的长度
        /// 失败：返回对应的错误码
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int EipReadTag(
        int iInstanceID,
        [MarshalAs(UnmanagedType.LPStr)] string pTagName,
        out TagType pType,
        byte[] pDest,
        int iDestLength,
        ushort iElementCount = 1,
        int iArrayPos = -1
   );

        /// <summary>
        /// 序号8：
        /// extern "C" __declspec(dllexport) int EipReadTagExt(
        /// int iInstanceID, 
        /// const char* pTagName, 
        /// TAG_TYPE * pType, 
        /// unsigned char* pDest, 
        /// int iDestLength, 
        /// unsigned short iElementCount = 1);
        /// 读取标签值
        /// </summary>
        /// <param name="iInstanceID">连接对应的实例ID</param>
        /// <param name="pTagName">标签名称（支持数组和结构体访问语法）</param>
        /// <param name="pType">返回的数据类型</param>
        /// <param name="pDest">返回的数据缓冲区指针</param>
        /// <param name="iDestLength">数据缓冲区长度（字节数）</param>
        /// <param name="iElementCount">元素个数，默认1</param>
        /// <returns>
        /// 成功：返回读取的字节数
        /// 失败：返回对应的错误码
        /// -1：其他错误
        /// -2：协议栈未开启
        /// -3：实例 ID 小于或等于 0
        /// -4：标签名长度大于 255 字节
        /// -5：目标标签不存在
        /// -6：响应超时，请检查设备是否离线
        /// -7：标签名解析错误
        /// -8：扫描标签信息失败
        /// </returns>
        /// <remarks>
        /// 函数作用：读取标签值。接口说明同EipReadTag()
        /// 区别是：EipReadTagExt()对标签名进行了解析转换。对于数组类型不用传索引参数，而是通过标签名加中括号的方式实现对数组指定元素的访问，如"tag[2]"访问数组标签tag下标为2的成员，通过标签名加“.”的方式访问结构体成员，如"Stru.arr"访问结构体标签Stru的成员arr；
        /// </remarks>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int EipReadTagExt(
            int iInstanceID,
            [MarshalAs(UnmanagedType.LPStr)] string pTagName,
            out TagType pType,
            byte[] pDest,
            int iDestLength,
            ushort iElementCount = 1
        );

        /// <summary>
        /// 序号：9
        /// extern "C" __declspec(dllexport) ERROR_NO EipReadTagExt2(int iInstanceID, TagReadDataBase* pTagReadData, TagRetValue* pTagDest);
        /// 读取单个标签值
        /// </summary>
        /// <param name="iInstanceID">访问连接的实例 ID</param>
        /// <param name="readDataTag">需要访问的标签信息指针，包括要访问的标签名及元素个数</param>
        /// <param name="readValueResult">返回的标签数据指针，返回标签数据、数据类型及长度信息</param>
        /// <returns>
        /// 成功：SUCCESS
        /// 失败：返回 ERROR_NO 类型对应的错误码
        /// </returns>
        /// <remarks>
        /// 函数作用：读取标签值。接口说明同 EipReadTagExt()，区别是：
        /// 1. EipReadTagExt2() 对其参数进行了封装，将用户配置的参数封装为结构体 pTagReadData，
        ///    返回的标签数据封装为结构体 pTagDest；
        /// 2. 增加标签扫描功能。发起请求时会先扫描标签信息，接口可不填数据类型参数 eType，
        ///    内部会通过扫描获取；
        /// 3. EipReadTagExt2() 取消了字节数限制；
        /// 
        /// 说明：
        /// 1. 对于数组类型，可以通过标签名加中括号的方式实现对数组指定元素的访问，
        ///    如 "tag[2]" 访问数组标签 tag 下标为 2 的成员；对于结构体类型，通过标签名
        ///    加 "." 的方式访问结构体成员，如 "Stru.mem1" 访问结构体标签 Stru 的成员 mem1。
        ///    其他接口相同。
        /// 2. 注意，调用此接口读取成功后需调用 DeleteTagListStru() 释放内存。
        /// </remarks>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ErrorCode EipReadTagExt2(
            int iInstanceID,
            [In] ref ReadDataTag readDataTag,
            [Out] out ReadDataResult readValueResult
        );

        /// <summary>
        /// 序号：10
        /// extern "C" __declspec(dllexport) ERROR_NO EipReadTagList(int iInstanceID, unsigned int iNumOfTags, TagReadData* pTagList, TagRetValue* pDest);
        /// 读取多标签值
        /// </summary>
        /// <param name="iInstanceID">连接对应的实例ID</param>
        /// <param name="iNumOfTags">标签个数</param>
        /// <param name="pReadDataTag">标签列表指针，包含要读取的标签信息数组</param>
        /// <param name="pReadDataResult">返回的标签数据列表指针，用于接收读取结果数组</param>
        /// <returns>
        /// 成功：<see cref="ErrorCode.SUCCESS"/>
        /// 失败：返回<see cref="ErrorCode"/>类型对应的错误码
        /// </returns>
        /// <remarks>
        /// 此方法用于批量读取多个标签的值，支持数组和结构体元素的访问。
        /// 需要预先创建标签信息数组和结果数组，数组大小必须与 iNumOfTags 一致。
        /// </remarks>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ErrorCode EipReadTagList(
            int iInstanceID,
            uint iNumOfTags,
            [In] ReadDataTag[] pReadDataTag,
            [Out] ReadDataResult[] pReadDataResult
        );

        /// <summary>
        /// 序号：11
        /// 读取多标签值（扩展版本）
        /// </summary>
        /// <param name="iInstanceID">连接对应的实例ID</param>
        /// <param name="iNumOfTags">标签个数</param>
        /// <param name="pReadDataTag">标签列表指针，包含要读取的标签信息数组</param>
        /// <param name="pReadDataResult">返回的标签数据列表指针，用于接收读取结果数组</param>
        /// <param name="bScan">
        /// 是否在发起请求前进行标签扫描。
        /// <list type="bullet">
        /// <item><description>false：不进行标签扫描，直接读取标签值（适用于标签已确认存在的情况）</description></item>
        /// <item><description>true：先扫描标签信息，确认标签存在后再读取（适用于动态标签或标签可能不存在的情况）</description></item>
        /// </list>
        /// 默认值为 false。
        /// </param>
        /// <returns>
        /// 成功：<see cref="ErrorCode.SUCCESS"/>
        /// 失败：返回<see cref="ErrorCode"/>类型对应的错误码
        /// </returns>
        /// <remarks>
        /// <para>此方法用于批量读取多个标签的值，是 <see cref="EipReadTagList"/> 的扩展版本，提供更灵活的标签名解析功能。</para>
        /// 
        /// <para><strong>与 EipReadTagList() 的主要区别：</strong></para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// <strong>标签名解析转换：</strong>支持更灵活的标签名语法：
        /// <list type="bullet">
        /// <item><description>访问数组元素：使用"标签名[索引]"格式，如 "DataArray[2]" 访问数组标签 DataArray 下标为 2 的成员</description></item>
        /// <item><description>访问结构体成员：使用"结构体名.成员名"格式，如 "Machine.Status" 访问结构体标签 Machine 的 Status 成员</description></item>
        /// <item><description>支持嵌套访问：如 "Machine.Motor[0].Speed" 访问嵌套结构体的数组成员</description></item>
        /// </list>
        /// 参数 <paramref name="pReadDataTag"/> 中的 TagReadDataBase 结构体省去了数组索引成员，统一通过标签名字符串表达。
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <strong>标签扫描控制：</strong>新增 <paramref name="bScan"/> 参数，控制是否在读取前进行标签扫描：
        /// <list type="bullet">
        /// <item><description>当 bScan = false 时，直接读取标签值，性能更高，但要求标签必须存在</description></item>
        /// <item><description>当 bScan = true 时，先扫描标签信息，确认标签存在后再读取，适用于标签可能动态创建或删除的场景</description></item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// 
        /// <para><strong>使用注意事项：</strong></para>
        /// <list type="bullet">
        /// <item><description>需要预先创建标签信息数组和结果数组，数组大小必须与 <paramref name="iNumOfTags"/> 一致</description></item>
        /// <item><description>标签名字符串支持 ANSI 编码，最大长度应符合 PLC 系统限制</description></item>
        /// <item><description>当读取不存在的标签时，返回的错误码取决于 <paramref name="bScan"/> 参数设置</description></item>
        /// <item><description>对于大型数组或嵌套结构体的访问，建议设置 bScan = true 以确保标签路径正确</description></item>
        /// </list>
        /// 
        /// <para><strong>性能建议：</strong></para>
        /// <list type="bullet">
        /// <item><description>对于已知存在的固定标签，使用 bScan = false 以获得最佳性能</description></item>
        /// <item><description>对于动态标签或用户输入的标签名，使用 bScan = true 以提高可靠性</description></item>
        /// <item><description>批量读取大量标签时，bScan = true 会有额外的性能开销</description></item>
        /// </list>
        /// 
        /// <para><strong>示例用法：</strong></para>
        /// <code>
        /// // 创建标签数组
        /// ReadDataTag[] tags = new ReadDataTag[]
        /// {
        ///     new ReadDataTag("DataArray[0]"),      // 读取数组第一个元素
        ///     new ReadDataTag("Machine.Status"),     // 读取结构体成员
        ///     new ReadDataTag("Machine.Motor[0].Speed")  // 读取嵌套结构体的数组成员
        /// };
        /// 
        /// // 创建结果数组
        /// ReadDataResult[] results = new ReadDataResult[tags.Length];
        /// 
        /// // 调用（不扫描，直接读取）
        /// ErrorCode error = EipReadTagListExt(instanceId, (uint)tags.Length, tags, results, false);
        /// 
        /// // 或者（先扫描再读取）
        /// error = EipReadTagListExt(instanceId, (uint)tags.Length, tags, results, true);
        /// 
        /// extern "C" __declspec(dllexport) ERROR_NO EipReadTagListExt(int iInstanceID, unsigned int iNumOfTags, TagReadDataBase* pTagList, TagRetValue* pDest, bool bScan = false);
        /// </code>
        /// </remarks>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipReadTagListExt(
            int iInstanceID,
            uint iNumOfTags,
            [In] ReadDataTag[] pReadDataTag,
            [Out] ReadDataResult[] pReadDataResult,
            [MarshalAs(UnmanagedType.I1)] bool bScan = false
        );

        /// <summary>
        /// 序号：12
        /// extern "C" __declspec(dllexport) ERROR_NO EipReadTagListRaw(int iInstanceID, unsigned int iNumOfTags, TagReadDataBase * pTagList, TagRetValue * pDest);
        /// 接口说明同 EipReadTagListExt()
        /// 区别是：接口返回协议原始数据（BOOL数组在协议中的排列方式见使用说明文档）
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipReadTagListRaw(
           int iInstanceID,
           uint iNumOfTags,
           [In] ReadDataTag[] pReadDataTag,
           [Out] ReadDataResult[] pReadDataResult
        );

        /// <summary>
        /// 序号：13
        /// 读取多标签值。接口说明同EipReadTagListExt()，区别是：接口参数增加对齐方式eAlignType
        /// extern "C" __declspec(dllexport) ERROR_NO EipReadTagListWithAlignment(
        /// int iInstanceID, 
        /// unsigned int iNumOfTags, 
        /// TAG_AlignType eAlignType, 
        /// TagReadDataBase * pTagList, 
        /// TagRetValue * pDest);
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipReadTagListWithAlignment(
              int iInstanceID,
              uint iNumOfTags,
              AlignType eAlignType,
              [In] ReadDataTag[] pReadDataTag,
              [Out] ReadDataResult[] pReadDataResult
           );

        /// <summary>
        /// 序号：14
        /// extern "C" __declspec(dllexport) ERROR_NO EipReadTagRaw(int iInstanceID, TagReadDataBase * pTagReadData, TagRetValue * pTagDest);
        /// 读取标签值。接口说明同EipReadTagExt2()，区别是：接口返回协议原始数据（BOOL数组在协议中的排列方式见使用说明文档）
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipReadTagRaw(
            int iInstanceID,
            uint iNumOfTags,
            [In] ref ReadDataTag pReadDataTag,
            [Out] out ReadDataResult pReadDataResult);

        /// <summary>
        /// 序号：15
        /// 读取标签值。
        /// 接口说明同EipReadTagExt2()
        /// 区别是：接口参数增加对齐方式eAlignType
        /// extern "C" __declspec(dllexport) ERROR_NO EipReadTagWithAlignment(int iInstanceID, TAG_AlignType eAlignType, TagReadDataBase * pTagReadData, TagRetValue * pTagDest);
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipReadTagWithAlignment(
            int iInstanceID,
            AlignType eAlignType,
            [In] ref ReadDataTag pReadDataTag,
            [Out] out ReadDataResult pReadDataResult);

        /// <summary>
        /// 序号：16
        /// 启动 EIP 协议栈
        /// extern "C" __declspec(dllexport) void EipStart();
        /// </summary>
        /// <remarks>
        /// 调用该函数，立即启动协议栈并返回。此接口默认绑定第一个 IP，如果电脑存在多个 IP，
        /// 建议使用 EipStartExt() 接口。
        /// </remarks>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void EipStart();

        /// <summary>
        /// 序号：17
        /// 启动 EIP 协议栈（扩展版本）
        /// extern "C" __declspec(dllexport) bool EipStartExt(const char* pIpAddress, unsigned int iPort = 0);
        /// </summary>
        /// <param name="pIpAddress">上位机指定的网卡 IP</param>
        /// <param name="iPort">端口号，目前暂不支持修改，输入 0 即可</param>
        /// <returns>
        /// 成功：true
        /// 失败：false
        /// </returns>
        /// <remarks>
        /// 输入指定的上位机网卡 IP（多网卡），立即启动协议栈并返回。
        /// </remarks>
        [DllImport(
            "EipTagSimple.dll",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool EipStartExt([MarshalAs(UnmanagedType.LPStr)] string pIpAddress, uint iPort = 0);

        /// <summary>
        /// 序号：18
        /// 开启EIP协议栈
        /// extern "C" __declspec(dllexport) bool EipStartProtocol();
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool EipStartProtocol();

        /// <summary>
        /// 序号：19
        /// 关闭协议栈
        /// extern "C" __declspec(dllexport) void  EipStop();
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void EipStop();


        /*
         * 这两个不需要导入
         * 20   13 00002100 EipUnclaimAllHostIPAddress
         * 21   14 00002070 EipUnclaimHostIPAddress
         */

        /// <summary>
        /// 序号：22
        /// 写入标签值
        /// extern "C" __declspec(dllexport) ERROR_NO EipWriteTag(
        /// int iInstanceID, 
        /// const char* pTagName, 
        /// TAG_TYPE eType, 
        /// const unsigned char* pSource, 
        /// int iDataLength, 
        /// unsigned short iElementCount = 1, 
        /// int iArrayPos = INVALID_MEMBER);
        /// </summary>
        /// <param name="iInstanceID">连接对应的实例ID</param>
        /// <param name="pTagName">标签名</param>
        /// <param name="pType">要写入标签的数据类型</param>
        /// <param name="pSource">要写入的数据首地址</param>
        /// <param name="iDataLength">写入数据的长度</param>
        /// <param name="iElementCount">请求写入的元素个数（针对数组类型，默认1）</param>
        /// <param name="iArrayPos">从数组指定位置开始写入（针对数组类型，默认-1表示不指定位置）</param>
        /// <returns>
        /// 成功：<see cref="ErrorCode.SUCCESS"/>
        /// 失败：返回对应的错误码
        /// </returns>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipWriteTag(
           int iInstanceID,
           [MarshalAs(UnmanagedType.LPStr)] string pTagName,
           TagType pType,
           byte[] pSource,
           int iDataLength,
           ushort iElementCount = 1,
           int iArrayPos = -1
       );

        /// <summary>
        /// 序号：23
        /// 
        /// EipWriteTagExt()对标签名进行了解析转换。
        /// 对于数组类型不用传索引参数，而是通过标签名加中括号的方式实现对数组指定元素的访问，
        /// 如"tag[2]"访问数组标签tag下标为2的成员，通过标签名加“.”的方式访问结构体成员，如"Stru.arr"访问结构体标签Stru的成员arr；
        /// 
        /// extern "C" __declspec(dllexport) ERROR_NO EipWriteTagExt(
        /// int iInstanceID, 
        /// const char* pTagName, 
        /// TAG_TYPE eType, 
        /// const unsigned char* pSource, 
        /// int iDataLength, 
        /// unsigned short iElementCount = 1);
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipWriteTagExt(
           int iInstanceID,
           [MarshalAs(UnmanagedType.LPStr)] string pTagName,
           TagType eType,
           byte[] pSource,
           int iDataLength,
           ushort iElementCount = 1
        );

        /// <summary>
        /// 序号：24
        /// 写入单个标签值
        /// </summary>
        /// <param name="iInstanceID">访问连接的实例 ID</param>
        /// <param name="pWriteDataTag">写入的标签数据，包括标签名，要写入的数据，数据长度和元素个数</param>
        /// <returns>
        /// 成功：SUCCESS
        /// 失败：返回 ErrorCode 类型对应的错误码
        /// </returns>
        /// <remarks>
        /// 函数作用：写入标签值。接口功能同 EipWriteTagExt()，区别是：
        /// 1. EipWriteTagExt2() 对其参数进行了封装，将用户配置的参数封装为结构体 pTagWritenData；
        /// 2. 增加标签扫描功能。发起请求时会先扫描标签信息，接口可不填数据类型参数 eType，内部会通过扫描获取；
        /// 3. EipWriteTagExt2() 取消了字节数限制；
        /// 
        /// 说明：发起请求时会先扫描标签信息，接口可不填数据类型参数 eType，内部会通过扫描获取。
        /// </remarks>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ErrorCode EipWriteTagExt2(
            int iInstanceID,
            [In] ref WriteDataTag pWriteDataTag
        );

        /// <summary>
        /// 序号：25
        /// 写入标签值。接口说明同EipWriteTagExt2()，区别是：将原来要传的结构体参数拆成多个参数
        /// extern "C" __declspec(dllexport) ERROR_NO EipWriteTagExt3(int iInstanceID, const char* pName, unsigned char* pData, int iDataLength, int iElementCount);
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ErrorCode EipWriteTagExt3(
            int iInstanceID,
            [MarshalAs(UnmanagedType.LPStr)] string pTagName,
            byte[] pData,
            int iDataLength,
            int iElementCount
        );

        /// <summary>
        /// 序号：26
        /// 写入多标签值
        /// extern "C" __declspec(dllexport) ERROR_NO EipWriteTagList(int iInstanceID, unsigned int iNumOfTags, TagWriteData* pTagWritenData);
        /// </summary>
        /// <param name="iInstanceID">连接对应的实例ID</param>
        /// <param name="iNumOfTags">标签个数</param>
        /// <param name="pWriteDataTag">写入的标签数据数组指针，包含要写入的标签信息</param>
        /// <returns>
        /// 成功：<see cref="ErrorCode.SUCCESS"/>
        /// 失败：返回<see cref="ErrorCode"/>类型对应的错误码
        /// </returns>
        /// <remarks>
        /// 此方法用于批量写入多个标签的值，支持数组和结构体元素的访问。
        /// 需要预先创建并初始化写入数据数组，数组大小必须与 iNumOfTags 一致。
        /// 注意：调用此方法后，WriteDataTag 结构体中的非托管内存需要手动释放。
        /// </remarks>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ErrorCode EipWriteTagList(
            int iInstanceID,
            uint iNumOfTags,
            [In] WriteDataTag[] pWriteDataTag
        );

        /// <summary>
        /// 序号：27
        /// 写入多标签值（扩展版本）
        /// </summary>
        /// <param name="iInstanceID">连接对应的实例ID</param>
        /// <param name="iNumOfTags">标签个数</param>
        /// <param name="pWriteDataTag">写入的标签数据数组指针，包含要写入的标签信息数组</param>
        /// <param name="bScan">
        /// 是否在发起写入请求前进行标签扫描。
        /// <list type="bullet">
        /// <item><description>false：不进行标签扫描，直接写入标签值（适用于标签已确认存在的情况）</description></item>
        /// <item><description>true：先扫描标签信息，确认标签存在后再写入（适用于动态标签或标签可能不存在的情况）</description></item>
        /// </list>
        /// 默认值为 false。
        /// </param>
        /// <returns>
        /// 成功：<see cref="ErrorCode.SUCCESS"/>
        /// 失败：返回<see cref="ErrorCode"/>类型对应的错误码
        /// </returns>
        /// <remarks>
        /// <para>此方法用于批量写入多个标签的值，是 <see cref="EipWriteTagList"/> 的扩展版本，提供更灵活的标签名解析功能。</para>
        /// 
        /// <para><strong>与 EipWriteTagList() 的主要区别：</strong></para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// <strong>标签名解析转换：</strong>支持更灵活的标签名语法：
        /// <list type="bullet">
        /// <item><description>写入数组元素：使用"标签名[索引]"格式，如 "DataArray[2]" 写入数组标签 DataArray 下标为 2 的成员</description></item>
        /// <item><description>写入结构体成员：使用"结构体名.成员名"格式，如 "Machine.Status" 写入结构体标签 Machine 的 Status 成员</description></item>
        /// <item><description>支持嵌套写入：如 "Machine.Motor[0].Speed" 写入嵌套结构体的数组成员</description></item>
        /// </list>
        /// 参数 <paramref name="pWriteDataTag"/> 中的 WriteDataTag 结构体省去了数组索引成员，统一通过标签名字符串表达。
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <strong>标签扫描控制：</strong>新增 <paramref name="bScan"/> 参数，控制是否在写入前进行标签扫描：
        /// <list type="bullet">
        /// <item><description>当 bScan = false 时，直接写入标签值，性能更高，但要求标签必须存在且类型匹配</description></item>
        /// <item><description>当 bScan = true 时，先扫描标签信息，确认标签存在且类型匹配后再写入，适用于标签可能动态创建或类型不确定的场景</description></item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// 
        /// <para><strong>使用注意事项：</strong></para>
        /// <list type="bullet">
        /// <item><description>需要预先创建并初始化写入数据数组，数组大小必须与 <paramref name="iNumOfTags"/> 一致</description></item>
        /// <item><description>标签名字符串支持 ANSI 编码，最大长度应符合 PLC 系统限制</description></item>
        /// <item><description>写入数据的类型必须与 PLC 中标签定义的类型匹配，否则会返回类型不匹配错误</description></item>
        /// <item><description>对于数组写入，需要确保索引不越界；对于结构体写入，需要确保成员存在</description></item>
        /// <item><description>调用此方法后，WriteDataTag 结构体中的非托管内存需要手动释放（如果使用了手动内存分配）</description></item>
        /// </list>
        /// 
        /// <para><strong>性能建议：</strong></para>
        /// <list type="bullet">
        /// <item><description>对于已知存在的固定标签，使用 bScan = false 以获得最佳性能</description></item>
        /// <item><description>对于动态标签、用户输入的标签名或不确定类型的标签，使用 bScan = true 以提高可靠性和安全性</description></item>
        /// <item><description>批量写入大量标签时，bScan = true 会有额外的性能开销，建议在调试阶段使用，生产环境酌情使用</description></item>
        /// </list>
        /// 
        /// <para><strong>错误处理：</strong></para>
        /// <list type="bullet">
        /// <item><description>当标签不存在时，如果 bScan = true 会返回标签不存在错误，如果 bScan = false 可能返回其他错误（如连接错误）</description></item>
        /// <item><description>当数据类型不匹配时，会返回类型错误</description></item>
        /// <item><description>当数组索引越界时，会返回数组索引错误</description></item>
        /// <item><description>当结构体成员不存在时，会返回成员不存在错误</description></item>
        /// </list>
        /// 
        /// <para><strong>示例用法：</strong></para>
        /// <code>
        /// // 创建写入标签数组
        /// WriteDataTag[] tags = new WriteDataTag[]
        /// {
        ///     new WriteDataTag("DataArray[0]", BitConverter.GetBytes(100), TagType.TAG_TYPE_DINT, 1),
        ///     new WriteDataTag("Machine.Status", BitConverter.GetBytes((short)1), TagType.TAG_TYPE_INT, 1),
        ///     new WriteDataTag("Machine.Motor[0].Speed", BitConverter.GetBytes(1500), TagType.TAG_TYPE_DINT, 1)
        /// };
        /// 
        /// // 调用（不扫描，直接写入）
        /// ErrorCode error = EipWriteTagListExt(instanceId, (uint)tags.Length, tags, false);
        /// 
        /// // 或者（先扫描再写入）
        /// error = EipWriteTagListExt(instanceId, (uint)tags.Length, tags, true);
        /// 
        /// // 记得释放内存
        /// foreach (var tag in tags)
        /// {
        ///     tag.Free();
        /// }
        /// 
        /// extern "C" __declspec(dllexport) ERROR_NO EipWriteTagListExt(int iInstanceID, unsigned int iNumOfTags, TagWriteDataBase* pTagWritenData, bool bScan = false);
        /// </code>
        /// </remarks>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipWriteTagListExt(
            int iInstanceID,
            uint iNumOfTags,
            [In] WriteDataTag[] pWriteDataTag,
            [MarshalAs(UnmanagedType.I1)] bool bScan = false
        );

        /// <summary>
        /// 序号：28
        /// 写入多标签值。接口说明同EipWriteTagListExt()，区别是：写入数据需遵循协议传输规则（BOOL数组在协议中的排列方式见使用说明文档）
        /// extern "C" __declspec(dllexport) ERROR_NO EipWriteTagListRaw(int iInstanceID, unsigned int iNumOfTags, TagWriteDataBase* pTagWritenData);
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipWriteTagListRaw(
            int iInstanceID,
            uint iNumOfTags,
            [In] WriteDataTag[] pWriteDataTag
        );

        /// <summary>
        /// 序号：29
        /// 写入多标签值。接口说明同EipWriteTagListExt()，区别是：接口参数增加对齐方式eAlignType
        /// extern "C" __declspec(dllexport) ERROR_NO EipWriteTagListWithAlignment(
        /// int iInstanceID, unsigned int iNumOfTags, 
        /// TAG_AlignType eAlignType, 
        /// TagWriteDataBase* pTagWritenData);
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipWriteTagListWithAlignment(
            int iInstanceID,
            uint iNumOfTags,
            AlignType eAlignType,
            [In] WriteDataTag[] pWriteDataTag
        );

        /// <summary>
        /// 序号：30
        /// 写入标签值，接口说明同 EipWriteTagExt2()
        /// 区别是：写入数据需遵循协议传输规则（BOOL数组在协议中的排列方式见使用说明文档）
        /// extern "C" __declspec(dllexport) ERROR_NO EipWriteTagRaw(int iInstanceID, TagWriteDataBase * pTagWritenData);
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipWriteTagRaw(
            int iInstanceID,
            [In] ref WriteDataTag pWriteDataTag
        );

        /// <summary>
        /// 序号：31
        /// 写入标签值。
        /// 接口说明同EipWriteTagExt2()，区别是：接口参数增加对齐方式eAlignType
        /// extern "C" __declspec(dllexport) ERROR_NO EipWriteTagWithAlignment(int iInstanceID, TAG_AlignType eAlignType, TagWriteDataBase * pTagWritenData);
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ErrorCode EipWriteTagWithAlignment(
            int iInstanceID,
            AlignType eAlignType,
            [In] ref WriteDataTag pWriteDataTag
        );

        /// <summary>
        /// 序号：32
        /// 重置标签信息。
        /// 当修改PLC标签变量并下载到PLC时，应调用此接口重置缓存的标签信息，否则可能会访问失败或数据异常。
        /// extern "C" __declspec(dllexport) void ResetTagInfo();
        /// </summary>
        [DllImport("EipTagSimple.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void ResetTagInfo();
    }
}