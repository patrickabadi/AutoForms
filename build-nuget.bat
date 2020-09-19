set config=Release
set platform=AnyCPU
set warnings=1591,1572,1573,1570,1000,1587

set buildargs=/p:Configuration="%config%" /p:Platform="%platform%" /p:NoWarn="%warnings%"

echo Building AutoForms...

msbuild AutoForms/AutoForms.csproj %buildargs%

echo Packaging NuGets...

nuget pack AutoForms/AutoForms.nuspec