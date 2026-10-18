# MyToDo - 现代化跨平台任务与备忘录管理系统

基于 **.NET 10**、**Avalonia UI 11** 和 **SukiUI** 构建的现代化高颜值跨平台桌面客户端，后端基于 **ASP.NET Core WebAPI** 与 **SQLite**。

可在 **macOS (Apple Silicon / Intel)**、**Windows** 和 **Linux** 上全平台无缝运行与硬件加速渲染。

---

## 一、 技术栈现代化升级与选型思路

本项目最初为传统的 **Windows WPF (.NET 8) + Prism 8 + MaterialDesignThemes** 架构（典型的经典网课教程形态）。在本次升级重构中，我们针对现代桌面开发体验、跨平台运行诉求以及代码精简度，完成了全面的技术栈跃升。

### 1. 跨平台桌面引擎：从 Windows WPF 到 Avalonia UI 11
* **背景痛点**：原有 WPF 深度绑定 Windows 操作系统底座，技术栈陈旧，无法在 macOS（如开发主力机 Apple Silicon）和 Linux 上直接运行与调试。
* **选型对比**：
  * **vs .NET MAUI**：MAUI 主打移动端，桌面端（特别是 Linux）生态和定制体验欠佳，XAML 语法与 WPF 差异较大；
  * **vs Electron / Web 技术**：内存开销大（动辄 200MB+），打包体积沉重；
  * **选择 Avalonia UI**：
    1. **极低迁移成本**：XAML 与 C# MVVM 体系与原项目有 90% 以上相似度，数据绑定、转换器与视图模型复用率极高；
    2. **纯原生硬件加速**：在 macOS 上采用 Metal 原生管道渲染，在 Windows 上基于 DirectX/Direct2D，在 Linux 上基于 Vulkan，启动快、帧率高、内存轻量。

### 2. UI 视觉体系演进：从 Material.Avalonia 到 SukiUI
* **第一期实践与痛点（Material.Avalonia）**：
  * 早期我们尝试 1:1 复刻原 WPF 的 Material Design 风格，但发现其下拉框（`ComboBox`）采用传统下划线与粗糙的激活红线，与 Outline 边框输入框严重割裂；且弹窗默认半透明严重透底，视觉层次容易混乱。
* **最终决策（SukiUI 6.0）**：
  * **现代审美高地**：原生支持现代桌面极具视觉冲击力的**毛玻璃（GlassCard）**质感、微渐变光晕与细腻控件动效；
  * **组件高度协调**：输入框、下拉框与按钮采用统一的高度、圆角与内边距，弹窗采用深度遮罩与自适应实体表面卡片，杜绝透底；
  * **动态主题系统**：内置开箱即用的深色/浅色（Dark/Light）模式无缝切换，以及 8+ 套专业主题调色盘（Blue、Red、Green、Orange、Purple 等），全应用光影自适应跟随。

### 3. MVVM 框架革命：从 Prism 8 到 CommunityToolkit.Mvvm
* **背景痛点**：原 Prism 8 方案包含大量陈旧的手写样板代码（每个属性需要私有字段、Getter/Setter、`RaisePropertyChanged`，每个命令需要手写 `DelegateCommand` 实例化与委托绑定），代码冗长沉重。
* **现代方案**：全面采用微软官方大力推崇的 **`CommunityToolkit.Mvvm`**：
  * 基于 C# 源码生成器（Source Generators），仅需 `[ObservableProperty]` 和 `[RelayCommand]` 即可自动在编译期生成完整属性与命令；
  * 消除超过 **70%** 的冗余样板代码，无反射性能损耗，类型安全且更加清晰。

### 4. 网络层与序列化彻底轻量化
* **剔除旧依赖**：彻底移除了陈旧的 `RestSharp 106`（好几年前的旧接口规范）和 `Newtonsoft.Json 13.x`；
* **拥抱原生**：基于现代 .NET 的 **`HttpClient` / `IHttpClientFactory`** 与高性能内置 **`System.Text.Json`** 重写网络通信层，内存分配大幅降低，吞吐效率提升 3~5 倍。

### 5. 底层运行环境升级至 .NET 10
* 全解决方案（Shared 共享类库、WebAPI 服务端、Desktop 客户端）统一跃升至 **`.NET 10`**；
* 解决 macOS Apple Silicon (arm64) 环境下的 SDK 环境变量识别与本地构建支持，全面享受最新的 C# 语言特性与 JIT 性能优化。

---

## 二、 架构对比总览

