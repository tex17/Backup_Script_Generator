@echo off
chcp 65001 >nul

setlocal enabledelayedexpansion

:: Устанавливаем переменные с путями к папкам
set "SOURCEDIR=C:\Users\MORF\Desktop\source"
set "BACKUPDIR=C:\Users\MORF\Desktop\backups"

:: Получаем текущую дату для создания новой подпапки
for /f "tokens=1-3 delims=. " %%a in ('date /t') do (
    set "DATESTR=%%c-%%b-%%a"
)

:: Проверяем, есть ли уже подпапка с текущей датой
if not exist "%BACKUPDIR%\!DATESTR!" (
    :: Создаем новую подпапку
    mkdir "%BACKUPDIR%\!DATESTR!"
)

:: Проходим по всем файлам в исходной папке
for %%a in ("%SOURCEDIR%\*.jpg") do (
    call :processfile "%%a"
)
for %%a in ("%SOURCEDIR%\*.jpeg") do (
    call :processfile "%%a"
)
for %%a in ("%SOURCEDIR%\*.bmp") do (
    call :processfile "%%a"
)
for %%a in ("%SOURCEDIR%\*.dcm") do (
    call :processfile "%%a"
)
for %%a in ("%SOURCEDIR%\*.raw") do (
    call :processfile "%%a"
)

echo Работа завершена успешно.
goto :eof

:processfile
set "FILENAME=%~n1"
set "FILEEXT=%~x1"

:: Проверяем, есть ли файл в бекапах
set "FOUND=0"
for /d %%b in ("%BACKUPDIR%\*") do (
    if exist "%%b\!FILENAME!!FILEEXT!" (
        set "FOUND=1"
        goto :nextfile
    )
)

:: Если файл не найден, копируем его в новую подпапку
if !FOUND! == 0 (
    copy "%~1" "%BACKUPDIR%\!DATESTR!" >nul
)

:nextfile
exit /b
