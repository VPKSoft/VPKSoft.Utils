::Usage: nugetme.bat SomeClassLibrary.csproj api_key SomeClassLibrary.1.0.0.1.nupkg
..\..\nuget\nuget.exe spec %1
echo "Edit the SomeClassLibrary.nuspec file not, then press enter..."
pause
..\..\nuget\nuget.exe pack -Prop Configuration=Release
..\..\nuget\nuget.exe push %3 %2 -Source https://api.nuget.org/v3/index.json
pause