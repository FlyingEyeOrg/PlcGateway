# PlcGateway

`PlcGateway` 是一个面向工业自动化场景的 PLC 通信网关库，当前提供以下驱动：

- `PlcGateway.Drivers.Inovance`：汇川 PLC 驱动
- `PlcGateway.Drivers.Beckhoff`：倍福 PLC 驱动

项目面向 Windows 环境使用，支持 x86 / x64 发布与运行。

## 特性

- 统一的读写接口
- 支持基础类型与驱动专用数据类型
- 汇川驱动打包了对应运行时的 native DLL
- 支持 GitHub Actions 自动打包与发布到 NuGet
- 适合上位机、工控服务和数据采集程序

## 安装

```bash
dotnet add package PlcGateway
```

## 快速开始

### 倍福驱动

```csharp
using PlcGateway.Drivers.Beckhoff;

using var driver = new BeckhoffAdsIndexDriver(
	"192.168.250.111.1.1",
	851);

driver.Connect();
var value = driver.Read<int>("[0x4020,0]");
driver.Write("[0x4020,0]", 123);
driver.Disconnect();
```

倍福驱动固定使用 `Beckhoff.TwinCAT.Ads 4.4.71`，同一套 API 可连接 TwinCAT 2 和 TwinCAT 3：

- TwinCAT 2 PLC Runtime 常用端口为 `801`、`811`、`821`、`831`。
- TwinCAT 3 PLC Runtime 常用端口为 `851`、`852`、`853`、`854`。
- `192.168.250.111` 是设备 IP；`192.168.250.111.1.1` 形式的是 AMS Net ID，两者不能混用。
- 运行程序的 Windows 设备需要安装并启动 TwinCAT ADS Router，并正确配置 AMS Route。

### 汇川驱动

```csharp
using PlcGateway.Drivers.Inovance;

var driver = new ExInovanceEIPDriver("192.168.0.10");

driver.Connect();
var value = driver.Read<int>("D100");
driver.Write("D100", 123);
driver.Disconnect();
```

## 说明

- 如果你发布的是 Windows x86 / x64 程序，当前包可直接引用。
- ADS 4.4.71 只提供 .NET Framework 资产；现代 .NET Windows 项目还原时可能出现 `NU1701` 兼容性提示。
- 汇川驱动依赖原生库，NuGet 包中已按运行时打包对应 `native` 资产。
- 需要发布时，请使用仓库的 GitHub Actions workflow。

## 主页

<https://github.com/FlyingEyeOrg/PlcGateway/>
