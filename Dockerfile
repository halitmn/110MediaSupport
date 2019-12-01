FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Support110Media/Support110Media.csproj", "Support110Media/"]
COPY ["Support110Media.Data/Support110Media.Data.csproj", "Support110Media.Data/"]
COPY ["Support110Media.Utils/Support110Media.Utils.csproj", "Support110Media.Utils/"]
COPY ["Support110Media.DataAccess/Support110Media.DataAccess.csproj", "Support110Media.DataAccess/"]
RUN dotnet restore "Support110Media/Support110Media.csproj"
COPY . .
WORKDIR "/src/Support110Media"
RUN dotnet build "Support110Media.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Support110Media.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Support110Media.dll"]