# Unity Project Creation Script - English Version
# Creates the project structure for "叠印元素" (Overlay Elements)

param(
    [string]$ProjectPath = "C:\Users\PC\Desktop\项目文件夹\02_程序代码\client\OverlayElements"
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Creating Unity Project: Overlay Elements" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# 1. Create directory structure
Write-Host "[1/4] Creating directory structure..." -ForegroundColor Yellow

$dirs = @(
    $ProjectPath,
    "$ProjectPath\Assets",
    "$ProjectPath\Assets\Scenes",
    "$ProjectPath\Assets\Scripts",
    "$ProjectPath\Assets\Scripts\Core",
    "$ProjectPath\Assets\Scripts\Card",
    "$ProjectPath\Assets\Scripts\Battle",
    "$ProjectPath\Assets\Scripts\UI",
    "$ProjectPath\Assets\Scripts\Network",
    "$ProjectPath\Assets\Prefabs",
    "$ProjectPath\Assets\Materials",
    "$ProjectPath\Assets\Sprites",
    "$ProjectPath\Assets\UI",
    "$ProjectPath\Assets\Audio",
    "$ProjectPath\Assets\Fonts",
    "$ProjectPath\Packages",
    "$ProjectPath\ProjectSettings"
)

foreach ($dir in $dirs) {
    if (-not (Test-Path $dir)) {
        New-Item -ItemType Directory -Path $dir -Force | Out-Null
        Write-Host "  Created: $dir" -ForegroundColor Gray
    }
}

Write-Host "  [OK] Directory structure created" -ForegroundColor Green

# 2. Create project configuration files
Write-Host ""
Write-Host "[2/4] Creating project configuration..." -ForegroundColor Yellow

# ProjectVersion.txt
$projectVersion = "m_EditorVersion: 2022.3.62f1`nm_EditorVersionWithRevision: 2022.3.62f1 (8f0d4a8d1e1c)"
Set-Content -Path "$ProjectPath\ProjectSettings\ProjectVersion.txt" -Value $projectVersion -Encoding UTF8

# Packages manifest
$manifest = @{
    "dependencies" = @{
        "com.unity.collab-proxy" = "2.3.1"
        "com.unity.feature.development" = "1.0.1"
        "com.unity.textmeshpro" = "3.0.6"
        "com.unity.timeline" = "1.7.6"
        "com.unity.ugui" = "1.0.0"
        "com.unity.visualscripting" = "1.9.1"
    }
} | ConvertTo-Json -Depth 5

Set-Content -Path "$ProjectPath\Packages\manifest.json" -Value $manifest -Encoding UTF8

$packagesLock = @{
    "dependencies" = @{}
} | ConvertTo-Json -Depth 5

Set-Content -Path "$ProjectPath\Packages\packages-lock.json" -Value $packagesLock -Encoding UTF8

Write-Host "  [OK] Project configuration created" -ForegroundColor Green

# 3. Create default scene
Write-Host ""
Write-Host "[3/4] Creating default scene..." -ForegroundColor Yellow

$simpleScene = @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!29 &1
OcclusionCullingSettings:
  m_ObjectHideFlags: 0
  serializedVersion: 2
  m_OcclusionBakeSettings:
    smallestOccluder: 5
    smallestHole: 0.25
    backfaceThreshold: 100
  m_SceneGUID: 00000000000000000000000000000000
  m_OcclusionCullingData: {fileID: 0}
--- !u!157 &3
LightmapSettings:
  m_ObjectHideFlags: 0
  serializedVersion: 12
  m_GIWorkflowMode: 1
--- !u!1 &100000
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  serializedVersion: 6
  m_Component:
  - component: {fileID: 400000}
  - component: {fileID: 2000000}
  - component: {fileID: 8100000}
  m_Layer: 0
  m_Name: Main Camera
  m_TagString: MainCamera
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!4 &400000
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 100000}
  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}
  m_LocalPosition: {x: 0, y: 1, z: -10}
  m_LocalScale: {x: 1, y: 1, z: 1}
  m_ConstrainProportionsScale: 0
  m_Children: []
  m_Father: {fileID: 0}
  m_RootOrder: 0
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
--- !u!20 &2000000
Camera:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 100000}
  m_Enabled: 1
  serializedVersion: 2
  m_ClearFlags: 1
  m_BackGroundColor: {r: 0.19215687, g: 0.3019608, b: 0.4745098, a: 0}
  m_projectionMatrixMode: 1
  m_GateFitMode: 2
  m_FOVAxisMode: 0
  m_SensorSize: {x: 36, y: 24}
  m_LensShift: {x: 0, y: 0}
  m_FocalLength: 50
  m_NormalizedViewPortRect:
    serializedVersion: 2
    x: 0
    y: 0
    width: 1
    height: 1
  near clip plane: 0.3
  far clip plane: 1000
  field of view: 60
  orthographic: 0
  orthographic size: 5
  m_Depth: -1
  m_CullingMask:
    serializedVersion: 2
    m_Bits: 4294967295
  m_RenderingPath: -1
  m_TargetTexture: {fileID: 0}
  m_TargetDisplay: 0
  m_TargetEye: 3
  m_HDR: 1
  m_AllowMSAA: 1
  m_AllowDynamicResolution: 0
  m_OcclusionCulling: 1
