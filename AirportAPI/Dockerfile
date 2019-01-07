FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["AirportAPI/AirportAPI.csproj", "AirportAPI/"]
RUN dotnet restore "AirportAPI/AirportAPI.csproj"
COPY . .
WORKDIR "/src/AirportAPI"
RUN dotnet build "AirportAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AirportAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AirportAPI.dll"]