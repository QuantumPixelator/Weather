using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Config
{
    public string api_key { get; set; }
    public string zip_code { get; set; }
}

public class WeatherForm : Form
{
    private readonly string apiKey;
    private string zipCode;
    private DateTime? lastUpdateTime;
    private Image thermometerIcon;
    private Image windIcon;
    private System.Windows.Forms.Timer updateTimer;
    private TextBox zipTextBox;
    private Button fetchButton;
    private Label nameLabel;
    private Label tempLabel;
    private Label weatherLabel;
    private Label windLabel;
    private Label moonLabel;
    private Label lastUpdateLabel;
    private PictureBox tempPictureBox;
    private PictureBox windPictureBox;
    private PictureBox weatherPictureBox;
    private Button alertButton;
    private string alertText = "";
    private bool hasAlerts = false;
    private bool hasUnreadAlerts = false;
    private Form alertsWindow = null;
private System.Collections.Generic.HashSet<string> currentHeadlines = new System.Collections.Generic.HashSet<string>();
private System.Collections.Generic.HashSet<string> acknowledgedAlerts = new System.Collections.Generic.HashSet<string>();

    public WeatherForm()
    {
        // Load config
        var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
        apiKey = config.api_key;
        zipCode = config.zip_code ?? "";

        // Load icons
        thermometerIcon = Image.FromFile("thermometer.png");
        windIcon = Image.FromFile("wind.png");

        // Setup UI
        InitializeComponent();
        zipTextBox.Text = zipCode;
        tempPictureBox.Image = thermometerIcon;
        windPictureBox.Image = windIcon;

        // Timer for auto-update
        updateTimer = new System.Windows.Forms.Timer();
        updateTimer.Interval = 10 * 60 * 1000; // 10 minutes
        updateTimer.Tick += (s, e) => _ = UpdateWeatherAsync();
        updateTimer.Start();

        // Initial fetch
        _ = UpdateWeatherAsync();
    }

    private void InitializeComponent()
    {
        this.Text = "Weather App";
        this.Size = new Size(320, 400);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.BackColor = Color.FromArgb(30, 30, 30);

        // Zip input
        zipTextBox = new TextBox { Location = new Point(20, 20), Width = 120, ForeColor = Color.White, BackColor = Color.FromArgb(50, 50, 50) };
        fetchButton = new Button { Text = "Go", Location = new Point(150, 18), Width = 50 };
        fetchButton.Click += (s, e) => { zipCode = zipTextBox.Text; _ = UpdateWeatherAsync(); };
        this.Controls.Add(zipTextBox);
        this.Controls.Add(fetchButton);

        // Name
        nameLabel = new Label { Location = new Point(20, 60), Width = 260, Font = new Font("Arial", 18, FontStyle.Bold), ForeColor = Color.Gold, BackColor = Color.Transparent };
        this.Controls.Add(nameLabel);

        // Temperature
        tempPictureBox = new PictureBox { Location = new Point(20, 100), Size = new Size(32, 32), BackColor = Color.Transparent };
        tempLabel = new Label { Location = new Point(60, 105), Width = 200, Font = new Font("Arial", 14), ForeColor = Color.White, BackColor = Color.Transparent };
        this.Controls.Add(tempPictureBox);
        this.Controls.Add(tempLabel);

        // Condition
        weatherPictureBox = new PictureBox { Location = new Point(20, 140), Size = new Size(32, 32), BackColor = Color.Transparent };
        weatherLabel = new Label { Location = new Point(60, 145), Width = 200, Font = new Font("Arial", 12), ForeColor = Color.Orange, BackColor = Color.Transparent };
        this.Controls.Add(weatherPictureBox);
        this.Controls.Add(weatherLabel);

        // Wind
        windPictureBox = new PictureBox { Location = new Point(20, 180), Size = new Size(32, 32), BackColor = Color.Transparent };
        windLabel = new Label { Location = new Point(60, 185), Width = 200, Font = new Font("Arial", 12), ForeColor = Color.Magenta, BackColor = Color.Transparent };
        this.Controls.Add(windPictureBox);
        this.Controls.Add(windLabel);

        // Moon
        moonLabel = new Label { Location = new Point(20, 220), Width = 260, Font = new Font("Arial", 12), ForeColor = Color.Cyan, BackColor = Color.Transparent };
        this.Controls.Add(moonLabel);

        // Alert button
        alertButton = new Button { Text = "ALERT", Location = new Point(20, 260), Width = 80, Enabled = false, BackColor = Color.Gray, ForeColor = Color.White };
        alertButton.Click += (s, e) => ShowAlertsWindow();
        this.Controls.Add(alertButton);

        // Last update
        lastUpdateLabel = new Label { Location = new Point(20, 300), Width = 260, Font = new Font("Arial", 10), ForeColor = Color.Gray, BackColor = Color.Transparent };
        this.Controls.Add(lastUpdateLabel);
    }

