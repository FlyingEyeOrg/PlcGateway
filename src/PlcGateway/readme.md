# 🏗️ 架构概览

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