FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["StoryGraph.Web/StoryGraph.Web.csproj", "StoryGraph.Web/"]
RUN dotnet restore "StoryGraph.Web/StoryGraph.Web.csproj"
COPY . .
WORKDIR "/src/StoryGraph.Web"
RUN dotnet build "StoryGraph.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StoryGraph.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoryGraph.Web.dll"]
