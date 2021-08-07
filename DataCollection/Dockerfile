FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["DataCollection/DataCollection.csproj", "DataCollection/"]
RUN dotnet restore "DataCollection/DataCollection.csproj"
COPY . .
WORKDIR "/src/DataCollection"
RUN dotnet build "DataCollection.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DataCollection.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DataCollection.dll"]