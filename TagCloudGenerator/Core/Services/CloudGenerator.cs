using System.Drawing;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Services
{
    public class CloudGenerator : ITagCloudGenerator
    {
        private readonly ITagCloudAlgorithm _algorithm;
        private readonly IAnalyzer _analyzer;
        private readonly IRenderer _renderer;
        private readonly IFontSizeCalculator _fontSizeCalculator;
        private readonly ITextMeasurer _textMeasurer;
        private readonly ISorter _sorter;

        public CloudGenerator(ITagCloudAlgorithm algorithm, 
            IAnalyzer analyzer, 
            IRenderer renderer,
            IFontSizeCalculator fontSizeCalculator,
            ITextMeasurer textMeasurer,
            ISorter sorterer)
        {
            _algorithm = algorithm;
            _analyzer = analyzer;
            _renderer = renderer;
            _fontSizeCalculator = fontSizeCalculator;
            _textMeasurer = textMeasurer;
            _sorter = sorterer;
        }

        public Result<Bitmap> Generate(List<string> words, CanvasSettings canvasSettings, TextSettings textSettings, IEnumerable<IFilter> filters)
        {
            _algorithm.Reset();
            
            var filteredWords = ApplyFilters(words, filters);

            var wordsWithFreq = _analyzer.Analyze(filteredWords);
            var sortedWords = _sorter.Sort(wordsWithFreq.Value);
            var initializedItems = InitializeCloudItems(sortedWords.Value, textSettings).ToList();

            return _renderer.Render(initializedItems, canvasSettings, textSettings);
        }

        private List<string> ApplyFilters(List<string> words, IEnumerable<IFilter> filters)
        {
            var filteredWords = new List<string>();

            foreach (var word in words)
            {
                foreach (var filter in filters)
                {
                    if (filter.ShouldInclude(word) == false)
                        break;
                }
                filteredWords.Add(word);
            }

            return filteredWords;
        }

        private IEnumerable<CloudItem> InitializeCloudItems(WordsWithFrequency items, TextSettings settings)
        {
            var itemsList = new List<CloudItem>();

            var minFrequency = items.MinFreq;
            var maxFrequency = items.MaxFreq;

            foreach (var item in items.WordsWithFreq)
            {
                var fontSize = _fontSizeCalculator.Calculate(
                    item.Frequency,
                    minFrequency,
                    maxFrequency,
                    settings.MinFontSize,
                    settings.MaxFontSize);

                var textSize = _textMeasurer.Measure(
                    item.Word,
                    fontSize,
                    settings.FontFamily);

                var itemRectangle = _algorithm.PutNextRectangle(textSize.Value);

                itemsList.Add(
                    new CloudItem(
                        word: item.Word,
                        rectangle: itemRectangle.Value,
                        fontSize: fontSize,
                        textColor: settings.TextColor,
                        fontFamily: settings.FontFamily,
                        frequency: item.Frequency
                        ));
            }
            return itemsList;
        }
    }
}
