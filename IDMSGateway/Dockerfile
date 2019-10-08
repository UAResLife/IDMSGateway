FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["IDMSGateway/IDMSGateway.csproj", "IDMSGateway/"]
RUN dotnet restore "IDMSGateway/IDMSGateway.csproj"
COPY . .
WORKDIR "/src/IDMSGateway"
RUN dotnet build "IDMSGateway.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IDMSGateway.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IDMSGateway.dll"]