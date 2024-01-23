namespace GeminiClient;

public partial class Gemini : Form
{
    private readonly HttpClient _httpClient;
    private List<Content> _contents = new List<Content>();

    public Gemini()
    {
        InitializeComponent();

        _httpClient = new HttpClient();
    }

    private async void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        try
        {
            textBox1.Enabled = false;
            var textBox = sender as TextBox;

            var novaContent = new List<Content>
            {
                new Content { Role = "user", Parts = new List<Part> { new Part { Text = textBox!.Text } } }
            };

            _contents.AddRange(novaContent);

            var clientRequest = new Client(novaContent[0].Role!, novaContent[0].Parts![0].Text!);
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

            var clientResponse = new Client(contentResponse.Role!, contentResponse.Parts![0].Text!);
            richTextBox1.AppendText(clientResponse.ToString());

            textBox1.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Um erro aconteceu: {ex.Message}\n\nTente novamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        finally
        {
            textBox1.Enabled = true;
            textBox1.Focus();
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        _contents.Clear();
        richTextBox1.Clear();
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
}
