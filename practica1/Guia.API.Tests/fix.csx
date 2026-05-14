using System.IO;

var dir = "Guia.API.Tests/Controllers";
foreach (var file in Directory.GetFiles(dir, "*.cs"))
{
    var text = File.ReadAllText(file);
    text = text.Replace("[AllureXunit]", "[Fact]");
    File.WriteAllText(file, text);
}
