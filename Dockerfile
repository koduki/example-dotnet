# 1. ビルドステージ
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY *.sln .
COPY example-dotnet.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish "example-dotnet.csproj" -c Release -o /app/publish

# 2. 公開ステージ
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# コンテナ起動時に実行するコマンドを指定します。
ENTRYPOINT ["dotnet", "example-dotnet.dll"]