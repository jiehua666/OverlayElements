# 叠印元素 (Overlay Elements) - Unity Client

## 项目概述
《叠印元素》Unity 游戏客户端

## 技术栈
- **引擎**: Unity 2022.3 LTS
- **语言**: C#
- **目标平台**: PC / Mobile (iOS/Android)
- **架构**: MVVM / ECS 混合

## 目录结构

```
OverlayElements/
├── Assets/
│   ├── Audio/           # 音效和背景音乐
│   ├── Fonts/           # 字体文件
│   ├── Materials/       # 材质
│   ├── Prefabs/         # 预制体
│   ├── Scenes/          # 游戏场景
│   ├── Scripts/         # 脚本代码
│   │   ├── Battle/      # 战斗系统
│   │   ├── Card/        # 卡牌系统
│   │   ├── Core/        # 核心系统
│   │   ├── Network/     # 网络通信
│   │   └── UI/          # UI系统
│   ├── Sprites/         # 精灵图片
│   └── UI/              # UI资源
├── Packages/            # Unity包管理
└── ProjectSettings/     # 项目设置
```

## 开发环境

| 工具 | 版本要求 |
|------|----------|
| Unity Editor | 2022.3 LTS (2022.3.62f1) |
| Visual Studio | 2022 17.10+ |
| .NET | 6.0+ |

## 快速开始

1. 安装 Unity 2022.3 LTS
2. 使用 Unity Hub 打开此项目
3. 等待包导入完成
4. 打开 `Assets/Scenes/SampleScene.unity`
5. 点击 Play 运行

## 核心系统

- [ ] 卡牌数据系统 - CardData.cs
- [ ] 战斗系统 - 回合制/叠印连锁
- [ ] UI系统 - 卡牌展示/战斗界面
- [ ] 网络系统 - 多人对战

## Git 分支规范

- `main` - 主分支，稳定版本
- `develop` - 开发分支
- `feature/*` - 功能分支
- `fix/*` - 修复分支

## 提交规范

- `feat:` - 新功能
- `fix:` - Bug修复
- `docs:` - 文档更新
- `refactor:` - 代码重构
- `chore:` - 构建/工具更新

---

**最后更新**: 2026-03-20
**版本**: v0.1.0
