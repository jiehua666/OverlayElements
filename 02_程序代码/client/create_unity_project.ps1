# Unity项目创建脚本
# 自动创建Unity项目结构

param(
    [string]$ProjectPath = "C:\Users\PC\Desktop\项目文件夹\02_程序代码\client\叠印元素",
    [string]$UnityPath = "C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe"
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "叠印元素 - Unity项目创建" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# 1. 创建目录结构
Write-Host "[1/4] 创建目录结构..." -ForegroundColor Yellow

$dirs = @(
    $ProjectPath,
    "$ProjectPath\Assets",
    "$ProjectPath\Assets\Scenes",
    "$ProjectPath\Assets\Scripts",
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
        Write-Host "  创建: $dir" -ForegroundColor Gray
    } else {
        Write-Host "  跳过: $dir (已存在)" -ForegroundColor DarkGray
    }
}

Write-Host "  [OK] 目录结构创建完成" -ForegroundColor Green

# 2. 创建项目配置文件
Write-Host ""
Write-Host "[2/4] 创建项目配置文件..." -ForegroundColor Yellow

# ProjectVersion.txt
$projectVersion = @"
m_EditorVersion: 2022.3.62f1
m_EditorVersionWithRevision: 2022.3.62f1 (8f0d4a8d1e1c)
m_ConfigurationId: OfficialUnityInstaller
"@
Set-Content -Path "$ProjectPath\ProjectSettings\ProjectVersion.txt" -Value $projectVersion -Encoding UTF8

# PackageManager\ProjectConfig
$pinnedPlatforms = @()
$projectConfig = @{
    "dependencies" = @{}
    scopedRegistries = @()
} | ConvertTo-Json -Depth 10

Set-Content -Path "$ProjectPath\Packages\packages-lock.json" -Value $projectConfig -Encoding UTF8

$manifest = @{
    "dependencies" = @{
        "com.unity.collab-proxy" = "2.3.1"
        "com.unity.feature.development" = "1.0.1"
        "com.unity.textmeshpro" = "3.0.6"
        "com.unity.timeline" = "1.7.6"
        "com.unity.ugui" = "1.0.0"
        "com.unity.visualscripting" = "1.9.1"
        "com.unity.modules.ai" = "1.0.0"
        "com.unity.modules.androidjni" = "1.0.0"
        "com.unity.modules.animation" = "1.0.0"
        "com.unity.modules.assetbundle" = "1.0.0"
        "com.unity.modules.audio" = "1.0.0"
        "com.unity.modules.cloth" = "1.0.0"
        "com.unity.modules.director" = "1.0.0"
        "com.unity.modules.imageconversion" = "1.0.0"
        "com.unity.modules.imgui" = "1.0.0"
        "com.unity.modules.jsonserialize" = "1.0.0"
        "com.unity.modules.particlesystem" = "1.0.0"
        "com.unity.modules.physics" = "1.0.0"
        "com.unity.modules.physics2d" = "1.0.0"
        "com.unity.modules.screencapture" = "1.0.0"
        "com.unity.modules.terrain" = "1.0.0"
        "com.unity.modules.terrainphysics" = "1.0.0"
        "com.unity.modules.tilemap" = "1.0.0"
        "com.unity.modules.ui" = "1.0.0"
        "com.unity.modules.uielements" = "1.0.0"
        "com.unity.modules.umbra" = "1.0.0"
        "com.unity.modules.unityanalytics" = "1.0.0"
        "com.unity.modules.unitywebrequest" = "1.0.0"
        "com.unity.modules.unitywebrequestassetbundle" = "1.0.0"
        "com.unity.modules.unitywebrequestaudio" = "1.0.0"
        "com.unity.modules.unitywebrequesttexture" = "1.0.0"
        "com.unity.modules.unitywebrequestwww" = "1.0.0"
        "com.unity.modules.vehicles" = "1.0.0"
        "com.unity.modules.video" = "1.0.0"
        "com.unity.modules.vr" = "1.0.0"
        "com.unity.modules.wind" = "1.0.0"
        "com.unity.modules.xr" = "1.0.0"
    }
} | ConvertTo-Json -Depth 10

Set-Content -Path "$ProjectPath\Packages\manifest.json" -Value $manifest -Encoding UTF8

Write-Host "  [OK] 项目配置文件创建完成" -ForegroundColor Green

# 3. 创建默认场景
Write-Host ""
Write-Host "[3/4] 创建默认场景..." -ForegroundColor Yellow

