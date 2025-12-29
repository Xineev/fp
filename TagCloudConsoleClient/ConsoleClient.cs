using CommandLine;
using System.Drawing;
using System.Drawing.Imaging;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Infrastructure;
using TagCloudGenerator.Infrastructure.Readers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TagCloudConsoleClient
{
    public class ConsoleClient : IClient
    {
        private readonly ITagCloudGenerator _generator;
        private readonly IEnumerable<IFilter> _filters;
        private readonly IReaderRepository _readersRepository;
        private readonly INormalizer _normalizer;

        public ConsoleClient(ITagCloudGenerator generator, IEnumerable<IFilter> filters, IReaderRepository readers, INormalizer normalizer)
        {
            _generator = generator;
            _filters = filters;
            _readersRepository = readers;
            _normalizer = normalizer;
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

            string inputFile = opts.InputFile;
            string outputFile = opts.OutputFile;

            Console.WriteLine("Starting tag cloud generation...");
            Console.WriteLine($"Input file: {inputFile}");
            Console.WriteLine($"Output file: {outputFile}");


            try
            {
                if (_readersRepository.TryGetReader(inputFile, out var outputReader))
                {
                    var words = _normalizer.Normalize(outputReader.TryRead(inputFile));
                    var image = _generator.Generate(words, canvasSettings, textSettings, _filters);

                    if (image != null) image.Save(outputFile, ImageFormat.Png);
                    image.Dispose();

                    Console.WriteLine("Tag cloud generation completed successfully!");
                }
                else
                {
                    throw new Exception("no suitable formate readers found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during generation: {ex.Message}");
            }
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
