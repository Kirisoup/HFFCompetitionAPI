dotnet new --list | findstr "kiriplugin" >nul
if %errorlevel% neq 0 (
    dotnet new install D:\dev\blackhff\template\Kirisoup.PluginTemplate
)
cd source
dotnet new kiriplugin -n %1
cd ..
dotnet sln add .\source\%1\%1.csproj