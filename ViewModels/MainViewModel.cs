using MonitorSistema.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Visiotech.HardwareInfo;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace MonitorSistema.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly System.Timers.Timer _timer;
        private int _interval = 1000;
        private bool _isRunning;
        private readonly Random _random = new Random();
        private readonly string cpuSerial;
        private readonly string motherboardSerial;
        private readonly string gpuSerial;

        public ObservableCollection<SystemSample> Samples { get; set; } = new();
        private PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        public RelayCommand StartCommand { get; }
        public RelayCommand StopCommand { get; }
        private bool isRunning;
        public event PropertyChangedEventHandler PropertyChanged;

        public float GetCpuUsage()
        {
            return (float)Math.Round(cpuCounter.NextValue(), 2);
        }

        private float GetRamUsage()
        {
            ulong totalMemory = 0;
            ulong freeMemory = 0;

            var searcher = new System.Management.ManagementObjectSearcher(
                "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");

            foreach (var obj in searcher.Get())
            {
                totalMemory = (ulong)obj["TotalVisibleMemorySize"]; // en KB
                freeMemory = (ulong)obj["FreePhysicalMemory"];      // en KB
            }

            ulong usedMemory = totalMemory - freeMemory;
            float usage = (usedMemory / (float)totalMemory) * 100f;
            return (float)Math.Round(usage, 2);
        }

        public int Interval
        {
            get => _interval;
            set
            {
                if (value >= 500 && value <= 10000)
                {
                    _interval = value;
                    OnPropertyChanged();
                    if (_timer != null)
                        _timer.Interval = _interval;
                }
            }
        }

        public bool IsRunning
        {
            get => isRunning;
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    OnPropertyChanged();

                    StartCommand?.RaiseCanExecuteChanged();
                    StopCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        public MainViewModel()
        {
            cpuSerial = HardwareInfo.GetProcessorID();
            motherboardSerial = HardwareInfo.GetMotherboardID();
            gpuSerial = "-";//HardwareInfo.GetGpuID();

            _timer = new System.Timers.Timer(_interval);
            _timer.Elapsed += (s, e) => CaptureSample();

            StartCommand = new RelayCommand(_ => Start(), _ => !IsRunning);
            StopCommand = new RelayCommand(_ => Stop(), _ => IsRunning);

            LoadData();
        }

        private void Start()
        {
            _timer.Start();
            IsRunning = true;
        }

        private void Stop()
        {
            _timer.Stop();
            IsRunning = false;
            SaveData();
        }

        private void CaptureSample()
        {
            var sample = new SystemSample
            {
                Timestamp = DateTime.Now,
                CpuSerial = cpuSerial,
                MotherboardSerial = motherboardSerial ,
                GpuSerial = gpuSerial,
                CpuUsage = GetCpuUsage(),
                RamUsage = GetRamUsage()
            };

            App.Current.Dispatcher.Invoke(() => Samples.Insert(0, sample));
        }

        private void LoadData()
        {
            if (File.Exists("data.json"))
            {
                var data = JsonConvert.DeserializeObject<List<SystemSample>>(File.ReadAllText("data.json"));
                foreach (var item in data)
                    Samples.Add(item);
            }
        }

        private void SaveData()
        {
            File.WriteAllText("data.json", JsonConvert.SerializeObject(Samples.ToList()));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
