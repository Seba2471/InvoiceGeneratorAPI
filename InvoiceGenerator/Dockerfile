#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt update
RUN apt install -y libgdiplus
RUN ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so
RUN apt-get install -y --no-install-recommends zlib1g fontconfig libfreetype6 libx11-6 libxext6 libxrender1 wget gdebi
RUN wget https://github.com/wkhtmltopdf/wkhtmltopdf/releases/download/0.12.5/wkhtmltox_0.12.5-1.stretch_amd64.deb
RUN gdebi --n wkhtmltox_0.12.5-1.stretch_amd64.deb
RUN apt install libssl1.1
RUN ln -s /usr/local/lib/libwkhtmltox.so /usr/lib/libwkhtmltox.so
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["InvoiceGenerator/InvoiceGenerator.csproj", "InvoiceGenerator/"]
RUN dotnet restore "InvoiceGenerator/InvoiceGenerator.csproj"
COPY . .
WORKDIR "/src/InvoiceGenerator"
RUN dotnet build "InvoiceGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InvoiceGenerator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InvoiceGenerator.dll"]