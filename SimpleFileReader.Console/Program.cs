


using SimpleFileReader;
using SimpleFileReader.Console;
using SimpleFileReader.Implementations;

IFileReader<TestObject> reader = new TomlFileReader<TestObject>();

var result = reader.ReadFile(@"D:\Workspaces\Visual Studio 2022 projects\FantasyItemGenerator\SimpleFileReader.Console\testinput.toml");

Console.WriteLine($"We got text {result.text}, number {result.number}, and dec {result.decimalnumber}");