    private async Task UpdateWeatherAsync()
    {
        if (string.IsNullOrWhiteSpace(zipCode)) return;
        try
        {
            using (var client = new HttpClient())
            {
                var url = string.Format("http://api.weatherapi.com/v1/current.json?key={0}&q={1}", apiKey, zipCode);
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                dynamic doc = JsonConvert.DeserializeObject(json);
                if (doc["current"] == null)
                {
                    tempLabel.Text = "Invalid zip code";
                    weatherLabel.Text = windLabel.Text = nameLabel.Text = lastUpdateLabel.Text = "";
                    weatherPictureBox.Image = null;
                    return;
                }
                double temp = doc["current"]["temp_f"];
                string condition = doc["current"]["condition"]["text"];
                double wind = doc["current"]["wind_mph"];
                string iconUrl = "https:" + (string)doc["current"]["condition"]["icon"];
                string location = doc["location"]["name"];
                nameLabel.Text = location;
                tempLabel.Text = string.Format("{0}°F", temp);
                weatherLabel.Text = condition;
                windLabel.Text = string.Format("{0} mph", wind);
                lastUpdateTime = DateTime.Now;
                lastUpdateLabel.Text = string.Format("Updated: {0}", lastUpdateTime.Value.ToString("hh:mm tt").ToUpper());
                if (temp <= 32) tempLabel.ForeColor = Color.Blue;
                else if (temp >= 85) tempLabel.ForeColor = Color.Red;
                else tempLabel.ForeColor = Color.Green;
                weatherPictureBox.Image = await DownloadImageAsync(iconUrl);
                await UpdateMoonAsync(client);
                await FetchAlertsAsync(client);
            }
        }
        catch (Exception ex)
        {
            tempLabel.Text = ex.Message;
        }
    }

    private async Task UpdateMoonAsync(HttpClient client)
    {
        try
        {
            var moonUrl = string.Format("http://api.weatherapi.com/v1/astronomy.json?key={0}&q={1}", apiKey, zipCode);
            var moonResp = await client.GetAsync(moonUrl);
            var moonJson = await moonResp.Content.ReadAsStringAsync();
            dynamic moonDoc = JsonConvert.DeserializeObject(moonJson);
            string moonPhase = moonDoc["astronomy"]["astro"]["moon_phase"];
            moonLabel.Text = string.Format("Moon Phase: {0}", moonPhase);
        }
        catch { moonLabel.Text = ""; }
    }

    private async Task FetchAlertsAsync(HttpClient client)
    {
        try
        {
            var url = string.Format("http://api.weatherapi.com/v1/alerts.json?key={0}&q={1}", apiKey, zipCode);
            var resp = await client.GetAsync(url);
            var json = await resp.Content.ReadAsStringAsync();
            dynamic doc = JsonConvert.DeserializeObject(json);
            if (doc["alerts"] != null && doc["alerts"]["alert"] != null && doc["alerts"]["alert"].Count > 0)
            {
                var alertTexts = new System.Collections.Generic.List<string>();
                currentHeadlines.Clear();
                foreach (var alert in doc["alerts"]["alert"])
                {
                    string headline = alert["headline"];
                    string msgtype = alert["msgtype"];
                    string severity = alert["severity"];
                    string urgency = alert["urgency"];
                    string areas = alert["areas"];
                    alertTexts.Add(string.Format("{0}\nType: {1}\nSeverity: {2}\nUrgency: {3}\nAreas: {4}\n", headline, msgtype, severity, urgency, areas));
                    currentHeadlines.Add(headline);
                }
                alertText = string.Join("\n\n", alertTexts);
                hasAlerts = true;
                var newAlerts = new System.Collections.Generic.HashSet<string>(currentHeadlines);
                newAlerts.ExceptWith(acknowledgedAlerts);
                hasUnreadAlerts = newAlerts.Count > 0;
                acknowledgedAlerts.IntersectWith(currentHeadlines);
                alertButton.Enabled = true;
                if (hasUnreadAlerts)
                {
                    alertButton.BackColor = Color.Red;
                    alertButton.ForeColor = Color.White;
                }
                else
                {
                    alertButton.BackColor = Color.Orange;
                    alertButton.ForeColor = Color.White;
                }
            }
            else
            {
                alertText = "";
                hasAlerts = false;
                hasUnreadAlerts = false;
                currentHeadlines.Clear();
                acknowledgedAlerts.Clear();
                alertButton.Enabled = false;
                alertButton.BackColor = Color.Gray;
                alertButton.ForeColor = Color.White;
            }
        }
        catch
        {
            if (!hasAlerts)
            {
                alertText = "";
                hasAlerts = false;
                hasUnreadAlerts = false;
                currentHeadlines.Clear();
                acknowledgedAlerts.Clear();
                alertButton.Enabled = false;
                alertButton.BackColor = Color.Gray;
                alertButton.ForeColor = Color.White;
            }
        }
    }

    private void ShowAlertsWindow()
    {
        if (alertsWindow == null || alertsWindow.IsDisposed)
        {
            alertsWindow = new Form
            {
                Text = "Weather Alerts",
                Size = new Size(400, 300),
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true,
                BackColor = Color.FromArgb(30, 30, 30)
            };
            var textBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Text = alertText,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 50, 50),
                ScrollBars = ScrollBars.Vertical
            };
            var closeButton = new Button { Text = "Close", Dock = DockStyle.Bottom };
            closeButton.Click += (s, e) => alertsWindow.Close();
            alertsWindow.Controls.Add(textBox);
            alertsWindow.Controls.Add(closeButton);
            alertsWindow.FormClosed += (s, e) =>
            {
                alertsWindow = null;
                if (hasAlerts)
                {
                    acknowledgedAlerts.UnionWith(currentHeadlines);
                    hasUnreadAlerts = false;
                    alertButton.BackColor = Color.Orange;
                    alertButton.ForeColor = Color.White;
                }
            };
        }
        alertsWindow.Show();
        alertsWindow.BringToFront();
    }

    private async Task<Image> DownloadImageAsync(string url)
    {
        using (var client = new HttpClient())
        {
            var bytes = await client.GetByteArrayAsync(url);
            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
