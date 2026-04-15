# PlcGateway 使用说明

`PlcGateway` 当前主要提供两个驱动：

- `PlcGateway.Drivers.Inovance`
- `PlcGateway.Drivers.Beckhoff`

本目录下的说明更偏向包使用与发布，适合在 NuGet 包页和仓库中快速了解当前能力。

## 支持的驱动

### 汇川驱动

命名空间：`PlcGateway.Drivers.Inovance`

特点：

- 支持索引地址与字符串地址
- 支持基础类型读写
- 使用原生库封装 EIP 通信

### 倍福驱动

命名空间：`PlcGateway.Drivers.Beckhoff`

特点：

- 支持 ADS 索引地址与符号地址
- 支持基础类型和倍福专用日期时间类型
- 适合 Win32 / Win64 环境下的 ADS 通信

## 安装

```bash
dotnet add package PlcGateway
```

## 示例

### 倍福索引驱动

```csharp
using PlcGateway.Drivers.Beckhoff;
using TwinCAT.Ads;

var driver = new BeckhoffAdsIndexDriver(new AmsNetId("1.2.3.4.5.6"), new AmsPort(851));
driver.Connect();

var value = driver.Read<int>("[0x4020,0]");
driver.Write("[0x4020,0]", 123);

driver.Disconnect();
```

### 倍福符号驱动

```csharp
using PlcGateway.Drivers.Beckhoff;
using TwinCAT.Ads;

var driver = new BeckhoffAdsSymbolDriver(new AmsNetId("1.2.3.4.5.6"), new AmsPort(851));
driver.Connect();

var value = driver.Read<string>("MAIN.MyString");
driver.Write("MAIN.MyString", "hello");

driver.Disconnect();
```

### 汇川驱动

```csharp
using PlcGateway.Drivers.Inovance;

var driver = new ExInovanceEIPDriver("192.168.0.10");
driver.Connect();

var value = driver.Read<int>("D100");
driver.Write("D100", 123);

driver.Disconnect();
```

## 发布说明

- 包版本来自 `src/PlcGateway/Directory.Build.props`
- 发布到 NuGet 时，使用仓库中的 GitHub Actions workflow
- 如果是 Windows x86 / x64 运行环境，当前包已按平台准备好输出与 native 资源