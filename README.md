simpleparser
============

SimpleParser - parsing command line arguments made simple.<br>
Just parse no sets etc. (maybe in the future)<br>
Simple: ```b -b --bool``` does work for bool arguments as well as ```f <filename> -f <filename> --file <fileName>``` works for parameters.<br>
For multiple strings use the ```ListOption``` attribute which will return a ```List<string>``` for ```Separator```-separated strings. _Default: comma separated._


# Option attributes:
---
```[Option("ShortName", "LongName", "DefaultValue")]``` <br>
Main attribute for returning parameters after ```ShortName``` or ```LongName```. Optionally specify a default value.

```[BoolOption("ShortName", "Index")``` <br>
Returns true/false if ```ShortName``` exists. Optionally you can specify an ```Index``` in ```args[]``` to meet.

```[IndexOption(Index)]``` <br>
Returns ```args[Index]```.

```[ListOption('Separator')]``` <br>
Special Option for ```List<string>``` properties.


# Usage:
---

Simply create a class called ```Options``` for example and set its properties attributes like this:

``` 
    class Options
    {
        [BoolOption("v", "verbose")]
        public bool Verbose { get; set; }

        [Option("o", "outdir")]
        public string OutputDir { get; set; }

        [ListOption("f", "files")]
        public List<string> Files { get; set; }

        [IndexOption(0)]
        public string First { get; set; }
    }
```

Now in your code run: 

```
Options options = new Options();
X_ToolZ.SimpleParser.Parse(args, options);
```

and your options instance's properties should be populated.

Have fun!

# Contact me:
---
MemphiZ AT X-ToolZ DOT com<br>
Web: <a href=http://www.X-ToolZ.com>X-ToolZ.com</a> (under construction)<br>
Twitter: <a href=https://twitter.com/xtoolz>@XToolZ</a><br>
