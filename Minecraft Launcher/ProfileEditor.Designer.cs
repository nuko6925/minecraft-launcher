using System.ComponentModel;

namespace Minecraft_Launcher;

sealed partial class ProfileEditor
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        profileName = new();
        gameDir = new();
        useVersion = new();
        jvmArgument = new();
        cancelButton = new();
        deleteButton = new();
        saveButton = new();
        gameDirCheck = new();
        jvmCheck = new();
        profileNameText = new();
        gameDirText = new();
        jvmText = new();
        versionDrop = new();
        runtime = new();
        runtimeText = new();
        runtimeCheck = new();
        gameDirButton = new();
        runtimeDirButton = new();

        profileName.Location = new Point(5, 15);
        profileName.Text = "Profile Name: ";
        profileName.AutoSize = true;

        profileNameText.Location = new Point(150, 15);
        profileNameText.Size = new Size(600, 20);
        profileNameText.ScrollBars = ScrollBars.None;
        profileNameText.Multiline = false;

        gameDirCheck.Location = new Point(5, 40);
        gameDirCheck.Size = new Size(20, 20);

        gameDir.Location = new Point(gameDirCheck.Width + 10, 40);
        gameDir.Text = "Game Directory: ";
        gameDir.AutoSize = true;

        gameDirText.Location = new Point(150, 40);
        gameDirText.Size = new Size(600, 20);
        gameDirText.ScrollBars = ScrollBars.None;
        gameDirText.Multiline = false;
        gameDirText.Enabled = false;

        gameDirButton.Location = new Point(500, 40);
        gameDirButton.Enabled = false;
        gameDirButton.Text = "Dir";
        gameDirButton.TextAlign = ContentAlignment.MiddleCenter;
        gameDirButton.AutoSize = true;

        useVersion.Location = new Point(5, 70);
        useVersion.Text = "Use version: ";
        useVersion.AutoSize = true;

        versionDrop.Location = new Point(150, 70);
        versionDrop.DropDownStyle = ComboBoxStyle.DropDownList;
        versionDrop.Size = new Size(600, 20);

        jvmCheck.Location = new Point(5, 100);
        jvmCheck.Size = new Size(20, 20);

        jvmArgument.Location = new Point(jvmCheck.Width + 10, 100);
        jvmArgument.Text = "JVM Arguments: ";
        jvmArgument.AutoSize = true;

        jvmText.Location = new Point(150, 100);
        jvmText.Size = new Size(600, 20);
        jvmText.ScrollBars = ScrollBars.None;
        jvmText.Multiline = false;
        jvmText.Enabled = false;

        cancelButton.Location = new Point(5, 420);
        cancelButton.Text = "Cancel";
        cancelButton.AutoSize = true;

        saveButton.Location = new Point(750, 420);
        saveButton.Text = "Save Profile";
        saveButton.AutoSize = true;

        deleteButton.Location = new Point(750, 420);
        deleteButton.Text = "Delete Profile";
        deleteButton.AutoSize = true;

        runtime.Location = new Point(30, 130);
        runtime.AutoSize = true;
        runtime.Text = "Runtime: ";

        runtimeCheck.Location = new Point(5, 130);
        runtimeCheck.Size = new Size(20, 20);

        runtimeText.Location = new Point(150, 130);
        runtimeText.Size = new Size(600, 20);
        runtimeText.ScrollBars = ScrollBars.None;
        runtimeText.Multiline = false;
        runtimeText.Enabled = false;

        runtimeDirButton.Location = new Point(500, 130);
        runtimeDirButton.Text = "Dir";
        runtimeDirButton.TextAlign = ContentAlignment.MiddleCenter;
        runtimeDirButton.AutoSize = true;
        runtimeDirButton.Enabled = false;
        runtimeDirButton.ForeColor = Color.AliceBlue;
        gameDirButton.ForeColor = Color.AliceBlue;
        
        
        
        
        
        this.components = new Container();
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(800, 450);
        Controls.Add(profileName);
        Controls.Add(gameDir);
        Controls.Add(useVersion);
        Controls.Add(jvmArgument);
        Controls.Add(cancelButton);
        Controls.Add(saveButton);
        Controls.Add(deleteButton);
        Controls.Add(gameDirCheck);
        Controls.Add(versionDrop);
        Controls.Add(jvmCheck);
        Controls.Add(profileNameText);
        Controls.Add(gameDirText);
        Controls.Add(jvmText);
        Controls.Add(runtime);
        Controls.Add(runtimeCheck);
        Controls.Add(runtimeText);
        Controls.Add(gameDirButton);
        Controls.Add(runtimeDirButton);
        this.Text = "ProfileEditor";
        StartPosition = FormStartPosition.CenterScreen;
        TopMost = true;
    }

    private Label profileName, gameDir, useVersion, jvmArgument, runtime;
    private Button cancelButton, saveButton, deleteButton, runtimeDirButton, gameDirButton;
    private CheckBox gameDirCheck, jvmCheck, runtimeCheck;
    private TextBox profileNameText, gameDirText, jvmText, runtimeText;
    private ComboBox versionDrop;

    #endregion
}