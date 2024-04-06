Sharp CLI Parser
==========================
A simple, strongly typed .NET command line parser library.

This library is a port and upgrade of [great work of TeamCity](https://fclp.github.io/fluent-command-line-parser/)

[![Nuget Badge](https://img.shields.io/nuget/dt/SharpCLIParser.svg)](https://nuget.org/packages/SharpCLIParser)

	  
### Download

You can install using [NuGet](http://nuget.org/packages/SharpCLIParser/) via the command line
```
cmd> nuget install SharpCLIParser
```
Or use the Package Manager console in Visual Studio:
```
PM> SharpCLIParser SharpCLIParser
```

You also can use the dotnet cli
```
dotnet add package SharpCLIParser
```  
### Usage

```csharp
using SharpCLIParser;

public class ApplicationArguments
{
   public int RecordId { get; set; }
   public bool Silent { get; set; }
   public string NewValue { get; set; }
}

static void Main(string[] args)
{
   // create a generic parser for the ApplicationArguments type
   var p = new SharpCLIParser<ApplicationArguments>();

   // specify which property the value will be assigned too.
   p.Setup(arg => arg.RecordId)
    .As('r', "record") // define the short and long option name
    .Required(); // using the standard fluent Api to declare this Option as required.

   p.Setup(arg => arg.NewValue)
    .As('v', "value")
    .Required();

   p.Setup(arg => arg.Silent)
    .As('s', "silent")
    .SetDefault(false); // use the standard fluent Api to define a default value if non is specified in the arguments

   var result = p.Parse(args);

   if(result.HasErrors == false)
   {
      // use the instantiated ApplicationArguments object from the Object property on the parser.
      application.Run(p.Object);
   }
}
```

You can also use the non-generic Sharp Command Line Parser to capture values without creating a container class.
```csharp
static void Main(string[] args)
{
  var p = new SharpCLIParser();

  p.Setup<int>('r')
   .Callback(record => RecordID = record)
   .Required();

  p.Setup<string>('v')
   .Callback(value => NewValue = value)
   .Required();

  p.Setup<bool>('s', "silent")
   .Callback(silent => InSilentMode = silent)
   .SetDefault(false);

  p.Parse(args);
}
```
### Parser Option Methods

`.Setup<int>('r')` Setup an option using a short name, 

`.Setup<int>('r', "record")` or short and long name.

`.Required()` Indicate the option is required and an error should be raised if it is not provided.

`.Callback(val => Value = val)` Provide a delegate to call after the option has been parsed

`.SetDefault(int.MaxValue)` Define a default value if the option was not specified in the args

`.WithDescription("Execute operation in silent mode without feedback")` Specify a help description for the option


### Parsing To Collections

Many arguments can be collected as part of a list. Types supported are `string`, `int32`, `int64`, `double`, `bool`, `Uri`, `DateTime` and `Enum`

For example arguments such as

`--filenames C:\file1.txt C:\file2.txt "C:\other file.txt"`

can be automatically parsed to a `List<string>` using
```csharp
static void Main(string[] args)
{
   var p = new SharpCLIParser();

   var filenames = new List<string>();

   p.Setup<List<string>>('f', "filenames")
    .Callback(items => filenames = items);

   p.Parse(args);

   Console.WriteLine("Input file names:");

   foreach (var filename in filenames)
   {
      Console.WriteLine(filename);
   }
}
```
output:
```
Input file names
C:\file1.txt
C:\file2.txt
C:\other file.txt
```
### Enum support
Since v1.2.3 enum types are now supported. 
```csharp
[Flags]
enum Direction
{
	Up = 1,
	Left = 2,
	Down = 4,
	Right = 8,
}
```
```csharp
p.Setup<Direction>("direction")
 .Callback(d => direction = d);
```
To specify 'Left' direction either the text can be provided or the enum integer.
```
directions.exe --direction Left
directions.exe --direction 2
```

You can also collect multiple Enum values into a `List<TEnum>`

```csharp
List<Direction> direction;

p.Setup<List<Direction>>('d', "direction")
 .Callback(d => direction = d);
```

For example, specifiying 'Down' and 'Left' values
```
directions.exe --direction Down Left
directions.exe --direction 4 2
```
Enum Flags are also supported
```csharp
Direction direction;

p.Setup<Direction>("direction")
 .Callback(d => direction = d);

p.Parse(args);

Assert.IsFalse(direction.HasFlag(Direction.North));
Assert.IsTrue(direction.HasFlag(Direction.Left));
Assert.IsTrue(direction.HasFlag(Direction.Down));
Assert.IsFalse(direction.HasFlag(Direction.West));
```

And the generic `SharpCLIParser<T>` also supports enums.

```csharp
public class Args
{
   public Direction Direction { get;set; }
   public List<Direction> Directions { get;set; }
}
```
```csharp
var p = new SharpCLIParser<Args>();

p.Setup(args => args.Direction)
 .As('d', "direction");

p.Setup(args => args.Directions)
 .As("directions");
```

Nullable enums also are supported.

### Help Screen
You can setup any help arguments, such as -? or --help to print all parameters which have been setup, along with their descriptions to the console by using SetupHelp(params string[]).

For example:
```csharp
// sets up the parser to execute the callback when -? or --help is detected
parser.SetupHelp("?", "help")
      .Callback(text => Console.WriteLine(text));
```
You can also choose to display the formatted help screen text manually, so that you can display it under other circunstances.


For example:
```csharp
var parser = new SharpCLIParser<Args>();
	
parser.SetupHelp("?", "help")
      .Callback(text => Console.WriteLine(text));
	
// triggers the SetupHelp Callback which writes the text to the console
parser.HelpOption.ShowHelp(parser.Options);
```


### Supported Syntax
`[-|--|/][switch_name][=|:| ][value]`

Supports boolean names
```
example.exe -s  // enable
example.exe -s- // disabled
example.exe -s+ // enable
```
Supports combined (grouped) options
```
example.exe -xyz  // enable option x, y and z
example.exe -xyz- // disable option x, y and z
example.exe -xyz+ // enable option x, y and z
``` 