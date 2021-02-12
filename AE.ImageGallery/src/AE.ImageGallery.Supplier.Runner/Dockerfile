FROM mcr.microsoft.com/dotnet/runtime:5.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 as restore
WORKDIR /app

COPY ./NuGet.config ./

COPY ./src/AE.ImageGallery.Supplier/AE.ImageGallery.Supplier.csproj ./AE.ImageGallery.Supplier/
COPY ./src/AE.ImageGallery.Supplier.Runner/AE.ImageGallery.Supplier.Runner.csproj ./AE.ImageGallery.Supplier.Runner/

RUN dotnet restore ./AE.ImageGallery.Supplier.Runner/AE.ImageGallery.Supplier.Runner.csproj

FROM restore as build
WORKDIR /app

COPY ./src/AE.ImageGallery.Supplier/ ./AE.ImageGallery.Supplier/
COPY ./src/AE.ImageGallery.Supplier.Runner/ ./AE.ImageGallery.Supplier.Runner/

# --no-restore doesn't work with 5.0 sdk - todo -> investigate
RUN dotnet build ./AE.ImageGallery.Supplier.Runner/AE.ImageGallery.Supplier.Runner.csproj -c Release -o ./release

FROM build as publish
WORKDIR /app

RUN dotnet publish ./AE.ImageGallery.Supplier.Runner/AE.ImageGallery.Supplier.Runner.csproj -c Release -o ./release

FROM base as final
WORKDIR /app
COPY --from=publish ./app/release .
ENTRYPOINT ["dotnet", "AE.ImageGallery.Supplier.Runner.dll"]
