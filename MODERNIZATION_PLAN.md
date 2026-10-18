# MyToDo 项目现代化与跨平台改造方案

> **目标**：将原基于 **Windows 专有 WPF + Prism 8** 的个人任务与备忘录系统，重构为**现代跨平台架构（macOS / Windows / Linux）**，同时精简技术栈、消除冗余设计，提升开发体验与运行性能。

---

## 一、 现状分析与改造对比

| 模块 | 现有技术方案（传统 WPF 时代） | 改造后现代方案（跨平台 & 现代 .NET） | 改造收益 |
| :--- | :--- | :--- | :--- |
| **桌面 UI 引擎** | **WPF** (`net8.0-windows`)<br>*(仅限 Windows)* | **Avalonia UI 11+**<br>*(原生支持 macOS, Windows, Linux)* | **实现全平台跨平台运行**（尤其满足当前在 macOS 上的开发与运行需求），XAML 与 WPF 相似度达 90%，学习和迁移成本最低。 |
| **UI 视觉与主题** | `MaterialDesignThemes` (WPF) | `Material.Avalonia` (或 Fluent 现代风格) | 延续现有的 Material Design 扁平风格、卡片设计与深/浅色主题调色板，UI 体验无缝过渡。 |
| **MVVM 框架** | **Prism 8.x (DryIoc)**<br>*(手写冗长的属性通知与命令)* | **CommunityToolkit.Mvvm**<br>*(基于 C# 源码生成器 Source Generator)* | 消除 70% 样板代码，使用 `[ObservableProperty]` 和 `[RelayCommand]`，编译期强类型检查，性能与可读性大幅提升。 |
| **依赖注入 (IoC)** | `Prism.DryIoc` | `Microsoft.Extensions.DependencyInjection` | 统一采用微软官方标准的 DI 容器，与 ASP.NET Core 服务端保持一致，生态更通用。 |
| **网络通信与序列化** | `RestSharp 106` + `Newtonsoft.Json` | 原生 `HttpClient` / `IHttpClientFactory` + `System.Text.Json` | 移除老旧第三方网络库，减少包体积；利用 .NET 8 高性能内置序列化器，内存与速度提升 3~5 倍。 |
| **后端 API 架构** | ASP.NET Core 8 WebAPI + 过度封装的 `Repository / UnitOfWork` 模式 | **精简版 ASP.NET Core 8 WebAPI** (可选用 Minimal APIs) + 直接 `DbContext` 查询 | 去除形式主义的无谓胶水代码，代码结构更扁平清爽。 |
| **数据库** | SQLite (`to.db`) + EF Core 8 | 保持 SQLite + EF Core 8 | 本地数据开箱即用，轻量且完全跨平台。 |

---

## 二、 推荐技术选型分析

### 为什么选择 Avalonia UI 而不是 .NET MAUI / Flutter / Electron？
1. **C# / XAML 资产高度复用**：原项目中的大部分 XAML 布局、数据绑定表达式（Binding）、转换器（Converters）以及 ViewModel 逻辑都可以直接平滑迁移到 Avalonia，重写成本极低。
2. **真正的跨平台原生渲染**：Avalonia 在 macOS 上使用 Metal 原生渲染管道，在 Windows 上使用 DirectX/Direct2D，在 Linux 上使用 Vulkan/X11，渲染帧率高、内存占用低，没有 Electron 庞大的 Chromium 运行时开销。
3. **针对 macOS 的完整支持**：原生支持 macOS 菜单栏、暗黑模式切换、Retina 视网膜屏适配以及 `.app` / `.dmg` 打包发布。

---

## 三、 目标架构设计

```
my-to-do/
├── MyToDo.Shared/              # 【保留并重构】跨平台公共层（netstandard2.0 或 net8.0）
│   ├── Dtos/                   # 业务 DTO（ToDoDto, MemoDto, UserDto, SummaryDto）
│   ├── Parameters/             # 分页与查询参数
│   └── Responses/              # 统一接口返回封装 ApiResponse<T>
│
├── MyToDo.Desktop/             # 【新建】现代化跨平台 Avalonia 客户端
│   ├── Assets/                 # 跨平台矢量图标与图片资源
│   ├── Services/               # 基于 HttpClient 的 API 通信服务
│   ├── ViewModels/             # 基于 CommunityToolkit.Mvvm 的视图模型
│   │   ├── Dialogs/            # 弹窗 VM（AddToDoViewModel, AddMemoViewModel 等）
│   │   ├── IndexViewModel.cs   # 首页看板 VM
│   │   ├── ToDoViewModel.cs    # 待办事项 VM
│   │   ├── MemoViewModel.cs    # 备忘录 VM
│   │   ├── SkinViewModel.cs    # 主题调色板 VM
│   │   └── LoginViewModel.cs   # 登录注册 VM
│   ├── Views/                  # Avalonia XAML 视图与弹窗
│   └── Program.cs / App.axaml  # 跨平台生命周期入口与全局 DI 配置
│
├── MyToDo.Api/                 # 【优化】轻量化 RESTful 后端服务
│   ├── Controllers/            # API 端点（Auth, ToDo, Memo, Summary）
│   ├── MyToDoContext.cs        # EF Core 上下文
│   └── to.db                   # SQLite 数据库文件
│
└── MyToDO/                     # 【保留归档】原有 Windows WPF 项目（供对照对比）
```

---

## 四、 核心代码重构对比示例

### 1. ViewModel 代码对比（以 `IndexViewModel` 为例）

#### 原 Prism 8 写法（繁重冗长）：
```csharp
private string welcome;
public string Welcome
{
    get => welcome;
    set { welcome = value; RaisePropertyChanged(); }
}

public DelegateCommand<string> ExecuteCommand { get; private set; }

public IndexViewModel()
{
    ExecuteCommand = new DelegateCommand<string>(Execute);
}

private void Execute(string cmd) { ... }
```

#### 现代 CommunityToolkit.Mvvm 写法（简洁高效）：
```csharp
[ObservableProperty]
private string _welcome;

[RelayCommand]
private void Execute(string command)
{
    // 自动生成 ExecuteCommand 属性，支持异步 ExecuteAsync
}
```

---

### 2. 网络层重构对比（移除 RestSharp）

#### 原方案：
依赖 `RestSharp.RestClient`，使用反射序列化与繁琐的 Request 参数构造。

#### 现代方案（原生标准封装）：
```csharp
public class HttpRestClient
{
    private readonly HttpClient _client;
    
    public HttpRestClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string url)
    {
        var response = await _client.GetFromJsonAsync<ApiResponse<T>>(url);
        return response ?? new ApiResponse<T> { Status = false, Message = "响应异常" };
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string url, object body)
    {
        var res = await _client.PostAsJsonAsync(url, body);
        return await res.Content.ReadFromJsonAsync<ApiResponse<T>>();
    }
}
```

---

## 五、 改造实施阶段路线图

### 第一阶段：公共类库与网络通信规范化（预计 1 天）
- [ ] 升级 `MyToDo.Shared`，移除旧序列化依赖，采用 `System.Text.Json`。
- [ ] 清理后端 `MyToDo.Api`：移除冗余的 Generic Repository / UnitOfWork 封装，直接利用 EF Core 8 上下文；更新 OpenAPI 规范。

### 第二阶段：搭建跨平台 Avalonia 客户端工程（预计 1~2 天）
- [ ] 创建 `MyToDo.Desktop`（Avalonia 11.x + .NET 8）。
- [ ] 集成 `Material.Avalonia` 主题库与全局样式。
- [ ] 配置 `Microsoft.Extensions.DependencyInjection` 注册基础服务（HttpClient、NavigationService、DialogService）。
- [ ] 搭建主界面导航骨架（侧边栏菜单、顶部状态栏、用户头像区与内容区域 ContentControl）。

### 第三阶段：功能模块逐一迁移与对接（预计 2~3 天）
- [ ] **用户鉴权模块**：登录页（LoginView）、注册页与 Token/用户信息本地持久化。
- [ ] **首页看板模块**：统计卡片（汇总/已完成/百分比/备忘录）、快捷添加弹窗、首页任务快速完成打勾。
- [ ] **待办事项模块**：列表渲染、状态过滤下拉框、关键词搜索、添加/编辑抽屉或弹窗、删除确认。
- [ ] **备忘录模块**：网格瀑布流便签卡片、便签编辑与保存。
- [ ] **个性化设置**：深色/浅色模式切换、主色调（Primary Color）调色盘切换。

### 第四阶段：跨平台构建与发布测试（预计 1 天）
- [ ] **macOS**：生成 `.app` Bundle 并测试在 Apple Silicon (M系列) / Intel 芯片下的流畅度与快捷键。
- [ ] **Windows**：生成单文件发布版本（Self-Contained / Framework-Dependent `.exe`）。
- [ ] **Linux**：验证基于 Ubuntu / Debian 环境的渲染与交互。

---

## 六、 总结与建议
通过该方案改造后：
1. **即刻摆脱对 Windows 的单一依赖**，完全可以在当前的 macOS 环境下直接通过 `dotnet run` 启动客户端和 API 并行调试；
2. 代码行数减少约 **40%**，大幅降低后期维护成本；
3. 保留原有 WPF 项目作为参考对照，采用增量新建 `MyToDo.Desktop` 方式，不破坏原有代码资产，保障项目平稳演进。
