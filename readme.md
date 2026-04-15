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

var driver = new BeckhoffAdsIndexDriver(
	new TwinCAT.AmsNetId("1.2.3.4.5.6"),
	new TwinCAT.AmsPort(851));

driver.Connect();
var value = driver.Read<int>("[0x4020,0]");
driver.Write("[0x4020,0]", 123);
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

## 说明

- 如果你发布的是 Windows x86 / x64 程序，当前包可直接引用。
- 汇川驱动依赖原生库，NuGet 包中已按运行时打包对应 `native` 资产。
- 需要发布时，请使用仓库的 GitHub Actions workflow。

## 主页

<https://github.com/FlyingEyeOrg/PlcGateway/>