$defaultScene = @"
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
--- !u!104 &2
RenderSettings:
  m_ObjectHideFlags: 0
  serializedVersion: 9
  m_Fog: 0
  m_FogColor: {r: 0.5, g: 0.5, b: 0.5, a: 1}
  m_FogMode: 3
  m_FogDensity: 0.01
  m_LinearFogStart: 0
  m_LinearFogEnd: 300
  m_AmbientSkyColor: {r: 0.212, g: 0.227, b: 0.259, a: 1}
  m_AmbientEquatorColor: {r: 0.114, g: 0.125, b: 0.133, a: 1}
  m_AmbientGroundColor: {r: 0.047, g: 0.043, b: 0.035, a: 1}
  m_AmbientIntensity: 1
  m_AmbientMode: 0
  m_SubtractiveShadowColor: {r: 0.42, g: 0.478, b: 0.627, a: 1}
  m_SkyboxMaterial: {fileID: 10304, guid: 0000000000000000f000000000000000, type: 0}
  m_HaloStrength: 0.5
  m_FlareStrength: 1
  m_FlareFadeSpeed: 3
  m_HaloTexture: {fileID: 0}
  m_SpotCookie: {fileID: 10001, guid: 0000000000000000e000000000000000, type: 0}
  m_DefaultReflectionMode: 0
  m_DefaultReflectionResolution: 128
  m_ReflectionBounces: 1
  m_ReflectionIntensity: 1
  m_CustomReflection: {fileID: 0}
  m_Sun: {fileID: 0}
  m_IndirectSpecularColor: {r: 0.18028378, g: 0.22571412, b: 0.30692285, a: 1}
  m_UseRadianceAmbientProbe: 0
--- !u!157 &3
LightmapSettings:
  m_ObjectHideFlags: 0
  serializedVersion: 12
  m_GIWorkflowMode: 1
  m_GISettings:
    serializedVersion: 2
    m_BounceScale: 1
    m_IndirectOutputScale: 1
    m_AlbedoBoost: 1
    m_EnvironmentLightingMode: 0
    m_EnableBakedLightmaps: 1
    m_EnableRealtimeLightmaps: 0
  m_LightmapEditorSettings:
    serializedVersion: 12
    m_Resolution: 2
    m_BakeResolution: 40
    m_AtlasSize: 1024
    m_AO: 0
    m_AOMaxDistance: 1
    m_CompAOExponent: 1
    m_CompAOExponentDirect: 0
    m_ExtractAmbientOcclusion: 0
    m_Padding: 2
    m_LightmapParameters: {fileID: 0}
    m_LightmapsBakeMode: 1
    m_TextureCompression: 1
    m_FinalGather: 0
    m_FinalGatherFiltering: 1
    m_FinalGatherRayCount: 256
    m_ReflectionCompression: 2
    m_MixedBakeMode: 2
    m_BakeBackend: 1
    m_PVRSampling: 1
    m_PVRDirectSampleCount: 32
    m_PVRSampleCount: 512
    m_PVRBounces: 2
    m_PVREnvironmentSampleCount: 256
    m_PVREnvironmentReferencePointCount: 2048
    m_PVRFilteringMode: 2
    m_PVRDenoiserTypeDirect: 0
    m_PVRDenoiserTypeIndirect: 0
    m_PVRDenoiserTypeAO: 0
    m_PVRFilterTypeDirect: 0
    m_PVRFilterTypeIndirect: 0
    m_PVRFilterTypeAO: 0
    m_PVREnvironmentMIS: 1
    m_PVRCulling: 1
    m_PVRFilteringGaussRadiusDirect: 1
    m_PVRFilteringGaussRadiusIndirect: 5
    m_PVRFilteringGaussRadiusAO: 2
    m_PVRFilteringAtrousPositionSigmaDirect: 0.5
    m_PVRFilteringAtrousPositionSigmaIndirect: 2
    m_PVRFilteringAtrousPositionSigmaAO: 1
    m_ExportTrainingData: 0
    m_TrainingDataDestination: TrainingData
    m_LightProbeSampleCountMultiplier: 4
  m_LightingDataAsset: {fileID: 0}
  m_LightingSettings: {fileID: 0}
--- !u!196 &4
NavMeshSettings:
  serializedVersion: 2
  m_ObjectHideFlags: 0
  m_BuildSettings:
    serializedVersion: 2
    agentTypeID: 0
    agentRadius: 0.5
    agentHeight: 2
    agentSlope: 45
    agentClimb: 0.4
    ledgeDropHeight: 0
    maxJumpAcrossDistance: 0
    minRegionArea: 2
    manualCellSize: 0
    cellSize: 0.16666667
    manualTileSize: 0
    tileSize: 256
    accuratePlacement: 0
    debug:
      m_Flags: 0
  m_NavMeshData: {fileID: 0}
