FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["StoryGraph.Validation.Web/StoryGraph.Validation.Web.csproj", "StoryGraph.Validation.Web/"]
RUN dotnet restore "StoryGraph.Validation.Web/StoryGraph.Validation.Web.csproj"
COPY . .
WORKDIR "/src/StoryGraph.Validation.Web"
RUN dotnet build "StoryGraph.Validation.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StoryGraph.Validation.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoryGraph.Validation.Web.dll"]
