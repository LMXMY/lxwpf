# WPF Modbus 数据采集系统

基于 **Prism + MVVM + EF Core + Modbus RTU读取** 的 WPF 上位机项目，模拟 MES 设备数据采集场景。

## 技术栈

| 技术 | 用途 |
|---|---|
| C# / WPF | 界面 |
| Prism | DI、导航、MVVM |
| EF Core + SQLite | 数据持久化 |
| NModbus4 | Modbus RTU 通信 |
| HandyControl | UI 控件库 |

## 功能

- **登录 / 注册**：用户管理，权限控制
- **参数配置**：配置多设备（串口、从站、地址、数量、周期）
- **单设备采集**：轮询（多设备 并发采集，多串口，独立轮询，有代码未测试）
- **历史记录**：值变化才存，支持查询、导出
- **报警**：阈值判断，记录，状态缓存去重
- **断线重连**：超时、断开自动重连

## 架构
Views/ 界面
ViewModels/ ViewModel
Services/ 业务接口
ServicesImpl/ 业务实现
Entities/ 实体
Repository/ DbContext

- 模拟从站可用 `Modbus Slave`
## 项目说明

本项目为个人学习练手项目，用于练习：

- Prism + MVVM 架构
- EF Core + SQLite 数据持久化
- Modbus RTU 通信、多设备采集
- 采集服务与界面解耦
- 历史记录、报警、断线重连

非生产项目，功能以学习为目的。
