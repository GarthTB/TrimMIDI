# Garth的MIDI批量修饰工具

程序基于[Melanchall.DryWetMidi](https://github.com/melanchall/drywetmidi)。

主要针对[GiantMIDI-Piano](https://github.com/bytedance/GiantMIDI-Piano)生成的原始MIDI。

所有目标文件的扩展名必须都是`.mid`（不区分大小写）。结果文件生成在原始目录中，缀以`_trimmed`。

仅在拖入MIDI文件/文件夹，或手动输入有效路径时，一次性载入所有的MIDI文件。载入后，在程序外修改、删除MIDI文件不会影响程序正常运行。

*不保证软件安全性。请在使用软件前做好备份。若使用软件时发生数据损失，恕不负责。*

## 环境要求

- [.NET 8.0运行时](https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0)

## 功能清单（依次执行）

1. 删去所有时值短于特定长度的音符
2. 缩短所有音尾离下一音头过近，甚至覆盖下一音头的音，使得音之间的间隔不小于指定长度；同时，间隔也不会大于这个音本身的长度。
3. 渐变（线性插值）地增益所有音，增益力度由4个关键点控制

## 快捷键

- F1：复制仓库链接
- F3：保存日志
- F5：清空

## 版本日志

### 0.1.1 (20240902)

- 修改：个人配置
- 优化：简化代码

### 0.1.0 (20240829)

- 发布！