--- !u!1 &852507328
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  serializedVersion: 6
  m_Component:
  - component: {fileID: 852507332}
  - component: {fileID: 852507331}
  - component: {fileID: 852507330}
  - component: {fileID: 852507329}
  m_Layer: 0
  m_Name: Main Camera
  m_TagString: MainCamera
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!81 &852507329
AudioListener:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 852507328}
  m_Enabled: 1
--- !u!92 &852507330
Behaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 852507328}
  m_Enabled: 1
--- !u!20 &852507331
Camera:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 852507328}
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
  m_StereoConvergence: 10
  m_StereoSeparation: 0.022
--- !u!4 &852507332
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 852507328}
  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}
  m_LocalPosition: {x: 0, y: 1, z: -10}
  m_LocalScale: {x: 1, y: 1, z: 1}
  m_ConstrainProportionsScale: 0
  m_Children: []
  m_Father: {fileID: 0}
  m_RootOrder: 0
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
--- !u!1 &962696323
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  serializedVersion: 6
  m_Component:
  - component: {fileID: 962696326}
  - component: {fileID: 962696325}
  - component: {fileID: 962696324}
  m_Layer: 0
  m_Name: Directional Light
  m_TagString: Untagged
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!114 &962696324
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 962696323}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: 7代表ab75a5be64e4e, type: 3}
  m_Name:
  m_EditorClassIdentifier:
  m_Type: 0
  m_Shape: 0
  m_Angle: 0.5
  m_Color: {r: 1, g: 0.95686275, b: 0.8392157, a: 1}
  m_Intensity: 1
  m_MainShadow: 1
  m_UseColorTemperature: 0
  m_ColorTemperature: 6570
  m_IndirectIntensity: 1
  m_ApplyToDeferredLights: 0
  m_RayTracingMask: 1
  m_SkipDeferredLights: 0
  m_LightLayerMask: 1
  m_CustomShadowLayers: 0
  m_ShadowLayerMask: 1
  m_ShadowDistance: 100
  m_ShadowResolution: 2048
  m_ShadowGeometry: 1
  m_ShadowRadius: 0
  m_ShadowAngle: 0
  m_ShadowSoftness: 0
  m_ShadowSoftnessLinear: 1
  m_ShadowNearPlane: 10
  m_ShadowCascadeCount: 4
  m_ShadowCascadeRatios:
  - 0.05
  - 0.1
  - 0.2
  - 0
  m_ShadowCascadeBorders:
  - 0.05
  - 0.05
  - 0.05
  - 0.05
  m_ShadowCascadeSplit: {x: 0.1, y: 0.2, z: 0.3, w: 1}
  m_ShadowDepthBias: 1
  m_ShadowNormalBias: 1
  m_ShadowCSMMatrices:
  - m_00: 0
    m_01: 0
    m_02: 0
    m_03: 0
    m_10: 0
    m_11: 0
    m_12: 0
    m_13: 0
    m_20: 0
    m_21: 0
    m_22: 0
    m_23: 0
    m_30: 0
    m_31: 0
    m_32: 0
    m_33: 0
  m_ShadowMaximumDistance: 1000
  m_ShadowCascade2Positive: {x: 0, y: 0, z: 0}
  m_ShadowCascade2Negative: {x: 0, y: 0, z: 0}
  m_ShadowCascade2Border: {x: 0, y: 0, z: 0}
  m_ShadowCascade2NickelResolution: 0
  m_ShadowCascade4Positive: {x: 0, y: 0, z: 0, w: 0}
  m_ShadowCascade4Border: {x: 0, y: 0, z: 0, w: 0}
  m_ShadowCascade4NickelResolution: 0
  m_UseCascade: 1
  m_UseReferencedCascade: 0
  m_ShadowRadius: 0
  m_ShadowAngle: 0
  m_DeprecatedShadow.radius: 0
  m_AltsWayPoint: 1
  m_ShadowBaked: 1
  m_ShadowCustomResolution: -1
  m_SpecularLightBounces: 1
  m_SpecularOcclusionLightBounces: 1
  m_SpecularOcclusionStrength: 1
  m_SkyboxBounces: 1
  m_FalloffBowlShape: 1
  m_FalloffBowlForward: {x: 0, y: 0, z: 1}
  m_FalloffBowlRadius: 5
  m_FalloffBowlFalloff: 2.56
  m_FalloffBowlCutoff: 0
  m_AreaBakedForward: 1
  m_AreaBakedForwardSize: 10
  m_AreaBakedForwardArea: 180
  m_AreaShadowsBaked: 1
  m_AreaShadowOnly: 0
