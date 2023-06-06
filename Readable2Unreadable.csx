#r "nuget: YiJingFramework.Annotating, 4.0.0"

using System.Text.Json;
using YiJingFramework.Annotating;

var skipList = new List<string>() {
    "2023-01-17-1.json"
};
var skipSet = new HashSet<string>(skipList);
if(skipSet.Count != skipList.Count)
{
    throw new Exception("改一下跳过的做法吧，有重名文件了。");
}

void Do(DirectoryInfo i, DirectoryInfo o)
{
    foreach (var file in i.GetFiles())
    {
        if (file.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
        {
            if(skipSet.Contains(file.Name))
                continue;

            var outFilePath = Path.Combine(o.FullName, file.Name);
            Console.WriteLine($"Trying {file.FullName} -> {outFilePath}");

            using var fileIn = file.OpenRead();
            var store = JsonSerializer.Deserialize<AnnotationStore>(fileIn);

            using var fileOut = File.Create(outFilePath);
            JsonSerializer.Serialize(fileOut, store);
        }
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