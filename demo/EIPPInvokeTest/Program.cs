using System;
using System.Data;
using System.Runtime.InteropServices;
using Inovance.EtherNetIP.Native;
using PlcGateway.Drivers.Inovance.Native;
using ConnectionState = PlcGateway.Drivers.Inovance.Native.ConnectionState;

namespace PInvokeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始测试 P/Invoke 封送...");

            try
            {
                TestAllMethods();
                Console.WriteLine("所有 P/Invoke 调用测试完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试失败: {ex.Message}");
                Console.WriteLine($"堆栈: {ex.StackTrace}");
            }

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }

        static void TestAllMethods()
        {
            int testInstanceId = 123;
            string testIp = "192.168.1.100";

            Console.WriteLine("\n=== 测试序号1: DeleteTagListStru ===");
            try
            {
                var results = new ReadDataResult[1];
                bool deleteResult = UnsafeNativeMethods.DeleteTagListStru(results, 1);
                Console.WriteLine($"DeleteTagListStru 调用成功，返回值: {deleteResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteTagListStru 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号4: EipCloseConnection ===");
            try
            {
                ErrorCode closeResult = UnsafeNativeMethods.EipCloseConnection(testInstanceId);
                Console.WriteLine($"EipCloseConnection 调用成功，返回值: {closeResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipCloseConnection 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号5: EipGetConnectionState ===");
            try
            {
                ConnectionState state = UnsafeNativeMethods.EipGetConnectionState(testInstanceId);
                Console.WriteLine($"EipGetConnectionState 调用成功，返回值: {state}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipGetConnectionState 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号6: EipOpenConnection ===");
            try
            {
                int instanceIdOut = 0;
                ErrorCode openResult = UnsafeNativeMethods.EipOpenConnection(testIp, out instanceIdOut);
                Console.WriteLine($"EipOpenConnection 调用成功，返回值: {openResult}, 实例ID: {instanceIdOut}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipOpenConnection 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号7: EipReadTag ===");
            try
            {
                TagType type = TagType.TAG_TYPE_UNDEFINE;
                byte[] buffer = new byte[100];
                ErrorCode readResult = UnsafeNativeMethods.EipReadTag(
                    testInstanceId, "TestTag", out type, buffer, buffer.Length, 1, -1);
                Console.WriteLine($"EipReadTag 调用成功，返回值: {readResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipReadTag 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号8: EipReadTagExt ===");
            try
            {
                TagType type = TagType.TAG_TYPE_UNDEFINE;
                byte[] buffer = new byte[100];
                ErrorCode readExtResult = UnsafeNativeMethods.EipReadTagExt(
                    testInstanceId, "TestTag", out type, buffer, buffer.Length, 1);
                Console.WriteLine($"EipReadTagExt 调用成功，返回值: {readExtResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipReadTagExt 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号9: EipReadTagExt2 ===");
            try
            {
                var readTag = new ReadDataTag("TestTag", 1);
                ReadDataResult result = default;
                ErrorCode readExt2Result = UnsafeNativeMethods.EipReadTagExt2(testInstanceId, ref readTag, out result);
                Console.WriteLine($"EipReadTagExt2 调用成功，返回值: {readExt2Result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipReadTagExt2 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号10: EipReadTagList ===");
            try
            {
                var tags = new ReadDataTag[] { new ReadDataTag("TestTag1", 1) };
                var results = new ReadDataResult[1];
                ErrorCode readListResult = UnsafeNativeMethods.EipReadTagList(
                    testInstanceId, 1, tags, results);
                Console.WriteLine($"EipReadTagList 调用成功，返回值: {readListResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipReadTagList 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号11: EipReadTagListExt ===");
            try
            {
                var tags = new ReadDataTag[] { new ReadDataTag("TestTag1", 1) };
                var results = new ReadDataResult[1];
                ErrorCode readListExtResult = UnsafeNativeMethods.EipReadTagListExt(
                    testInstanceId, 1, tags, results, false);
                Console.WriteLine($"EipReadTagListExt 调用成功，返回值: {readListExtResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipReadTagListExt 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号12: EipReadTagListRaw ===");
            try
            {
                var tags = new ReadDataTag[] { new ReadDataTag("TestTag1", 1) };
                var results = new ReadDataResult[1];
                ErrorCode readListRawResult = UnsafeNativeMethods.EipReadTagListRaw(
                    testInstanceId, 1, tags, results);
                Console.WriteLine($"EipReadTagListRaw 调用成功，返回值: {readListRawResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipReadTagListRaw 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号13: EipReadTagListWithAlignment ===");
            try
            {
                var tags = new ReadDataTag[] { new ReadDataTag("TestTag1", 1) };
                var results = new ReadDataResult[1];
                ErrorCode readListAlignmentResult = UnsafeNativeMethods.EipReadTagListWithAlignment(
                    testInstanceId, 1, AlignType.AT_DEFAULT, tags, results);
                Console.WriteLine($"EipReadTagListWithAlignment 调用成功，返回值: {readListAlignmentResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipReadTagListWithAlignment 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号14: EipReadTagRaw ===");
            try
            {
                var readTag = new ReadDataTag("TestTag", 1);
                ReadDataResult result = default;
                ErrorCode readRawResult = UnsafeNativeMethods.EipReadTagRaw(testInstanceId, 1, ref readTag, out result);
                Console.WriteLine($"EipReadTagRaw 调用成功，返回值: {readRawResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipReadTagRaw 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号15: EipReadTagWithAlignment ===");
            try
            {
                var readTag = new ReadDataTag("TestTag", 1);
                ReadDataResult result = default;
                ErrorCode readAlignmentResult = UnsafeNativeMethods.EipReadTagWithAlignment(
                    testInstanceId, AlignType.AT_DEFAULT, ref readTag, out result);
                Console.WriteLine($"EipReadTagWithAlignment 调用成功，返回值: {readAlignmentResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipReadTagWithAlignment 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号16: EipStart ===");
            try
            {
                UnsafeNativeMethods.EipStart();
                Console.WriteLine("EipStart 调用成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipStart 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号17: EipStartExt ===");
            try
            {
                bool startExtResult = UnsafeNativeMethods.EipStartExt(testIp, 0);
                Console.WriteLine($"EipStartExt 调用成功，返回值: {startExtResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipStartExt 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号18: EipStartProtocol ===");
            try
            {
                bool startProtocolResult = UnsafeNativeMethods.EipStartProtocol();
                Console.WriteLine($"EipStartProtocol 调用成功，返回值: {startProtocolResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipStartProtocol 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号19: EipStop ===");
            try
            {
                UnsafeNativeMethods.EipStop();
                Console.WriteLine("EipStop 调用成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipStop 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号22: EipWriteTag ===");
            try
            {
                byte[] writeData = new byte[] { 1, 0, 0, 0 }; // 写入整数 1
                ErrorCode writeResult = UnsafeNativeMethods.EipWriteTag(
                    testInstanceId, "TestTag", TagType.TAG_TYPE_DINT, writeData, writeData.Length, 1, -1);
                Console.WriteLine($"EipWriteTag 调用成功，返回值: {writeResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipWriteTag 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号23: EipWriteTagExt ===");
            try
            {
                byte[] writeData = new byte[] { 1, 0, 0, 0 }; // 写入整数 1
                ErrorCode writeExtResult = UnsafeNativeMethods.EipWriteTagExt(
                    testInstanceId, "TestTag", TagType.TAG_TYPE_DINT, writeData, writeData.Length, 1);
                Console.WriteLine($"EipWriteTagExt 调用成功，返回值: {writeExtResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipWriteTagExt 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号24: EipWriteTagExt2 ===");
            try
            {
                byte[] data = new byte[] { 1, 0, 0, 0 };
                var writeTag = new WriteDataTag("TestTag", data, TagType.TAG_TYPE_DINT, 1);
                ErrorCode writeExt2Result = UnsafeNativeMethods.EipWriteTagExt2(testInstanceId, ref writeTag);
                writeTag.Free();
                Console.WriteLine($"EipWriteTagExt2 调用成功，返回值: {writeExt2Result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipWriteTagExt2 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号25: EipWriteTagExt3 ===");
            try
            {
                byte[] data = new byte[] { 1, 0, 0, 0 };
                ErrorCode writeExt3Result = UnsafeNativeMethods.EipWriteTagExt3(
                    testInstanceId, "TestTag", data, data.Length, 1);
                Console.WriteLine($"EipWriteTagExt3 调用成功，返回值: {writeExt3Result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipWriteTagExt3 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号26: EipWriteTagList ===");
            try
            {
                byte[] data = new byte[] { 1, 0, 0, 0 };
                var writeTags = new WriteDataTag[]
                {
                    new WriteDataTag("TestTag1", data, TagType.TAG_TYPE_DINT, 1)
                };
                ErrorCode writeListResult = UnsafeNativeMethods.EipWriteTagList(testInstanceId, 1, writeTags);

                // 清理内存
                foreach (var tag in writeTags)
                {
                    tag.Free();
                }

                Console.WriteLine($"EipWriteTagList 调用成功，返回值: {writeListResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipWriteTagList 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号27: EipWriteTagListExt ===");
            try
            {
                byte[] data = new byte[] { 1, 0, 0, 0 };
                var writeTags = new WriteDataTag[]
                {
                    new WriteDataTag("TestTag1", data, TagType.TAG_TYPE_DINT, 1)
                };
                ErrorCode writeListExtResult = UnsafeNativeMethods.EipWriteTagListExt(
                    testInstanceId, 1, writeTags, false);

                // 清理内存
                foreach (var tag in writeTags)
                {
                    tag.Free();
                }

                Console.WriteLine($"EipWriteTagListExt 调用成功，返回值: {writeListExtResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipWriteTagListExt 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号28: EipWriteTagListRaw ===");
            try
            {
                byte[] data = new byte[] { 1, 0, 0, 0 };
                var writeTags = new WriteDataTag[]
                {
                    new WriteDataTag("TestTag1", data, TagType.TAG_TYPE_DINT, 1)
                };
                ErrorCode writeListRawResult = UnsafeNativeMethods.EipWriteTagListRaw(testInstanceId, 1, writeTags);

                // 清理内存
                foreach (var tag in writeTags)
                {
                    tag.Free();
                }

                Console.WriteLine($"EipWriteTagListRaw 调用成功，返回值: {writeListRawResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipWriteTagListRaw 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号29: EipWriteTagListWithAlignment ===");
            try
            {
                byte[] data = new byte[] { 1, 0, 0, 0 };
                var writeTags = new WriteDataTag[]
                {
                    new WriteDataTag("TestTag1", data, TagType.TAG_TYPE_DINT, 1)
                };
                ErrorCode writeListAlignmentResult = UnsafeNativeMethods.EipWriteTagListWithAlignment(
                    testInstanceId, 1, AlignType.AT_DEFAULT, writeTags);

                // 清理内存
                foreach (var tag in writeTags)
                {
                    tag.Free();
                }

                Console.WriteLine($"EipWriteTagListWithAlignment 调用成功，返回值: {writeListAlignmentResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipWriteTagListWithAlignment 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号30: EipWriteTagRaw ===");
            try
            {
                byte[] data = new byte[] { 1, 0, 0, 0 };
                var writeTag = new WriteDataTag("TestTag", data, TagType.TAG_TYPE_DINT, 1);
                ErrorCode writeRawResult = UnsafeNativeMethods.EipWriteTagRaw(testInstanceId, ref writeTag);
                writeTag.Free();
                Console.WriteLine($"EipWriteTagRaw 调用成功，返回值: {writeRawResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipWriteTagRaw 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号31: EipWriteTagWithAlignment ===");
            try
            {
                byte[] data = new byte[] { 1, 0, 0, 0 };
                var writeTag = new WriteDataTag("TestTag", data, TagType.TAG_TYPE_DINT, 1);
                ErrorCode writeAlignmentResult = UnsafeNativeMethods.EipWriteTagWithAlignment(
                    testInstanceId, AlignType.AT_DEFAULT, ref writeTag);
                writeTag.Free();
                Console.WriteLine($"EipWriteTagWithAlignment 调用成功，返回值: {writeAlignmentResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EipWriteTagWithAlignment 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 测试序号32: ResetTagInfo ===");
            try
            {
                UnsafeNativeMethods.ResetTagInfo();
                Console.WriteLine("ResetTagInfo 调用成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ResetTagInfo 调用失败: {ex.Message}");
            }

            Console.WriteLine("\n=== 所有方法测试完成 ===");
        }
    }
}