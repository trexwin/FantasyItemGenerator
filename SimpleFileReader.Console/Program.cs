using FantasyItemGenerator;
using SimpleFileReader.Parser.Implementations;


var toml = new TomlParser();
var x = toml.LoadAndParse(Path.Combine(Environment.CurrentDirectory, @"testfile.toml"));


return;




Console.WriteLine(" = Welcome to FantasyItemGenerator = ");
Console.WriteLine("Write \"Help\" to view all commands.");
ItemGenerator generator = new ItemGenerator();
bool running = true;
while (running)
{
    Console.Write("> ");
    var input = Console.ReadLine()?.Trim() ?? string.Empty;
    string output;

    if (input == "Quit")
    {
        running = false;
        output = "Thanks for using FantasyItemGenerator!";
    }
    else if(input.StartsWith("Load "))
    {
        var path = input.Substring(5);
        try { 
            generator.ReadSettings(path);
            output = "File successfully loaded in.";
        } 
        catch(Exception e) 
        { output = $"Could not read specified file, got exception \"{e.Message}\""; }
    }
    else if(input.StartsWith("Generate "))
    {
        var amountInput = input.Substring(9);
        try
        {
            int amount = int.Parse(amountInput);
            if (amount < 0 ||  amount > 100)
            {
                output = "Can only generate between 0 and 100 items at a time.";
            }
            else
            {
                var items = generator.GenerateItem(amount);
                if (items == null)
                    output = "Please load a file beforehand.";
                else
                    output = string.Join('\n', items.Select(s => "- " + s));
            }

        }
        catch (FormatException)
        { output = $"Please provide a valid number as input, got \"{amountInput}\"."; }
    }
    else
    {
        // Simple inputs
        output = input switch
        {
            "Examples" => "The following example files are available:\n" +
                          string.Join('\n', generator.GetExampleFiles().Select(s => "- " + s)),
            "Help" =>
@"- Examples: Shows the location of the available example files.
- Load ARG: Tries to load the file located at ARG.
- Generate NUM: Generates NUM items if a file is loaded.
- Quit: Closes the application.",
            _ => $"Unknown command \"{input}\"."
        };
    }

    // Simple commands


    
    Console.WriteLine(output);
}
/*
ItemGenerator generator = new ItemGenerator();
generator.ReadSettings(Path.Combine(Environment.CurrentDirectory, "FantasyInput.toml"));
var items = generator.GenerateItem(10);
Console.WriteLine(String.Join('\n', items));

IFileReader<TestObject> reader = new TomlFileReader<TestObject>();
var result = reader.ReadFile(@"D:\Workspaces\Visual Studio 2022 projects\FantasyItemGenerator\SimpleFileReader.Console\testinput.toml");
Console.WriteLine(result.Print());
*/