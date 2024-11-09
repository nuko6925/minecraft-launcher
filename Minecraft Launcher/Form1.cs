using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.ProcessBuilder;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using XboxAuthNet.Game.Accounts;

namespace Minecraft_Launcher;

[SuppressMessage("Usage", "CA2211:非定数フィールドは表示されません")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("Interoperability", "SYSLIB1054:コンパイル時に P/Invoke マーシャリング コードを生成するには、\'DllImportAttribute\' の代わりに \'LibraryImportAttribute\' を使用します")]
[SuppressMessage("ReSharper", "AsyncVoidMethod")]
public partial class Form1 : Form
{
    [DllImport("kernel32.dll")]
    private static extern bool AllocConsole();
    private const string News = "https://mcupdate.tumblr.com/";//"https://www.minecraft.net/ja-jp/articles";

    public static readonly Dictionary<string, string> Versions = new();
    private static bool _setupList;

    private const string DbName = "mlauncher.db";

    public static bool IsLProfile;
    public static bool IsNew;
    private const string AppVer = " 1.2.6";
    private static IXboxGameAccount? _account;

    private static (string, string, string?) _playedVersion = ("a", "a", "a");


    public static List<ProfileDetails> LauncherProfile = [];

    private static MSession _session = MSession.CreateOfflineSession("TestUser");
    private static readonly JELoginHandler LoginHandler = JELoginHandlerBuilder.BuildDefault();

    private static XboxGameAccountCollection CheckAccounts()
    {
        return LoginHandler.AccountManager.GetAccounts();
    }

    private void LogWrite(object? o)
    {
        log.Text += $"{o}\r\n";
        log.SelectionStart = log.Text.Length;
        log.Focus();
        log.ScrollToCaret();
    }

    private void Log(object? o)
    {
        Invoke((Action)(() =>
        {
            LogWrite(o);
        }));
    }

    private void LogWriteLine(object? s1)
    {
        var thread = new Thread(Log);
        thread.Start(s1);
    }

    private static async void LoadLaunchProfile()
    {
        const string profile = "launcher_profile.json";
        if (!File.Exists(profile)) return;
        var line = await File.ReadAllTextAsync(profile);
        LauncherProfile = JsonConvert.DeserializeObject<List<ProfileDetails>>(line)!;
        Console.WriteLine(line);
    }

    /*private async void LoadJvmProfile()
    {
        const string profile = "profile.txt";
        if (!File.Exists(profile)) return;
        var line = await File.ReadAllTextAsync(profile);
        line = line.Replace("[", "").Replace("]", "");
        if (line.Contains('|'))
        {
            foreach (var value in line.Split('|'))
            {
                JvmProfile.Add(value.Split(',')[0], value.Split(',')[1]);
                Console.WriteLine($"{value.Split(',')[0]}, {value.Split(',')[1]}");
                LogWriteLine($"{value.Split(',')[0]}, {value.Split(',')[1]}");
            }
        }
        else
        {
            JvmProfile.Add(line.Split(',')[0], line.Split(',')[1]);
            Console.WriteLine($"{line.Split(',')[0]}, {line.Split(',')[1]}");
            LogWriteLine($"{line.Split(',')[0]}, {line.Split(',')[1]}");
        }
    }*/
    
