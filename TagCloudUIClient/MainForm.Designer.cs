using System.Drawing;
using System.Drawing.Imaging;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Infrastructure;
using TagCloudGenerator.Infrastructure.Filters;
using TagCloudGenerator.Infrastructure.Readers;

namespace TagCloudUIClient
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private readonly ITagCloudGenerator _generator;
        private readonly IReaderRepository _readersRepository;
        private readonly INormalizer _normalizer;
        private string _lastOutputPath = string.Empty;

        private List<string> wordsToRender;

        private Bitmap? _generatedBitmap;

        private Button btnChooseFile;
        private Label lblFilePath;
        private GroupBox groupBoxSettings;
        private Label lblWidth;
        private NumericUpDown numWidth;
        private Label lblHeight;
        private NumericUpDown numHeight;
        private Label lblMinSize;
        private NumericUpDown numMinSize;
        private Label lblMaxSize;
        private NumericUpDown numMaxSize;
        private Button btnChooseFont;
        private Label lblFont;
        private Button btnTextColor;
        private Button btnBgColor;
        private CheckedListBox clbExcludedWords;
        private Button btnGenerate;
        private PictureBox picturePreview;
        private Button btnSave;


        public MainForm(ITagCloudGenerator generator, IReaderRepository readers, INormalizer normalizer)
        {
            _generator = generator;
            _readersRepository = readers;
            _normalizer = normalizer;
            InitializeComponent();
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog();
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lblFilePath.Text = dlg.FileName;
                LoadExcludedWords(dlg.FileName);
            }
        }

        private void LoadExcludedWords(string path)
        {
            clbExcludedWords.Items.Clear();
            try
            {
                if (_readersRepository.TryGetReader(path, out var outputReader))
                {
                    var words = outputReader.TryRead(path);
                    wordsToRender = _normalizer.Normalize(words);
                }
                else
                {
                    throw new Exception("no suitable formate readers found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                   $"Ошибка чтения: {ex.Message}",
                   "Формат файла не поддерживается",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }

            var text = File.ReadAllText(path);
            var distinctWords = wordsToRender.Distinct().OrderBy(w => w);

            foreach (var w in distinctWords)
                clbExcludedWords.Items.Add(w, true);
        }

        private void btnChooseFont_Click(object sender, EventArgs e)
        {
            using var fontDlg = new FontDialog();
            fontDlg.Font = lblFont.Tag as Font ?? this.Font;
            if (fontDlg.ShowDialog() == DialogResult.OK)
            {
                lblFont.Tag = fontDlg.Font;
                lblFont.Text = $"{fontDlg.Font.Name}, {fontDlg.Font.Size}pt";
            }
        }

        private void btnTextColor_Click(object sender, EventArgs e)
        {
            using var colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
                lblTextColor.BackColor = colorDlg.Color;
        }

        private void btnBgColor_Click(object sender, EventArgs e)
        {
            using var colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
                lblBgColor.BackColor = colorDlg.Color;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblFilePath.Text) || !File.Exists(lblFilePath.Text))
            {
                MessageBox.Show(
                    "Пожалуйста, выберите корректный текстовый файл.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var allWords = clbExcludedWords.Items.Cast<string>();

            var excluded = allWords
                .Where(word => !clbExcludedWords.CheckedItems.Contains(word))
                .ToList();

            var filters = new List<IFilter> { new BoringWordsFilter(excluded) };

            var font = lblFont.Tag as Font ?? Font;
            var bgColor = lblBgColor.BackColor;
            var textColor = lblTextColor.BackColor;

            var canvasSettings = new CanvasSettings()
                .SetBackgroundColor(bgColor)
                .SetWidth((int)numWidth.Value)
                .SetHeight((int)numHeight.Value);

            var textSettings = new TextSettings()
                .SetFontFamily(font.FontFamily.Name)
                .SetMaxFontSize((int)numMaxSize.Value)
                .SetMinFontSize((int)numMinSize.Value)
                .SetTextColor(textColor);

            try
            {
                var bitmap = _generator.Generate(
                    wordsToRender,
                    canvasSettings,
                    textSettings,
                    filters);

                picturePreview.Image?.Dispose();

                _generatedBitmap = bitmap;
                picturePreview.Image = bitmap;

                btnSave.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка генерации: {ex.Message}",
                    "Генерация не удалась",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_generatedBitmap == null)
                return;

            using var saveDlg = new SaveFileDialog
            {
                Filter = "PNG Image|*.png|JPEG Image|*.jpg",
                Title = "Сохранить облако тегов…",
                FileName = "tagcloud.png"
            };

            if (saveDlg.ShowDialog() != DialogResult.OK)
                return;

            var ext = Path.GetExtension(saveDlg.FileName).ToLowerInvariant();

            var format = ext == ".jpg" || ext == ".jpeg" ? ImageFormat.Jpeg : ImageFormat.Png;

            _generatedBitmap.Save(saveDlg.FileName, format);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnChooseFile = new Button();
            lblFilePath = new Label();
            groupBoxSettings = new GroupBox();
            lblWidth = new Label();
            numWidth = new NumericUpDown();
            lblHeight = new Label();
            numHeight = new NumericUpDown();
            lblMinSize = new Label();
            numMinSize = new NumericUpDown();
            lblMaxSize = new Label();
            numMaxSize = new NumericUpDown();
            btnChooseFont = new Button();
            lblFont = new Label();
            btnTextColor = new Button();
            btnBgColor = new Button();
            clbExcludedWords = new CheckedListBox();
            btnGenerate = new Button();
            picturePreview = new PictureBox();
            btnSave = new Button();
            lblBgColor = new Label();
            lblTextColor = new Label();
            groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picturePreview).BeginInit();
            SuspendLayout();
            // 
            // btnChooseFile
            // 
            btnChooseFile.Location = new Point(12, 12);
            btnChooseFile.Name = "btnChooseFile";
            btnChooseFile.Size = new Size(120, 30);
            btnChooseFile.TabIndex = 0;
            btnChooseFile.Text = "Выбрать файл...";
            btnChooseFile.UseVisualStyleBackColor = true;
            btnChooseFile.Click += btnChooseFile_Click;
            // 
            // lblFilePath
            // 
            lblFilePath.AutoSize = true;
            lblFilePath.Location = new Point(150, 20);
            lblFilePath.Name = "lblFilePath";
            lblFilePath.Size = new Size(124, 20);
            lblFilePath.TabIndex = 1;
            lblFilePath.Text = "Файл не выбран";
            // 
            // groupBoxSettings
            // 
            groupBoxSettings.Controls.Add(lblWidth);
            groupBoxSettings.Controls.Add(numWidth);
            groupBoxSettings.Controls.Add(lblHeight);
            groupBoxSettings.Controls.Add(numHeight);
            groupBoxSettings.Controls.Add(lblMinSize);
            groupBoxSettings.Controls.Add(numMinSize);
            groupBoxSettings.Controls.Add(lblMaxSize);
            groupBoxSettings.Controls.Add(numMaxSize);
            groupBoxSettings.Controls.Add(btnChooseFont);
            groupBoxSettings.Controls.Add(lblFont);
            groupBoxSettings.Location = new Point(12, 60);
            groupBoxSettings.Name = "groupBoxSettings";
            groupBoxSettings.Size = new Size(380, 180);
            groupBoxSettings.TabIndex = 2;
            groupBoxSettings.TabStop = false;
            groupBoxSettings.Text = "Настройки генерации";
            // 
            // lblWidth
            // 
            lblWidth.AutoSize = true;
            lblWidth.Location = new Point(10, 25);
            lblWidth.Name = "lblWidth";
            lblWidth.Size = new Size(70, 20);
            lblWidth.TabIndex = 0;
            lblWidth.Text = "Ширина:";
            // 
            // numWidth
            // 
            numWidth.Location = new Point(80, 23);
            numWidth.Maximum = new decimal(new int[] { 4000, 0, 0, 0 });
            numWidth.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numWidth.Name = "numWidth";
            numWidth.Size = new Size(80, 27);
            numWidth.TabIndex = 1;
            numWidth.Value = new decimal(new int[] { 800, 0, 0, 0 });
            // 
            // lblHeight
            // 
            lblHeight.AutoSize = true;
            lblHeight.Location = new Point(180, 25);
            lblHeight.Name = "lblHeight";
            lblHeight.Size = new Size(62, 20);
            lblHeight.TabIndex = 2;
            lblHeight.Text = "Высота:";
            // 
            // numHeight
            // 
            numHeight.Location = new Point(250, 23);
            numHeight.Maximum = new decimal(new int[] { 4000, 0, 0, 0 });
            numHeight.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numHeight.Name = "numHeight";
            numHeight.Size = new Size(80, 27);
            numHeight.TabIndex = 3;
            numHeight.Value = new decimal(new int[] { 600, 0, 0, 0 });
            // 
            // lblMinSize
            // 
            lblMinSize.AutoSize = true;
            lblMinSize.Location = new Point(10, 60);
            lblMinSize.Name = "lblMinSize";
            lblMinSize.Size = new Size(218, 20);
            lblMinSize.TabIndex = 4;
            lblMinSize.Text = "Минимальный размер текста:";
            // 
            // numMinSize
            // 
            numMinSize.Location = new Point(250, 58);
            numMinSize.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numMinSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMinSize.Name = "numMinSize";
            numMinSize.Size = new Size(60, 27);
            numMinSize.TabIndex = 5;
            numMinSize.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // lblMaxSize
            // 
            lblMaxSize.AutoSize = true;
            lblMaxSize.Location = new Point(10, 95);
            lblMaxSize.Name = "lblMaxSize";
            lblMaxSize.Size = new Size(222, 20);
            lblMaxSize.TabIndex = 6;
            lblMaxSize.Text = "Максимальный размер текста:";
            // 
            // numMaxSize
            // 
            numMaxSize.Location = new Point(250, 93);
            numMaxSize.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numMaxSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxSize.Name = "numMaxSize";
            numMaxSize.Size = new Size(60, 27);
            numMaxSize.TabIndex = 7;
            numMaxSize.Value = new decimal(new int[] { 80, 0, 0, 0 });
            numMaxSize.ValueChanged += numMaxSize_ValueChanged;
            //
            // btnChooseFont
            // 
            btnChooseFont.Location = new Point(10, 135);
            btnChooseFont.Name = "btnChooseFont";
            btnChooseFont.Size = new Size(120, 25);
            btnChooseFont.TabIndex = 8;
            btnChooseFont.Text = "Выбрать шрифт...";
            btnChooseFont.Click += btnChooseFont_Click;
            // 
            // lblFont
            // 
            lblFont.AutoSize = true;
            lblFont.Location = new Point(150, 135);
            lblFont.Name = "lblFont";
            lblFont.Size = new Size(144, 20);
            lblFont.TabIndex = 9;
            lblFont.Text = "(шрифт не выбран)";
            // 
            // btnTextColor
            // 
            btnTextColor.Location = new Point(22, 246);
            btnTextColor.Name = "btnTextColor";
            btnTextColor.Size = new Size(120, 25);
            btnTextColor.TabIndex = 10;
            btnTextColor.Text = "Цвет текста...";
            btnTextColor.Click += btnTextColor_Click;
            // 
            // btnBgColor
            // 
            btnBgColor.Location = new Point(22, 290);
            btnBgColor.Name = "btnBgColor";
            btnBgColor.Size = new Size(120, 25);
            btnBgColor.TabIndex = 12;
            btnBgColor.Text = "Цвет фона...";
            btnBgColor.Click += btnBgColor_Click;
            // 
            // clbExcludedWords
            // 
            clbExcludedWords.CheckOnClick = true;
            clbExcludedWords.FormattingEnabled = true;
            clbExcludedWords.Location = new Point(22, 338);
            clbExcludedWords.Name = "clbExcludedWords";
            clbExcludedWords.Size = new Size(250, 180);
            clbExcludedWords.TabIndex = 3;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(22, 524);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(139, 30);
            btnGenerate.TabIndex = 4;
            btnGenerate.Text = "Сгенерировать";
            btnGenerate.Click += btnGenerate_Click;
            // 
            // picturePreview
            // 
            picturePreview.BorderStyle = BorderStyle.FixedSingle;
            picturePreview.Location = new Point(410, 60);
            picturePreview.Name = "picturePreview";
            picturePreview.Size = new Size(688, 534);
            picturePreview.SizeMode = PictureBoxSizeMode.Zoom;
            picturePreview.TabIndex = 5;
            picturePreview.TabStop = false;
            // 
            // btnSave
            // 
            btnSave.Enabled = false;
            btnSave.Location = new Point(164, 524);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(110, 30);
            btnSave.TabIndex = 6;
            btnSave.Text = "Сохранить...";
            btnSave.Click += btnSave_Click;
            // 
            // lblBgColor
            // 
            lblBgColor.AutoSize = true;
            lblBgColor.BackColor = Color.White;
            lblBgColor.BorderStyle = BorderStyle.FixedSingle;
            lblBgColor.Location = new Point(162, 290);
            lblBgColor.Name = "lblBgColor";
            lblBgColor.Size = new Size(2, 22);
            lblBgColor.TabIndex = 13;
            // 
            // lblTextColor
            // 
            lblTextColor.AutoSize = true;
            lblTextColor.BackColor = Color.Black;
            lblTextColor.BorderStyle = BorderStyle.FixedSingle;
            lblTextColor.Location = new Point(162, 249);
            lblTextColor.Name = "lblTextColor";
            lblTextColor.Size = new Size(2, 22);
            lblTextColor.TabIndex = 11;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1110, 660);
            Controls.Add(btnChooseFile);
            Controls.Add(lblFilePath);
            Controls.Add(groupBoxSettings);
            Controls.Add(clbExcludedWords);
            Controls.Add(btnGenerate);
            Controls.Add(picturePreview);
            Controls.Add(btnSave);
            Controls.Add(btnTextColor);
            Controls.Add(btnBgColor);
            Controls.Add(lblBgColor);
            Controls.Add(lblTextColor);
            Name = "MainForm";
            Text = "Генератор облака тегов";
            groupBoxSettings.ResumeLayout(false);
            groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)picturePreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBgColor;
        private Label lblTextColor;
    }
}