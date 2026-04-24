# 🦴 MegaBonk Battle Arena — Version 2.0.1  
**A turn-based console battle game developed entirely in C# (.NET 8)**  
**Created by Tezas (Dimos Ioannis)**  
> Second major version of the project — evolved from the original prototype made within **10 days of learning C#**

---

## 🎮 Overview
**MegaBonk Battle Arena v2.0.1** is an upgraded **turn-based console fighting game** that blends **tactical decision-making**, **character abilities**, and **cinematic ASCII-style animations**.  
Each fighter has distinct traits, bonuses, and effects that interact dynamically through a balanced “rock-paper-scissors” inspired combat system.

This version improves:
- Combat balance  
- Buff and parry mechanics  
- Dodge and dizzy interactions  
- Character-specific animations  
- Console clarity and timing effects  

---

## ⚔️ Battle System
Every round follows synchronized **turn resolution** logic:

1. Both player and CPU select **Attack** or **Defend**.  
2. Defense applies first (block + reflection).  
3. Attacks resolve at the same time.  
4. Special effects (stun, dizzy, dodge, armor buffs) apply immediately within the same turn.

### ⚙️ Core Rules
- **Defend:** halves incoming damage and reflects **40–65%**.  
- **Stun/Dizzy:** cancels the opponent’s attack on the same turn.  
- **Defend overrides** any stun/dizzy.  
- **Parry damage** scales dynamically based on attack power.  
- Real-time **text animations** and effects per action.  

---

## 🧩 Playable Characters

| Character | Description |
|------------|--------------|
| **🦴 Calcium** | Throws bouncing bones 💀 with a **33% chance to double-hit** the opponent. |
| **🏹 Robinette** | Agile sniper with **25% dodge**, increased to **50% when HP < 20%**. |
| **🤖 CL4NK** | Cybernetic gunslinger with a **33% critical hit chance** and rapid-fire revolver animation. |
| **💪 Megachad** | Brute powerhouse who can **stun opponents (25%)** with sheer strength. |
| **⚔️ Sir Oofie** | Noble knight gaining **temporary armor buffs** before each strike or defense, drastically reducing damage and balancing reflected hits. |
| **🥪 Mike K** | Meme legend who throws a **flying toast** 🥪 with **35% dizzy chance** and full-screen shake animation effect. |

---

## 🧠 Features
- ✅ Full **OOP combat engine** (inheritance, polymorphism, overrides).  
- ✅ **Simultaneous turn logic** with balanced outcomes.  
- ✅ **Dynamic console animations** for all characters.  
- ✅ Advanced mechanics: **Defend, Parry, Dodge, Buffs, Dizzy, Stun.**  
- ✅ **Screen spin effect** when dizzy triggers.  
- ✅ Clean code, modular character structure, and extensible battle system.  

---

## ⚙️ Tech Stack
- **Language:** C# (.NET 8.0)  
- **IDE:** Visual Studio 2022  
- **Platform:** Windows Console  
- **Architecture:** Object-Oriented (Character-based inheritance)  
- **Build:** Self-contained `.exe` (win-x64, single file)  
- **Version:** 2.0.1  

---

## 🚀 Run
To build or run:
```bash
dotnet run
```

To play the published standalone version:
```bash
MegaBonk Battle Arena.exe
```

---

## 🧱 Development Notes
- Built as a continuation of the original *MegaBonk Battle Arena (v1)*.  
- Developed for testing **C# gameplay logic**, **real-time console visuals**, and **turn-based AI combat.**  
- This version introduces **dynamic animation sync**, **damage logic balancing**, and **new special effects**.  

---

## 👨‍💻 Credits
**Author:** Dimos Ioannis *(Tezas Studio)*  
**Concept, Code & Design:** Tezas  
**Version:** 2.0.1  
**Year:** 2025  

---

> 🦴 *“Every bonk tells a story — every version refines the art of battle.”*

## License

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

Licensed under the Apache License 2.0 - see [LICENSE](./LICENSE) and [NOTICE](./NOTICE).  
Created by [Ioannis (wannabexaker)](https://github.com/wannabexaker) - Attribution required.