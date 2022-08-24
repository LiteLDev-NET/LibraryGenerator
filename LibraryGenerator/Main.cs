using System.IO;

new GeneratingUnit()
{
    InputPath = Path.Combine(Directory.GetCurrentDirectory(), "SDK", "Header"),
    Module = "LiteLoader"
}.Run();

new GeneratingUnit()
{
    InputPath = Path.Combine(Directory.GetCurrentDirectory(), "SDK", "Header", "MC"),
    Module = "Minecraft"
}.Run();

new GeneratingUnit()
{
    InputPath = Path.Combine(Directory.GetCurrentDirectory(), "SDK", "Header", "PERM"),
    Module = "Permission"
}.Run();
