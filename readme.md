# PlcGateway - PLC 工业网关库

**PlcGateway** 是一个统一的多品牌 PLC 通信网关库，为 .NET 应用提供标准化的 PLC 设备通信接口。
支持汇川(Inovance)、西门子(Siemens)、罗克韦尔(Allen-Bradley)、三菱(Mitsubishi)等主流 PLC 品牌，以及 Modbus 协议。

## ✨ 特性

- **🚀 统一接口** - 单一 API 访问不同品牌 PLC
- **🔌 多协议支持** - 支持主流 PLC 通信协议
- **🎯 高性能** - 异步优先，内存高效
- **🧪 易于测试** - 接口抽象，便于模拟测试
- **🔧 可扩展** - 易于添加新品牌驱动
- **📦 开箱即用** - 零配置快速开始
- **📄 强类型** - 完整 XML 文档注释
- **🧩 模块化** - 按需引用驱动包

## 📦 安装

### 完整安装
```bash
dotnet add package PlcGateway
```

## 🏗️ 架构概览

```
PlcGateway.                          ← 根命名空间
├── PlcGateway.Core                  ← 核心抽象层
├── PlcGateway.Abstractions          ← 接口定义
├── PlcGateway.Drivers.Inovance      ← 汇川驱动
├── PlcGateway.Drivers.AllenBradley  ← 罗克韦尔驱动  
├── PlcGateway.Drivers.Siemens       ← 西门子驱动
├── PlcGateway.Drivers.Mitsubishi    ← 三菱驱动
├── PlcGateway.Drivers.Modbus        ← Modbus通用驱动
├── PlcGateway.Extensions            ← 扩展方法
└── PlcGateway.Samples               ← 示例代码
```


## 🔧 支持的 PLC 品牌

| 品牌 | 驱动包 | 支持的协议 | 特性 |
|------|--------|------------|------|
| **汇川** | `PlcGateway.Drivers.Inovance` | AMCP, Modbus TCP | 国产PLC支持，高性能 |
| **西门子** | `PlcGateway.Drivers.Siemens` | S7, Fetch/Write | 工业4.0，S7-1200/1500 |
| **罗克韦尔** | `PlcGateway.Drivers.AllenBradley` | CIP, Ethernet/IP | Compact/ControlLogix |
| **三菱** | `PlcGateway.Drivers.Mitsubishi` | MC, SLMP | FX/Q系列支持 |
| **通用** | `PlcGateway.Drivers.Modbus` | Modbus TCP/RTU | 跨品牌通用协议 |

## 🙏 致谢

感谢所有贡献者和用户的支持！

## ⭐ 如果这个项目对你有帮助，请给我们一个 Star！

<https://github.com/FlyingEyeOrg/PlcGateway/>

---

**Made with ❤️ for the industrial automation community**