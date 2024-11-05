using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        string sourceDir = GetDirectoryPath("Введите путь к исходной папке: ");
        string backupDir = GetDirectoryPath("Введите путь к папке для бекапов: ");

        if (!Directory.Exists(sourceDir))
        {
            Console.WriteLine("Исходная папка не существует.");
            return;
        }

        if (!Directory.Exists(backupDir))
        {
            Console.WriteLine("Папка для бекапов не существует.");
            return;
        }

        try
        {
            CreateBackupScript(sourceDir, backupDir);
            Console.WriteLine("Скрипт успешно создан: backup_script.bat");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании скрипта: {ex.Message}");
        }
    }

    static string GetDirectoryPath(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    static void CreateBackupScript(string sourceDir, string backupDir)
    {
        string outputFile = "backup_script.bat";
        using (StreamWriter writer = new StreamWriter(outputFile))
        {
            writer.WriteLine("@echo off");
            writer.WriteLine("chcp 65001 >nul");
            writer.WriteLine();
            writer.WriteLine("setlocal enabledelayedexpansion");
            writer.WriteLine();
            writer.WriteLine(":: Устанавливаем переменные с путями к папкам");
            writer.WriteLine($"set \"SOURCEDIR={sourceDir}\"");
            writer.WriteLine($"set \"BACKUPDIR={backupDir}\"");
            writer.WriteLine();
            writer.WriteLine(":: Получаем текущую дату для создания новой подпапки");
            writer.WriteLine("for /f \"tokens=1-3 delims=. \" %%a in ('date /t') do (");
            writer.WriteLine("    set \"DATESTR=%%c-%%b-%%a\"");
            writer.WriteLine(")");
            writer.WriteLine();
            writer.WriteLine(":: Проверяем, есть ли уже подпапка с текущей датой");
            writer.WriteLine("if not exist \"%BACKUPDIR%\\!DATESTR!\" (");
            writer.WriteLine("    :: Создаем новую подпапку");
            writer.WriteLine("    mkdir \"%BACKUPDIR%\\!DATESTR!\"");
            writer.WriteLine(")");
            writer.WriteLine();
            writer.WriteLine(":: Проходим по всем файлам в исходной папке");
            string[] extensions = { "*.jpg", "*.jpeg", "*.bmp", "*.dcm", "*.raw" };
            foreach (string ext in extensions)
            {
                writer.WriteLine($"for %%a in (\"%SOURCEDIR%\\{ext}\") do (");
                writer.WriteLine("    call :processfile \"%%a\"");
                writer.WriteLine(")");
            }
            writer.WriteLine();
            writer.WriteLine("echo Работа завершена успешно.");
            writer.WriteLine("goto :eof");
            writer.WriteLine();
            writer.WriteLine(":processfile");
            writer.WriteLine("set \"FILENAME=%~n1\"");
            writer.WriteLine("set \"FILEEXT=%~x1\"");
            writer.WriteLine();
            writer.WriteLine(":: Проверяем, есть ли файл в бекапах");
            writer.WriteLine("set \"FOUND=0\"");
            writer.WriteLine("for /d %%b in (\"%BACKUPDIR%\\*\") do (");
            writer.WriteLine("    if exist \"%%b\\!FILENAME!!FILEEXT!\" (");
            writer.WriteLine("        set \"FOUND=1\"");
            writer.WriteLine("        goto :nextfile");
            writer.WriteLine("    )");
            writer.WriteLine(")");
            writer.WriteLine();
            writer.WriteLine(":: Если файл не найден, копируем его в новую подпапку");
            writer.WriteLine("if !FOUND! == 0 (");
            writer.WriteLine("    copy \"%~1\" \"%BACKUPDIR%\\!DATESTR!\" >nul");
            writer.WriteLine(")");
            writer.WriteLine();
            writer.WriteLine(":nextfile");
            writer.WriteLine("exit /b");
        }
    }
}