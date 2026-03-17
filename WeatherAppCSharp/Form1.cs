using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        private string apiKey;
        private string zipCode;
        private DateTime? lastUpdateTime;
        private string alertText = "";
        private bool hasAlerts = false;
        private bool hasUnreadAlerts = false;
        private Form alertsWindow = null;
        private System.Collections.Generic.HashSet<string> currentHeadlines = new System.Collections.Generic.HashSet<string>();
        private System.Collections.Generic.HashSet<string> acknowledgedAlerts = new System.Collections.Generic.HashSet<string>();

        public Form1()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            LoadConfig();
            tempPictureBox.Image = Properties.Resources.thermometer;
            windPictureBox.Image = Properties.Resources.wind;
            zipTextBox.Text = zipCode;
            updateTimer.Interval = 10 * 60 * 1000;
            updateTimer.Tick += async (s, e) => await UpdateWeatherAsync();
            updateTimer.Start();
            _ = UpdateWeatherAsync();
        }

        private void LoadConfig()
        {
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            apiKey = config.api_key;
            zipCode = config.zip_code ?? "";
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
                        alertButton.BackColor = Color.Gray;
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

        private void fetchButton_Click(object sender, EventArgs e)
        {
            zipCode = zipTextBox.Text;
            _ = UpdateWeatherAsync();
        }

        private void alertButton_Click(object sender, EventArgs e)
        {
            ShowAlertsWindow();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int x = Properties.Settings.Default.FormLocationX;
            int y = Properties.Settings.Default.FormLocationY;
            if (x != -1 && y != -1)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(x, y);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Properties.Settings.Default.FormLocationX = this.Location.X;
            Properties.Settings.Default.FormLocationY = this.Location.Y;
            Properties.Settings.Default.Save();
            base.OnFormClosing(e);
        }
    }

    public class Config
    {
        public string api_key { get; set; }
        public string zip_code { get; set; }
    }
}
