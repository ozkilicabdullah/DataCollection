FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["SwEpApi/SwEpApi.csproj", "SwEpApi/"]
RUN dotnet restore "SwEpApi/SwEpApi.csproj"
COPY . .
WORKDIR "/src/SwEpApi"
RUN dotnet build "SwEpApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SwEpApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SwEpApi.dll"]