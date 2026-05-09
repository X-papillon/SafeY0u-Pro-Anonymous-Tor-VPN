using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SafeY0u
{
    public partial class Form1 : Form
    {
        // Windows API constants
       


        private Process torProcess;
        private Process selectedBrowserProcess;
        private int currentProgress = 0;
        private string torProxyIP = "127.0.0.1";
        private string torProxyPort = "9050";
        private bool torReady = false;
        private string userPublicIP = "";
        private string torPublicIP = "";
        private string lastTorPublicIP = "";
        private bool isInternetAvailable = false;
        private bool isClosing = false;
        private string selectedBrowserPath = "";
        private System.Windows.Forms.Timer internetCheckTimer;
        private System.Windows.Forms.Timer torHealthCheckTimer;
        private System.Windows.Forms.Timer publicIPCheckTimer;
        private NotifyIcon notifyIcon;
       
        public Form1()
        {

            
            InitializeComponent();
            InitializeAdditionalComponents();
            LoadSavedSettings();
            InitializeNotificationIcon();
           


        }

        

        private void InitializeNotificationIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Shield;
            notifyIcon.Visible = false;
        }

        private void LoadSavedSettings()
        {
            try
            {
                if (Properties.Settings.Default.BrowserPath != null &&
                    !string.IsNullOrEmpty(Properties.Settings.Default.BrowserPath) &&
                    File.Exists(Properties.Settings.Default.BrowserPath))
                {
                    selectedBrowserPath = Properties.Settings.Default.BrowserPath;
                    txtBrowserPath.Text = selectedBrowserPath;
                    btnRemoveBrowser.Enabled = true;
                }
            }
            catch { }
        }

        private void SaveBrowserPath(string path)
        {
            try
            {
                Properties.Settings.Default.BrowserPath = path;
                Properties.Settings.Default.Save();
            }
            catch { }
        }

        private void BtnSelectBrowser_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Browser Executable";
                openFileDialog.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedBrowserPath = openFileDialog.FileName;
                    txtBrowserPath.Text = selectedBrowserPath;
                    btnRemoveBrowser.Enabled = true;
                    SaveBrowserPath(selectedBrowserPath);
                    LogMessage($"✅ Browser selected: {Path.GetFileName(selectedBrowserPath)}", Color.Green);
                }
            }
        }

        private void BtnRemoveBrowser_Click(object sender, EventArgs e)
        {
            selectedBrowserPath = "";
            txtBrowserPath.Text = "";
            btnRemoveBrowser.Enabled = false;
            SaveBrowserPath("");
            LogMessage("🗑 Browser selection removed", Color.Yellow);
        }

        private async void InitializeAdditionalComponents()
        {
            try
            {
                // Timer for internet connection check
                internetCheckTimer = new System.Windows.Forms.Timer();
                internetCheckTimer.Interval = 5000;
                internetCheckTimer.Tick += InternetCheckTimer_Tick;

                // Timer for Tor health check
                torHealthCheckTimer = new System.Windows.Forms.Timer();
                torHealthCheckTimer.Interval = 3000;
                torHealthCheckTimer.Tick += TorHealthCheckTimer_Tick;

                // Timer for checking public IP changes (every 15 seconds)
                publicIPCheckTimer = new System.Windows.Forms.Timer();
                publicIPCheckTimer.Interval = 15000;
                publicIPCheckTimer.Tick += PublicIPCheckTimer_Tick;

                // Initialize default values
                UpdateProxyInfo();
                UpdateUserRelayIP("Detecting...");
                UpdateTorPublicIP("Waiting for Tor...");
                UpdateOnlineStatus("Checking...");

                // Start auto detection
                internetCheckTimer.Start();
                await CheckInternetConnection();
                _ = GetUserRealIP();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Init error: {ex.Message}");
            }
        }

        private async void PublicIPCheckTimer_Tick(object sender, EventArgs e)
        {
            if (torReady && !isClosing)
            {
                string newTorIP = await GetTorPublicIP();
                if (!string.IsNullOrEmpty(newTorIP) && newTorIP != lastTorPublicIP)
                {
                    lastTorPublicIP = newTorIP;
                    torPublicIP = newTorIP;
                    UpdateTorPublicIP(torPublicIP);
                    LogMessage($"🔄 Tor Public IP changed to: {torPublicIP}", Color.Cyan);
                    ShowNotification("IP Changed", $"Your Tor Public IP has changed to:\n{torPublicIP}");
                }
            }
        }

        private async Task<string> GetTorPublicIP()
        {
            if (!torReady) return null;

            try
            {
                string torProxyHost = "127.0.0.1";
                int torProxyPort = 9050;
                string targetHost = "api.ipify.org";
                int targetPort = 80;

                using (TcpClient client = new TcpClient())
                {
                    var connectTask = client.ConnectAsync(torProxyHost, torProxyPort);
                    if (await Task.WhenAny(connectTask, Task.Delay(5000)) != connectTask)
                        return null;

                    await connectTask;

                    using (NetworkStream stream = client.GetStream())
                    {
                        // SOCKS5 Handshake
                        stream.Write(new byte[] { 0x05, 0x01, 0x00 }, 0, 3);
                        byte[] response = new byte[2];
                        var readTask = stream.ReadAsync(response, 0, 2);
                        if (await Task.WhenAny(readTask, Task.Delay(3000)) != readTask)
                            return null;
                        await readTask;

                        if (response[0] != 0x05 || response[1] != 0x00)
                            return null;

                        // Connect via Tor
                        byte[] domainBytes = Encoding.ASCII.GetBytes(targetHost);
                        byte[] request = new byte[7 + domainBytes.Length];
                        request[0] = 0x05;
                        request[1] = 0x01;
                        request[2] = 0x00;
                        request[3] = 0x03;
                        request[4] = (byte)domainBytes.Length;
                        Array.Copy(domainBytes, 0, request, 5, domainBytes.Length);
                        request[request.Length - 2] = (byte)(targetPort >> 8);
                        request[request.Length - 1] = (byte)(targetPort & 0xFF);
                        await stream.WriteAsync(request, 0, request.Length);

                        byte[] reply = new byte[10];
                        readTask = stream.ReadAsync(reply, 0, 10);
                        if (await Task.WhenAny(readTask, Task.Delay(3000)) != readTask)
                            return null;
                        await readTask;

                        if (reply[1] != 0x00)
                            return null;

                        // Send HTTP GET Request
                        string httpRequest = $"GET / HTTP/1.1\r\nHost: {targetHost}\r\nConnection: close\r\n\r\n";
                        byte[] httpBytes = Encoding.ASCII.GetBytes(httpRequest);
                        await stream.WriteAsync(httpBytes, 0, httpBytes.Length);

                        // Read Response with timeout
                        var readStreamTask = Task.Run(async () =>
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                return await reader.ReadToEndAsync();
                            }
                        });

                        if (await Task.WhenAny(readStreamTask, Task.Delay(5000)) != readStreamTask)
                            return null;

                        string fullResponse = readStreamTask.Result;
                        string[] parts = fullResponse.Split(new[] { "\r\n\r\n" }, StringSplitOptions.None);
                        if (parts.Length > 1)
                        {
                            string ip = parts[1].Trim();
                            if (Regex.IsMatch(ip, @"^[\d\.]+$"))
                                return ip;
                        }
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private async Task GetUserRealIP()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string[] services = {
                        "https://icanhazip.com",
                        "https://api.ipify.org",
                        "https://ipinfo.io/ip"
                    };

                    foreach (var url in services)
                    {
                        try
                        {
                            client.Timeout = TimeSpan.FromSeconds(3);
                            string ip = await client.GetStringAsync(url);
                            string cleanIp = ip.Trim();

                            if (!string.IsNullOrEmpty(cleanIp) && cleanIp.Length > 6 && Regex.IsMatch(cleanIp, @"^[\d\.]+$"))
                            {
                                userPublicIP = cleanIp;
                                UpdateUserRelayIP(userPublicIP);
                                LogMessage($"🖥️ Your Public IP: {userPublicIP}", Color.Cyan);
                                return;
                            }
                        }
                        catch { }
                    }
                    UpdateUserRelayIP("Unable to detect");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Could not detect Public IP: {ex.Message}", Color.Gray);
                UpdateUserRelayIP("Unable to detect");
            }
        }

        private async Task CheckInternetConnection()
        {
            try
            {
                bool hasInternet = false;

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);

                    try
                    {
                        var response = await client.GetAsync("https://www.google.com");
                        hasInternet = response.IsSuccessStatusCode;
                    }
                    catch
                    {
                        using (var ping = new Ping())
                        {
                            try
                            {
                                var reply = await ping.SendPingAsync("8.8.8.8", 2000);
                                hasInternet = (reply != null && reply.Status == IPStatus.Success);
                            }
                            catch { }
                        }
                    }
                }

                isInternetAvailable = hasInternet;
                UpdateOnlineStatus(isInternetAvailable ? "Online ✓" : "Offline ✗");

                if (isInternetAvailable && string.IsNullOrEmpty(userPublicIP))
                {
                    _ = GetUserRealIP();
                }
            }
            catch
            {
                isInternetAvailable = false;
                UpdateOnlineStatus("Offline ✗");
            }
        }

        private async void InternetCheckTimer_Tick(object sender, EventArgs e)
        {
            await CheckInternetConnection();
        }

        private string GetTorAppDataPath()
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "safeyoutor");
            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);
            return Path.Combine(appDataPath, "tor.exe");
        }

        private bool EnsureTorExists()
        {
            string torPath = GetTorAppDataPath();

           
            if (File.Exists(torPath))
            {
                
                try
                {
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(torPath);
                    if (versionInfo.ProductName?.Contains("Tor") == true)
                    {
                      
                        File.SetAttributes(torPath, File.GetAttributes(torPath) | FileAttributes.Hidden);
                        return true;
                    }
                }
                catch { }

                
                try { File.Delete(torPath); } catch { }
            }

           
            try
            {
                byte[] torBytes = Properties.Resources.tor;
                if (torBytes != null && torBytes.Length > 0)
                {
                    File.WriteAllBytes(torPath, torBytes);
                    File.SetAttributes(torPath, File.GetAttributes(torPath) | FileAttributes.Hidden);
                    LogMessage($"✅ tor.exe exported to AppData folder", Color.Green);
                    return true;
                }
                else
                {
                    LogMessage("❌ tor.exe resource not found or empty", Color.Red);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"❌ Failed to export tor.exe: {ex.Message}", Color.Red);
                return false;
            }
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Check and export tor.exe before starting
                if (!EnsureTorExists())
                {
                    LogMessage("❌ Cannot start Tor: tor.exe is missing!", Color.Red);
                    UpdateStatus("Error: tor.exe missing", Color.Red);
                    return;
                }

                if (torProcess != null && !torProcess.HasExited)
                {
                    LogMessage("Tor is already running!", Color.Yellow);
                    return;
                }

                btnStart.Enabled = false;
                btnStop.Enabled = true;
                richLog.Clear();
                currentProgress = 0;
                torReady = false;
                lastTorPublicIP = "";
                UpdateProgress(0);
                UpdateStatus("Starting Tor...", Color.Yellow);
                UpdateTorPublicIP("Waiting for Tor...");

                // Kill any existing Tor processes
                foreach (var process in Process.GetProcessesByName("tor"))
                {
                    try { process.Kill(); await Task.Delay(500); } catch { }
                }

                // Use Tor path from AppData
                string torPath = GetTorAppDataPath();
                if (!File.Exists(torPath))
                {
                    LogMessage("❌ ERROR: tor.exe not found!", Color.Red);
                    UpdateStatus("Error: tor.exe not found", Color.Red);
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    return;
                }

                torProcess = new Process();
                torProcess.StartInfo.FileName = torPath;
                torProcess.StartInfo.UseShellExecute = false;
                torProcess.StartInfo.RedirectStandardOutput = true;
                torProcess.StartInfo.RedirectStandardError = true;
                torProcess.StartInfo.CreateNoWindow = true;
                torProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                torProcess.OutputDataReceived += TorProcess_OutputDataReceived;
                torProcess.ErrorDataReceived += TorProcess_ErrorDataReceived;

                torProcess.Start();
                torProcess.BeginOutputReadLine();
                torProcess.BeginErrorReadLine();

                LogMessage($"✅ Tor started (PID: {torProcess.Id})", Color.Green);
                UpdateStatus("Tor is bootstrapping...", Color.Yellow);
                torHealthCheckTimer?.Start();

                _ = Task.Run(() =>
                {
                    torProcess.WaitForExit();
                    this.Invoke(new Action(() =>
                    {
                        if (!torReady && !isClosing)
                        {
                            LogMessage("❌ Tor exited unexpectedly!", Color.Red);
                            UpdateStatus("Tor stopped unexpectedly", Color.Red);
                            btnStart.Enabled = true;
                            btnStop.Enabled = false;
                            torReady = false;
                            ShowNotification("Tor Crashed", "Tor process exited unexpectedly!");
                        }
                    }));
                });
            }
            catch (Exception ex)
            {
                LogMessage($"❌ Error: {ex.Message}", Color.Red);
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }

        }

        private void TorProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                this.Invoke(new Action(() => ProcessTorOutput(e.Data)));
        }

        private void TorProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                this.Invoke(new Action(() => ProcessTorOutput(e.Data, true)));
        }

        private async void ProcessTorOutput(string line, bool isError = false)
        {
            var bootMatch = Regex.Match(line, @"Bootstrapped\s+(\d+)%");
            if (bootMatch.Success)
            {
                int progress = int.Parse(bootMatch.Groups[1].Value);
                if (progress > currentProgress)
                {
                    currentProgress = progress;
                    UpdateProgress(progress);

                    if (progress == 100)
                    {
                        torReady = true;
                        UpdateStatus("✅ Tor is ready!", Color.Green);
                        LogMessage("🎉 Tor bootstrap complete!", Color.Green);

                        // Wait 2-3 seconds before getting IP and launching browser
                        

                        // Wait additional 1 second then launch browser
                        await Task.Delay(1000);

                        if (!string.IsNullOrEmpty(selectedBrowserPath) && File.Exists(selectedBrowserPath))
                        {
                            string browserName = Path.GetFileNameWithoutExtension(selectedBrowserPath).ToLower();
                            bool isRunning = false;

                            // Check if browser process is already running
                            foreach (var process in Process.GetProcesses())
                            {
                                try
                                {
                                    string processName = process.ProcessName.ToLower();
                                    if (processName == browserName)
                                    {
                                        isRunning = true;
                                        break;
                                    }
                                }
                                catch { }
                            }

                            if (isRunning)
                            {
                                LogMessage($"⚠️ {Path.GetFileName(selectedBrowserPath)} is already running! Not launching a new instance.", Color.Yellow);
                                ShowNotification("Browser Running", $"{Path.GetFileName(selectedBrowserPath)} is already open.\nPlease use the existing window.");
                            }
                            else
                            {
                                LogMessage($"🚀 Launching {Path.GetFileName(selectedBrowserPath)}...", Color.Green);
                                try
                                {
                                    Process.Start(selectedBrowserPath);
                                    LogMessage($"✅ Browser launched successfully", Color.Green);
                                }
                                catch (Exception ex)
                                {
                                    LogMessage($"❌ Failed to launch browser: {ex.Message}", Color.Red);
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(selectedBrowserPath))
                        {
                            LogMessage($"⚠️ Browser file not found: {selectedBrowserPath}", Color.Orange);
                        }

                        await Task.Delay(3500);

                        // Get initial Tor IP
                        string ip = await GetTorPublicIP();
                        if (!string.IsNullOrEmpty(ip))
                        {
                            torPublicIP = ip;
                            lastTorPublicIP = ip;
                            UpdateTorPublicIP(torPublicIP);
                            LogMessage($"🌐 Tor Public IP: {torPublicIP}", Color.Green);
                            ShowNotification("Tor Connected", $"Tor is ready!\nExit IP: {torPublicIP}");
                        }

                        // Start the public IP check timer
                        publicIPCheckTimer?.Start();

                    }
                }
            }

            var socksMatch = Regex.Match(line, @"Opened Socks listener on ([\d\.]+):(\d+)");
            if (socksMatch.Success)
            {
                torProxyIP = socksMatch.Groups[1].Value;
                torProxyPort = socksMatch.Groups[2].Value;
                UpdateProxyInfo();
            }

            if (!bootMatch.Success && !line.Contains("Bootstrapped"))
            {
                Color logColor = Color.LightGray;
                if (line.Contains("[notice]")) logColor = Color.White;
                else if (line.Contains("[warn]")) logColor = Color.Orange;
                else if (line.Contains("[error]")) logColor = Color.Red;
                LogMessage(line, logColor);
            }

            if (richLog != null)
            {
                richLog.SelectionStart = richLog.TextLength;
                richLog.ScrollToCaret();
            }
        }

        private async void TorHealthCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!isClosing && torProcess != null && !torProcess.HasExited && torReady)
            {
                try
                {
                    using (var tcpClient = new TcpClient())
                    {
                        var connectTask = tcpClient.ConnectAsync(torProxyIP, int.Parse(torProxyPort));
                        var timeoutTask = Task.Delay(1000);

                        if (await Task.WhenAny(connectTask, timeoutTask) == connectTask && !connectTask.IsFaulted)
                        {
                            UpdateStatus("✅ Tor is running securely", Color.Green);
                        }
                        else
                        {
                            LogMessage("⚠️ Tor proxy not responding!", Color.Red);
                            UpdateStatus("Tor proxy issue!", Color.Red);
                        }
                    }
                }
                catch { }
            }
            else if (!isClosing && torProcess != null && torProcess.HasExited && torReady)
            {
                LogMessage("🚨 Tor stopped unexpectedly!", Color.Red);
                UpdateStatus("Tor stopped - UNSAFE!", Color.Red);
                torReady = false;
                torHealthCheckTimer?.Stop();
                ShowNotification("Tor Crashed", "Tor stopped unexpectedly! Your connection is not safe.");
            }
        }

        private void UpdateProgress(int percent)
        {
            if (progressBarTor?.InvokeRequired == true)
            {
                progressBarTor.Invoke(new Action(() => UpdateProgress(percent)));
                return;
            }
            if (progressBarTor != null) progressBarTor.Value = percent;
            if (lblProgressPercent != null) lblProgressPercent.Text = $"{percent}%";
        }

        private void UpdateStatus(string status, Color color)
        {
            if (lblStatus?.InvokeRequired == true)
            {
                lblStatus.Invoke(new Action(() => UpdateStatus(status, color)));
                return;
            }
            if (lblStatus != null)
            {
                lblStatus.Text = $"✅ Status: {status}";
                lblStatus.ForeColor = color;
            }
        }

        private void UpdateProxyInfo()
        {
            if (lblProxy?.InvokeRequired == true)
            {
                lblProxy.Invoke(new Action(() => UpdateProxyInfo()));
                return;
            }
            if (lblProxy != null)
                lblProxy.Text = $"🔌 SOCKS5 Proxy: {torProxyIP}:{torProxyPort}";
        }

        private void UpdateUserRelayIP(string ip)
        {
            if (lblUserRelayIP?.InvokeRequired == true)
            {
                lblUserRelayIP.Invoke(new Action(() => UpdateUserRelayIP(ip)));
                return;
            }
            if (lblUserRelayIP != null)
                lblUserRelayIP.Text = $"🖥️ Your Public IP: {ip}";
        }

        private void UpdateTorPublicIP(string ip)
        {
            if (lblTorPublicIP?.InvokeRequired == true)
            {
                lblTorPublicIP.Invoke(new Action(() => UpdateTorPublicIP(ip)));
                return;
            }
            if (lblTorPublicIP != null)
                lblTorPublicIP.Text = $"🌐 Tor Public IP: {ip}";
        }

        private void UpdateOnlineStatus(string status)
        {
            if (lblOnlineStatus?.InvokeRequired == true)
            {
                lblOnlineStatus.Invoke(new Action(() => UpdateOnlineStatus(status)));
                return;
            }
            if (lblOnlineStatus != null)
            {
                lblOnlineStatus.Text = $"📡 Internet: {status}";
                lblOnlineStatus.ForeColor = status.Contains("Online") ? Color.Green : Color.Red;
            }
        }

        private void LogMessage(string message, Color color)
        {
            if (richLog?.InvokeRequired == true)
            {
                richLog.Invoke(new Action(() => LogMessage(message, color)));
                return;
            }
            if (richLog != null)
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                string formattedMessage = $"[{timestamp}] {message}";

                richLog.SelectionStart = richLog.TextLength;
                richLog.SelectionColor = color;
                richLog.AppendText(formattedMessage + Environment.NewLine);
                richLog.ScrollToCaret();
            }
        }

        private void ShowNotification(string title, string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ShowNotification(title, message)));
                return;
            }

            try
            {
                if (notifyIcon == null)
                {
                    notifyIcon = new NotifyIcon();
                    notifyIcon.Icon = SystemIcons.Shield;
                }

                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(5000, title, message, ToolTipIcon.Info);

                // Auto hide after 6 seconds
                Task.Delay(6000).ContinueWith(_ =>
                {
                    if (notifyIcon != null && !isClosing)
                    {
                        this.Invoke(new Action(() => notifyIcon.Visible = false));
                    }
                });
            }
            catch { }
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            if (btnStop != null) btnStop.Enabled = false;
            UpdateStatus("Stopping...", Color.Yellow);

            internetCheckTimer?.Stop();
            torHealthCheckTimer?.Stop();
            publicIPCheckTimer?.Stop();

            //foreach (var process in Process.GetProcessesByName("Firef0x"))
            //{
            //    try { process.Kill(); } catch { }
            //}

            //foreach (var process in Process.GetProcessesByName("firefox"))
            //{
            //    try { process.Kill(); } catch { }
            //}

            //foreach (var process in Process.GetProcessesByName("chrome"))
            //{
            //    try { process.Kill(); } catch { }
            //}

            //foreach (var process in Process.GetProcessesByName("tor"))
            //{
            //    try { process.Kill(); } catch { }
            //}

            if (torProcess != null && !torProcess.HasExited)
            {
                try
                {
                    torProcess.Kill();
                    await Task.Delay(100);
                    torProcess.Dispose();
                }
                catch { }
                torProcess = null;
            }

            currentProgress = 0;
            torReady = false;
            torPublicIP = "";
            lastTorPublicIP = "";
            UpdateProgress(0);
            UpdateStatus("Stopped", Color.Gray);
            UpdateTorPublicIP("Waiting for Tor...");
            if (btnStart != null) btnStart.Enabled = true;

            LogMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", Color.Gray);
            LogMessage("Tor stopped. Click START to begin again.", Color.Gray);

            if (notifyIcon != null)
                notifyIcon.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;
            internetCheckTimer?.Stop();
            torHealthCheckTimer?.Stop();
            publicIPCheckTimer?.Stop();

            foreach (var process in Process.GetProcessesByName("tor"))
            {
                try { process.Kill(); } catch { }
            }

            if (torProcess != null)
            {
                try { if (!torProcess.HasExited) torProcess.Kill(); torProcess.Dispose(); } catch { }
            }

            if (notifyIcon != null)
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
            }

            Application.Exit();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            richLog.Clear();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ko-fi.com/ashraf07");
        }
    }

    public enum NotificationType
    {
        Info,
        Warning,
        Error,
        Success
    }
}