    public Form1()
    {
        StartPosition = FormStartPosition.CenterScreen;
        InitializeComponent();
        ServicePointManager.DefaultConnectionLimit = 256;
        timer1.Tick += (_, _) =>
        {
            try
            {
                if (_logQueue.IsEmpty)
                    return;

                var sb = new StringBuilder();
                while (_logQueue.TryDequeue(out var msg))
                {
                    if (msg.Contains("log4j:Event")) continue;
                    if (msg.Contains("log4j:Message"))
                    {
                        //    <log4j:Message><![CDATA[237 Datafixer optimizations took 318 milliseconds]]></log4j:Message>
                        var target =
                            msg.Substring(msg.IndexOf('[') + 7);
                        msg = target.Substring(0, target.LastIndexOf(']') - 1);
                        msg = $"[{DateTime.Now:HH:mm:ss}] {msg}";
                        Console.WriteLine(msg);
                    }

                    sb.AppendLine(msg);

                }

                log.AppendText(sb.ToString());
                log.ScrollToCaret();
            }
            catch (ArgumentOutOfRangeException)
            {
                // ignored
            }
            catch (IndexOutOfRangeException)
            {
                // ignored
            }
        };
        Closing += (_, _) =>
        {
            var accountId = "a";
            if (_account is { Identifier: not null })
            {
                accountId = _account.Identifier;
            }
            File.WriteAllText(DbName, $"{_playedVersion.Item1},{_playedVersion.Item2},{accountId}");
        };
        Shown += async (_, _) =>
        {
            Text += AppVer;
            _webView2.Size = new Size(ClientSize.Width, ClientSize.Height - 80);
            newButton.SetBounds(5, ClientSize.Height - 28, 100, 20);
            editButton.SetBounds(110, ClientSize.Height - 28, 100, 20);
            refreshButton.SetBounds(215, ClientSize.Height - 28, 60, 20);
            label1.SetBounds(5, ClientSize.Height - 49, 45, 20);
            combo1.SetBounds(50, ClientSize.Height - 51, 160, 20);
            combo2.SetBounds(210, ClientSize.Height - 51, 60, 20);
            playButton.SetBounds(275, ClientSize.Height - 51, ClientSize.Width - 550, 45);
            loginButton.Location = new Point(ClientSize.Width - loginButton.Width - 35,
                ClientSize.Height - (loginButton.Height + 5));
            _playedVersion = LoadVersionType();
            Console.WriteLine("1");
            var accounts = CheckAccounts();
            if (_playedVersion.Item3 != null && !_playedVersion.Item3.Equals("a"))
            {
                Console.WriteLine($"{_playedVersion.Item3}");
                try
                {
                    _account = LoginHandler.AccountManager.GetAccounts().GetAccount(_playedVersion.Item3);
                    _session = await LoginHandler.Authenticate(_account);
                    Console.WriteLine("2");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    _session = MSession.CreateOfflineSession("TestUser");
                    Console.WriteLine("3");
                }
            }
            else if (accounts.Count >= 1)
            {
                _account ??= LoginHandler.AccountManager.GetDefaultAccount();
                _session = await LoginHandler.Authenticate(_account);
                Console.WriteLine("4");
            }
            else
            {
                _session = MSession.CreateOfflineSession("TestUser");
                Console.WriteLine("5");
            }

            label2.Text = $"Welcome, {_session.Username}\r\nReady to play Minecraft {combo1.Text}";
            Console.WriteLine($"Welcome, {_session.Username} Ready to play Minecraft {combo1.Text}");
            label2.Location = new Point(ClientSize.Width - (label2.Width + 5), ClientSize.Height - (label2.Height + 28));
            log.Size = new Size(ClientSize.Width, ClientSize.Height - 80);
            listView.Size = new Size(ClientSize.Width, ClientSize.Height - 80);
            //LoadJvmProfile();
            LoadLaunchProfile();
        };
        SizeChanged += (_, _) =>
        {
            _webView2.Size = new Size(ClientSize.Width, ClientSize.Height - 80);
            newButton.SetBounds(5, ClientSize.Height - 28, 100, 20);
            editButton.SetBounds(110, ClientSize.Height - 28, 100, 20);
            refreshButton.SetBounds(215, ClientSize.Height - 28, 60, 20);
            label1.SetBounds(5, ClientSize.Height - 49, 45, 20);
            label2.Location = new Point(ClientSize.Width - (label2.Width + 5), ClientSize.Height - (label2.Height + 28));
            combo1.SetBounds(50, ClientSize.Height - 51, 160, 20);
            combo2.SetBounds(210, ClientSize.Height - 51, 60, 20);
            playButton.SetBounds(275, ClientSize.Height - 51, ClientSize.Width - 550, 45);
            loginButton.Location = new Point(ClientSize.Width - loginButton.Width - 35,
                ClientSize.Height - (loginButton.Height + 5));
            log.Size = new Size(ClientSize.Width, ClientSize.Height - 80);
            listView.Size = new Size(ClientSize.Width, ClientSize.Height - 80);
        };
        tabControl.SelectedIndexChanged += (_, _) =>
        {
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    _webView2.Show();
                    listView.Hide();
                    log.Hide();
                    break;
                case 1:
                    _webView2.Hide();
                    listView.Hide();
                    log.Show();
                    break;
                case 2:
                    _webView2.Hide();
                    listView.Show();
                    log.Hide();
                    if (!_setupList)
                    {
                        foreach (var prof in LauncherProfile)
                        {
                            var item = listView.Items.Add(prof.Name);
                            item.SubItems.Add(prof.Version);
                            item.SubItems.Add(prof.Jvm);
                            item.SubItems.Add(prof.Dir);
                            if (!string.IsNullOrEmpty(prof.Runtime))
                            {
                                item.SubItems.Add(prof.Runtime);
                            }
                            else
                            {
                                item.SubItems.Add(MinecraftPath.GetOSDefaultPath() + @"\runtime");
                            }
                        }
                        /*foreach (var prof in JvmProfile)
                        {
                            var item = listView.Items.Add(prof.Key);
                            item.SubItems.Add(prof.Value.TrimStart());
                            Console.WriteLine($"{prof.Key}, {prof.Value.TrimStart()}");
                        }*/
                        _setupList = true;
                    }
                    break;
            }
        };
        editButton.Click += (_, _) =>
        {
            if (string.IsNullOrEmpty(combo1.Text)) return;
            IsLProfile = true;
            if (combo2.Text.Equals("LProfile"))
            {
                var profile = LauncherProfile.Find(a => a.Name.Equals(combo1.Text));
                ProfileEditor.ProfileStr = profile.Name;
                ProfileEditor.GameDirStr = profile.Dir;
                ProfileEditor.JvmStr = profile.Jvm;
                ProfileEditor.VerStr = profile.Version;
                ProfileEditor.TypeStr = profile.Type;
                ProfileEditor.RuntimeStr = string.IsNullOrEmpty(profile.Runtime) ? null : profile.Runtime;
            }
            else
            {
                ProfileEditor.ProfileStr = combo1.Text;
                ProfileEditor.GameDirStr = MinecraftPath.GetOSDefaultPath();
                ProfileEditor.JvmStr = "-Xmx2G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=16M";
                ProfileEditor.VerStr = combo1.Text;
                ProfileEditor.TypeStr = combo2.Text;
                ProfileEditor.RuntimeStr = MinecraftPath.GetOSDefaultPath() + @"\runtime";
            }
            /*else
            {
                IsLProfile = false;
                ProfileEditor.VerStr = combo1.Text;
                var defaultJvm = "-Xmx2G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=16M";
                JvmProfile.TryGetValue(combo1.Text, out var value);
                if (!string.IsNullOrEmpty(value))
                {
                    defaultJvm = value;
                }
                ProfileEditor.JvmStr = defaultJvm;
            }*/

            IsNew = false;
            
            var profileEditor = new ProfileEditor();
            profileEditor.Show();
        };
        newButton.Click += (_, _) =>
        {
            IsLProfile = true;//combo2.Text.Equals("LProfile");
            IsNew = true;
            
            var profileEditor = new ProfileEditor();
            profileEditor.Show();
        };
        loginButton.Click += async (_, _) =>
        {
            loginButton.Enabled = false;
            playButton.Enabled = false;

            try
            {
                var selectedAccount = LoginHandler.AccountManager.NewAccount();
                _account = selectedAccount;
                _session = await LoginHandler.Authenticate(selectedAccount);
                Console.WriteLine(
                    "Login result: \n" +
                    $"Username: {_session.Username}\n" +
                    $"UUID: {_session.UUID}");
                LogWriteLine("Login result: \n" +
                             $"Username: {_session.Username}\n" +
                             $"UUID: {_session.UUID}");
                label2.Text = $"Welcome, {_session.Username}\r\nReady to play Minecraft {combo1.Text}";
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                loginButton.Enabled = true;
                playButton.Enabled = true;
                label2.Text = "An internal error!\r\nPlease try again!";
            }
        };
        playButton.Click += async (_, _) =>
        {
            playButton.Enabled = false;
            combo1.Enabled = false;
            playButton.Text = "Installing...";
            var type = combo2.Text;
            var ver = combo1.Text;
            var jvmArgument =
                "-XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=16M";
            //var path = MinecraftPath.GetOSDefaultPath();//new MinecraftPath($"./{combo1.Text.Replace('.', '_')}");
            var path = new MinecraftPath
            {
                Versions = MinecraftPath.GetOSDefaultPath() + "\\versions",
                Assets = MinecraftPath.GetOSDefaultPath() + "\\assets",
                BasePath = MinecraftPath.GetOSDefaultPath(),
                Library = MinecraftPath.GetOSDefaultPath() + "\\libraries",
                Resource = MinecraftPath.GetOSDefaultPath() + "\\resources",
                Runtime = MinecraftPath.GetOSDefaultPath() + "\\runtime"
            };
            var runtime = path.Runtime;
            if (combo2.Text.Equals("LProfile"))
            {
                var profile = LauncherProfile.Find(a => a.Name.Equals(ver));
                path.BasePath = profile.Dir;
                type = profile.Type;
                jvmArgument = profile.Jvm;
                ver = profile.Version;
                if (!string.IsNullOrEmpty(profile.Runtime))
                {
                    runtime = profile.Runtime;
                }
            }

            _playedVersion = (type, ver, _account!.Identifier!);
            var launcher = new MinecraftLauncher(path);
            launcher.FileProgressChanged += (_, args) =>
            {
                Console.WriteLine($"Name: {args.Name}");
                Console.WriteLine($"Type: {args.EventType}");
                Console.WriteLine($"Total: {args.TotalTasks}");
                Console.WriteLine($"Progressed: {args.ProgressedTasks}");
                //Log($"Name: {args.Name}\r\nType: {args.EventType}\r\nTotal: {args.TotalTasks}\r\nProgressed: {args.ProgressedTasks}");
            };
            launcher.ByteProgressChanged += (_, args) =>
            {
                playButton.Text = $"{args.ProgressedBytes / 1024.0 / 1024.0:F1} MB / {args.TotalBytes / 1024.0 / 1024.0:F1} MB";
                Console.WriteLine($"{args.ProgressedBytes} bytes / {args.TotalBytes} bytes");
                //Log($"{args.ProgressedBytes} bytes / {args.TotalBytes} bytes");
            };
            
            if (!type.Equals("local"))
                await launcher.InstallAsync(ver);
            
            /*if (JvmProfile.TryGetValue(ver, out var value))
            {
                jvmArgument = value;
            }*/

            Process? process;
            if (combo2.Text.Equals("LProfile"))
            {
                process = await launcher.BuildProcessAsync(ver, new MLaunchOption
                {
                    Session = _session,
//                Session = MSession.CreateOfflineSession("TestPlayer"),
                    ExtraJvmArguments = 
                    [
                        new MArgument(jvmArgument.TrimStart().Split(' '))
                    ],
                });
                if (!string.IsNullOrEmpty(runtime))
                {
                    if (File.Exists(Path.Combine(runtime, "javaw.exe")))
                    {
                        process.StartInfo.FileName = process.StartInfo.FileName.Replace(
                            $@"C:\Users\{Environment.UserName}\AppData\Roaming\.minecraft\runtime\windows-x64\jre-legacy\bin",
                            runtime);
                    }
                }
            }
            else
            {
                process = await launcher.BuildProcessAsync(ver, new MLaunchOption
                {
                    Session = _session,
//                Session = MSession.CreateOfflineSession("TestPlayer"),
                    ExtraJvmArguments = 
                    [
                        new MArgument(jvmArgument.TrimStart().Split(' '))
                    ],
                });
            }
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            process.EnableRaisingEvents = true;
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.ErrorDataReceived += Process_DataReceived;
            process.OutputDataReceived += Process_DataReceived;
            Output(process.StartInfo.Arguments);
            playButton.Enabled = true;
            playButton.Text = "Play";
            combo1.Enabled = true;
        };
//        AllocConsole();
        var launcher = new MinecraftLauncher();
        var versions = launcher.GetAllVersionsAsync().Result;
        foreach (var ver in versions)
        {
            Versions.Add(ver.Name, ver.Type!);
            if (!combo2.Items.Contains(ver.Type))
            {
                combo2.Items.Add(ver.Type!);
            }
            if (ver.Type == "release")
                combo1.Items.Add(ver.Name);
        }

