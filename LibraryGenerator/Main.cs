/*
 * 
 * 将LLSDK放至SDK文件夹即可
 * 文件将生成于output文件夹
 * 
 */
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
