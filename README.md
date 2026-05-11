# Dead Rising - 丧尸围城

<p align="center">
  <img src="https://img.shields.io/badge/Unity-2021.3+-green.svg" alt="Unity Version">
  <img src="https://img.shields.io/badge/C%23-9.0-blue.svg" alt="C# Version">
  <img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="License">
</p>

> 🎮 一款基于 Unity 3D 开发的塔防 + 第三人称动作游戏，融合了经典塔防策略与即时战斗操作。

## 📖 项目简介

**Dead Rising - 丧尸围城** 是一款以末日丧尸题材为背景的塔防策略游戏。玩家需要选择不同的角色，建造和升级防御塔，抵御一波又一波的丧尸进攻，保护主塔不被摧毁。

### 🎯 游戏特色

- **6种可解锁角色**：工程师、军官、步兵、狙击手、火焰兵、枪手
- **9种防御塔类型**：加农炮、机枪炮、闪电炮，每种3级升级
- **18种丧尸敌人**：包含不同攻击方式、移动速度和生命值
- **多章节关卡**：目前已开放「缄默圣庭」等关卡

---

## 🛠️ 技术栈

| 技术领域 | 使用技术 |
|---------|---------|
| **引擎** | Unity 2021.3 LTS |
| **语言** | C# 9.0 |
| **UI系统** | Unity UGUI |
| **寻路系统** | NavMesh Agent |
| **动画系统** | Animator State Machine |
| **数据持久化** | LitJson + PlayerPrefs |
| **音效系统** | Unity AudioSource |
| **代码架构** | 单例模式 + 事件订阅 |

---

## 🏗️ 项目架构

```
Assets/
├── Scripts/                    # 项目核心代码
│   ├── BasePanel.cs           # UI面板基类（泛型+淡入淡出）
│   ├── GameManager.cs         # 游戏主管理器（单例）
│   ├── UIManager.cs           # UI面板管理系统
│   ├── BGM.cs                 # 背景音乐控制
│   │
│   ├── BeginScenes/           # 开始场景
│   │   ├── CameraAnimator.cs  # 相机动画控制
│   │   └── UI/                # 开始场景UI面板
│   │       ├── BeginPanel.cs
│   │       ├── ChooseHeroPanel.cs
│   │       ├── ChooseScenesPanel.cs
│   │       ├── SettingPanel.cs
│   │       └── UIManager.cs
│   │
│   ├── GameScenes/            # 游戏场景
│   │   ├── GameManager.cs     # 游戏逻辑管理
│   │   ├── CameraMove.cs      # 第三人称跟随相机
│   │   ├── Object/            # 游戏对象
│   │   │   ├── Player.cs      # 玩家控制器
│   │   │   ├── Tower.cs       # 防御塔AI
│   │   │   ├── Zombie.cs      # 丧尸AI
│   │   │   └── MainTower.cs   # 主塔
│   │   └── UI/                # 游戏UI
│   │       ├── GamePanel.cs
│   │       ├── PausePanel.cs
│   │       └── OverPanel.cs
│   │
│   ├── Data/                  # 数据层
│   │   ├── GameDataMgr.cs     # 数据管理器
│   │   ├── RoleInfo.cs
│   │   ├── TowerData.cs
│   │   ├── ZombieInfo.cs
│   │   └── PlayerData.cs
│   │
│   └── Json/Json/             # JSON序列化
│       ├── JsonMgr.cs
│       └── LitJson/           # LitJson库
│
├── StreamingAssets/           # 配置数据（JSON）
│   ├── RoleInfos.json         # 角色配置
│   ├── ZombieInfos.json       # 丧尸配置
│   ├── TowerData.json         # 防御塔配置
│   └── ScenesInfos.json       # 关卡配置
│
├── Resources/                 # 运行时资源
│   ├── Role/                  # 角色预制体
│   ├── Tower/                 # 防御塔预制体
│   ├── Zombie/                # 丧尸预制体
│   ├── Effects/               # 特效资源
│   └── Music/                 # 音效资源
│
└── Scenes/                    # Unity场景文件
    ├── BeginScene/
    └── GameScene1/
```

---

## ✨ 核心功能实现

### 1. 单例模式架构

游戏使用单例模式管理全局核心对象，实现模块间解耦：

```csharp
// GameManager - 游戏全局状态管理
public class GameManager {
    private static GameManager instance = new GameManager();
    public static GameManager Instance => instance;
    
    public Player player;
    public List<Zombie> zombiesList = new List<Zombie>();
    private List<MonsterPoint> monsterPoints = new List<MonsterPoint>();
}

// UIManager - UI面板管理（泛型设计）
public class UIManager : MonoBehaviour {
    private Dictionary<string, BasePanel> panelDict = new Dictionary<string, BasePanel>();
    
    public T ShowPanel<T>() where T : BasePanel {
        string panelName = typeof(T).Name;
        // 动态加载面板预制体并实例化
    }
}
```

### 2. UI 面板系统

基于 `BasePanel` 抽象类实现统一的界面管理，支持淡入淡出动画：

```csharp
public abstract class BasePanel : MonoBehaviour {
    private CanvasGroup canvasGroup;
    
    // 淡入淡出效果
    protected virtual void Update() {
        if (isShowing && canvasGroup.alpha != 1f) {
            canvasGroup.alpha += fadeSpeed * Time.unscaledDeltaTime;
        }
    }
}
```

