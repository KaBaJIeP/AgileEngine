FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as restore
WORKDIR /app

COPY ./NuGet.config ./

COPY ./src/AE.ImageGallery.Domain/AE.ImageGallery.Domain.csproj ./AE.ImageGallery.Domain/AE.ImageGallery.Domain.csproj
COPY ./src/AE.ImageGallery.Application/AE.ImageGallery.Application.csproj ./AE.ImageGallery.Application/AE.ImageGallery.Application.csproj
COPY ./src/AE.ImageGallery.Infrastructure/AE.ImageGallery.Infrastructure.csproj ./AE.ImageGallery.Infrastructure/AE.ImageGallery.Infrastructure.csproj
COPY ./src/AE.ImageGallery.PublicApi.Contracts/AE.ImageGallery.PublicApi.Contracts.csproj ./AE.ImageGallery.PublicApi.Contracts/AE.ImageGallery.PublicApi.Contracts.csproj
COPY ./src/AE.ImageGallery.PublicApi/AE.ImageGallery.PublicApi.csproj ./AE.ImageGallery.PublicApi/AE.ImageGallery.PublicApi.csproj

RUN ls
RUN ls ./AE.ImageGallery.PublicApi
RUN dotnet restore ./AE.ImageGallery.PublicApi/AE.ImageGallery.PublicApi.csproj

FROM restore as build
WORKDIR /app

COPY ./src/AE.ImageGallery.Domain/ ./AE.ImageGallery.Domain/
COPY ./src/AE.ImageGallery.Application/ ./AE.ImageGallery.Application/
COPY ./src/AE.ImageGallery.Infrastructure/ ./AE.ImageGallery.Infrastructure/
COPY ./src/AE.ImageGallery.PublicApi.Contracts/ ./AE.ImageGallery.PublicApi.Contracts/
COPY ./src/AE.ImageGallery.PublicApi/ ./AE.ImageGallery.PublicApi/

RUN dotnet build --no-restore ./AE.ImageGallery.PublicApi/AE.ImageGallery.PublicApi.csproj -c Release -o ./release

FROM build as publish
WORKDIR /app

RUN dotnet publish ./AE.ImageGallery.PublicApi/AE.ImageGallery.PublicApi.csproj -c Release -o ./release

FROM base as final
WORKDIR /app
COPY --from=publish /release .
ENTRYPOINT ["dotnet", "AE.ImageGallery.PublicApi.dll"]
