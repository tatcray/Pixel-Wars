# pixel-wars

Pixel-Wars is a game made on Unity, using only 4 MonoBehaviours

![Screenshots](https://github.com/re-mouse/Image-sources/blob/master/photo_2022-11-18_14-16-45.jpg?raw=true)

[Link on GooglePlay](https://play.google.com/store/apps/details?id=com.remouse.pixelwars&hl=ru&gl=US)

## 🚀Installation

- Clone the project.

```bash
git clone https://github.com/re-mouse/pixel-wars.git
```
- Install Unity Editor 2021.3.12f1 or higher

- Build on platform that you need
> UI Designed only to vertical aspect ratios (9:16, 9:18, etc.)
---

## Project structure

- [Assets](./Assets) folder contains all plugins, assets
- [Assets/Project](./Assets/Project) contains all Scripts, Meshes, Textures and everything according to project

### Code entrypoint

> All scripts containing in [Assets/Project/Scripts](./Assets/Project/Scripts) folder

Game starts from [Core/GameIntializer.cs](./Assets/Project/Scripts/Core/GameInitializer.cs)
```csharp
private void Start()
        {
            InitializeEnvironment();
            
            InitializeSaves();
            InitializeCubes();
            InitializeCrosshair();
            InitializeWeapon();
            InitializeUpgrades();
            InitializeCamera();

            InitializeUI();
            InitializeTutorial();
            
            InitializeAnalyticsEvents();
            InitializeAds();
            InitializeGameEvents();
        }
```

## Used plugins
- [Dotween](http://dotween.demigiant.com/)
- [Appmetrica](https://appmetrica.yandex.ru/docs/mobile-sdk-dg/plugins/unity/unity-plugin.html)
- [Applovin](https://dash.applovin.com/documentation/mediation/unity/getting-started/integration)

## Used assets
- [Epic toon FX](https://assetstore.unity.com/packages/vfx/particles/epic-toon-fx-57772)
- [SimpliCity Weapons and Explosives](https://assetstore.unity.com/packages/3d/props/weapons/simplicity-weapons-explosives-46730)
