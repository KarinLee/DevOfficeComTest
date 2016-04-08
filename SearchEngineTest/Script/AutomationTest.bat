@ECHO OFF
REM This file is to invoke schtasks.exe to execute scheduled test.

pushd %~dp0
set tmpFile=temp.txt
PowerShell.exe -ExecutionPolicy ByPass .\GetConfigFileNode.ps1 'StartTime'>>temp.txt

SETLOCAL ENABLEDELAYEDEXPANSION
for /f "delims=" %%a IN (!tmpFile!) DO (
set startTime=%%a
)
schtasks /Create /F /SC Daily /ST:!startTime! /TR "cmd /c %~dp0\runTest.bat /Browser:Chrome" /TN "Test task"
endlocal
del temp.txt
pause
@ECHO ON