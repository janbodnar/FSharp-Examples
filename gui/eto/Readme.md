GUI examples with the Eto library  

`$ dotnet new -i "Eto.Forms.Templates::*"`  # installs Eto template pack  
`$ dotnet new etoapp --help`  # shows project creation options    

`app.fsproj`   

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <Compile Include="Program.fs" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Eto.Platform.Gtk" Version="2.5.0" />
  </ItemGroup>

</Project>
```  

