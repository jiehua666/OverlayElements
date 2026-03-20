@echo off
chcp 65001 >nul
echo ========================================
echo 叠印元素 - Unity 项目创建脚本
echo ========================================
echo.

set UNITY_PATH=C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe
set PROJECT_PATH=C:\Users\PC\Desktop\项目文件夹\02_程序代码\client\叠印元素

echo [1/3] 检查Unity安装...
if exist "%UNITY_PATH%" (
    echo [OK] Unity路径正确
) else (
    echo [ERROR] Unity未找到: %UNITY_PATH%
    pause
    exit /b 1
)

echo.
echo [2/3] 创建项目目录...
if exist "%PROJECT_PATH%" (
    echo [跳过] 项目已存在
) else (
    mkdir "%PROJECT_PATH%"
    echo [OK] 目录创建完成
)

echo.
echo [3/3] 启动Unity创建项目...
echo 项目路径: %PROJECT_PATH%
echo.

REM 使用Unity批量模式创建项目
start "" "%UNITY_PATH%" -createProject "%PROJECT_PATH%" -quit

echo.
echo Unity已启动，正在创建项目...
echo 等待完成后按任意键继续...
pause
