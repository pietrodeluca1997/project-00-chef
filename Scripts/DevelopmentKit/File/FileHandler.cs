using Godot;
using System;

namespace Project00ChefGame.Scripts.DevelopmentKit.File;

public static class FileHandler
{
    private const string ErrorMessageFormat = "Error when trying to load file from path: '{0}'...";

    public static string ReadFileText(string filePath)
    {
        try
        {
            FileAccess fileAccess = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);

            if (fileAccess is null)
            {
                GD.PrintErr(string.Format(ErrorMessageFormat, FileAccess.GetOpenError()));

                return string.Empty;
            }

            return fileAccess.GetAsText();
        }
        catch (Exception ex)
        {
            GD.PrintErr(string.Format(ErrorMessageFormat, ex.Message));
            throw;
        }
    }
}
