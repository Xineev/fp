using CommandLine;

namespace TagCloudConsoleClient
{
    public class Options
    {
        [Value(0, MetaName = "input",
            HelpText = "Path to the input file containing text.",
            Required = true)]
        public string InputFile { get; set; }

        [Value(1, MetaName = "output",
            HelpText = "Path for the output image (e.g., out.png).",
            Required = true)]
        public string OutputFile { get; set; }

        [Option("width", HelpText = "Canvas width (default is 1000).")]
        public int? Width { get; set; }

        [Option("height", HelpText = "Canvas height (default is 1000).")]
        public int? Height { get; set; }

        [Option("padding", HelpText = "Canvas padding (default if 50).")]
        public int? Padding { get; set; }

        [Option("bgcolor", HelpText = "Background color (name or #RRGGBB).")]
        public string BackgroundColor { get; set; }

        [Option("textcolor", HelpText = "Text color (name or #RRGGBB).")]
        public string TextColor { get; set; }

        [Option("font", HelpText = "Font family name (e.g., Arial).")]
        public string FontFamily { get; set; }

        [Option("minsize", HelpText = "Minimum font size.")]
        public float? MinFontSize { get; set; }

        [Option("maxsize", HelpText = "Maximum font size.")]
        public float? MaxFontSize { get; set; }
    }
}