--- !u!20 &962696325
Light:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 962696323}
  m_Enabled: 1
  serializedVersion: 10
  m_Type: 1
  m_Shape: 0
  m_Color: {r: 1, g: 1, b: 1, a: 1}
  m_Intensity: 1
  m_LightCookie: {fileID: 0}
  m_LightCookieSize: 10
  m_LightCookieOffset: {x: 0, y: 0}
  m_Shadow:
    m_Type: 2
    m_Resolution: -1
    m_CustomResolution: -1
    m_Strength: 1
    m_Bias: 0.05
    m_NormalBias: 0.4
    m_NearPlane: 0.2
    m_CullingMatrixOverride:
      m_00: 1
      m_01: 0
      m_02: 0
      m_03: 0
      m_10: 0
      m_11: 1
      m_12: 0
      m_13: 0
      m_20: 0
      m_21: 0
      m_22: 1
      m_23: 0
      m_30: 0
      m_31: 0
      m_32: 0
      m_33: 1
    m_UseCullingMatrixOverride: 0
    m_Culling:
      m_ShadowCullingEnabled: 0
      m_InsideShadowShadowDistance: 100
      m_ShadowDistanceNoCull: 500
      m_ShadowNearPlane: 0.1
    m_LightLayers: 1
    m_LightLayerMask: 1
    m_ShadowLayerMask: 1
    m_Shadow掏出一个: 0
    m_Shadow掏出一个Reverse: 0
  m_AreaBakedShadowResolution: 1024
  m_AreaRealTimeShadowResolution: 512
  m_ShadowBlend: 0
  m_ShadowRadius: 0
  m_ShadowAngle: 0
  m_ShadowLength: 0.5
  m_Coormal阴: 1
  m_Workaround: 0
  m_UseBoundingSphereOverride: 0
  m_BoundingSphereOverride:
    serializedVersion: 2
    m_Element: 0
  m_UseOcclusionMask: 0
  m_OcclusionMask:
    m_Bits: 0
  m_Albedo: 1
  m_InternalCookie: {fileID: 0}
  m_Cookies: {fileID: 8400000, guid: 0000000000000000e000000000000000, type: 2}
  m_DontCook: 0
  m_BakingWorldBakingSet: 1
  m_ShadowLayer: 0
  m_ShowShadowRadius: 1
  m_ShowCookieSize: 1
  m_ShowFalloff: 1
  m_ShowShadowAreaLight_Forward: 1
  m_ShowShadowAreaLight_Point: 1
  m_ShowShadowAreaLight_Spot: 1
  m_ShowShadowAreaLight_Rect: 1
--- !u!4 &962696326
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 962696323}
  m_LocalRotation: {x: 0.40821788, y: -0.23456968, z: 0.10938163, w: 0.8754261}
  m_LocalPosition: {x: 0, y: 3, z: 0}
  m_LocalScale: {x: 1, y: 1, z: 1}
  m_ConstrainProportionsScale: 0
  m_Children: []
  m_Father: {fileID: 0}
  m_RootOrder: 1
  m_LocalEulerAnglesHint: {x: 50, y: -30, z: 0}
--- !u!1 &2024686921
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  serializedVersion: 6
  m_Component:
  - component: {fileID: 2024686924}
  - component: {fileID: 2024686923}
  m_Layer: 0
  m_Name: EventSystem
  m_TagString: Untagged
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!114 &2024686923
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 2024686921}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: 7代表7f7562a3b6414a8, type: 3}
  m_Name:
  m_EditorClassIdentifier:
  m_SendPointerHoverToParent: 1
--- !u!4 &2024686924
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 2024686921}
  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}
  m_LocalPosition: {x: 0, y: 0, z: 0}
  m_LocalScale: {x: 1, y: 1, z: 1}
  m_ConstrainProportionsScale: 0
  m_Children: []
  m_Father: {fileID: 0}
  m_RootOrder: 2
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
--- !u!1 &2147483647
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  serializedVersion: 6
  m_Component:
  - component: {fileID: 2147483646}
  - component: {fileID: 2147483645}
  m_Layer: 0
  m_Name: UI Canvas
  m_TagString: Untagged
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!224 &2147483646
RectTransform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 2147483647}
  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}
  m_LocalPosition: {x: 0, y: 0, z: 0}
  m_LocalScale: {x: 0, y: 0, z: 0}
  m_ConstrainProportionsScale: 0
  m_Children:
  - {fileID: 962696326}
  - {fileID: 852507332}
  - {fileID: 2024686924}
  m_Father: {fileID: 0}
  m_RootOrder: 3
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
  m_AnchorMin: {x: 0, y: 0}
  m_AnchorMax: {x: 0, y: 0}
  m_AnchoredPosition: {x: 0, y: 0}
  m_SizeDelta: {x: 0, y: 0}
  m_Pivot: {x: 0, y: 0}
