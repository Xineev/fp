using CommandLine;
using System.Drawing;
using System.Drawing.Imaging;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Infrastructure;

namespace TagCloudConsoleClient
{
    public class ConsoleClient : IClient
    {
        private readonly ITagCloudGenerator _generator;
        private readonly ICollection<IFilter> _filters;
        private readonly IReaderRepository _readersRepository;
        private readonly INormalizer _normalizer;
        private readonly IRenderer _renderer;

        public ConsoleClient(ITagCloudGenerator generator, ICollection<IFilter> filters, IReaderRepository readers, INormalizer normalizer, IRenderer renderer)
        {
            _generator = generator;
            _filters = filters;
            _readersRepository = readers;
            _normalizer = normalizer;
            _renderer = renderer;
        }

        public void Run(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunWithOptions)
                .WithNotParsed(errors => { });
        }

        private void RunWithOptions(Options opts)
        {
            if (!File.Exists(opts.InputFile))
            {
                Console.WriteLine($"Error: input file '{opts.InputFile}' does not exist.");
                return;
            }

            var canvasSettings = new CanvasSettings()
                .SetSize(opts.Width, opts.Height)
                .SetBackgroundColor(TryParseColor(opts.BackgroundColor))
                .WithShowRectangles()
                .SetPadding(opts.Padding);

            var textSettings = new TextSettings()
                .SetFontFamily(opts.FontFamily)
                .SetFontSizeRange(opts.MinFontSize, opts.MaxFontSize)
                .SetTextColor(TryParseColor(opts.TextColor));

            var inputFile = opts.InputFile;
            var outputFile = opts.OutputFile;

            Console.WriteLine("Starting tag cloud generation...");
            Console.WriteLine($"Input file: {inputFile}");
            Console.WriteLine($"Output file: {outputFile}");


            _readersRepository.TryGetReader(inputFile)
                .Then(reader => reader.TryRead(inputFile))
                .Then(words => _normalizer.Normalize(words))
                .Then(words => _generator.Generate(words, canvasSettings, textSettings, _filters))
                .Then(items => _renderer.Render(items, canvasSettings, textSettings))
                .Then(image => Result.OfAction(() => 
                {
                    image.Save(outputFile, ImageFormat.Png);
                    image.Dispose();
                }, "Failed to save output image"))
                .OnFail(error =>
                {
                    Console.WriteLine("Error during generation:");
                    Console.WriteLine(error);
                });
        }

        private static Color? TryParseColor(string colorStr)
        {
            if (string.IsNullOrWhiteSpace(colorStr))
                return null;

            try
            {
                var known = Color.FromName(colorStr);
                if (known.IsKnownColor)
                    return known;

                return ColorTranslator.FromHtml(colorStr);
            }
            catch
            {
                Console.WriteLine($"Color '{colorStr}' could not be parsed. Default color will be used.");
                return null;
            }
        }
    }
}
