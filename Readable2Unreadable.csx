#r "nuget: YiJingFramework.Annotating, 2.1.1"

using System.Text.Json;
using YiJingFramework.Annotating;

void Do(DirectoryInfo i, DirectoryInfo o)
{
    foreach (var file in i.GetFiles())
    {
        var outFilePath = Path.Combine(o.FullName, file.Name);
        if (file.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
        {
            using var fileIn = file.OpenRead();
            var store = JsonSerializer.Deserialize<AnnotationStore>(fileIn);

            using var fileOut = File.Create(outFilePath);
            JsonSerializer.Serialize(fileOut, store);
        }
        else
        {
            File.Copy(file.FullName, outFilePath, true);
        }
        Console.WriteLine($"{file.FullName} -> {outFilePath}");
    }
    foreach (var dir in i.EnumerateDirectories())
    {
        Do(dir, o.CreateSubdirectory(dir.Name));
    }
}

var inDir = new DirectoryInfo("./readable");
var outDir = new DirectoryInfo("./unreadable");

if (!inDir.Exists)
    return;

outDir.Create();
Do(inDir, outDir);