        combo2.Items.Add("LProfile");

        combo2.Text = "release";
        refreshButton.Click += (_, _) =>
        {
            combo1.Items.Clear();
            /*if (combo2.Text.Equals("JProfile"))
            {
                playButton.Enabled = false;
                if (JvmProfile.Count >= 1)
                {
                    foreach (var prof in JvmProfile)
                    {
                        combo1.Items.Add(prof.Key);
                    }
                }
            }
            else */if (combo2.Text.Equals("LProfile"))
            {
                playButton.Enabled = true;
                if (LauncherProfile.Count >= 1)
                {
                    foreach (var prof in LauncherProfile)
                    {
                        combo1.Items.Add(prof.Name);
                    }
                }
            }
            else
            {
                playButton.Enabled = true;
                foreach (var ver in versions)
                {
                    if (ver.Type != combo2.Text) continue;
                    combo1.Items.Add(ver.Name);
                }
            }

            if (combo1.Items.Count >= 1)
            {
                combo1.SelectedIndex = 0;
            }
        };

        combo2.SelectedIndexChanged += (_, _) =>
        {
            combo1.Items.Clear();
            /*if (combo2.Text.Equals("JProfile"))
            {
                playButton.Enabled = false;
                if (JvmProfile.Count >= 1)
                {
                    foreach (var prof in JvmProfile)
                    {
                        combo1.Items.Add(prof.Key);
                    }
                }
            }
            else */if (combo2.Text.Equals("LProfile"))
            {
                playButton.Enabled = true;
                if (LauncherProfile.Count >= 1)
                {
                    foreach (var prof in LauncherProfile)
                    {
                        combo1.Items.Add(prof.Name);
                    }
                }
            }
            else
            {
                playButton.Enabled = true;
                foreach (var ver in versions)
                {
                    if (ver.Type != combo2.Text) continue;
                    combo1.Items.Add(ver.Name);
                }
            }

            if (combo1.Items.Count >= 1)
            {
                combo1.SelectedIndex = 0;
            }
        };

