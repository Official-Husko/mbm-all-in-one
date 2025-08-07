# 🐉 Monster Black Market All-In-One Mod

Welcome to the **Monster Black Market All-In-One Mod** project!
This is a powerful, modular mod for MBM or Monster Black Market, built as a **BepInEx** mod, designed to enhance gameplay with a suite of cheats, mod management, and developer tools—all accessible through a sleek in-game menu.

---

## ✨ Features

- **🧑‍💼 Player Cheats:**  
  Instantly add gold, items, reputation, and more to your player profile.

- **🎉 Event Launcher:**  
  Trigger special in-game events on demand for testing or fun.

- **🧬 NPC Spawner:**  
  Spawn special NPCs with a customizable dropdown menu.

- **🧪 Experimental Mods:**  
  Try out new, in-development features and mods.

- **🛠️ Mod Management:**  
  View and manage stable, experimental, and broken mods from a categorized tabbed interface.

- **🎨 Modern UI:**  
  Access all features from a draggable, tabbed in-game menu with color-coded sections and clickable author credits.

---

## 🖥️ How It Works

- Built as a **BepInEx** plugin for Unity games.
- Uses a modular architecture:
  - Cheats and mods are registered automatically via reflection.
  - Each cheat implements a common interface for easy extension.
- The in-game menu is toggled with `Insert` or `F1` and features:
  - Tabs for Player, Events, NPCs, Experimental, and Mods.
  - Custom popups for item spawning.
  - Dropdowns for NPC selection.
  - Color-coded labels for mod status (Stable, Experimental, Broken).

---

## 🚀 Getting Started

1. **Build the DLL:**  
   Run the provided `build.sh` script or use your preferred .NET build tool.

2. **Install:**  
   Place the compiled DLL into your game’s `BepInEx/plugins` directory.

3. **Launch the Game:**  
   Press `Insert` or `F1` in-game to open the mod menu.

---

## 🧩 Extending & Developing

- Add new cheats by implementing the `IRegisterableCheat` interface in the `src/modules/cheats/` directory.
- Mods are auto-registered and appear in the menu based on their category.
- UI utilities and helpers are available in `src/modules/utils/`.

---

## 📂 Project Structure

```text
src/
  base.cs                # Main plugin logic and UI
  modules/
    cheats/              # Individual cheat implementations
    utils/               # UI and helper utilities
  configurations/        # Mod configuration files
build.sh                 # Build script
mbm-all-in-one.csproj    # Project file
```

---

## 👤 Author

Created by [Official-Husko](https://github.com/Official-Husko)  
Click the author label in the mod menu to visit the GitHub page!

---

## ⚠️ Disclaimer

This project is for educational and personal use.  
Use responsibly and respect the terms of service of any game you modify.

---

Enjoy modding! 🚀
