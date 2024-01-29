namespace GeminiClient;

public partial class Gemini : Form
{
    private readonly HttpClient _httpClient;
    private List<Content> _contents = new List<Content>();
    private string? _base64Image;

    private VideoCapture _capture = new VideoCapture(0);
    private Mat _image = new Mat();
    Thread? thread;

    public Gemini()
    {
        InitializeComponent();

        _httpClient = new HttpClient();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        thread = new Thread(new ThreadStart(CaptureCameraCallback));
        thread.Start();

        IfExistsContentsCaptureIsDisabled();
    }

    private void IfExistsContentsCaptureIsDisabled()
    {
        var ifExistsContentsButtonIsDisabled = _contents.Any() == false;
        button3.Enabled = ifExistsContentsButtonIsDisabled;
    }

    public void CaptureCameraCallback()
    {
        while (true)
        {
            _capture.Read(_image);
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => pictureBox1.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_image)));
            }
            else
            {
                pictureBox1.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_image);
            }
        }
    }

    private async void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        try
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.Image = Resources.load_37_128;

            textBox1.Enabled = false;
            var textBox = sender as TextBox;

            var isThereImage = string.IsNullOrEmpty(_base64Image) == false;
            var novaContent = isThereImage ? new List<Content>
            {
                new Content { Role = "user", Parts = new List<object> { new Part { Text = textBox!.Text }, new { inline_data = new InlineData { Data = _base64Image, MimeType = "image/png" } } } }
            } :
            new List<Content>
            {
                new Content { Role = "user", Parts = new List<object> { new Part { Text = textBox!.Text } } }
            };

            if (isThereImage)
                _contents.Clear();

            _contents.AddRange(novaContent);

            var clientRequest = new Client(novaContent[0].Role!, ((Part)novaContent[0].Parts![0]).Text!);
            richTextBox1.AppendText(clientRequest.ToString());

            var geminiProRequest = new GeminiPro();
            geminiProRequest.Contents = _contents;

            var json = JsonSerializer.Serialize(geminiProRequest);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7284/api/chat", stringContent);
            response.EnsureSuccessStatusCode();

            var geminiProResponse = await response.Content.ReadFromJsonAsync<GeminiProResponse>();
            var contentResponse = geminiProResponse!.Candidates![0].Content!;
            _contents.Add(contentResponse);

            var part = JsonSerializer.Deserialize<Part>(contentResponse.Parts![0].ToString()!);

            var clientResponse = new Client(contentResponse.Role!, part!.Text!);
            richTextBox1.AppendText(clientResponse.ToString());

            textBox1.Clear();
            if (string.IsNullOrEmpty(_base64Image) == false)
            {
                _base64Image = string.Empty;
                _contents.Clear();

                richTextBox1.AppendText("\n*** AVISO --> Com o reconhecimento de imagens não é possível fazer conversas multi-turnos (bate-papo com a IA), mas você pode continuar fazendo outras perguntas não relacionadas a imagem ou então disponibilizar outra imagem. ***\n\n");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Um erro aconteceu: {ex.Message}\n\nTente novamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        finally
        {
            textBox1.Enabled = true;
            textBox1.Focus();
            IfExistsContentsCaptureIsDisabled();

            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = null;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        _contents.Clear();
        richTextBox1.Clear();
        textBox1.Clear();
        pictureBox2.Image = null;
        IfExistsContentsCaptureIsDisabled();
    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
        var textoCompleto = richTextBox1.Text;
        var nomeArquivo = string.Format("{0}_BardAI", DateTime.Now.ToString("dd_MM_yyyy_hh_mm"));
        saveFileDialog1.FileName = nomeArquivo;
        saveFileDialog1.Title = "Exportar para arquivo...";
        saveFileDialog1.Filter = "Arquivo de Texto|.txt";
        saveFileDialog1.DefaultExt = ".txt";
        var resultado = saveFileDialog1.ShowDialog();
        if (resultado == DialogResult.Cancel)
            return;

        using var fileStream = new FileStream(saveFileDialog1.FileName, FileMode.Create);
        using var writer = new StreamWriter(fileStream);
        writer.Write(textoCompleto);
        writer.Close();

        MessageBox.Show(string.Format("Arquivo salvo com sucesso!\n{0}", saveFileDialog1.FileName), "Exportar para arquivo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void button3_Click(object sender, EventArgs e)
    {
        ImageConverter converter = new ImageConverter();
        var bytes = (byte[])converter.ConvertTo((Bitmap)pictureBox1.Image, typeof(byte[]))!;

        _base64Image = Convert.ToBase64String(bytes);
        pictureBox2.Image = System.Drawing.Image.FromStream(new MemoryStream(Convert.FromBase64String(_base64Image)));
    }

    private void button4_Click(object sender, EventArgs e)
    {
        openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        openFileDialog1.Title = "Carregar imagem...";
        openFileDialog1.Filter = "Imagens (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
        openFileDialog1.DefaultExt = ".png";
        var resultado = openFileDialog1.ShowDialog();
        if (resultado == DialogResult.Cancel)
            return;

        var fileInfo = new FileInfo(openFileDialog1.FileName);
        var fileSize = fileInfo.Length;
        if(fileSize > 1000000)
        {
            MessageBox.Show("As imagens não podem ter mais de 1MB...", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var image = System.Drawing.Image.FromFile(openFileDialog1.FileName);
        pictureBox2.Image = image;

        byte[] bytes = File.ReadAllBytes(openFileDialog1.FileName);
        _base64Image = Convert.ToBase64String(bytes);
    }
}