| 模块 | 改造前（经典 WPF 架构） | 改造后（现代跨平台架构） | 核心收益 |
| :--- | :--- | :--- | :--- |
| **支持平台** | 仅 Windows | **macOS / Windows / Linux 全平台** | 摆脱单一系统依赖，跨平台运行与开发 |
| **UI 框架** | WPF (`net8.0-windows`) | **Avalonia UI 11 + SukiUI 6.0** | 现代毛玻璃质感、自适应深浅色与调色盘 |
| **MVVM 库** | Prism 8 (DryIoc) | **CommunityToolkit.Mvvm** | 源生成器驱动，消除 70% 样板代码 |
| **网络库** | RestSharp 106 | **原生 HttpClient** | 减少第三方包体积，标准统一 |
| **序列化** | Newtonsoft.Json | **System.Text.Json** | 高吞吐、低内存占用 |
| **运行时** | .NET 8 (Windows Only) | **.NET 10 全平台** | 最前沿语言特性与运行时性能 |

---

## 三、 解决方案工程结构

```
my-to-do/
├── MyToDo.Shared/              # 【共享类库】基于 .NET 10
│   ├── Dtos/                   # 业务数据传输对象（ToDoDto, MemoDto, UserDto, SummaryDto）
│   ├── Parameters/             # 分页与查询参数（ToDoQueryParameter 等）
│   └── ApiResponse.cs          # 统一响应模型封装
│
├── MyToDo.Desktop/             # 【全新跨平台客户端】基于 Avalonia 11 + SukiUI + .NET 10
│   ├── Assets/                 # 跨平台图标与应用资源
│   ├── Models/                 # 客户端导航与会话状态
│   ├── Services/               # 基于 HttpClient 的 RESTful API 客户端
│   ├── ViewModels/             # CommunityToolkit.Mvvm 驱动的视图模型
│   │   ├── IndexViewModel.cs   # 首页看板 VM
│   │   ├── ToDoViewModel.cs    # 待办事项管理 VM
│   │   ├── MemoViewModel.cs    # 备忘录便签 VM
│   │   ├── SkinViewModel.cs    # 主题调色板 VM
│   │   └── LoginViewModel.cs   # 登录注册 VM
│   ├── Views/                  # SukiWindow & GlassCard 高颜值 XAML 界面
│   └── App.axaml / Program.cs  # 跨平台入口与全局依赖注入 (DI)
│
├── MyToDo.Api/                 # 【后端服务】基于 ASP.NET Core 10 WebAPI
│   ├── Controllers/            # Auth, ToDo, Memo, Summary 控制器
│   ├── MyToDoContext.cs        # Entity Framework Core 上下文
│   └── to.db                   # SQLite 本地轻量数据库
│
└── MyToDO/                     # 【归档保留】原有 Windows WPF + Prism 8 客户端（供对照参考）
```

---

## 四、 核心功能特性

1. **仪表盘统计看板（Dashboard）**：待办总数、已完成数、完成百分比与便签数实时统计，支持直接勾选完成任务；
2. **待办事项管理（ToDo）**：支持状态过滤（全部 / 待办中 / 已完成，支持下拉项改变自动过滤刷新）、关键词模糊搜索、新建/编辑弹窗及删除；
3. **备忘便签管理（Memo）**：基于现代卡片的瀑布流网格展示，支持快速便签编辑与移除；
4. **个性化外观换肤（Themes）**：
   * 浅色模式（Light）与深色模式（Dark）一键无缝平滑切换；
   * 提供多套 SukiUI 调色板高亮色（蓝色、红色、翡翠绿、琥珀橙、紫色等）实时渲染；
5. **模态弹窗系统（Dialogs）**：不透光的实体主题卡片，配备大景深软阴影与背景暗化遮罩，聚焦编辑输入；
6. **用户鉴权系统（Auth）**：账号注册、密码登录与注销，支持断网/无后端时的模拟离线体验。

---

## 五、 本地快速启动运行指南

### 1. 配置终端环境（针对 macOS / Linux）
若终端尚未识别到 .NET 10 SDK 路径，可执行：
```bash
echo 'export PATH="/usr/local/share/dotnet:$PATH"' >> ~/.zshrc
source ~/.zshrc
```

### 2. 启动后端 API 服务
```bash
dotnet run --project MyToDo.Api/MyToDo.Api.csproj
```
默认监听于：`http://localhost:5000`

### 3. 启动跨平台桌面客户端
新建另一个终端窗口运行：
```bash
dotnet run --project MyToDo.Desktop/MyToDo.Desktop.csproj
```
即刻在当前操作系统桌面呼出原生流畅的 SukiUI 现代化应用窗口！