        combo1.SelectedIndex = 0;
        if (!_playedVersion.Item1.Equals("a"))
        {
            combo2.Text = _playedVersion.Item1;
            
            combo1.Items.Clear();
            /*if (_playedVersion.Item1.Equals("JProfile"))
            {
                playButton.Enabled = false;
                if (JvmProfile.Count >= 1)
                {
                    foreach (var prof in JvmProfile)
                    {
                        combo1.Items.Add(prof.Key);
                    }
                }
            }
            else */if (_playedVersion.Item1.Equals("LProfile"))
            {
                playButton.Enabled = true;
                if (LauncherProfile.Count >= 1)
                {
                    foreach (var prof in LauncherProfile)
                    {
                        combo1.Items.Add(prof.Name);
                    }
                }
            }
            else
            {
                playButton.Enabled = true;
                foreach (var ver in versions)
                {
                    if (ver.Type != _playedVersion.Item1) continue;
                    combo1.Items.Add(ver.Name);
                }
            }
            
            combo1.Text = _playedVersion.Item2;
        }
        _webView2.Focus();
        InitializeAsync();
    }

    private (string, string, string) LoadVersionType()
    {
        if (!File.Exists(DbName))
        {
            LoginHandler.AccountManager.ClearAccounts();
            return ("a", "a", "a");
        }
        var line = File.ReadAllText(DbName);
        return line.Equals(",") ? ("a", "a", "a") : (line.Split(',')[0], line.Split(',')[1], line.Split(',')[2]);

    }
    
    private readonly ConcurrentQueue<string> _logQueue = new();

    private void Process_DataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
            _logQueue.Enqueue(e.Data);
    }
    private void Output(string msg) => _logQueue.Enqueue(msg);
        
    private async void InitializeAsync()
    {
        var cacheDir = Path.Combine(Path.GetTempPath(), "MineLauncher-Cache");
        var webView2Environment = await CoreWebView2Environment.CreateAsync(null, cacheDir);
        await _webView2.EnsureCoreWebView2Async(webView2Environment);
        _webView2.NavigationCompleted += WebView_NavigationCompleted!;
        _webView2.CoreWebView2.Settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36";
        _webView2.CoreWebView2.Navigate(News);
    }
        
    private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        if (_webView2.Visible) return;
        _webView2.Show();
        _webView2.CoreWebView2.Reload();
        _webView2.ZoomFactor = 0.5f;
    }
}