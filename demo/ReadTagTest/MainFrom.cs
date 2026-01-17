using System.Net;
using System.Text;
using Inovance.EtherNetIP.Native;
using PlcGateway.Abstractions;
using PlcGateway.Drivers.Inovance;
using PlcGateway.Drivers.Inovance.Data;

namespace ReadTagTest
{
    public partial class MainFrom : Form
    {
        private IPLCDriver? _driver;

        public MainFrom()
        {
            InitializeComponent();
            this.Load += MainFrom_Load;
        }

        private void MainFrom_Load(object sender, EventArgs e)
        {
            this.ReadTagDataTypeComboBox.DataSource = Enum.GetValues(typeof(DataType));
            this.WriteTagDataTypeComboBox.DataSource = Enum.GetValues(typeof(DataType));
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
        }

        private void DisconnectPLCButton_Click(object sender, EventArgs e)
        {
            if (_driver != null)
            {
                try
                {
                    _driver.Disconnect();
                    _driver = null;
                    this.Log("断开成功！");
                }
                catch (Exception ex)
                {
                    this.Log("断开失败");
                    this.Log(ex.ToString());
                }
            }
        }

        private void ReadTagButton_Click(object sender, EventArgs e)
        {
            if (_driver == null)
            {
                return;
            }

            try
            {
                var value = this.Read(this.ReadTagAddressTextBox.Text,
                    (DataType)Enum.Parse(typeof(DataType), ReadTagDataTypeComboBox.Text));
                this.Log($"读取标签：{this.ReadTagAddressTextBox.Text}");
                this.Log($"数据类型：{ReadTagDataTypeComboBox.Text}");
                this.Log($"标签数据：{value}");
            }
            catch (Exception ex)
            {
                this.Log("读取失败");
                this.Log(ex.ToString());
            }
        }

        private string Read(string address, DataType dataType)
        {
            if (_driver is null)
            {
                throw new Exception("未连接");
            }

            return dataType switch
            {
                DataType.Boolean => _driver.Read<bool>(address).ToString(),
                DataType.SByte => _driver.Read<sbyte>(address).ToString(),
                DataType.Byte => _driver.Read<byte>(address).ToString(),
                DataType.Int16 => _driver.Read<short>(address).ToString(),
                DataType.UInt16 => _driver.Read<ushort>(address).ToString(),
                DataType.Int32 => _driver.Read<int>(address).ToString(),
                DataType.UInt32 => _driver.Read<uint>(address).ToString(),
                DataType.Int64 => _driver.Read<long>(address).ToString(),
                DataType.UInt64 => _driver.Read<ulong>(address).ToString(),
                DataType.Single => _driver.Read<float>(address).ToString(),
                DataType.Double => _driver.Read<double>(address).ToString(),
                DataType.String => _driver.Read<string>(address).ToString(),
                DataType.Bits8Bit => BytesToHexString(_driver.Read<Bits8Bit>(address).GetBytes()),
                DataType.Bits16Bit => BytesToHexString(_driver.Read<Bits16Bit>(address).GetBytes()),
                DataType.Bits32Bit => BytesToHexString(_driver.Read<Bits32Bit>(address).GetBytes()),
                DataType.Bits64Bit => BytesToHexString(_driver.Read<Bits64Bit>(address).GetBytes()),
                DataType.Structure => BytesToHexString(_driver.Read<Structure>(address).GetBytes()),
                _ => throw new Exception("不支持类型")
            };
        }

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
            if (_driver == null)
            {
                return;
            }

            try
            {
                Write(
                    this.WriteTagAddressTextBox.Text,
                    (DataType)Enum.Parse(typeof(DataType), WriteTagDataTypeComboBox.Text),
                    WriteDataTextBox.Text);
            }
            catch (Exception ex)
            {
                this.Log("写入失败");
                this.Log(ex.ToString());
            }
        }

        private void Write(string address, DataType dataType, string value)
        {
            if (_driver is null)
            {
                throw new Exception("未连接");
            }

            switch (dataType)
            {
                case DataType.Boolean: _driver.Write<bool>(address, bool.Parse(value)); break;
                case DataType.SByte: _driver.Write<sbyte>(address, sbyte.Parse(value)); break;
                case DataType.Byte: _driver.Write<byte>(address, byte.Parse(value)); break;
                case DataType.Int16: _driver.Write<short>(address, short.Parse(value)); break;
                case DataType.UInt16: _driver.Write<ushort>(address, ushort.Parse(value)); break;
                case DataType.Int32: _driver.Write<int>(address, int.Parse(value)); break;
                case DataType.UInt32: _driver.Write<uint>(address, uint.Parse(value)); break;
                case DataType.Int64: _driver.Write<long>(address, long.Parse(value)); break;
                case DataType.UInt64: _driver.Write<ulong>(address, ulong.Parse(value)); break;
                case DataType.Single: _driver.Write<float>(address, float.Parse(value)); break;
                case DataType.Double: _driver.Write<double>(address, double.Parse(value)); break;
                case DataType.String: _driver.Write<string>(address, value); break;
                default: throw new Exception("不支持类型");
            }
            ;
        }

        private void CleanTagButton_Click(object sender, EventArgs e)
        {
            UnsafeNativeMethods.ResetTagInfo();
        }

        private void CleanLogsButton_Click(object sender, EventArgs e)
        {
            this.LogRichTextBox.Text = "";
        }
    }
}
