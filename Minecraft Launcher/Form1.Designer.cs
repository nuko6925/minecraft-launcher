using Microsoft.Web.WebView2.WinForms;
using Timer = System.Windows.Forms.Timer;

namespace Minecraft_Launcher;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        _webView2 = new();
        tab1 = new();
        tab2 = new();
        tab3 = new();
        tabControl = new();
        newButton = new();
        editButton = new();
        refreshButton = new();
        playButton = new();
        loginButton = new();
        label1 = new();
        label2 = new();
        combo1 = new();
        combo2 = new();
        log = new();
        timer1 = new();
        listView = new();
        this.SuspendLayout();
        
        _webView2.SetBounds(0, 20, 100, 100);
        _webView2.TabIndex = 100;
        
        newButton.SetBounds(5, 5, 5, 5);
        newButton.Text = "New Profile";
        newButton.TabIndex = 4;
        
        editButton.SetBounds(5, 5, 5, 5);
        editButton.Text = "Edit Profile";
        editButton.TabIndex = 5;
        
        refreshButton.SetBounds(5, 5, 5, 5);
        refreshButton.Text = "Refresh";
        refreshButton.TabIndex = 10;
        
        playButton.SetBounds(5, 5, 5, 5);
        playButton.Text = "Play";
        playButton.TextAlign = ContentAlignment.MiddleCenter;
        playButton.Font = new Font(playButton.Font, FontStyle.Bold);
        playButton.TabIndex = 6;
        
        loginButton.SetBounds(5, 5, 5, 5);
        loginButton.Text = "Switch User";
        loginButton.TextAlign = ContentAlignment.MiddleCenter;
        loginButton.TabIndex = 8;
        loginButton.AutoSize = true;
        
        label1.SetBounds(5, 5, 5, 5);
        label1.Text = "Profile:";
        
        label2.SetBounds(5, 5, 5, 5);
        label2.Text = "Welcome, TestUser\r\nReady to play Version";
        label2.AutoSize = true;
        label2.TextAlign = ContentAlignment.MiddleCenter;
        
        combo1.SetBounds(5, 5, 5, 5);
        combo1.DropDownStyle = ComboBoxStyle.DropDownList;
        combo1.TabIndex = 3;
        
        combo2.SetBounds(5, 5, 5, 5);
        combo2.DropDownStyle = ComboBoxStyle.DropDownList;
        combo2.TabIndex = 7;
        
        log.SetBounds(0, 20, 5, 5);
        log.Multiline = true;
        log.ReadOnly = true;
        log.ScrollBars = ScrollBars.Both;
        log.TabIndex = 9;
        log.BackColor = Color.FromArgb(27, 27, 27);
        log.ForeColor = Color.AliceBlue;

        timer1.Enabled = true;
        timer1.Interval = 1000;
        
        listView.SetBounds(0, 20, 5, 5);
        listView.Clear();
        listView.View = View.Details;
        listView.GridLines = true;
        listView.FullRowSelect = true;
        listView.Scrollable = true;

        var header1 = new ColumnHeader();
        header1.Text = "Name";
        header1.Width = 200;
        header1.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        var header2 = new ColumnHeader();
        header2.Text = "Version";
        header2.Width = 200;
        header2.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        var header3 = new ColumnHeader();
        header3.Text = "JVM";
        header3.Width = 200;
        header3.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        var header4 = new ColumnHeader();
        header4.Text = "GameDir";
        header4.Width = 200;
        header4.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        var header5 = new ColumnHeader();
        header5.Text = "Runtime";
        header5.Width = 200;
        header5.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        ColumnHeader[] headers = { header1, header2, header3, header4, header5 };
        listView.Columns.AddRange(headers);

        tab1.Text = "Update Notes";
        tab1.TabIndex = 0;
        tab1.AutoSize = true;

        tab2.Text = "Launcher Log";
        tab2.TabIndex = 1;
        tab2.AutoSize = true;

        tab3.Text = "Profile Editor";
        tab3.TabIndex = 2;
        tab3.AutoSize = true;
        
        tabControl.SetBounds(0, 0, 800, 20);
        tabControl.SelectedIndex = 0;
        tabControl.TabIndex = 0;
        
        tab2.Controls.Add(log);
        tab3.Controls.Add(listView);
        
        tabControl.Controls.Add(tab1);
        tabControl.Controls.Add(tab2);
        tabControl.Controls.Add(tab3);
        
        
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(tabControl);
        Controls.Add(_webView2);
        Controls.Add(newButton);
        Controls.Add(editButton);
        Controls.Add(refreshButton);
        Controls.Add(playButton);
        Controls.Add(loginButton);
        Controls.Add(label1);
        Controls.Add(label2);
        Controls.Add(combo1);
        Controls.Add(combo2);
        Controls.Add(listView);
        Controls.Add(log);
        this.Text = "Minecraft Launcher";
        this.Name = "Form1";
        this.ResumeLayout(false);
    }

    private WebView2 _webView2;
    private TabControl tabControl;
    private TabPage tab1;
    private TabPage tab2;
    private TabPage tab3;
    private Button newButton, editButton, refreshButton;
    private Button playButton, loginButton;
    private Label label1, label2;
    private ComboBox combo1, combo2;
    private TextBox log;
    private Timer timer1;
    private ListView listView;


    #endregion
}