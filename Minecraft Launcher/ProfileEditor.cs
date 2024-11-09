using System.Diagnostics.CodeAnalysis;
using CmlLib.Core;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;

namespace Minecraft_Launcher;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("Usage", "CA2211:非定数フィールドは表示されません")]
[SuppressMessage("ReSharper", "NotAccessedField.Global")]
public sealed partial class ProfileEditor : Form
{
    public static string? GameDirStr, JvmStr, ProfileStr, VerStr, TypeStr, RuntimeStr;
    private static bool IsLProfile;
    private static string ToJsonAll(List<ProfileDetails> profile)
    {
        var json = JsonConvert.SerializeObject(profile);
        return json;
    }
    public ProfileEditor()
    {
        InitializeComponent();
        BackColor = Color.FromArgb(28, 28, 28);
        ForeColor = Color.White;
        gameDirButton.Click += (_, _) =>
        {
            using var cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = true;
            cofd.DefaultDirectory = MinecraftPath.GetOSDefaultPath();
            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                gameDirText.Text = cofd.FileName;
            }
        };
        runtimeDirButton.Click += (_, _) =>
        {
            using var cofd = new CommonOpenFileDialog();
            cofd.DefaultDirectory = MinecraftPath.GetOSDefaultPath() + @"\runtime\windows-x64\jre-legacy\bin";
            cofd.DefaultFileName = "javaw.exe";
            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                runtimeText.Text = cofd.FileName;
            }
        };
        jvmCheck.CheckedChanged += (_, _) => { jvmText.Enabled = jvmCheck.Checked; };
        gameDirCheck.CheckedChanged += (_, _) =>
        {
            gameDirText.Enabled = gameDirCheck.Checked;
            gameDirButton.Enabled = gameDirCheck.Checked;
        };
        runtimeCheck.CheckedChanged += (_, _) =>
        {
            runtimeText.Enabled = runtimeCheck.Checked;
            runtimeDirButton.Enabled = runtimeCheck.Checked;
        };
        versionDrop.SelectedIndexChanged += (_, _) =>
        {
            if (string.IsNullOrEmpty(profileNameText.Text))
            {
                profileNameText.Text = versionDrop.Text;
            }
        };
        Shown += (_, _) =>
        {
            jvmText.Width = ClientSize.Width - 160;
            gameDirText.Width = ClientSize.Width - 160 - gameDirButton.Width - 10;
            profileNameText.Width = ClientSize.Width - 160;
            versionDrop.Width = ClientSize.Width - 160;
            runtimeText.Width = ClientSize.Width - 160 - runtimeDirButton.Width - 10;
            gameDirButton.Location = new Point(ClientSize.Width - gameDirButton.Width - 5, 40);
            runtimeDirButton.Location = new Point(ClientSize.Width - runtimeDirButton.Width - 5, 130);
            cancelButton.Location = new Point(5, ClientSize.Height - cancelButton.Height - 10);
            saveButton.Location = new Point(ClientSize.Width - saveButton.Width - 5, ClientSize.Height - cancelButton.Height - 10);
            deleteButton.Location = new Point(ClientSize.Width / 2 - deleteButton.Width / 2, ClientSize.Height - cancelButton.Height - 10);
            IsLProfile = Form1.IsLProfile;
            if (IsLProfile)
            {
                Text = "Launcher Profile Editor";
                profileName.Show();
                profileNameText.Show();
                gameDirCheck.Show();
                gameDir.Show();
                gameDirText.Show();
                deleteButton.Show();
                runtime.Show();
                runtimeCheck.Show();
                runtimeText.Show();
            }
            else
            {
                Text = "Profile Editor";
                profileName.Hide();
                profileNameText.Hide();
                gameDirCheck.Hide();
                gameDir.Hide();
                gameDirText.Hide();
                deleteButton.Hide();
                runtime.Hide();
                runtimeCheck.Hide();
                runtimeText.Hide();
            }
            var launcher = new MinecraftLauncher();
            var versions = launcher.GetAllVersionsAsync().Result;
            foreach (var ver in versions)
            {
                versionDrop.Items.Add(ver.Name);
            }

            if (Form1.IsNew)
            {
                jvmText.Text = "-Xmx2G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=16M";
                gameDirText.Text = MinecraftPath.GetOSDefaultPath();
                versionDrop.SelectedIndex = 0;
                runtimeText.Text = MinecraftPath.GetOSDefaultPath() + "\\runtime";
            }
            else
            {
                jvmText.Text = JvmStr;
                gameDirText.Text = GameDirStr;
                profileNameText.Text = ProfileStr;
                versionDrop.Text = VerStr;
                RuntimeStr ??= MinecraftPath.GetOSDefaultPath() + "\\runtime";
                runtimeText.Text = RuntimeStr;
            }
        };
        SizeChanged += (_, _) =>
        {
            jvmText.Width = ClientSize.Width - 160;
            gameDirText.Width = ClientSize.Width - 160 - gameDirButton.Width - 10;
            profileNameText.Width = ClientSize.Width - 160;
            versionDrop.Width = ClientSize.Width - 160;
            runtimeText.Width = ClientSize.Width - 160 - runtimeDirButton.Width - 10;
            gameDirButton.Location = new Point(ClientSize.Width - gameDirButton.Width - 5, 40);
            runtimeDirButton.Location = new Point(ClientSize.Width - runtimeDirButton.Width - 5, 130);
            cancelButton.Location = new Point(5, ClientSize.Height - 10);
            saveButton.Location = new Point(ClientSize.Width - saveButton.Width - 5, ClientSize.Height - 10);
            deleteButton.Location = new Point(ClientSize.Width / 2 - deleteButton.Width / 2, ClientSize.Height - cancelButton.Height - 10);
        };
        deleteButton.Click += (_, _) =>
        {
            if (IsLProfile)
            {
                if (Form1.LauncherProfile.ToArray().Contains(Form1.LauncherProfile.Find(a => a.Name.Equals(profileNameText.Text))))
                {
                    Form1.LauncherProfile.Remove(Form1.LauncherProfile.Find(a => a.Name.Equals(profileNameText.Text)));
                }
                File.WriteAllText("launcher_profile.json", ToJsonAll(Form1.LauncherProfile));
            }
            /*else
            {
                Form1.JvmProfile.Remove(profileNameText.Text);
                File.WriteAllText("profile.txt", string.Join('|', Form1.JvmProfile));
            }*/
            Close();
        };
        cancelButton.Click += (_, _) =>
        {
            Close();
        };
        saveButton.Click += (_, _) =>
        {
            if (IsLProfile)
            {
                if (Form1.LauncherProfile.ToArray().Contains(Form1.LauncherProfile.Find(a => a.Name.Equals(profileNameText.Text))))
                {
                    Form1.LauncherProfile.Remove(Form1.LauncherProfile.Find(a => a.Name.Equals(profileNameText.Text)));
                }

                if (!Form1.Versions.TryGetValue(versionDrop.Text, out var type)) return;
                if (runtimeText.Text.ToLower().EndsWith("javaw.exe"))
                {
                    if (File.Exists(runtimeText.Text))
                    {
                        runtimeText.Text = runtimeText.Text.ToLower().Replace("\\javaw.exe", "");
                    }
                }
                var profile = new ProfileDetails
                {
                    Name = profileNameText.Text,
                    Dir = gameDirText.Text,
                    Jvm = jvmText.Text,
                    Type = type, //TypeStr,
                    Version = versionDrop.Text,
                    Runtime = runtimeText.Text
                };
                Form1.LauncherProfile.Add(profile);
                File.WriteAllText("launcher_profile.json", ToJsonAll(Form1.LauncherProfile));
                if (!Directory.Exists(gameDirText.Text) && !string.IsNullOrEmpty(gameDirText.Text))
                {
                    Directory.CreateDirectory(gameDirText.Text);
                }
                //Console.WriteLine(ToJson(profile));
            }
            /*else
            {
                Form1.JvmProfile.Remove(versionDrop.Text);
                Form1.JvmProfile.Add(versionDrop.Text, jvmText.Text);
                File.WriteAllText("profile.txt", string.Join('|', Form1.JvmProfile));
            }*/
            Close();
        };
    }
}