FROM microsoft/dotnet-framework:4.7.2-sdk
ARG source
WORKDIR /app
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["C:\\app\\OddTwitterStreamer.exe"]
