@echo off
REM Variáveis
set PROJECT_KEY=CashFlow
set SONAR_HOST_URL=http://localhost:9000
set SONAR_TOKEN=sqp_33956b2cd3d845a7d99260049814f42c8e2458c1

REM Iniciar o SonarScanner
dotnet sonarscanner begin /k:"%PROJECT_KEY%" /d:sonar.host.url="%SONAR_HOST_URL%" /d:sonar.token="%SONAR_TOKEN%"

REM Construir o projeto
dotnet build

REM Finalizar o SonarScanner
dotnet sonarscanner end /d:sonar.token="%SONAR_TOKEN%"
