using FantasyItemGenerator;


ItemGenerator generator = new ItemGenerator();
generator.ReadSettings(Path.Combine(Environment.CurrentDirectory, @"FantasyInput.toml"));
var items = generator.GenerateItem(3);
Console.WriteLine(String.Join(", ", items));


/*
IFileReader<TestObject> reader = new TomlFileReader<TestObject>();
var result = reader.ReadFile(@"D:\Workspaces\Visual Studio 2022 projects\FantasyItemGenerator\SimpleFileReader.Console\testinput.toml");
Console.WriteLine(result.Print());
*/