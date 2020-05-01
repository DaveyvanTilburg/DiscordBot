FROM mcr.microsoft.com/dotnet/core/runtime:3.1.3-buster-slim-arm32v7
COPY bin/Release/netcoreapp3.1/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "DiscordBot.dll"]