using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YaparabiSerialMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort serialPort;
        private ManagementEventWatcher portWatcher;
        private System.Windows.Threading.DispatcherTimer connectionCheckTimer;
        private CancellationTokenSource cancellationTokenSource;
        private readonly ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
        private readonly StringBuilder terminalBuffer = new StringBuilder();
        private volatile bool isProcessingQueue;
        private const int MAX_BUFFER_SIZE = 1000000; // ~1MB text limit
        private readonly SemaphoreSlim serialPortLock = new SemaphoreSlim(1, 1);
        private CancellationTokenSource closingCts;  // Add this
        public MainWindow()
        {
            InitializeComponent();
            closingCts = new CancellationTokenSource();  // Initialize it
            SetupPortWatcher();
            InitializeSerialPortsAsync();
            autoScrollCheckBox.IsChecked = true;

            // Setup UI update timer instead of direct updates
            var uiUpdateTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100) // Update UI every 100ms
            };
            uiUpdateTimer.Tick += ProcessMessageQueue;
            uiUpdateTimer.Start();

            connectionCheckTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            connectionCheckTimer.Tick += async (s, e) => await CheckConnectionAsync();
            connectionCheckTimer.Start();
        }
        private async void ProcessMessageQueue(object sender, EventArgs e)
        {
            if (isProcessingQueue) return;
            isProcessingQueue = true;

            try
            {
                int processedCount = 0;
                while (messageQueue.TryDequeue(out string message) && processedCount < 100)
                {
                    terminalBuffer.Append(message);
                    processedCount++;

                    if (terminalBuffer.Length > MAX_BUFFER_SIZE)
                    {
                        terminalBuffer.Remove(0, terminalBuffer.Length - MAX_BUFFER_SIZE);
                    }

                    dataTextBox.Document.Blocks.Clear();
                    var paragraph = new Paragraph(new Run(terminalBuffer.ToString()));
                    dataTextBox.Document.Blocks.Add(paragraph);

                    if (autoScrollCheckBox.IsChecked == true)
                    {
                        dataTextBox.ScrollToEnd();
                    }
                }
            }
            finally
            {
                isProcessingQueue = false;
            }
        }
        private async Task CheckConnectionAsync()
        {
            if (serialPort == null) return;

            try
            {
                await serialPortLock.WaitAsync();
                try
                {
                    // First check if port still exists
                    if (!await Task.Run(() => SerialPort.GetPortNames().Contains(serialPort.PortName)))
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            DisconnectSerialPort();
                            UpdateConnectionStatus(false, "USB bağlantısı kesildi!");
                        });
                        return;
                    }

                    // Test if port is still accessible
                    var testTask = Task.Run(() =>
                    {
                        try
                        {
                            return serialPort?.IsOpen == true && serialPort?.BaseStream?.CanRead == true;
                        }
                        catch
                        {
                            return false;
                        }
                    });

                    if (await Task.WhenAny(testTask, Task.Delay(100)) != testTask || !await testTask)
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            DisconnectSerialPort();
                            UpdateConnectionStatus(false, "Bağlantı kesildi!");
                        });
                    }
                }
                finally
                {
                    serialPortLock.Release();
                }
            }
            catch (Exception)
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    DisconnectSerialPort();
                    UpdateConnectionStatus(false, "Bağlantı hatası!");
                });
            }
        }
        private void AppendToTerminal(string text)
        {
            Dispatcher.Invoke(() =>
            {
                terminalBuffer.Append($"{DateTime.Now:HH:mm:ss.fff} > {text}");

                dataTextBox.Document.Blocks.Clear();
                var paragraph = new Paragraph(new Run(terminalBuffer.ToString()));
                dataTextBox.Document.Blocks.Add(paragraph);

                if (autoScrollCheckBox.IsChecked == true)
                {
                    dataTextBox.ScrollToEnd();
                }
            });
        }
        private async void InitializeSerialPortsAsync()
        {
            await InitializeSerialPorts();
        }
        private void SetupPortWatcher()
        {
            try
            {
                var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 OR EventType = 3");
                portWatcher = new System.Management.ManagementEventWatcher(query);
                portWatcher.EventArrived += PortWatcher_EventArrived;
                portWatcher.Start();
            }
            catch (Exception ex)
            {
                //  receivedData.Add($"Port izleme hatası: {ex.Message}");
            }
        }

        private async void ConnectionCheckTimer_Tick(object sender, EventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                try
                {
                    // Port kontrolü
                    if (!await Task.Run(() => IsPortAvailable(serialPort.PortName)))
                    {
                        DisconnectSerialPort();
                        UpdateConnectionStatus(false, "USB bağlantısı kesildi!");
                        return;
                    }

                    // Bağlantı testi
                    await Task.Run(() =>
                    {
                        try
                        {
                            serialPort.Write(new byte[] { }, 0, 0); // Boş write ile test
                        }
                        catch
                        {
                            Dispatcher.Invoke(() =>
                            {
                                DisconnectSerialPort();
                                UpdateConnectionStatus(false, "Bağlantı kesildi!");
                            });
                        }
                    });
                }
                catch
                {
                    DisconnectSerialPort();
                    UpdateConnectionStatus(false, "Bağlantı hatası!");
                }
            }
        }
        private void PortWatcher_EventArrived(object sender, System.Management.EventArrivedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    // Mevcut portları kontrol et
                    string[] currentPorts = SerialPort.GetPortNames();

                    // Eğer bağlı olduğumuz port artık mevcut değilse
                    if (!currentPorts.Contains(serialPort.PortName))
                    {
                        DisconnectSerialPort();
                        UpdateConnectionStatus(false, "USB bağlantısı kesildi!");
                    }
                }

                // Port listesini güncelle
                InitializeSerialPorts();
            });
        }
        private enum StatusType
        {
            Normal,
            Connecting,
            Connected,
            Error
        }
        private void UpdateConnectionStatus(bool isConnected, string message = null, StatusType status = StatusType.Normal)
        {
            Brush statusColor = status switch
            {
                StatusType.Connecting => Brushes.Orange,
                StatusType.Connected => Brushes.LightGreen,
                StatusType.Error => Brushes.Red,
                StatusType.Normal => Brushes.Gray,  // Changed from Red to Gray for disconnected state
                _ => Brushes.Gray
            };

            connectionIndicator.Fill = statusColor;
            statusText.Text = message ?? (isConnected ? "Connected" : "Not Connected");
        }

        private async Task InitializeSerialPorts()
        {
            try
            {
                string selectedPort = portComboBox.SelectedItem?.ToString();

                HashSet<string> uniquePorts = new HashSet<string>(await Task.Run(() => SerialPort.GetPortNames()));
                var ports = uniquePorts.OrderBy(x => x).ToArray();

                portComboBox.Items.Clear();

                // ESP32 veya Arduino port bilgileri
                string esp32Port = "";
                string arduinoPort = "";
                int defaultBaudRate = 115200;  // ESP32 default

                foreach (string port in ports)
                {
                    if (!portComboBox.Items.Contains(port))
                    {
                        portComboBox.Items.Add(port);

                        try
                        {
                            await Task.Run(() =>
                            {
                                using (var searcher = new System.Management.ManagementObjectSearcher(
                                    "root\\CIMV2",
                                    $"SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%({port})%'"))
                                {
                                    foreach (var device in searcher.Get())
                                    {
                                        string description = device["Description"]?.ToString() ?? device["Caption"]?.ToString();
                                        if (description != null)
                                        {
                                            Dispatcher.Invoke(() =>
                                            {
                                                portDescriptionText.Text = description;

                                                // ESP32 veya CH340 içeren port ise
                                                if (description.Contains("CP210", StringComparison.OrdinalIgnoreCase) ||
                                                    description.Contains("CH340", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    esp32Port = port;
                                                    defaultBaudRate = 115200;
                                                }
                                                // Arduino içeren port ise
                                                else if (description.Contains("Arduino", StringComparison.OrdinalIgnoreCase) ||
                                                        description.Contains("USB Serial", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    arduinoPort = port;
                                                    defaultBaudRate = 9600;
                                                }
                                            });
                                        }
                                    }
                                }
                            });
                        }
                        catch
                        {
                            portDescriptionText.Text = "Port açıklaması alınamadı";
                        }
                    }
                }

                // Otomatik port seçimi
                if (!string.IsNullOrEmpty(esp32Port))
                {
                    portComboBox.SelectedItem = esp32Port;
                }
                else if (!string.IsNullOrEmpty(arduinoPort))
                {
                    portComboBox.SelectedItem = arduinoPort;
                }
                else if (portComboBox.Items.Count > 0)
                {
                    portComboBox.SelectedIndex = 0;
                }

                // Baudrate ayarla
                foreach (ComboBoxItem item in baudComboBox.Items)
                {
                    if (item.Content.ToString() == defaultBaudRate.ToString())
                    {
                        baudComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Port listesi alınamadı: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void PortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (portComboBox.SelectedItem != null)
            {
                try
                {
                    using (var searcher = new System.Management.ManagementObjectSearcher(
                        "root\\CIMV2",
                        $"SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%({portComboBox.SelectedItem})%'"))
                    {
                        foreach (var device in searcher.Get())
                        {
                            string description = device["Description"]?.ToString() ?? device["Caption"]?.ToString();
                            if (description != null)
                            {
                                portDescriptionText.Text = description;
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    portDescriptionText.Text = "No Port Info";
                }
            }
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                connectButton.IsEnabled = false;
                portComboBox.IsEnabled = false;
                baudComboBox.IsEnabled = false;
                UpdateConnectionStatus(false, "Connecting...", StatusType.Connecting);

                SerialPort tempPort = null;
                try
                {
                    string selectedPort = portComboBox.SelectedItem?.ToString();
                    int baudRate = int.Parse(((ComboBoxItem)baudComboBox.SelectedItem).Content.ToString());

                    if (string.IsNullOrEmpty(selectedPort))
                    {
                        MessageBox.Show("Please select a port", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    tempPort = new SerialPort(
                        selectedPort,
                        baudRate,
                        Parity.None,
                        8,
                        StopBits.One)
                    {
                        DtrEnable = false,
                        RtsEnable = false,
                        WriteTimeout = 500,
                        ReadTimeout = -1
                    };

                    using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
                    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, closingCts.Token);

                    bool success = await Task.Run(async () =>
                    {
                        try
                        {
                            // Check for cancellation before trying to open
                            linkedCts.Token.ThrowIfCancellationRequested();

                            tempPort.Open();
                            return true;
                        }
                        catch (OperationCanceledException)
                        {
                            return false;
                        }
                        catch
                        {
                            return false;
                        }
                    }, linkedCts.Token);

                    if (success)
                    {
                        tempPort.DataReceived += SerialPort_DataReceived;
                        serialPort = tempPort;
                        connectButton.Content = "Disconnect";
                        UpdateConnectionStatus(true, "Connected", StatusType.Connected);
                    }
                    else
                    {
                        if (tempPort != null)
                        {
                            try
                            {
                                if (tempPort.IsOpen) tempPort.Close();
                                tempPort.Dispose();
                            }
                            catch { }
                        }
                        if (!closingCts.Token.IsCancellationRequested) // Only show message if not closing
                        {
                            MessageBox.Show($"Could not open port {selectedPort}. The port might be in use or unavailable.",
                                          "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (tempPort != null)
                    {
                        try
                        {
                            if (tempPort.IsOpen) tempPort.Close();
                            tempPort.Dispose();
                        }
                        catch { }
                    }
                    if (!closingCts.Token.IsCancellationRequested) // Only show message if not closing
                    {
                        MessageBox.Show($"Connection error: {ex.Message}",
                                      "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                finally
                {
                    if (!closingCts.Token.IsCancellationRequested) // Only update UI if not closing
                    {
                        connectButton.IsEnabled = true;

                        if (serialPort == null || !serialPort.IsOpen)
                        {
                            portComboBox.IsEnabled = true;
                            baudComboBox.IsEnabled = true;
                            connectButton.Content = "Connect";
                            UpdateConnectionStatus(false, "Not Connected", StatusType.Normal);
                        }
                    }
                }
            }
            else
            {
                await DisconnectSerialPortAsync();
            }
        }

        private async Task DisconnectSerialPortAsync()
        {
            if (serialPort == null) return;

            try
            {
                await serialPortLock.WaitAsync();
                try
                {
                    serialPort.DataReceived -= SerialPort_DataReceived;
                    if (serialPort.IsOpen)
                    {
                        serialPort.RtsEnable = false;
                        serialPort.DtrEnable = false;
                        await Task.Delay(100);
                        serialPort.Close();
                    }
                    serialPort.Dispose();
                    serialPort = null;
                }
                finally
                {
                    serialPortLock.Release();
                }

                await Task.Delay(500);

                await Dispatcher.InvokeAsync(() =>
                {
                    connectButton.Content = "Connect";
                    portComboBox.IsEnabled = true;
                    baudComboBox.IsEnabled = true;
                    UpdateConnectionStatus(false, "Not Connected", StatusType.Normal);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesme hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            closingCts.Cancel(); // Cancel any ongoing operations

            if (connectionCheckTimer != null)
            {
                connectionCheckTimer.Stop();
            }

            if (portWatcher != null)
            {
                portWatcher.Stop();
                portWatcher.Dispose();
            }

            DisconnectSerialPort();
            closingCts.Dispose(); // Clean up
            base.OnClosing(e);
        }
        private void DisconnectSerialPort()
        {
            if (serialPort != null)
            {
                try
                {
                    // Önce event handler'ı kaldır
                    serialPort.DataReceived -= SerialPort_DataReceived;

                    if (serialPort.IsOpen)
                    {
                        // Sinyalleri kapat
                        serialPort.RtsEnable = false;
                        serialPort.DtrEnable = false;
                        System.Threading.Thread.Sleep(100);

                        // Portu kapat
                        serialPort.Close();
                    }

                    serialPort.Dispose();
                    serialPort = null;

                    System.Threading.Thread.Sleep(500); // Portun tam kapanması için bekle

                    connectButton.Content = "Connect";
                    portComboBox.IsEnabled = true;
                    baudComboBox.IsEnabled = true;

                    UpdateConnectionStatus(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bağlantı kesme hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private readonly StringBuilder messageBuffer = new StringBuilder();
        private async void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort == null) return;

            try
            {
                await serialPortLock.WaitAsync();
                try
                {
                    if (!serialPort.IsOpen) return;

                    // Read available data
                    string data = await Task.Run(() =>
                    {
                        try
                        {
                            serialPort.Encoding = Encoding.UTF8;
                            return serialPort.ReadExisting();
                        }
                        catch (TimeoutException)
                        {
                            return string.Empty;
                        }
                    });

                    if (!string.IsNullOrEmpty(data))
                    {
                        // Add the new data to our buffer
                        messageBuffer.Append(data);

                        // Process complete lines
                        string bufferStr = messageBuffer.ToString();
                        int newlineIndex;

                        while ((newlineIndex = bufferStr.IndexOf('\n')) != -1)
                        {
                            // Extract the complete line
                            string line = bufferStr.Substring(0, newlineIndex).Trim();
                            if (!string.IsNullOrEmpty(line))
                            {
                                messageQueue.Enqueue($"{DateTime.Now:HH:mm:ss.fff} > {line}{Environment.NewLine}");
                            }

                            // Remove the processed line from the buffer
                            bufferStr = bufferStr.Substring(newlineIndex + 1);
                        }

                        // Update the buffer with any remaining incomplete data
                        messageBuffer.Clear();
                        messageBuffer.Append(bufferStr);
                    }
                }
                finally
                {
                    serialPortLock.Release();
                }
            }
            catch (Exception ex)
            {
                if (ex is System.IO.IOException || ex is InvalidOperationException)
                {
                    await Dispatcher.InvokeAsync(() => DisconnectSerialPort());
                }
            }
        }


        private bool IsPortAvailable(string portName)
        {
            try
            {
                return SerialPort.GetPortNames().Contains(portName);
            }
            catch
            {
                return false;
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await SendDataAsync();
        }

        private async void SendTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await SendDataAsync();
            }
        }


        private async Task SendDataAsync()
        {
            if (serialPort == null || !serialPort.IsOpen) return;

            string textToSend = sendTextBox.Text;
            if (string.IsNullOrEmpty(textToSend)) return;

            try
            {
                await serialPortLock.WaitAsync();
                try
                {
                    bool sendSuccess = await Task.Run(() =>
                    {
                        try
                        {
                            serialPort.WriteLine(textToSend);
                            return true;
                        }
                        catch (TimeoutException)
                        {
                            return false;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    });

                    if (sendSuccess)
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            sendTextBox.Clear();
                            UpdateConnectionStatus(true, "Connected", StatusType.Connected);
                        });
                    }
                    else
                    {
                        UpdateConnectionStatus(true, "Error sending data", StatusType.Error);
                        await Task.Delay(2000); // Show error for 2 seconds
                        UpdateConnectionStatus(true, "Connected", StatusType.Connected);
                    }
                }
                finally
                {
                    serialPortLock.Release();
                }
            }
            catch (Exception ex)
            {
                UpdateConnectionStatus(true, "Error sending data", StatusType.Error);
                await Task.Delay(2000);
                UpdateConnectionStatus(true, "Connected", StatusType.Connected);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            terminalBuffer.Clear();
            dataTextBox.Document.Blocks.Clear();
        }
        private void AutoScrollCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (autoScrollCheckBox.IsChecked == true && dataTextBox != null)
            {
                dataTextBox.ScrollToEnd();
            }
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                try
                {
                    // RTS true, DTR false yaparak başla
                    serialPort.RtsEnable = true;
                    serialPort.DtrEnable = false;
                    System.Threading.Thread.Sleep(50);

                    // Her ikisini de true yap
                    serialPort.RtsEnable = true;
                    serialPort.DtrEnable = true;
                    System.Threading.Thread.Sleep(50);

                    // RTS false yap
                    serialPort.RtsEnable = false;
                    System.Threading.Thread.Sleep(50);

                    // Son durumu ayarla
                    serialPort.RtsEnable = true;
                    serialPort.DtrEnable = true;

                    //receivedData.Add($"{DateTime.Now:HH:mm:ss.fff} > Device reset requested");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Reset hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
