using TagCloudGenerator.Core.Interfaces;

namespace TagCloudUIClient
{
    public class WinFormsClient : IClient
    {
        private readonly ITagCloudGenerator _generator;
        private readonly IReaderRepository _reader;
        private readonly INormalizer _normalizer;
        private readonly IRenderer _renderer;

        public WinFormsClient(ITagCloudGenerator generator, IReaderRepository repository, INormalizer normalizer, IRenderer renderer)
        {
            _generator = generator;
            _reader = repository;
            _normalizer = normalizer;
            _renderer = renderer;
        }

        public void Run(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new MainForm(_generator, _reader, _normalizer, _renderer);
            Application.Run(mainForm);
        }
    }
}
