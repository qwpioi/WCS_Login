# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

WCS (Warehouse Control System) Login — a Windows Forms desktop application (.NET Framework 4.7.2) that manages barcode scanner integration, Siemens S7 PLC communication, and warehouse workflow control.

**Tech Stack**: C#, WinForms, DevExpress v22.2, SQL Server, HslCommunication v12.7.0 (S7 protocol), Newtonsoft.Json v13.0.1

## Build & Run

- **Solution**: `WCS_Login.sln` / **Project**: `WCS_Login.csproj`
- **Build**: Open in Visual Studio 2022, build `Debug|Any CPU`
- **Output**: `bin\Debug\WCS_Login.exe` (WinExe)
- **Config**: `App.config` — database connection string named `WCS_Connection`

## Architecture

```
WCS_Login/
├── DAL/            Data Access Layer — DbHelper.cs (SQL Server, parameterized queries)
├── Forms/          WinForms UI — login, main MDI, config panels, query/debug tools
├── PLC/            Device communication — S7PlcHelper.cs (Siemens S7), TcpScannerListener.cs (barcode scanners)
├── Utils/          Logger.cs (async file logger → D:\VS\Data\WCS_Login_Logger)
├── WCS/            WcsController.cs (core workflow: scanner → DB lookup → PLC write)
├── Program.cs      Entry point (global CurrentUserName)
└── App.config      Connection strings
```

### Key Architectural Patterns

- **Layered**: DAL → PLC → WCS → Forms (UI)
- **Thread safety**: `ConcurrentDictionary` for box rule cache, `SemaphoreSlim` for serialized PLC writes
- **Async I/O**: Scanner TCP listener, PLC heartbeat (30s), logger (ConcurrentQueue + background thread)
- **PLC handshake**: Write control value to DB31.0 → wait for PLC to clear → next write (verification loop)
- **Deduplication**: 2-second window prevents duplicate box processing
- **Cache**: Box rules cached in `ConcurrentDictionary`, refreshed every 5 minutes

### DevExpress Grid Row Numbers

All child forms with GridControl use an **Unbound Column** (`gridColumnRowNo`) for auto-numbered rows:

1. **Designer.cs**: `gridColumnRowNo` declared, `UnboundDataType = typeof(string)`, `VisibleIndex = 0`, all other columns shifted +1, `CustomUnboundColumnData` event registered
2. **Designer.cs bottom**: `private DevExpress.XtraGrid.Columns.GridColumn gridColumnRowNo;` field declaration (must not omit)
3. **.cs file**: `gridView1_CustomUnboundColumnData` handler returns `(e.ListSourceRowIndex + 1).ToString()` when `e.Column.FieldName == "RowNo" && e.IsGetData`
4. **Forms affected**: `FrmPLC_IP_Config`, `FrmEthernetScanner_Config`, `FrmStation_Config`, `FrmPLC_WriteAddress_Config`, `FrmBoxScanRecord_Query`, `FrmBoxTask_Query`, `FrmManualDebug`

### Control Values

- `1111` = 直行 (straight pass-through)
- `2222` = 移栽 (transfer/divert)

### Database Tables (inferred from code)

| Table | Key Columns |
|---|---|
| Users | UserName, Password, Role |
| T_SystemLog | UserName, Operation, Module, Content, LogLevel |
| T_PLC_IP_Config | PlcNo, IP, Port, PlcType |
| T_EthernetScanner_Config | ScannerNo, IP, Port |
| T_Box_Task | BoxNo, TaskType, TaskRule, CreateTime, CreateUser, Remark |
| T_BoxScanRecord | Id, BoxNo, ScannerName, ScanTime, ScanResult, StationName, Remark |

## Forms

| Form | Purpose |
|---|---|
| FormLogin | User authentication |
| FrmMain | MDI container, WCS start/stop, navigation |
| FrmBase | Base form with toolbar and grid helper |
| FrmPLC_IP_Config | PLC IP/port configuration |
| FrmEthernetScanner_Config | Scanner TCP configuration |
| FrmStation_Config | Station configuration |
| FrmPLC_WriteAddress_Config | PLC write address mapping |
| FrmBoxScanRecord_Query | Scan record query |
| FrmBoxTask_Query | Box task definition query |
| FrmManualDebug | Manual debug controls |
| FrmPLC_Monitor | Real-time PLC value monitor |

## Default Test Credentials

admin/123456, user1/111111, user2/222222
