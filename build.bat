@echo off
dotnet build "%~dp0Terkoiz.Skipper.csproj" -c Release %*
exit /b %errorlevel%
