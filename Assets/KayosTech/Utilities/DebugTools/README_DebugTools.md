
# 🛠️ DebugTools for Unity

**KayosTech.Utilities.DebugTools** is a modular logging utility package for Unity that provides centralized logging with console output, file persistence, and in-game UI visualization. Designed for both developers and testers, this tool makes debugging intuitive and structured.

---

## 📦 Features

- 🔧 Console and in-game UI log routing
- 📝 Persistent log file generation
- 🔍 Custom log levels: Info, Success, Alert, Error, Internal
- 📁 Rolling log file management (limit old logs)
- 🎨 Configurable UI prefabs with fade and auto-destroy

---

## 🚀 Installation

1. Copy the `DebugTools` folder into your Unity project’s `Assets` directory.
2. Make sure TextMeshPro is imported in your project (required for UI logs).
3. Assign the appropriate prefabs in the `UILogDispatcher` component.
4. (Optional) Add the `UILogDispatcher` prefab to your persistent UI canvas.

---

## 🧩 Folder Structure

```
DebugTools/
├── Core/
│   ├── LogEntry.cs
│   ├── LogRouter.cs
│   └── LogUtility.cs
├── File/
│   └── LogFileWriter.cs
└── UI/
    ├── UILogDispatcher.cs
    ├── UILogMessage.cs
    ├── Prefabs/
    └── Fonts/Images/
```

---

## 🧠 How It Works

### Emit a Log
```csharp
LogRouter.Log("System", "Initialization complete.", LogLevel.Success);
```

### Log Levels
| Level    | Console     | UI Visible | File Saved |
|----------|-------------|------------|------------|
| Internal | ✅           | ❌         | ✅         |
| Info     | ✅           | ✅         | ✅         |
| Success  | ✅           | ✅         | ✅         |
| Alert    | ⚠️ Warning   | ✅         | ✅         |
| Error    | ❌ Error     | ✅         | ✅         |

### Lifecycle
- `LogRouter` collects and routes logs.
- `LogFileWriter` buffers and saves them.
- `UILogDispatcher` spawns the correct UI prefab.
- `UILogMessage` fades out and self-destroys.

---

## 🛠 Customization

### 💬 UI Prefabs
- Modify prefab visuals in `DebugTools/UI/Prefabs/`
- Each prefab uses `UILogMessage` with a `TextMeshProUGUI` field.
- Attach icons, set background colors, or animate entries.

### 🎨 Styling
- Fonts are located in `UI/Fonts/`
- Icon example: `UI/Images/bug-report-icon.png`

### 🧹 File Cleanup
```csharp
LogFileWriter.ClearAllLogs();
```

---

## 🔌 Extension Ideas

- Add additional log handlers (e.g., send to Discord or webhook).
- Filter logs by tag or severity.
- Display logs in a developer-only debug panel.

---

## 📂 Log Files

- Stored under:
  - **Windows/Editor**: `%LOCALAPPDATA%/{GameName}Logs`
  - **Other Platforms**: `Application.persistentDataPath/{GameName}Logs`
- Keeps latest **10** logs, auto-purges older ones.

---

## 📄 License
MIT License — use freely in personal and commercial Unity projects.

---

## 👤 Author
**KayosTech**

This utility is part of the KayosTech Unity Tooling Suite.
