FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app


# Modify the OpenSSL configuration
RUN sed -i '/\[openssl_init\]/a ssl_conf = ssl_sect' /etc/ssl/openssl.cnf
RUN printf "\n[ssl_sect]\nsystem_default = system_default_sect\n" >> /etc/ssl/openssl.cnf
RUN printf "\n[system_default_sect]\nMinProtocol = TLSv1.2\nCipherString = DEFAULT@SECLEVEL=0\n" >> /etc/ssl/openssl.cnf

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY WebApp/*.csproj ./WebApp/
RUN dotnet restore WebApp/COCOApp.csproj
COPY . .
WORKDIR "/src/WebApp"
RUN dotnet build "COCOApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "COCOApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "COCOApp.dll"]