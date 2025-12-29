using TagCloudGenerator.Core.Interfaces;

namespace TagCloudUIClient
{
    public class WinFormsClient : IClient
    {
        private readonly ITagCloudGenerator _generator;
        private readonly IReaderRepository _reader;
        private readonly INormalizer _normalizer;

        public WinFormsClient(ITagCloudGenerator generator, IReaderRepository repository, INormalizer normalizer)
        {
            _generator = generator;
            _reader = repository;
            _normalizer = normalizer;
        }

        public void Run(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new MainForm(_generator, _reader, _normalizer);
            Application.Run(mainForm);
        }
    }
}
