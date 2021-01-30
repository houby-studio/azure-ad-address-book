#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["azure-ad-address-book.csproj", ""]
RUN dotnet restore "./azure-ad-address-book.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "azure-ad-address-book.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "azure-ad-address-book.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "azure-ad-address-book.dll"]