--- !u!81 &8100000
AudioListener:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 100000}
  m_Enabled: 1
"@

Set-Content -Path "$ProjectPath\Assets\Scenes\SampleScene.unity" -Value $simpleScene -Encoding UTF8

Write-Host "  [OK] Default scene created" -ForegroundColor Green

# 4. Create initial C# scripts
Write-Host ""
Write-Host "[4/4] Creating initial scripts..." -ForegroundColor Yellow

# GameEntry.cs
$gameEntry = @"
// Overlay Elements - Game Entry Point
// Version: 0.1.0
// Created: 2026-03-20

using UnityEngine;

namespace OverlayElements
{
    /// <summary>
    /// Game entry point and basic initialization
    /// </summary>
    public class GameEntry : MonoBehaviour
    {
        [Header("Game Configuration")]
        [SerializeField] private string gameVersion = "0.1.0";
        [SerializeField] private bool enableDebugMode = true;

        private void Awake()
        {
            InitializeGame();
        }

        private void Start()
        {
            if (enableDebugMode)
            {
                Debug.Log($"[Overlay Elements] Game Started - Version {gameVersion}");
            }
        }

        private void InitializeGame()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 1;

            if (enableDebugMode)
            {
                Debug.Log("[Overlay Elements] Game systems initialized");
            }
        }
    }
}
"@

Set-Content -Path "$ProjectPath\Assets\Scripts\Core\GameEntry.cs" -Value $gameEntry -Encoding UTF8

# CardData.cs - Card data structure
$cardData = @"
// Card data structure
// Version: 0.1.0

using System;
using System.Collections.Generic;
using UnityEngine;

namespace OverlayElements.Card
{
    /// <summary>
    /// Card element types
    /// </summary>
    public enum ElementType
    {
        Fire,   // 火
        Water,  // 水
        Wind,   // 风
        Wood    // 木
    }

    /// <summary>
    /// Card rarity levels
    /// </summary>
    public enum CardRarity
    {
        Common = 1,    // 普通
        Rare = 2,      // 稀有
        Epic = 3,      // 史诗
        Legendary = 4,  // 传说
        Mythic = 5     // 神话
    }

    /// <summary>
    /// Card data template
    /// </summary>
    [Serializable]
    public class CardData
    {
        [Header("Basic Info")]
        public string cardId;          // Unique card ID (e.g., "FIRE_001")
        public string cardName;       // Card display name
        public ElementType element;   // Element type
        public CardRarity rarity;     // Rarity level

        [Header("Combat Stats")]
        public int cost;              // Energy cost
        public int attack;             // Attack power
        public int health;             // Health points
        public int maxHealth;          // Max health for this card

        [Header("Visual")]
        public string artworkPath;     // Path to artwork sprite
        public string description;     // Card description text

        [Header("Skills")]
        public List<string> skillIds; // List of skill IDs

        /// <summary>
        /// Initialize card with default values
        /// </summary>
        public void Initialize()
        {
            cardId = "";
            cardName = "";
            element = ElementType.Fire;
            rarity = CardRarity.Common;
            cost = 0;
            attack = 0;
            health = 0;
            maxHealth = 0;
            artworkPath = "";
            description = "";
            skillIds = new List<string>();
        }

        /// <summary>
        /// Get element color for UI display
        /// </summary>
        public Color GetElementColor()
        {
            return element switch
            {
                ElementType.Fire => new Color(1f, 0.3f, 0f),
                ElementType.Water => new Color(0.2f, 0.5f, 1f),
                ElementType.Wind => new Color(0.3f, 0.8f, 0.3f),
                ElementType.Wood => new Color(0.6f, 0.4f, 0.2f),
                _ => Color.white
            };
        }
    }
}
"@

Set-Content -Path "$ProjectPath\Assets\Scripts\Card\CardData.cs" -Value $cardData -Encoding UTF8

Write-Host "  [OK] Initial scripts created" -ForegroundColor Green

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Unity Project Creation Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Project Path: $ProjectPath" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "  1. Open Unity Hub" -ForegroundColor Gray
Write-Host "  2. Click 'Open' and select the OverlayElements folder" -ForegroundColor Gray
Write-Host "  3. Wait for Unity to import packages" -ForegroundColor Gray
Write-Host ""