--- !u!223 &2147483645
Canvas:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 2147483647}
  m_Enabled: 1
  serializedVersion: 3
  m_RenderMode: 0
  m_Camera: {fileID: 0}
  m_PlaneDistance: 100
  m_PixelPerfect: 0
  m_ReceivesEvents: 1
  m_OverrideSorting: 0
  m_OverridePixelPerfect: 0
  m_SortingBucketNormalizedSize: 0
  m_AdditionalShaderChannelsFlag: 25
  m_SortingLayerID: 0
  m_SortingOrder: 0
  m_TargetDisplay: 0
"@

# 简化版本 - 最小Scene
$simpleScene = "%YAML 1.1
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
--- !u!104 &2
RenderSettings:
  m_ObjectHideFlags: 0
  serializedVersion: 9
  m_Fog: 0
  m_FogColor: {r: 0.5, g: 0.5, b: 0.5, a: 1}
  m_FogMode: 3
  m_FogDensity: 0.01
  m_LinearFogStart: 0
  m_LinearFogEnd: 300
  m_AmbientSkyColor: {r: 0.212, g: 0.227, b: 0.259, a: 1}
  m_AmbientEquatorColor: {r: 0.114, g: 0.125, b: 0.133, a: 1}
  m_AmbientGroundColor: {r: 0.047, g: 0.043, b: 0.035, a: 1}
  m_AmbientIntensity: 1
  m_AmbientMode: 0
  m_SubtractiveShadowColor: {r: 0.42, g: 0.478, b: 0.627, a: 1}
  m_SkyboxMaterial: {fileID: 10304, guid: 0000000000000000f000000000000000, type: 0}
  m_HaloStrength: 0.5
  m_FlareStrength: 1
  m_FlareFadeSpeed: 3
  m_HaloTexture: {fileID: 0}
  m_SpotCookie: {fileID: 10001, guid: 0000000000000000e000000000000000, type: 0}
  m_DefaultReflectionMode: 0
  m_DefaultReflectionResolution: 128
  m_ReflectionBounces: 1
  m_ReflectionIntensity: 1
  m_CustomReflection: {fileID: 0}
  m_Sun: {fileID: 0}
  m_IndirectSpecularColor: {r: 0.18028378, g: 0.22571412, b: 0.30692285, a: 1}
  m_UseRadianceAmbientProbe: 0
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
  m_StereoConvergence: 10
  m_StereoSeparation: 0.022
--- !u!81 &8100000
AudioListener:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 100000}
  m_Enabled: 1
"

Set-Content -Path "$ProjectPath\Assets\Scenes\SampleScene.unity" -Value $simpleScene -Encoding UTF8

Write-Host "  [OK] 默认场景创建完成" -ForegroundColor Green

# 4. 创建初始C#脚本
Write-Host ""
Write-Host "[4/4] 创建初始脚本..." -ForegroundColor Yellow

$initialScript = @"
// 叠印元素 - 初始脚本
// Created by Hanako

using UnityEngine;

namespace OverlayElements
{
    /// <summary>
    /// 游戏入口点
    /// </summary>
    public class GameEntry : MonoBehaviour
    {
        [Header("游戏配置")]
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
                Debug.Log($"[叠印元素] 游戏启动 - 版本 {gameVersion}");
            }
        }

        private void InitializeGame()
        {
            // 初始化游戏系统
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 1;

            if (enableDebugMode)
            {
                Debug.Log("[叠印元素] 游戏系统初始化完成");
            }
        }
    }
}
"@

$scriptPath = "$ProjectPath\Assets\Scripts\GameEntry.cs"
Set-Content -Path $scriptPath -Value $initialScript -Encoding UTF8

Write-Host "  [OK] 初始脚本创建完成" -ForegroundColor Green

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Unity项目创建完成!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "项目路径: $ProjectPath" -ForegroundColor Cyan
Write-Host ""
Write-Host "下一步:" -ForegroundColor Yellow
Write-Host "  1. 双击运行此脚本重新初始化" -ForegroundColor Gray
Write-Host "  2. 或使用Unity Hub打开项目" -ForegroundColor Gray
Write-Host ""
