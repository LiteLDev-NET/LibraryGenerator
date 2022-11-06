using System.IO;

new GeneratingUnit()
{
    InputPath = Path.Combine(Directory.GetCurrentDirectory(), "SDK", "include", "llapi", "mc"),
    Module = "Minecraft"
}.Run();

new GeneratingUnit()
{
    InputPath = Path.Combine(Directory.GetCurrentDirectory(), "SDK", "include", "llapi"),
    Module = "LiteLoader"
}.Run();

new GeneratingUnit()
{
    InputPath = Path.Combine(Directory.GetCurrentDirectory(), "SDK", "include", "llapi", "perm"),
    Module = "Permission"
}.Run();
