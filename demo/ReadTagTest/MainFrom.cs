using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inovance.EtherNetIP.Native;
using PlcGateway.Abstractions;
using PlcGateway.Drivers.Inovance;
using PlcGateway.Drivers.Inovance.Native;

namespace ReadTagTest
{
    public partial class MainFrom : Form
    {
        private IPLCDriver? _driver;

        public MainFrom()
        {
            InitializeComponent();
        }

        private void EipStartButton_Click(object sender, EventArgs e)
        {
            //var isOK = UnsafeNativeMethods.EipStartExt(this.HostIPTextBox.Text);

            //if (isOK)
            //{
            //    Log("打开协议栈成功");
            //}
            //else
            //{
            //    Log("打开协议栈失败");
            //}
        }

        private void ConnectPLCButton_Click(object sender, EventArgs e)
        {
            _driver = new InovanceEIPDriver(IPAddress.Parse(this.HostIPTextBox.Text), IPAddress.Parse(this.PLCIPTextBox.Text));
            try
            {
                _driver.Connect();
                this.Log("连接成功");
            }
            catch (Exception ex)
            {
                this.Log("连接失败");
                this.Log(ex.ToString());
            }

            //var result = UnsafeNativeMethods.EipOpenConnection(this.PLCIPTextBox.Text, out var id);

            //if (result != PlcGateway.Drivers.Inovance.Native.ErrorCode.SUCCESS)
            //{
            //    Log($"连接 PLC 失败：{result.ToString()}");
            //    this.ConnectIdTextBox.Text = string.Empty;
            //}
            //else
            //{
            //    Log("连接 PLC 成功");
            //    this.ConnectIdTextBox.Text = id.ToString();
            //}
        }

        private void DisconnectPLCButton_Click(object sender, EventArgs e)
        {
            if (_driver != null)
            {
                try
                {
                    _driver.Disconnect();
                    this.Log("断开成功！");
                }
                catch (Exception ex)
                {
                    this.Log("断开失败");
                    this.Log(ex.ToString());
                }
            }

            //var result = UnsafeNativeMethods.EipCloseConnection(int.Parse(this.ConnectIdTextBox.Text));

            //if (result != PlcGateway.Drivers.Inovance.Native.ErrorCode.SUCCESS)
            //{
            //    this.LogRichTextBox.Text += $"断开连接 PLC 失败：{result.ToString()}\n";
            //}
            //else
            //{
            //    this.LogRichTextBox.Text += "断开连接 PLC 成功\n";
            //    this.ConnectIdTextBox.Text = string.Empty;
            //}
        }

        private void EipStopButton_Click(object sender, EventArgs e)
        {
            //UnsafeNativeMethods.EipStop();
            //this.LogRichTextBox.Text += "关闭协议栈成功\n";
        }

        private void ReadTagButton_Click(object sender, EventArgs e)
        {
            if (_driver == null)
            {
                return;
            }

            try
            {
                var value = _driver.Read<float>(this.ReadTagAddressTextBox.Text);
            }
            catch (Exception ex)
            {
                this.Log("读取失败");
                this.Log(ex.ToString());
            }
        }

        //private void ReadTagButton_Click(object sender, EventArgs e)
        //{
        //    if (!this.UseReadTagExtCheckBox.Checked)
        //    {
        //        var tag = new ReadDataTag()
        //        {
        //            pName = this.ReadTagAddressTextBox.Text,
        //            iElementCount = 1,
        //        };
        //        PlcGateway.Drivers.Inovance.Native.ErrorCode code
        //            = UnsafeNativeMethods.EipReadTagExt2(int.Parse(this.ConnectIdTextBox.Text), ref tag, out var result);

        //        if (code == PlcGateway.Drivers.Inovance.Native.ErrorCode.SUCCESS)
        //        {
        //            this.Log("读取数据成功！");
        //            this.Log($"数据：{BytesToHexString(result.GetData())}");
        //            this.Log($"数据类型：{result.eType.ToString()}");
        //            this.Log($"数据长度：{result.iDataLength.ToString()}");

        //            UnsafeNativeMethods.DeleteTagListStru([result], 1);
        //        }
        //        else
        //        {
        //            this.Log($"读取数据失败：{code.ToString()}");
        //        }
        //    }
        //    else
        //    {
        //        byte[] buffer = new byte[1400];
        //        var code = UnsafeNativeMethods.EipReadTagExt(
        //            int.Parse(this.ConnectIdTextBox.Text),
        //            this.ReadTagAddressTextBox.Text,
        //            out var tagType,
        //            buffer,
        //            1400, 1);

        //        if (code > 0)
        //        {
        //            this.Log("读取数据成功！");
        //            this.Log($"数据：{BytesToHexString(buffer.Take(code).ToArray())}");
        //            this.Log($"数据类型：{tagType.ToString()}");
        //            this.Log($"数据长度：{code.ToString()}");
        //        }
        //        else
        //        {
        //            this.Log($"读取数据失败：{(((ReadTagErrorCode)code)).ToString()}");
        //        }
        //    }
        //}

        private void Log(string line)
        {
            this.LogRichTextBox.Text += line + "\n";
        }

        public static string BytesToHexString(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return string.Empty;

            var hexBuilder = new StringBuilder(bytes.Length * 3); // 每个字节2字符+1空格

            for (int i = 0; i < bytes.Length; i++)
            {
                hexBuilder.Append(bytes[i].ToString("X2"));  // X2 表示大写十六进制，2位

                if (i < bytes.Length - 1)
                {
                    hexBuilder.Append(' ');  // 添加空格分隔
                }
            }

            return hexBuilder.ToString();
        }

        private void WriteTagButton_Click(object sender, EventArgs e)
        {
            this.Log("该功能暂时不能使用");
            return;

            WriteDataTag tag = new WriteDataTag()
            {
                pName = this.WriteTagAddressTextBox.Text,
                eType = PlcGateway.Drivers.Inovance.Native.TagType.TAG_TYPE_REAL,

            };

            var code = UnsafeNativeMethods.EipWriteTagExt2(int.Parse(this.ConnectIdTextBox.Text), ref tag);

            if (code != PlcGateway.Drivers.Inovance.Native.ErrorCode.SUCCESS)
            {
                this.Log($"数据写入失败：{code.ToString()}");
            }
            else
            {
                this.Log("数据写入成功");
            }
        }

        private void CleanTagButton_Click(object sender, EventArgs e)
        {
            UnsafeNativeMethods.ResetTagInfo();
        }
    }
}
