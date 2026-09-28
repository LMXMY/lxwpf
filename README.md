# WPF Modbus 数据采集系统

基于 **Prism + MVVM + EF Core + Modbus RTU读取** 的 WPF 上位机项目，模拟 MES 设备数据采集场景。

## 技术栈

| 技术 | 用途 |
|---|---|
| C# / WPF | 界面 |
| Prism | DI、导航、MVVM |
| EF Core + SQLite | 数据持久化 |
| NModbus4 | Modbus RTU 读取 |

## 功能

- **登录 / 注册**：用户账号添加
- **参数配置**：配置多设备（串口、从站、地址、数量、周期）
- **单设备采集**：轮询（多设备 并发采集，多串口，独立轮询，有代码未测试）
- **历史记录**：值变化才存，支持查询、导出
- **报警**：阈值判断，记录
- **断线重连**：状态判断，自动重连

## 架构
- Views/ 界面
- ViewModels/ ViewModel
- Services/ 业务接口
- ServicesImpl/ 业务实现
- Entities/ 实体
- Repository/ DbContext


## 项目说明

本项目为个人学习项目 （参考Susalem EasyDemo）：
- Prism + MVVM 架构
- EF Core + SQLite 数据持久化
- Modbus RTU 读取、主单设备采集（多设备采集有代码）
- 采集服务与界面解耦（有代码）
- 历史记录、报警、断线重连
- 模拟从站可用 `Modbus Slave`

非生产项目，功能以学习为目的。

## 截图
<img width="1929" height="1116" alt="注册" src="https://github.com/user-attachments/assets/0a56c9dc-1fc4-4e81-9b91-f0445e919ad8" />
<img width="1712" height="1190" alt="登录" src="https://github.com/user-attachments/assets/cee214ce-04db-48e2-b6bb-47280483c799" />
<img width="1978" height="1173" alt="Modbus参数" src="https://github.com/user-attachments/assets/6285016b-a906-4fc1-aaf3-e03a8f78bd4b" />
<img width="1695" height="1177" alt="参数配置(用于多设备)" src="https://github.com/user-attachments/assets/be54eb8f-4f7e-4657-ba84-4eb8c1237dd9" />
<img width="1708" height="1162" alt="历史记录" src="https://github.com/user-attachments/assets/40fa4ac8-c2ca-45fb-bac0-33dca981d063" />
<img width="1927" height="1235" alt="报警列表" src="https://github.com/user-attachments/assets/420f728c-20a8-4993-9742-710087ea192a" />