### 3. 角色控制系统

- **Animator 动画状态机**：管理移动、攻击、翻滚、防御等状态
- **攻击判定**：
  - 近战：使用 `OverlapSphere` 球形检测
  - 远程：使用 `RaycastAll` 射线检测

```csharp
// 近战攻击检测
Collider[] colliders = Physics.OverlapSphere(
    transform.position + transform.forward + transform.up, 
    1f, 
    1 << LayerMask.NameToLayer("Monster")
);

// 远程射击检测
RaycastHit[] raycastHits = Physics.RaycastAll(
    new Ray(firePoint.position, transform.forward), 
    shootDis, 
    1 << LayerMask.NameToLayer("Monster")
);
```

### 4. NavMesh 寻路系统

丧尸使用 Unity 内置寻路系统实现自动导航：

```csharp
public class Zombie : MonoBehaviour {
    private NavMeshAgent agent;
    
    public void InitInfo(ZombieInfo info) {
        agent.speed = agent.acceleration = info.moveSpeed;
        agent.angularSpeed = info.roundSpeed;
        agent.stoppingDistance = 5f;
        target = MainTower.Instance.transform;
    }
    
    public void BornOver() {
        agent.SetDestination(target.position);
    }
}
```

### 5. 波次生成系统

```csharp
public class MonsterPoint : MonoBehaviour {
    public void SpawnMonster() {
        ZombieInfo info = GameDataMgr.Instance.zombieInfos[curMonsterId - 1];
        GameObject zombieObj = Instantiate(
            Resources.Load<GameObject>(info.res),
            this.transform.position, 
            Quaternion.identity
        );
        Zombie zombie = zombieObj.GetComponent<Zombie>();
        zombie.InitInfo(info);
        GameManager.Instance.AddZombie(zombie);
    }
}
```

### 6. 数据持久化

使用 LitJson 实现游戏配置的序列化与反序列化：

```csharp
public class JsonMgr {
    // 存储数据
    public void SaveData(object data, string fileName) {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        string jsonStr = JsonMapper.ToJson(data);
        File.WriteAllText(path, jsonStr);
    }
    
    // 读取数据（双路径策略）
    public T LoadData<T>(string fileName) {
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        if (!File.Exists(path))
            path = Application.persistentDataPath + "/" + fileName + ".json";
        // ...
    }
}
```

### 7. 音效系统

基于事件订阅的音效管理：

```csharp
public void PlaySound(string name, Vector3 pos) {
    AudioClip clip = Resources.Load<AudioClip>("Music/" + name);
    if (clip != null && musicData.soundOn) {
        AudioSource.PlayClipAtPoint(clip, pos, musicData.soundVolume);
    }
}
```

---

## 🎮 游戏玩法

### 操作说明

| 按键 | 功能 |
|-----|-----|
| `WASD` | 移动 |
| `鼠标移动` | 视角旋转 |
| `鼠标左键` | 攻击 |
| `鼠标右键` | 翻滚 |
| `左Ctrl` | 防御 |
| `1/2/3` | 建造防御塔 |
| `空格` | 升级防御塔 |
| `ESC` | 暂停游戏 |

### 游戏流程

```
主菜单 → 角色选择 → 关卡选择 → 游戏战斗 → 结算 → 返回
   ↓           ↓           ↓
角色解锁    场景预览    建造防御/击杀丧尸
```

---

## 📊 数据配置示例

### 角色配置 (RoleInfos.json)

```json
{
  "id": 6,
  "res": "Role/gunner",
  "damage": 15,
  "lockMoney": 1000,
  "hp": 120,
  "tips": "gunner",
  "money": 100
}
```

### 防御塔配置 (TowerData.json)

```json
{
  "id": 1,
  "name": "加农炮1",
  "money": 200,
  "atk": 2,
  "atkRange": 4,
  "offsetTime": 1,
  "next": 2,
  "type": 1
}
```

---

## 🚀 运行项目

### 环境要求

- Unity 2021.3 LTS 或更高版本
- Windows 10/11

### 打开项目

1. 克隆或下载本项目
2. 使用 Unity Hub 打开 `Dead Rising` 文件夹
3. 等待导入完成
4. 在 Unity Editor 中打开 `Assets/Scenes/BeginScene` 场景
5. 点击运行按钮开始游戏

---

## 📝 项目亮点总结

| 模块 | 技术亮点 |
|-----|---------|
| **架构设计** | 单例模式 + 泛型 UI 管理系统 |
| **动画控制** | Animator 状态机 + 动画事件触发 |
| **寻路 AI** | NavMesh Agent 自动寻路导航 |
| **战斗系统** | 球形检测 + 射线检测双判定 |
| **数据驱动** | JSON 配置化设计，策划程序解耦 |
| **持久化** | LitJson 序列化 + 双路径存储策略 |
| **UI 交互** | CanvasGroup 淡入淡出 + 事件订阅 |

---

## 👤 作者

**Your Name**
- GitHub: [Your GitHub](https://github.com/yourusername)
- Email: your.email@example.com

---

## 📄 许可证

本项目仅供学习和交流使用，未经授权禁止商用。

---

<p align="center">
  ⭐ 如果这个项目对你有帮助，欢迎 Star！
</p>
