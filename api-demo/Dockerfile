FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
ARG buildconfig
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["api-demo.csproj", "./"]
RUN dotnet restore "./api-demo.csproj"
COPY . .
WORKDIR /src

RUN dotnet build "api-demo.csproj" -c Debug -o /app

FROM build AS publish
RUN dotnet publish "api-demo.csproj" -c Debug -o /app

FROM base AS final
WORKDIR /app

COPY --from=publish /app .

RUN     apt-get update && \
        apt-get install -y --no-install-recommends apt-utils && \
        apt-get install curl unzip procps mongodb -y && \
        curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /publish/vsdbg; 

ENTRYPOINT ["dotnet", "api-demo.dll"]
