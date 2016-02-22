@ECHO OFF
if "%*"=="/?" (
 echo Usage: runTest.cmd [Options]
 echo Note:Each option should be separated with blank space.
 echo.
 echo Options:
 echo.
 echo [/Browser:^<Browser Name^>]
 echo The browser used to access the site. The value should be one of following: IE32, IE64, Chrome, Firefox.
 echo.
 echo [/BaseAddress:^<Base Url^>]
 echo The root url of the site under test.
 echo.
 echo [/ScreenShotSavePath:^<Save Path^>]
 echo An absolute or relative path to save the screenshot file.
 echo.
 echo [/DefaultWaitTime:^<Wait Time^>]
 echo Default wait time when finding element.
 echo.
 echo [/Tests:^<Test Case Name[,Test Case Name]...^>]
 echo Run tests with names that match the provided values.
 echo More details see vstest.console.exe /?
 echo.
 echo [/TestCaseFilter:^<Expression^>]
 echo Run tests that match the given expression.
 echo ^<Expression^> is of the format ^<property^>Operator^<value^>[^|^&^<Expression^>]
 echo More details see vstest.console.exe /?
 echo.
 echo [/PlayList:^<PlayList File Name^>]
 echo A playList file which contains a list of tests to be run.
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
if "!te:~1,7!"=="Browser" (
 set flag=1
 set browser=!te:~9!
 )
if "!te:~1,11!"=="BaseAddress" (
 set flag=1
 set url=!te:~13!
 )
if "!te:~1,18!"=="ScreenShotSavePath" (
 set flag=1
 set savePath=!te:~20!
 )
if "!te:~1,15!"=="DefaultWaitTime" (
 set flag=1
 set waitTime=!te:~17!
 )
)
if "!flag!"=="0" (
 endlocal
 goto TestRun
)

for /f tokens^=1^,2^,3^,4*^ delims^=^" %%J in ("%%I") do (
 set te=%%L
 if "!te:~1,5!"=="value" (
  set flag2=0
  if "%%K"=="Browser" (
   if defined browser (
    set flag2=1
    echo %%J"%%K"%%L"!browser!"%%N>>.\TestFramework\_App.config
   )
  )
  if "%%K"=="BaseAddress" (
   if defined url (
    set flag2=1
    echo %%J"%%K"%%L"!url!"%%N>>.\TestFramework\_App.config
   )
  )
  if "%%K"=="ScreenShotSavePath" (
   if defined savePath (
    set flag2=1
    echo %%J"%%K"%%L"!savePath!"%%N>>.\TestFramework\_App.config
   )
  )
  if "%%K"=="DefaultWaitTime" (
   if defined waitTime (
    set flag2=1
    echo %%J"%%K"%%L"!waitTime!"%%N>>.\TestFramework\_App.config
   )
  )
  if "!flag2!"=="0" (
    echo %%J"%%K"%%L"%%M"%%N>>.\TestFramework\_App.config
  )
 ) else (
  ECHO.!str!>>.\TestFramework\_App.config
 )
)
endlocal
)
DEL .\TestFramework\App.config
rename .\TestFramework\_App.config App.config

:TestRun
SETLOCAL ENABLEDELAYEDEXPANSION
set flag3=0
FOR %%b IN (%*) DO (
set te=%%b
if "!te:~1,8!"=="PlayList" (
 set playList=!te:~10!
 )
if "!te:~1,5!"=="Tests" (
 set flag3=1
 set testCases=!te!
 )
if "!te:~1,14!"=="TestCaseFilter" (
 set testFilter=!te!
 )
if "!flag3!"=="1" (
 if not "!te:~0,1!"=="/" (
   set testCases=!testCases!,!te!
  )
 )
)
if defined testFilter (
 vstest.console.exe .\Tests\bin\Debug\Tests.dll /logger:trx !testFilter!
) else if defined testCases (
 vstest.console.exe .\Tests\bin\Debug\Tests.dll /logger:trx !testCases!
) else if defined playList (
 set tests=/Tests:
 for /f "delims=" %%c IN (!playList!) DO (
  for %%d in (%%c) do (
   set te=%%d
   if "!te:~1,5!"=="Tests" (
    set tests=!tests!!te:~1,-1!,
   )
  )
 )
 vstest.console.exe .\Tests\bin\Debug\Tests.dll /logger:trx !tests:~0,-1!
) else (
 vstest.console.exe .\Tests\bin\Debug\Tests.dll /logger:trx
)
endlocal

cd .\Script
:end
PAUSE
@ECHO ON