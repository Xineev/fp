using Autofac;
using TagCloudGenerator.Algorithms;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Services;
using TagCloudGenerator.Infrastructure.Analyzers;
using TagCloudGenerator.Infrastructure.Calculators;
using TagCloudGenerator.Infrastructure.Filters;
using TagCloudGenerator.Infrastructure.Measurers;
using TagCloudGenerator.Infrastructure.Normalizers;
using TagCloudGenerator.Infrastructure.Readers;
using TagCloudGenerator.Infrastructure.Renderers;
using TagCloudGenerator.Infrastructure.Sorterers;

namespace TagCloudGenerator.DI
{
    public class TagCloudModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TxtReader>().As<IFormatReader>().SingleInstance();
            builder.RegisterType<DocxReader>().As<IFormatReader>().SingleInstance();

            builder.RegisterType<ReaderRepository>()
                   .As<IReaderRepository>()
                   .SingleInstance();

            builder.RegisterType<FrequencyDescendingSorterer>().As<ISorterer>().SingleInstance();

            builder.RegisterType<LowerCaseNormalizer>().As<INormalizer>();

            builder.RegisterType<BasicTagCloudAlgorithm>().As<ITagCloudAlgorithm>();

            builder.RegisterType<LinearFontSizeCalculator>()
               .As<IFontSizeCalculator>();

            builder.RegisterType<GraphicsTextMeasurer>()
               .As<ITextMeasurer>();

            builder.RegisterType<WordsFrequencyAnalyzer>()
               .As<IAnalyzer>();

            builder.RegisterType<PngRenderer>().As<IRenderer>();

            builder.RegisterType<CloudGenerator>().As<ITagCloudGenerator>();
        }
    }
}
