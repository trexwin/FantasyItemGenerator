using FantasyItemGenerator;
using SimpleFileReader;
using SimpleFileReader.Console.Data;
using SimpleFileReader.Implementations;


ItemGenerator generator = new ItemGenerator();
generator.GenerateItem("");
/*
IFileReader<TestObject> reader = new TomlFileReader<TestObject>();
var result = reader.ReadFile(@"D:\Workspaces\Visual Studio 2022 projects\FantasyItemGenerator\SimpleFileReader.Console\testinput.toml");
Console.WriteLine(result.Print());
*/