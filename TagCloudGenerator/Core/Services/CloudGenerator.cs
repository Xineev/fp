using System.Drawing;
using TagCloudGenerator.Algorithms;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Infrastructure;
using TagCloudGenerator.Infrastructure.Filters;

namespace TagCloudGenerator.Core.Services
{
    public class CloudGenerator : ITagCloudGenerator
    {
        private readonly ITagCloudAlgorithm _algorithm;
        private readonly IAnalyzer _analyzer;
        private readonly IRenderer _renderer;
        private readonly IFontSizeCalculator _fontSizeCalculator;
        private readonly ITextMeasurer _textMeasurer;
        private readonly ISorterer _sorterer;
        private readonly Point _center;

        public CloudGenerator(ITagCloudAlgorithm algorithm, 
            IAnalyzer analyzer, 
            IRenderer renderer,
            IFontSizeCalculator fontSizeCalculator,
            ITextMeasurer textMeasurer,
            ISorterer sorterer)
        {
            this._algorithm = algorithm;
            _analyzer = analyzer;
            _renderer = renderer;
            _fontSizeCalculator = fontSizeCalculator;
            _textMeasurer = textMeasurer;
            _sorterer = sorterer;
            _center = new Point(0, 0);
        }

        public Bitmap? Generate(List<string> words, CanvasSettings canvasSettings, TextSettings textSettings, IEnumerable<IFilter> filters)
        {
            words = ApplyFilters(words, filters);
            if (words.Count == 0) return null;

            var wordsWithFreq = _sorterer.Sort(_analyzer.Analyze(words));

            var initializedItems = InitializeCloudItems(wordsWithFreq, textSettings).ToList();

            _algorithm.Reset();

            return _renderer.Render(initializedItems, canvasSettings, textSettings);
        }

        private List<string> ApplyFilters(List<string> words, IEnumerable<IFilter> filters)
        {
            var filteredWords = words;
            foreach (var filter in filters)
            {
                filteredWords = filter.Filter(filteredWords);
            }
            return filteredWords;
        }

        private IEnumerable<CloudItem> InitializeCloudItems(IEnumerable<(string Word, int Frequency)> items, TextSettings settings)
        {
            var itemsList = new List<CloudItem>();

            var minFrequency = items.Min(i => i.Frequency);
            var maxFrequency = items.Max(i => i.Frequency);

            foreach (var item in items)
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

                var itemRectangle = _algorithm.PutNextRectangle(textSize);

                itemsList.Add(
                    new CloudItem(
                        word: item.Word,
                        rectangle: itemRectangle,
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
