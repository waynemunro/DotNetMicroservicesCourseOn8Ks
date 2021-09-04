git init
dotnet new gitignore
#git status
git add -A
git commit -m "initial commit"
git branch PlatformServiceAPI
git checkout PlatformServiceAPI
dotnet new webapi -n PlatformService --force
dotnet build .\PlatformService
git add -A
git commit -m "PlatformService Api initial commit"
cd .\PlatformService\
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet restore
git add -A
git commit -m "Nuget Package dependencies added commit"
#code .


