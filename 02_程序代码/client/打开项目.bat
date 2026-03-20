@echo off
chcp 65001 >nul
echo ========================================
echo 叠印元素 - 启动Unity项目
echo ========================================
echo.
echo 请确保以普通用户身份运行Unity
echo 如果Unity显示管理员权限警告，请选择"取消"
echo.

set UNITY_PATH=C:\Program Files\Unity\Hub\Editor\2022.3.62f3c1\Editor\Unity.exe
set PROJECT_PATH="C:\Users\PC\Desktop\项目文件夹\02_程序代码\client\叠印元素"

start "" "%UNITY_PATH%" -projectPath %PROJECT_PATH%

echo.
echo Unity已启动，正在初始化项目...
echo 首次打开可能需要几分钟来导入资源
echo 请耐心等待...
pause
