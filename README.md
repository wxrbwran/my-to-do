# MyToDo - 跨平台任务与备忘录管理系统

基于 **.NET 10**、**Avalonia UI 11** 和 **Material Design** 构建的现代化跨平台桌面客户端，后端基于 **ASP.NET Core WebAPI** 与 **SQLite**。

可在 **macOS**、**Windows** 和 **Linux** 跨平台无缝运行。

---

## 一、 项目模块结构

| 模块工程 | 目标框架 | 架构定位与核心技术栈 |
| :--- | :--- | :--- |
| **`MyToDo.Desktop`** | `net10.0` | **【全新】跨平台现代化桌面客户端**<br>• UI 引擎：**Avalonia UI 11**（原生 Metal/DirectX/Vulkan 硬件加速）<br>• UI 风格：**Material.Avalonia 3.9** + **Material.Icons.Avalonia**<br>• MVVM 框架：**CommunityToolkit.Mvvm**（源生成器驱动）<br>• 依赖注入：`Microsoft.Extensions.DependencyInjection`<br>• 网络与序列化：原生 `HttpClient` + `System.Text.Json` |
| **`MyToDo.Api`** | `net10.0` | **后端 RESTful API 服务**<br>• 框架：ASP.NET Core 10 WebAPI<br>• ORM / 数据库：Entity Framework Core + SQLite (`to.db`)<br>• 模块：用户鉴权、待办事项 CRUD、备忘录 CRUD、首页看板统计 |
| **`MyToDo.Shared`** | `net10.0` | **前后端共用数据契约与模型**<br>• 包含各类 DTO（`ToDoDto`, `MemoDto`, `UserDto`, `SummaryDto`）、分页模型与参数 |
| **`MyToDO`** | `net8.0-windows` | 【归档保留】原有 Windows 专有 WPF + Prism 8 客户端（供对照参考） |

---

## 二、 核心功能特性

1. **首页统计看板**：待办总数、已完成数、完成比例、便签数统计，支持待办一键勾选完成与快捷编辑。
2. **待办事项管理**：支持按状态（全部 / 待办中 / 已完成）与关键字筛选搜索，支持新建、编辑、删除。
3. **备忘便签管理**：瀑布流便签卡片展示，支持增删改查。
4. **个性化外观换肤**：
   - 浅色（Light）与深色（Dark）主题无缝切换；
   - 包含深海蓝、宝石青、罗兰紫、翡翠绿、琥珀橙等 Material 主调色板动态切换。
5. **用户账号系统**：登录、注册、注销，支持离线/演示兜底。

---

## 三、 本地快速启动运行指南

### 1. 配置终端环境变量（macOS / Linux）
```bash
echo 'export PATH="/usr/local/share/dotnet:$PATH"' >> ~/.zshrc
echo 'export PATH="$HOME/.dotnet/tools:$PATH"' >> ~/.zshrc
source ~/.zshrc
```

### 2. 启动后端 API 服务
```bash
dotnet run --project MyToDo.Api/MyToDo.Api.csproj
```
默认监听：`http://localhost:5000`

### 3. 启动跨平台桌面客户端
在另一个终端标签中运行：
```bash
dotnet run --project MyToDo.Desktop/MyToDo.Desktop.csproj
```
即刻在当前操作系统（macOS / Windows / Linux）弹出原生桌面窗口！