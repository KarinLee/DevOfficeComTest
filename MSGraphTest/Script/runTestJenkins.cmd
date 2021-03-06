@ECHO OFF
if "%*"=="/?" (
 echo Usage: runTest.cmd [Options]
 echo Options:
 echo.
 echo [/Browser:^<Browser Name^>]
 echo The browser used to access the site. The value should be one of following: IE32, IE64, Chrome, Firefox.
 echo.
 echo [/MSGraphBaseAddress:^<Base Url^>]
 echo The root url of the site under test.
 echo.
 echo [/DefaultWaitTime:^<Wait Time^>]
 echo The default wait time when finding element. 
 rem echo.
 rem echo [/ScreenShotSavePath:^<Save Path^>]
 rem echo An absolute or relative path to save the screenshot file.
 echo.
 echo [/Tests:^<Test Case Name[,Test Case Name]...^>]
 echo Run tests with names that match the provided values.
 echo If this option is present, all other options to choose tests should be ignored.
 echo More details see vstest.console.exe /?
 echo.
 echo [/TestCaseFilter:^<Expression^>]
 echo Run tests that match the given expression.
 echo If this option is present, all other options to choose tests should be ignored.
 echo ^<Expression^> is of the format ^<property^>Operator^<value^>[^|^&^<Expression^>]
 echo More details see vstest.console.exe /?
 echo.
 echo [/PlayList:^<PlayList File Name^>]
 echo A playList file which contains a list of tests to be run.
 echo If this option is present, all other options to choose tests should be ignored.
 echo.
 echo Note:
 echo      Each option should be separated with blank space.
 echo      All the options could be ignored. 
 echo      If the options appeared in App.config file are ignored, the default value in App.config file will be used.
 echo      If all the options to choose tests are ignored, all the tests will be run by default.
 echo.
 echo Examples:
 echo      To run tests in playlist file Graph-BVTs.playlist with BaseAddress set to http://graph.microsoft.io
 echo        ^>runTest.cmd /MSGraphBaseAddress:http://graph.microsoft.io /PlayList:MSGraphTest\Graph-BVTs.playlist
 echo      To run tests which test name contains "TC01" with DefaultWaitTime set to 30
 echo        ^>runTest.cmd /DefaultWaitTime:30 /TestCaseFilter:Name~TC01
 goto end
)
cd ..
IF EXIST .\TestFramework\_App.config DEL .\TestFramework\_App.config
FOR /F "delims=" %%I IN (.\TestFramework\App.config) DO (
set str=%%I
SETLOCAL ENABLEDELAYEDEXPANSION
set flag=0
FOR %%a IN (%*) DO (
set te=%%a
IF /i "!te:~1,7!"=="Browser" (
 set flag=1
 set browser=!te:~9!
 )
IF /i "!te:~1,18!"=="MSGraphBaseAddress" (
 set flag=1
 set url=!te:~20!
 )
IF /i "!te:~1,18!"=="ScreenShotSavePath" (
 set flag=1
rem set savePath=!te:~20!
 )
IF /i "!te:~1,15!"=="DefaultWaitTime" (
 set flag=1
 set waitTime=!te:~17!
 )
)
IF "!flag!"=="0" (
 endlocal
 goto TestRun
)

for /f tokens^=1^,2^,3^,4*^ delims^=^" %%J in ("%%I") do (
 set te=%%L
 IF /i "!te:~1,5!"=="value" (
  set flag2=0
  IF /i "%%K"=="Browser" (
   IF defined browser (
    set flag2=1
    echo %%J"%%K"%%L"!browser!"%%N>>.\TestFramework\_App.config
   )
  )
  IF /i "%%K"=="MSGraphBaseAddress" (
   if defined url (
    set flag2=1
    echo %%J"%%K"%%L"!url!"%%N>>.\TestFramework\_App.config
   )
  )
  IF /i "%%K"=="ScreenShotSavePath" (
   if defined savePath (
    set flag2=1
    echo %%J"%%K"%%L"!savePath!"%%N>>.\TestFramework\_App.config
   )
  )
  IF /i "%%K"=="DefaultWaitTime" (
   if defined waitTime (
    set flag2=1
    echo %%J"%%K"%%L"!waitTime!"%%N>>.\TestFramework\_App.config
   )
  )
  IF "!flag2!"=="0" (
    echo !str!>>.\TestFramework\_App.config
  )
 ) else (
  ECHO !str!>>.\TestFramework\_App.config
 )
)
endlocal
)

DEL .\TestFramework\App.config
rename .\TestFramework\_App.config App.config
"%MSBuildV14%" .\MSGraphTest.sln /t:Rebuild /clp:ErrorsOnly /v:m

:TestRun
SETLOCAL ENABLEDELAYEDEXPANSION
set flag3=0
FOR %%b IN (%*) DO (
set te=%%b
IF /i "!te:~1,8!"=="PlayList" (
 set playList=!te:~10!
 )
IF /i "!te:~1,5!"=="Tests" (
 set flag3=1
 set testCases=!te!
 )
IF /i "!te:~1,14!"=="TestCaseFilter" (
 set testFilter=!te!
 )
if "!flag3!"=="1" (
 if not "!te:~0,1!"=="/" (
   set testCases=!testCases!,!te!
  )
 )
)
if defined testFilter (
 "%VSTest%" .\MSGraphTest\bin\Debug\MSGraphTest.dll /logger:trx !testFilter!
) else if defined testCases (
 "%VSTest%" .\MSGraphTest\bin\Debug\MSGraphTest.dll /logger:trx !testCases!
) else if defined playList (
 set tests=/Tests:
 for /f "delims=" %%c IN (!playList!) DO (
  for %%d in (%%c) do (
   set te=%%d
   if "!te:~1,11!"=="MSGraphTest" (
    set tests=!tests!!te:~1,-1!,
   )
  )
 )
 "%VSTest%" .\MSGraphTest\bin\Debug\MSGraphTest.dll /logger:trx !tests:~0,-1!
) else (
 "%VSTest%" .\MSGraphTest\bin\Debug\MSGraphTest.dll /logger:trx
)
endlocal

cd .\Script
:end
PAUSE
@ECHO ON