
Console.WriteLine("Program: Copy Code to MyDocuments Visual Studio location!");

IEnumerable<string>? filePaths = FindFiles();
if (filePaths == null || !filePaths.Any())
{
    Console.WriteLine("No files found to export.. Terminating program (press key to continue)");
    Console.ReadKey();
    return;
}

string exportPath = GetExportLocation();
if (string.IsNullOrEmpty(exportPath))
{
    Console.WriteLine($"Preferred export location not found.. VS2022 not installed?");
    Console.ReadKey();
    return;
}

foreach (var file in filePaths)
{
    string fileName = Path.GetFileName(file);
    Console.WriteLine($"Copying file {fileName} to {exportPath}");
    File.Copy(file, Path.Combine(exportPath, fileName), true);
}

Console.WriteLine($"Program finished! Press a key to close window..");
Console.ReadKey();

static IEnumerable<string> GetSnippetFilesFromDirectory(string currentPath)
{
    Console.WriteLine($"Finding files in {currentPath}");

    return Directory.GetFiles(currentPath, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".snippet", StringComparison.OrdinalIgnoreCase));
}

static IEnumerable<string>? FindFiles()
{
    string currentPath = Directory.GetCurrentDirectory();
    
    IEnumerable<string> filePaths = GetSnippetFilesFromDirectory(currentPath);
    if (filePaths is null) return null;
    
    if (!filePaths.Any())
    {
        DirectoryInfo? info = Directory.GetParent(currentPath);
        if (info is null) return filePaths;

        filePaths = GetSnippetFilesFromDirectory(info.FullName);
    }

    Console.WriteLine($"Found {filePaths.Count()} file(s)");
    return filePaths;
}

static string GetExportLocation()
{
    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    path = Path.Combine(path, @"Visual Studio 2022\Code Snippets\Visual C#\My Code Snippets");
    if (!Directory.Exists(path))
    {
        return string.Empty;
    }
    
    return path;
}