﻿using CefSharp;
using DC_AutocompleteMenuNS;
using e621_ReBot.Modules;
using e621_ReBot_v2.CustomControls;
using e621_ReBot_v2.Forms;
using e621_ReBot_v2.Modules;
using e621_ReBot_v2.Modules.Grabber;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace e621_ReBot_v2
{
    //Comment Searching RegEx ^((\s*/+)).
    public partial class Form_Main : Form
    {
        //designer for big form doesn't work becase of this ???
        protected override void WndProc(ref Message message)
        {
            if (message.Msg == Program.WM_SHOWFIRSTINSTANCE)
            {
                WinApi.ShowToFront(Handle);
            }
            base.WndProc(ref message);
        }

        public Form_Main()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            InitializeComponent();

            ServicePointManager.DefaultConnectionLimit = 64;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            panel_Holder.MouseDown += Holder_MouseDown;
            Title_Label.MouseDown += Holder_MouseDown;
            Version_Label.MouseDown += Holder_MouseDown;
            label_UserLevel.MouseDown += Holder_MouseDown;
            label_Credit_Upload.MouseDown += Holder_MouseDown;
            label_Credit_Flag.MouseDown += Holder_MouseDown;
            label_Credit_Note.MouseDown += Holder_MouseDown;
            panel_Holder.MouseMove += Holder_MouseMove;
            Title_Label.MouseMove += Holder_MouseMove;
            Version_Label.MouseMove += Holder_MouseMove;
            label_UserLevel.MouseMove += Holder_MouseMove;
            label_Credit_Upload.MouseMove += Holder_MouseMove;
            label_Credit_Flag.MouseMove += Holder_MouseMove;
            label_Credit_Note.MouseMove += Holder_MouseMove;
            panel_Holder.MouseUp += Holder_MouseUp;
            Title_Label.MouseUp += Holder_MouseUp;
            Version_Label.MouseUp += Holder_MouseUp;
            label_UserLevel.MouseUp += Holder_MouseUp;
            label_Credit_Upload.MouseUp += Holder_MouseUp;
            label_Credit_Flag.MouseUp += Holder_MouseUp;
            label_Credit_Note.MouseUp += Holder_MouseUp;

            BQB_Start.Click += BrowserQuickButton_Click;
            BQB_HicceArs.Click += BrowserQuickButton_Click;
            BQB_Inkbunny.Click += BrowserQuickButton_Click;
            BQB_Pixiv.Click += BrowserQuickButton_Click;
            BQB_FurAffinity.Click += BrowserQuickButton_Click;
            BQB_Twitter.Click += BrowserQuickButton_Click;
            BQB_Newgrounds.Click += BrowserQuickButton_Click;
            BQB_SoFurry.Click += BrowserQuickButton_Click;
            BQB_Mastodon.Click += BrowserQuickButton_Click;
            BQB_Plurk.Click += BrowserQuickButton_Click;
            BQB_Pawoo.Click += BrowserQuickButton_Click;
            BQB_Weasyl.Click += BrowserQuickButton_Click;
            BQB_Baraag.Click += BrowserQuickButton_Click;
            BQB_HentaiFoundry.Click += BrowserQuickButton_Click;

            cCheckGroupBox_Grab.Paint += CCheckGroupBox_Jobs_Paint;
            cCheckGroupBox_Upload.Paint += CCheckGroupBox_Jobs_Paint;
            cCheckGroupBox_Convert.Paint += CCheckGroupBox_Jobs_Paint;
            cCheckGroupBox_Retry.Paint += CCheckGroupBox_Jobs_Paint;

            UpdateDays_1.CheckedChanged += UpdateDays_CheckedChanged;
            UpdateDays_3.CheckedChanged += UpdateDays_CheckedChanged;
            UpdateDays_7.CheckedChanged += UpdateDays_CheckedChanged;
            UpdateDays_15.CheckedChanged += UpdateDays_CheckedChanged;
            UpdateDays_30.CheckedChanged += UpdateDays_CheckedChanged;
            Naming_e6_0.CheckedChanged += NamingE6_CheckedChanged;
            Naming_e6_1.CheckedChanged += NamingE6_CheckedChanged;
            Naming_e6_2.CheckedChanged += NamingE6_CheckedChanged;
            Naming_web_0.CheckedChanged += NamingWeb_CheckedChanged;
            Naming_web_1.CheckedChanged += NamingWeb_CheckedChanged;
            Naming_web_2.CheckedChanged += NamingWeb_CheckedChanged;
            radioButton_GridItemStyle0.CheckedChanged += RadioButton_GridItemStyle_CheckedChanged;
            radioButton_GridItemStyle1.CheckedChanged += RadioButton_GridItemStyle_CheckedChanged;
            radioButton_ProgressBarStyle0.CheckedChanged += RadioButton_ProgressBarStyle_CheckedChanged;
            radioButton_ProgressBarStyle1.CheckedChanged += RadioButton_ProgressBarStyle_CheckedChanged;
            radioButton_GrabDisplayOrder0.CheckedChanged += RadioButton_GrabDisplayOrder_CheckedChanged;
            radioButton_GrabDisplayOrder1.CheckedChanged += RadioButton_GrabDisplayOrder_CheckedChanged;

            cTreeView_GrabQueue.BeforeSelect += CTreeView_BeforeSelect;
            cTreeView_GrabQueue.NodeMouseClick += CTreeView_NodeMouseClick;
            cTreeView_UploadQueue.BeforeSelect += CTreeView_BeforeSelect;
            cTreeView_UploadQueue.NodeMouseClick += CTreeView_NodeMouseClick;
            cTreeView_ConversionQueue.BeforeSelect += CTreeView_BeforeSelect;
            cTreeView_ConversionQueue.NodeMouseClick += CTreeView_NodeMouseClick;
            cTreeView_RetryQueue.BeforeSelect += CTreeView_BeforeSelect;
            cTreeView_RetryQueue.NodeMouseClick += CTreeView_NodeMouseClick;
            cTreeView_DownloadQueue.BeforeSelect += CTreeView_BeforeSelect;
            cTreeView_DownloadQueue.NodeMouseClick += CTreeView_NodeMouseClick;

            contextMenuStrip_cTreeView.Opening += ContextMenuStrip_cTreeView_Opening;
            contextMenuStrip_cTreeView.Closing += ContextMenuStrip_cTreeView_Closing;
            contextMenuStrip_Conversion.Opening += ContextMenuStrip_cTreeView_Opening;
            contextMenuStrip_Conversion.Closing += ContextMenuStrip_cTreeView_Closing;
            contextMenuStrip_Download.Opening += ContextMenuStrip_cTreeView_Opening;
            contextMenuStrip_Download.Closing += ContextMenuStrip_cTreeView_Closing;

            tabPage_Grid.MouseWheel += TabPage_Grid_MouseWheel;

            RadioButton_DL1.CheckedChanged += RadioButton_DL_CheckedChanged;
            RadioButton_DL2.CheckedChanged += RadioButton_DL_CheckedChanged;
            RadioButton_DL3.CheckedChanged += RadioButton_DL_CheckedChanged;
            RadioButton_DL4.CheckedChanged += RadioButton_DL_CheckedChanged;

            textBox_DelayGrabber.Enter += TextBox_Delay_Enter;
            textBox_DelayUploader.Enter += TextBox_Delay_Enter;
            textBox_DelayDownload.Enter += TextBox_Delay_Enter;
            textBox_DelayGrabber.KeyDown += TextBox_Delay_KeyDown;
            textBox_DelayUploader.KeyDown += TextBox_Delay_KeyDown;
            textBox_DelayDownload.KeyDown += TextBox_Delay_KeyDown;
            textBox_DelayGrabber.KeyPress += TextBox_Delay_KeyPress;
            textBox_DelayUploader.KeyPress += TextBox_Delay_KeyPress;
            textBox_DelayDownload.KeyPress += TextBox_Delay_KeyPress;

            FolderDialogFix = new Timer();
            FolderDialogFix.Tick += FolderDialogFix_Tick;
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

#if DEBUG
            DevTools_Button.Visible = true;
            bU_GetGenders.Visible = true;
            bU_GetDNPs.Visible = true;
#endif

            panel_BrowserDisplay.Controls.Add(Module_CefSharp.CefSharpBrowser);
            //heavy shit       
            SetQuickButtonPanelRegion();
            CreateTrackList();

            Version_Label.Text = $"v{Application.ProductVersion}";

            Module_DB.CreateDBs();

            if (!string.IsNullOrEmpty(Properties.Settings.Default.AppName))
            {
                AppName_Label.Text = Properties.Settings.Default.AppName;
            }

            if (!string.IsNullOrEmpty(Properties.Settings.Default.UserID))
            {
                new Thread(Module_Credits.Check_Credit_All).Start();
            }

            if (string.IsNullOrEmpty(Properties.Settings.Default.API_Key))
            {
                bU_APIKey.Text = "Add API Key";
            }
            else
            {
                bU_APIKey.Text = "Remove API Key";
                Module_APIControler.ToggleStatus();
            }

            cCheckGroupBox_Convert.Checked = File.Exists("ffmpeg.exe");

            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastStats))
            {
                string[] LastStatsString = Properties.Settings.Default.LastStats.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                label_Credit_Upload.Text = LastStatsString[0];
                label_Credit_Flag.Text = LastStatsString[1];
                label_Credit_Note.Text = LastStatsString[2];
            }

            CheckBox_ConverterKeepOriginal.Checked = Properties.Settings.Default.Converter_KeepOriginal;
            CheckBox_ConverterDontConvertVideos.Checked = Properties.Settings.Default.Converter_DontConvertVideos;
            CheckBox_BigMode.Checked = Properties.Settings.Default.LoadBigForm;
            CheckBox_AutocompleteTags.Checked = Properties.Settings.Default.AutocompleteTags;
            CheckBox_ManualInferiorSave.Checked = Properties.Settings.Default.ManualInferiorSave;
            CheckBox_ExpandedDescription.Checked = Properties.Settings.Default.ExpandedDescription;
            CheckBox_RemoveBVAS.Checked = Properties.Settings.Default.RemoveBVAS;
            CheckBox_ClearCache.Checked = Properties.Settings.Default.ClearCache;
            CheckBox_DisableGPU.Checked = Properties.Settings.Default.DisableGPU;
            CheckBox_EnableReplacement.Checked = Properties.Settings.Default.ReplacementBeta;
            //CheckBox_DontFlag.Checked = Properties.Settings.Default.DontFlag;
            //CheckBox_DontFlag.Visible = !string.IsNullOrEmpty(Properties.Settings.Default.UserLevel) && Module_Credits.UserLevels[Properties.Settings.Default.UserLevel] > 2;

            if (string.IsNullOrEmpty(Properties.Settings.Default.DownloadsFolderLocation))
            {
                label_DownloadsFolder.Text = $"{Application.StartupPath}\\Downloads\\";
                Properties.Settings.Default.DownloadsFolderLocation = label_DownloadsFolder.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                label_DownloadsFolder.Text = Properties.Settings.Default.DownloadsFolderLocation;
            }

            if (File.Exists("tags.txt"))
            {
                Read_AutoTags();
            }
            else
            {
                DownloadWhat = "tag";
                cGroupBoxColored_AutocompleteTagEditor.Enabled = false;
            }
            if (File.Exists("pools.txt"))
            {
                Read_AutoPools();
            }
            else
            {
                DownloadWhat = (DownloadWhat == null ? "pool" : "tag and pool");
            }
            AutoTags.AllowsTabKey = true;
            AutoTags.LeftPadding = 0;
            Read_Genders();
            Read_DNPs();

            ((RadioButton)cGroupBoxColored_Update.Controls.Find("UpdateDays_" + Properties.Settings.Default.UpdateDays, false).FirstOrDefault()).Checked = true;

            ((RadioButton)cGroupBoxColored_NamingE621.Controls[Properties.Settings.Default.Naming_e6]).Checked = true;
            ((RadioButton)cGroupBoxColored_NamingWeb.Controls[Properties.Settings.Default.Naming_web]).Checked = true;

            TrackBar_Volume.Value = Module_VolumeControl.GetApplicationVolume();

            ((RadioButton)tabPage_Settings.Controls.Find("radioButton_GridItemStyle" + Properties.Settings.Default.GridItemStyle, true).FirstOrDefault()).Checked = true;
            ((RadioButton)tabPage_Settings.Controls.Find("radioButton_ProgressBarStyle" + Properties.Settings.Default.ProgressBarStyle, true).FirstOrDefault()).Checked = true;
            ((RadioButton)tabPage_Settings.Controls.Find("radioButton_GrabDisplayOrder" + Properties.Settings.Default.GrabDisplayOrder, true).FirstOrDefault()).Checked = true;
            ((RadioButton)tabPage_Download.Controls.Find("RadioButton_DL" + Properties.Settings.Default.DLThreadsCount, true).FirstOrDefault()).Checked = true;

            textBox_DelayGrabber.Text = Properties.Settings.Default.DelayGrabber.ToString();
            textBox_DelayUploader.Text = Properties.Settings.Default.DelayUploader.ToString();
            textBox_DelayDownload.Text = Properties.Settings.Default.DelayDownload.ToString();

            if (Properties.Settings.Default.Bookmarks != null)
            {
                foreach (string BookmarkedURL in Properties.Settings.Default.Bookmarks)
                {
                    URL_ComboBox.Items.Add(BookmarkedURL);
                }
                panel_ComboBoxBlocker.Visible = false;
            }

            comboBox_PuzzleRows.SelectedIndex = 2;
            comboBox_PuzzleCollumns.SelectedIndex = 2;
            labelPuzzle_SelectedPost.Cursor = Form_Loader.Cursor_ReBotNav;
        }

        private void Form_Main_Shown(object sender, EventArgs e)
        {
            if (DesignMode) return;

            QuickButtonPanel.Visible = !Properties.Settings.Default.FirstRun;

            if (Properties.Settings.Default.Blacklist != null && Properties.Settings.Default.Blacklist.Count > 0)
            {
                foreach (string BlacklistString in Properties.Settings.Default.Blacklist)
                {
                    Blacklist.Add(BlacklistString);
                }
            }

            if (Module_APIControler.APIEnabled && !string.IsNullOrEmpty(Properties.Settings.Default.PoolWatcher))
            {
                new Thread(Form_PoolWatcher.PoolWatcher_Check4New).Start();
            }

            RetryQueue_Load();

            timer_FadeIn.Start();
        }

        private string DownloadWhat = null;
        private void Timer_FadeIn_Tick(object sender, EventArgs e)
        {
            //to remove shitty flicker
            Opacity += 0.05;
            if (Opacity == 1)
            {
                timer_FadeIn.Stop();
                Menu_Btn_Click(null, null);

                Module_UpdaterUpdater.UpdateTheUpdater();

                if (Properties.Settings.Default.AutocompleteTags && DownloadWhat != null)
                {
                    MessageBox.Show($"You should download {DownloadWhat} data if you intend to use the autocomplete feature.\n\nYou can do so by going to the settings tab and clicking the button for it.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (Properties.Settings.Default.FirstRun)
                {
                    TrackBar_Volume.Value = 25;
                    TrackBar_Volume_Scroll(null, null);
                    if (MessageBox.Show("Since this is our first date, would you like to know more about me?", "e621 ReBot Tutorial", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        panel_Browser.Visible = true;
                        Module_CefSharp.CefSharpBrowser.Load("https://e621.net/session/new");
                        MessageBox.Show("Thanks for trying me out.\n\nFor a start, you should log in into e621 and provide me with API key so I could do the tasks you require.\n\nI opened the login page for you.", "e621 ReBot");
                    }
                    else
                    {
                        Properties.Settings.Default.FirstRun = false;
                        QuickButtonPanel.Visible = true;
                    }
                }

                if (string.IsNullOrEmpty(Properties.Settings.Default.Note))
                {
                    bU_NoteRemove.Enabled = false;
                }
                else
                {
                    bU_NoteAdd.Text = "Edit Note";
                    toolTip_Display.SetToolTip(bU_NoteAdd, "Edit existing note.");
                    new Form_Notes();
                    Form_Notes._FormReference.StartPosition = FormStartPosition.Manual;
                    Form_Notes._FormReference.Location = new Point(Location.X + Width / 2 - Form_Notes._FormReference.Width / 2, Location.Y + Height / 2 - Form_Notes._FormReference.Height / 2);
                    Form_Notes._FormReference.ShowDialog();
                }
            }
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cTreeView_UploadQueue.Nodes.Count > 0 | Module_TableHolder.Download_Table.Rows.Count > 0 | cTreeView_ConversionQueue.Nodes.Count > 0)
            {
                if (MessageBox.Show("There are currently some jobs active, are you sure you want to close me?", "e621 ReBot Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            Cef.Shutdown();
            Module_CefSharp.CefSharpBrowser.Dispose();

            Properties.Settings.Default.LastStats = $"{label_Credit_Upload.Text},{label_Credit_Flag.Text},{label_Credit_Note.Text}";
            RetryQueue_Save();

            if (Properties.Settings.Default.ClearCache)
            {
                bool DeleteWorked = false;
                do
                {
                    try
                    {
                        Directory.Delete("CefSharp Cache", true);
                        DeleteWorked = true;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(500);
                    }
                } while (DeleteWorked == false);
            }
        }

        public Process ConversionQueueProcess;
        public Process UploadQueueProcess;
        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ConversionQueueProcess != null)
            {
                ConversionQueueProcess.Kill();
            }

            if (UploadQueueProcess != null)
            {
                UploadQueueProcess.Kill();
            }
            Application.Exit();
        }

        private void ToolStripMenuItem_NotificationArea_Click(object sender, EventArgs e)
        {
            notifyIcon_Main.Visible = false;
            ShowInTaskbar = true;
            Show();
            WindowState = FormWindowState.Normal;
        }







        #region "Drag"

        private bool FormMoving;
        private Point FormMoving_CursorPosition;
        private bool CanMoveAgain;
        private void Holder_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoving = true;
            FormMoving_CursorPosition = e.Location;
            CanMoveAgain = true;
            timer_Refresh.Start();
        }

        private void Holder_MouseMove(object sender, MouseEventArgs e)
        {
            if (FormMoving && CanMoveAgain)
            {
                Location = new Point(Location.X - FormMoving_CursorPosition.X + e.X, Location.Y - FormMoving_CursorPosition.Y + e.Y);
                if (Form_Menu._FormReference != null)
                {
                    Form_Menu._FormReference.Location = new Point(Location.X - 16, Location.Y + 128);
                }
                CanMoveAgain = false;
            }
        }

        private void Holder_MouseUp(object sender, MouseEventArgs e)
        {
            FormMoving = false;
            timer_Refresh.Stop();
        }

        private void Timer_Refresh_Tick(object sender, EventArgs e)
        {
            CanMoveAgain = true;
        }

        #endregion








        private void BU_KoFi_Click(object sender, EventArgs e)
        {
            Process.Start("https://ko-fi.com/e621rebot");
        }

        private void Opene6_btn_Click(object sender, EventArgs e)
        {
            Button_Unfocusable e6Btn = (Button_Unfocusable)sender;

            if (ModifierKeys.HasFlag(Keys.Control))
            {
                Process.Start("https://e621.net/");
            }
            else
            {
                QuickButtonPanel.Visible = false;
                Module_CefSharp.CefSharpBrowser.Load(e6Btn.Tag.ToString());
                panel_Browser.Visible = true;
                cTabControl_e621ReBot.SelectedIndex = 0;
            }
        }

        private void Menu_Btn_Click(object sender, EventArgs e)
        {
            cTabControl_e621ReBot.Focus();
            Form_Menu Form_Menu_Temp = new Form_Menu
            {
                Owner = this,
                Location = new Point(Location.X - 128, Location.Y + 128)
            };
            Menu_Btn.Visible = false;
            Form_Menu_Temp.Show();
        }

        private void CTabControl_e621ReBot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Form_Menu._FormReference != null)
            {
                Button_Menu TemoMBHolder = (Button_Menu)Form_Menu._FormReference.Menu_FlowLayoutPanel.Controls[cTabControl_e621ReBot.SelectedIndex];
                TemoMBHolder.Active = true;
                TemoMBHolder.MB_Highlight();
                TemoMBHolder.ClearOthers();
            }

            if (Form_Tagger._FormReference != null && Form_Tagger._FormReference.Owner == this)
            {
                Form_Tagger._FormReference.Close();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (cTabControl_e621ReBot.SelectedIndex == 1)
            {
                switch (keyData)
                {
                    case Keys.Left:
                        {
                            if (GB_Left.Visible)
                            {
                                GB_Left_Click(null, null);
                            }
                            break;
                        }

                    case Keys.Right:
                        {
                            if (GB_Right.Visible)
                            {
                                GB_Right_Click(null, null);
                            }
                            break;
                        }
                }

                if (_Selected_e6GridItem != null)
                {
                    switch (keyData)
                    {
                        case Keys.E:
                        case Keys.Q:
                        case Keys.S:
                            {
                                _Selected_e6GridItem._Rating = keyData.ToString();
                                if (Form_Preview._FormReference != null && Form_Preview._FormReference.IsHandleCreated && ReferenceEquals(_Selected_e6GridItem._DataRowReference, Form_Preview._FormReference.Preview_RowHolder))
                                {
                                    Form_Preview._FormReference.UpdateRatingDLButtons();
                                }
                                break;
                            }

                        case Keys.T:
                            {
                                if (_Selected_e6GridItem != null)
                                {
                                    Point GridPoint = _Selected_e6GridItem.PointToScreen(Point.Empty);
                                    Form_Tagger.OpenTagger(this, _Selected_e6GridItem._DataRowReference, new Point(GridPoint.X + 200, GridPoint.Y));
                                }
                                break;
                            }

                        case Keys.Oemplus:
                        case Keys.Add:
                        case Keys.NumPad1:
                        case Keys.D1:
                            {
                                _Selected_e6GridItem.cCheckBox_UPDL.Checked = true;
                                break;
                            }

                        case Keys.OemMinus:
                        case Keys.Subtract:
                        case Keys.NumPad0:
                        case Keys.D0:
                            {
                                _Selected_e6GridItem.cCheckBox_UPDL.Checked = false;
                                break;
                            }

                        case Keys.Delete:
                            {
                                if (cTreeView_UploadQueue.Nodes.ContainsKey((string)_Selected_e6GridItem._DataRowReference["Grab_MediaURL"]))
                                {
                                    MessageBox.Show("Image can't be removed while it is queued for upload.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                //else if (this.TreeView_DownloadQueue.Nodes.ContainsKey((string)Selectione6Img_Selected.RowReference["Grab_MediaURL"]))
                                //{
                                //    MessageBox.Show("Image can't be removed while it is queued for download.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //}
                                else
                                {
                                    flowLayoutPanel_Grid.SuspendLayout();
                                    Module_Grabber._Grabbed_MediaURLs.Remove((string)_Selected_e6GridItem._DataRowReference["Grab_MediaURL"]);
                                    //_Selected_e6GridItem.Dispose();
                                    _Selected_e6GridItem.StartAnimation_Remove();
                                    _Selected_e6GridItem = null;
                                    flowLayoutPanel_Grid.ResumeLayout();
                                    Paginator();
                                    if (Form_Preview._FormReference != null)
                                    {
                                        Form_Preview._FormReference.Close();
                                    }
                                }
                                break;
                            }
                    }
                }
            }

            return false;
        }








        #region "Browser Tab"

        private void Panel_Browser_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel_Browser.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
            using (Pen TempPen = new Pen(Color.Black, 1))
            {
                e.Graphics.DrawLine(TempPen, new Point(panel_BrowserDisplay.Location.X, panel_BrowserDisplay.Location.Y - 1), new Point(panel_BrowserDisplay.Width, panel_BrowserDisplay.Location.Y - 1));
            }
        }

        private void SetQuickButtonPanelRegion()
        {

            GraphicsPath PanelRegionPath = new GraphicsPath();
            Bitmap RegionImage = new Bitmap(Properties.Resources.BrowserButtonsRegion);

            int BGWidth = RegionImage.Width;
            int BGHeight = RegionImage.Height;

            BitmapData bmpData = RegionImage.LockBits(new Rectangle(0, 0, BGWidth, BGHeight), ImageLockMode.ReadWrite, RegionImage.PixelFormat);

            // PixelSize is 4 bytes for a 32bpp Argb image.
            int PixelSize = 4; // 32bits/8=4 bytes
            // Declare an array to hold the bytes of the bitmap.
            int numBytes = bmpData.Stride * bmpData.Height; // BGWidth * PixelSize * BGHeight
            byte[] rgbValues = new byte[numBytes - 1 + 1];
            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbValues, 0, numBytes);

            int LineStart;
            for (int y = 0; y < BGHeight; y++)
            {
                LineStart = -1;
                for (int x = 0; x < BGWidth; x++)
                {
                    // Get the various pixel locations  This calculation is for a 32bpp Argb bitmap
                    int ByteLocation = (y * BGWidth * PixelSize) + (x * PixelSize);

                    if (rgbValues[ByteLocation + 3] != 0)
                    {
                        if (x == 0)
                        {
                            PanelRegionPath.AddRectangle(new Rectangle(new Point(0, y), new Size(BGWidth, 1)));
                            break;
                        }
                        if (LineStart == -1)
                        {
                            LineStart = x;
                        }
                    }
                    else
                    {
                        if (LineStart != -1)
                        {
                            PanelRegionPath.AddRectangle(new Rectangle(new Point(LineStart, y), new Size(x - LineStart, 1)));
                            break;
                        }
                    }
                }
            }
            RegionImage.UnlockBits(bmpData);
            QuickButtonPanel.Region = new Region(PanelRegionPath);
        }

        private void BrowserQuickButton_Click(object sender, EventArgs e)
        {
            QuickButtonPanel.Visible = false;
            Button_Browser _sender = (Button_Browser)sender;
            URL_ComboBox.Text = _sender.Tag.ToString();
            Module_CefSharp.CefSharpBrowser.Load(_sender.Tag.ToString());
            panel_Browser.Visible = true;
        }

        private void BB_Bookmarks_Click(object sender, EventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                Properties.Settings.Default.Bookmarks.Clear();
                URL_ComboBox.Enabled = false;
                URL_ComboBox.Items.Clear();
                URL_ComboBox.Enabled = true;
                BB_Bookmarks.BackColor = Color.Gray;
                panel_ComboBoxBlocker.Visible = true;
            }
            else
            {
                string WebAdress = WebUtility.UrlDecode(Module_CefSharp.CefSharpBrowser.Address);
                if (Properties.Settings.Default.Bookmarks == null)
                {
                    Properties.Settings.Default.Bookmarks = new StringCollection();
                }

                if (Properties.Settings.Default.Bookmarks.Contains(WebAdress))
                {
                    Properties.Settings.Default.Bookmarks.Remove(WebAdress);
                    BB_Bookmarks.BackColor = Color.Gray;
                    toolTip_Display.SetToolTip(BB_Bookmarks, "Bookmark current page.\n\nHold Ctrl when clicking to clear all Bookmarks.");
                    URL_ComboBox.Items.Remove(WebAdress);
                    URL_ComboBox.Text = WebAdress; // Fix bug, item removal removing text.
                    panel_ComboBoxBlocker.Visible = URL_ComboBox.Items.Count == 0;
                    if (Properties.Settings.Default.Bookmarks.Count == 0)
                    {
                        Properties.Settings.Default.Bookmarks = null;
                    }
                }
                else
                {
                    Properties.Settings.Default.Bookmarks.Add(WebAdress);
                    BB_Bookmarks.BackColor = Color.RoyalBlue;
                    toolTip_Display.SetToolTip(BB_Bookmarks, "Remove Bookmark.\n\nHold Ctrl when clicking to clear all Bookmarks.");
                    URL_ComboBox.Items.Add(WebAdress);
                    panel_ComboBoxBlocker.Visible = false;
                }
            }
            Properties.Settings.Default.Save();
        }

        private void BB_Backward_Click(object sender, EventArgs e)
        {
            BB_Backward.Enabled = false;
            Module_CefSharp.CefSharpBrowser.Back();
        }

        private void BB_Backward_EnabledChanged(object sender, EventArgs e)
        {
            BB_Backward.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("BB_Backward_" + BB_Backward.Enabled.ToString());
        }

        private void BB_Reload_Click(object sender, EventArgs e)
        {
            Module_CefSharp.CefSharpBrowser.Reload();
            Module_Twitter.TwitterJSONHolder = null;
        }

        private void BB_Forward_Click(object sender, EventArgs e)
        {
            BB_Forward.Enabled = false;
            Module_CefSharp.CefSharpBrowser.Forward();
        }

        private void BB_Forward_EnabledChanged(object sender, EventArgs e)
        {
            BB_Forward.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("BB_Forward_" + BB_Forward.Enabled.ToString());
        }

        private void BB_Home_Click(object sender, EventArgs e)
        {
            QuickButtonPanel.Visible = !QuickButtonPanel.Visible;
        }

        private void URL_ComboBox_DropDown(object sender, EventArgs e)
        {
            URL_ComboBox.SelectionLength = 0;
        }

        private void URL_ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        e.SuppressKeyPress = true; // disable sound
                        BB_Navigate.PerformClick();
                        break;
                    }

                case Keys.Delete:
                    {
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                        if (URL_ComboBox.SelectedIndex == -1) return;

                        string BookmarkURL = URL_ComboBox.SelectedItem.ToString();
                        string CurrentWebAdress = WebUtility.UrlDecode(Module_CefSharp.CefSharpBrowser.Address);

                        if (Properties.Settings.Default.Bookmarks.Contains(BookmarkURL))
                        {
                            if (BookmarkURL.Equals(CurrentWebAdress))
                            {
                                BB_Bookmarks.BackColor = Color.Gray;
                                toolTip_Display.SetToolTip(BB_Bookmarks, "Bookmark current page.\n\nHold Ctrl when clicking to clear all Bookmarks.");
                            }
                            Properties.Settings.Default.Bookmarks.Remove(BookmarkURL);
                            if (Properties.Settings.Default.Bookmarks.Count == 0) Properties.Settings.Default.Bookmarks = null;
                            Properties.Settings.Default.Save();
                        }

                        string TextHolder = URL_ComboBox.Text;
                        int TextSelectionHolder = URL_ComboBox.SelectionStart;
                        URL_ComboBox.DroppedDown = false;
                        if (URL_ComboBox.Items.Count == 1)
                        {                          
                            URL_ComboBox.Items.Clear();
                        }
                        URL_ComboBox.Items.Remove(BookmarkURL);
                        URL_ComboBox.Text = TextHolder;
                        URL_ComboBox.Select(TextSelectionHolder,0);
                        URL_ComboBox.SelectedIndex = -1;
                        URL_ComboBox.SelectedItem = null;
                        panel_ComboBoxBlocker.Visible = URL_ComboBox.Items.Count == 0;
                        break;
                    }

                case Keys.V:
                    {
                        // Detect Paste
                        if (ModifierKeys.HasFlag(Keys.Control) && Clipboard.GetDataObject().GetDataPresent(DataFormats.StringFormat))
                        {
                                string ClipboardText = (string)Clipboard.GetDataObject().GetData(DataFormats.StringFormat);
                                ClipboardText = WebUtility.UrlDecode(ClipboardText);
                                e.SuppressKeyPress = true; // disable original paste

                                if (URL_ComboBox.SelectedText.Length > 0)
                                {
                                    URL_ComboBox.Text = URL_ComboBox.Text.Replace(URL_ComboBox.SelectedText, ClipboardText);
                                }
                                else
                                {
                                    URL_ComboBox.Text = ClipboardText;
                                }
                                URL_ComboBox.SelectionStart = URL_ComboBox.Text.Length;
                        }
                        break;
                    }

                default:
                    {  
                        break;
                    }
            }
        }

        private void URL_ComboBox_TextChanged(object sender, EventArgs e)
        {
            BB_Navigate.Enabled = !string.IsNullOrEmpty(URL_ComboBox.Text);
        }

        private void URL_ComboBox_Leave(object sender, EventArgs e)
        {
            URL_ComboBox.Text = URL_ComboBox.Text.Trim();
        }

        private void URL_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (URL_ComboBox.SelectedItem != null) BB_Navigate.PerformClick();
        }

        private void BB_Navigate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(URL_ComboBox.Text))
            {
                Title_Label.Focus();
                BB_Bookmarks.Enabled = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(CheckURLExists), URL_ComboBox.Text);
            }
        }

        private void CheckURLExists(object WebURLObject)
        {
            string WebURL = WebURLObject.ToString();
            if (WebURL.Equals("about:blank"))
            {
                Module_CefSharp.CefSharpBrowser.Load(WebURL);
                return;
            }

            if (!WebURL.Contains("."))
            {
                Module_CefSharp.CefSharpBrowser.Load("https://www.google.com/search?q=" + WebURL);
                return;
            }

            string WebURLCheck = WebURL;
            if (!WebURLCheck.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                WebURLCheck = $"http://{WebURLCheck}";
            }

            HttpWebRequest URLChecker = (HttpWebRequest)WebRequest.Create(WebURLCheck);
            CookieContainer CookieContainerUrlCheck = new CookieContainer();
            Module_CookieJar.GetCookies(WebURLCheck, ref CookieContainerUrlCheck);
            URLChecker.CookieContainer = CookieContainerUrlCheck;
            URLChecker.UserAgent = Form_Loader.GlobalUserAgent;
            URLChecker.Method = "HEAD";
            URLChecker.Timeout = 5000;
            try
            {
                HttpWebResponse UrlCheckerRepose = (HttpWebResponse)URLChecker.GetResponse();
                switch (UrlCheckerRepose.StatusCode)
                {
                    case HttpStatusCode.OK: //200
                        {
                            Module_CefSharp.CefSharpBrowser.Load(WebURLCheck);
                            break;
                        }

                    case HttpStatusCode.BadRequest: //400
                    case HttpStatusCode.Forbidden: //403
                    case HttpStatusCode.NotFound: //404
                    case HttpStatusCode.InternalServerError: //500
                    case HttpStatusCode.BadGateway: //502
                    case HttpStatusCode.ServiceUnavailable: //503
                    case HttpStatusCode.GatewayTimeout: //504
                        {
                            Module_CefSharp.CefSharpBrowser.Load($"https://www.google.com/search?q={WebURL}");
                            break;
                        }
                }
                UrlCheckerRepose.Dispose();
            }
            catch (WebException ex0)
            {
                if (ex0.Response == null)
                {
                    Invoke(new Action(() => { label_NavError.Text = ex0.Message; }));
                    Module_CefSharp.CefSharpBrowser.Load($"https://www.google.com/search?q={WebURL}");
                }
                else
                {
                    HttpWebResponse ErrorResponse = (HttpWebResponse)ex0.Response;
                    Invoke(new Action(() => { label_NavError.Text = $"{ErrorResponse.StatusCode} - {ErrorResponse.StatusDescription}"; }));
                    switch (ErrorResponse.StatusCode)
                    {
                        case HttpStatusCode.BadRequest: //400
                        case HttpStatusCode.Forbidden: //403
                        case HttpStatusCode.NotFound: //404
                        case HttpStatusCode.InternalServerError: //500
                        case HttpStatusCode.BadGateway: //502
                        case HttpStatusCode.ServiceUnavailable: //503
                        case HttpStatusCode.GatewayTimeout: //504
                            {
                                Module_CefSharp.CefSharpBrowser.Load($"https://www.google.com/search?q={WebURL}");
                                break;
                            }
                    }
                }

            }
            catch (Exception ex1)
            {
                Invoke(new Action(() => { label_NavError.Text = ex1.Message; }));
                Module_CefSharp.CefSharpBrowser.Load($"https://www.google.com/search?q={WebURL}");
            }
            finally
            {
                //nothing?
            }
        }

        private void Label_NavError_TextChanged(object sender, EventArgs e)
        {
            BB_Navigate.Visible = false;
            label_NavError.Visible = true;
            timer_NavError.Start();
        }

        private void Timer_NavError_Tick(object sender, EventArgs e)
        {
            timer_NavError.Stop();
            label_NavError.Visible = false;
            label_NavError.TextChanged -= Label_NavError_TextChanged;
            label_NavError.Text = null;
            label_NavError.TextChanged += Label_NavError_TextChanged;
            BB_Navigate.Visible = true;
        }

        private readonly List<UnmanagedMemoryStream> TrackList = new List<UnmanagedMemoryStream>();
        private void CreateTrackList()
        {
            TrackList.Add(Properties.Resources.PeasantPissed3);
            TrackList.Add(Properties.Resources.PeasantWhat3);
            TrackList.Add(Properties.Resources.PeasantYes2);
            TrackList.Add(Properties.Resources.PeasantYesAttack2);
            TrackList.Add(Properties.Resources.PeasantYesAttack4);
            TrackList.Add(Properties.Resources.PeonReady1);
            TrackList.Add(Properties.Resources.PeonYes3);
            TrackList.Add(Properties.Resources.PeonYesAttack1);
            TrackList.Add(Properties.Resources.PeonYesAttack3);
        }

        private readonly Random RandomGenerator = new Random();
        private void Worker_Sound()
        {
            int trackNum = RandomGenerator.Next(0, 8);
            int ChanceProc = RandomGenerator.Next(0, 100);
            if (ChanceProc < 25)
            {
                UnmanagedMemoryStream Track = TrackList[trackNum];
                SoundPlayer WorkerPlayer = new SoundPlayer(Track);
                Track.Seek(0, SeekOrigin.Begin);
                WorkerPlayer.Play();
            }
        }

        private void BB_Grab_All_Click(object sender, EventArgs e)
        {
            if (BB_Grab_All.Text.Equals("Stop"))
            {
                timer_TwitterGrabber.Stop();
                LastBrowserPosition = 0;
                LastBrowserPositionCounter = 0;
                BB_Grab_All.Text = "Grab All";
                BB_Grab.Visible = true;
            }
            else
            {
                Worker_Sound();
                BB_Grab_All.Text = "Stop";
                timer_TwitterGrabber.Start();
                BB_Grab.Visible = false;
            }
        }

        public int LastBrowserPosition;
        public int LastBrowserPositionCounter;
        private void Timer_TwitterGrabber_Tick(object sender, EventArgs e)
        {
            Module_CefSharp.CefSharpBrowser.ExecuteScriptAsync("window.scrollBy(0, 2048)");
            int WindowPosition = int.Parse(Module_CefSharp.CefSharpBrowser.EvaluateScriptAsync("window.pageYOffset;").Result.Result.ToString());
            cTreeView_GrabQueue.SuspendLayout();
            cTreeView_GrabQueue.BeginUpdate();
            Module_Grabber.PrepareLink(Module_CefSharp.CefSharpBrowser.Address);
            if (WindowPosition == LastBrowserPosition)
            {
                LastBrowserPositionCounter += 1;
                if (LastBrowserPositionCounter == 3)
                {
                    timer_TwitterGrabber.Stop();
                    BB_Grab_All.Text = "Grab All";
                    LastBrowserPosition = 0;
                    LastBrowserPositionCounter = 0;
                    BB_Grab.Visible = true;
                }
            }
            else
            {
                LastBrowserPositionCounter = 0;
                LastBrowserPosition = WindowPosition;
            }
            cTreeView_GrabQueue.EndUpdate();
            cTreeView_GrabQueue.ResumeLayout();
            Module_CefSharp.CefSharpBrowser.ExecuteScriptAsync("document.querySelectorAll(\"article[data-testid='tweet'] article[role='article'] div[role='button']:not([aria-label])\").forEach(button=>button.click());");
            //Module_CefSharp.CefSharpBrowser.ExecuteScriptAsync("document.querySelectorAll(\"article div[data-testid='tweet'] div[role='button']:not([aria-label])\").forEach(button=>button.click());");
        }

        private void BB_Grab_Click(object sender, EventArgs e)
        {
            if (Module_CefSharp.CefSharpBrowser.Address.Contains("twitter.com"))
            {
                Module_CefSharp.CefSharpBrowser.ExecuteScriptAsync("document.querySelectorAll(\"article[data-testid='tweet'] article[role='article'] div[role='button']:not([aria-label])\").forEach(button=>button.click())");
            }
            cTreeView_GrabQueue.SuspendLayout();
            cTreeView_GrabQueue.BeginUpdate();
            BB_Grab.Visible = false;
            Worker_Sound();
            Module_Grabber.PrepareLink(BB_Grab.Tag.ToString());
            cTreeView_GrabQueue.EndUpdate();
            cTreeView_GrabQueue.ResumeLayout();
        }

        private void BB_Download_Click(object sender, EventArgs e)
        {
            BB_Download.Visible = false;
            Worker_Sound();
            Module_Downloader.Grab_e621();
            Module_Downloader.UpdateTreeViewNodes();
            Module_Downloader.timer_Download.Start();
        }

        private void DevTools_Button_Click(object sender, EventArgs e)
        {
            //https://blog.dotnetframework.org/2018/10/26/intercepting-ajax-requests-in-cefsharp-chrome-for-c/
            Module_CefSharp.CefSharpBrowser.ShowDevTools();
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #endregion









        #region "Grid Tab"

        private void TabPage_Grid_Paint(object sender, PaintEventArgs e)
        {
            Form_Loader.LastAltState = !Form_Loader.LastAltState;
            using (Pen TempPen = new Pen(Color.Black, 1))
            {
                e.Graphics.DrawLine(TempPen, new Point(0, tabPage_Grid.Height - 27), new Point(tabPage_Grid.Width, tabPage_Grid.Height - 27)); //Horizontal, at bottom
                e.Graphics.DrawLine(TempPen, new Point(36 - 1, tabPage_Grid.Height - 27), new Point(36 - 1, tabPage_Grid.Height)); // Vertical, left
                e.Graphics.DrawLine(TempPen, new Point(tabPage_Grid.Width - 36, tabPage_Grid.Height - 27), new Point(tabPage_Grid.Width - 36, tabPage_Grid.Height)); // Vertical, right
            }

        }

        public int GridIndexTracker = 0;
        public e6_GridItem _Selected_e6GridItem;
        public void AddGridItem(ref DataRow RowReff, bool MultiAdd)
        {
            if (!MultiAdd)
            {
                UIDrawController.SuspendDrawing(flowLayoutPanel_Grid);
                flowLayoutPanel_Grid.SuspendLayout();
            }

            e6_GridItem NewGridItem = new e6_GridItem()
            {
                _DataRowReference = RowReff,
                _Rating = (string)RowReff["Upload_Rating"]
            };
            NewGridItem.cCheckBox_UPDL.Checked = (bool)RowReff["UPDL_Queued"];
            if (RowReff["Thumbnail_Image"] == DBNull.Value)
            {
                if (RowReff["Thumbnail_DLStart"] == DBNull.Value)
                {
                    RowReff["Thumbnail_DLStart"] = true;
                    Module_Grabber.GrabDownloadThumb(RowReff);
                }
            }
            else
            {
                NewGridItem.pictureBox_ImageHolder.Image = (Image)RowReff["Thumbnail_Image"];
                NewGridItem.pictureBox_ImageHolder.BackgroundImage = null;
            }
            if (RowReff["Uploaded_As"] != DBNull.Value)
            {
                NewGridItem.cLabel_isUploaded.Text = (string)RowReff["Uploaded_As"];
            }
            flowLayoutPanel_Grid.Controls.Add(NewGridItem);

            if (RowReff["Info_TooBig"] == DBNull.Value && RowReff["Info_MediaByteLength"] != DBNull.Value && RowReff["Info_MediaWidth"] != DBNull.Value)
            {
                RowReff["Info_TooBig"] = Module_Uploader.Media2BigCheck(ref RowReff);
            }
            if (!MultiAdd)
            {
                flowLayoutPanel_Grid.ResumeLayout();
                UIDrawController.ResumeDrawing(flowLayoutPanel_Grid);
            }
            if (Form_Preview._FormReference != null)
            {
                Form_Preview._FormReference.UpdateNavButtons();
            }
        }

        public void Paginator()
        {
            int CurrentPage = GridIndexTracker / Form_Loader._GridMaxControls + 1;
            int TotalPages = (int)Math.Ceiling((float)Module_TableHolder.Database_Table.Rows.Count / Form_Loader._GridMaxControls);
            Label_PageShower.Text = $"{CurrentPage} / {TotalPages}";
            Label_LeftPage.Text = (CurrentPage - 1).ToString();
            Label_RightPage.Text = (CurrentPage + 1).ToString();
        }

        public e6_GridItem IsE6PicVisibleInGrid(ref DataRow RefDataRow)
        {
            int RowIndex = Module_TableHolder.Database_Table.Rows.IndexOf(RefDataRow);
            if (RowIndex >= GridIndexTracker && RowIndex < (GridIndexTracker + Form_Loader._GridMaxControls))
            {
                return (e6_GridItem)flowLayoutPanel_Grid.Controls[RowIndex - GridIndexTracker];
            }
            return null;
        }

        private void FlowLayoutPanel_Grid_ControlAdded(object sender, ControlEventArgs e)
        {
            if (Form_Menu._FormReference != null)
            {
                Form_Menu._FormReference.MB_Grid.Visible = true;
            }

            e6_GridItem E6ImagePic = (e6_GridItem)e.Control;
            if (Name == "Form_MainBig")
            {
                E6ImagePic.Margin = new Padding(2, 1, 1, 24);
            }

            if (((string)E6ImagePic._DataRowReference["Upload_Tags"]).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length > 5)
            {
                E6ImagePic.cLabel_TagWarning.Visible = false;
            }
            if (Form_Preview._FormReference != null)
            {
                Form_Preview._FormReference.UpdateNavButtons();
            }
        }

        private void FlowLayoutPanel_Grid_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (flowLayoutPanel_Grid.Controls.Count == 0)
            {
                if (Module_TableHolder.Database_Table.Rows.Count > 0)
                {
                    GB_Left_Click(null, null);
                }
                else
                {
                    cTabControl_e621ReBot.SelectedIndex = 0;
                    GB_Right.Visible = false;
                    if (Form_Menu._FormReference != null)
                    {
                        Form_Menu._FormReference.MB_Grid.Visible = false;
                    }
                }
            }
            else
            {
                int CountImagesAfterThisPage = Module_TableHolder.Database_Table.Rows.Count - GridIndexTracker;
                if (CountImagesAfterThisPage >= Form_Loader._GridMaxControls)
                {
                    GB_Right.Visible = CountImagesAfterThisPage > Form_Loader._GridMaxControls;
                    DataRow DataRowTemp = Module_TableHolder.Database_Table.Rows[GridIndexTracker + flowLayoutPanel_Grid.Controls.Count];
                    AddGridItem(ref DataRowTemp, true);
                }
                else
                {
                    GB_Right.Visible = false;
                }
            }
        }

        public void PopulateGrid(int StartIndex)
        {
            int LoopTo = Math.Min(StartIndex + Form_Loader._GridMaxControls - 1, Module_TableHolder.Database_Table.Rows.Count - 1);
            for (int x = StartIndex; x <= LoopTo; x++)
            {
                DataRow DataRowTemp = Module_TableHolder.Database_Table.Rows[x];
                AddGridItem(ref DataRowTemp, true);
            }
        }

        private void ClearGrid()
        {
            flowLayoutPanel_Grid.ControlRemoved -= FlowLayoutPanel_Grid_ControlRemoved;
            while (flowLayoutPanel_Grid.Controls.Count > 0)
            {
                flowLayoutPanel_Grid.Controls[0].Dispose();
            }
            flowLayoutPanel_Grid.Controls.Clear();
            flowLayoutPanel_Grid.ControlRemoved += FlowLayoutPanel_Grid_ControlRemoved;
            _Selected_e6GridItem = null;
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void GB_Left_Click(object sender, EventArgs e)
        {
            UIDrawController.SuspendDrawing(flowLayoutPanel_Grid);
            flowLayoutPanel_Grid.SuspendLayout();
            ClearGrid();
            GridIndexTracker -= Form_Loader._GridMaxControls;
            if (GridIndexTracker <= 0)
            {
                GridIndexTracker = 0;
                GB_Left.Visible = false;
            }
            GB_Right.Visible = Module_TableHolder.Database_Table.Rows.Count - Form_Loader._GridMaxControls > GridIndexTracker;
            PopulateGrid(GridIndexTracker);
            Paginator();
            flowLayoutPanel_Grid.ResumeLayout();
            UIDrawController.ResumeDrawing(flowLayoutPanel_Grid);
        }

        private void GB_Left_VisibleChanged(object sender, EventArgs e)
        {
            Label_LeftPage.Visible = GB_Left.Visible;
        }

        private void GB_Right_Click(object sender, EventArgs e)
        {
            UIDrawController.SuspendDrawing(flowLayoutPanel_Grid);
            flowLayoutPanel_Grid.SuspendLayout();
            ClearGrid();
            GridIndexTracker += Form_Loader._GridMaxControls;
            GB_Right.Visible = Module_TableHolder.Database_Table.Rows.Count - Form_Loader._GridMaxControls > GridIndexTracker;
            GB_Left.Visible = true;
            PopulateGrid(GridIndexTracker);
            Paginator();
            flowLayoutPanel_Grid.ResumeLayout();
            UIDrawController.ResumeDrawing(flowLayoutPanel_Grid);
        }

        private void GB_Right_VisibleChanged(object sender, EventArgs e)
        {
            Label_RightPage.Visible = GB_Right.Visible;
        }

        private void TabPage_Grid_MouseWheel(object sender, MouseEventArgs e)
        {
            tabPage_Grid.MouseWheel -= TabPage_Grid_MouseWheel;
            if (e.Delta > 0) // Up / Left
            {
                if (GB_Left.Visible)
                {
                    GB_Left_Click(null, null);
                }
            }
            else
            {
                if (GB_Right.Visible) // Down / Right
                {
                    GB_Right_Click(null, null);
                }

            }
            var ScrollDisableTimer = new Timer
            {
                Interval = 100
            };
            ScrollDisableTimer.Tick += ScrollDisable_Tick;
            ScrollDisableTimer.Start();
        }

        private void ScrollDisable_Tick(object sender, EventArgs e)
        {
            Timer ScrollDisableTimer = (Timer)sender;
            ScrollDisableTimer.Stop();
            ScrollDisableTimer.Dispose();
            tabPage_Grid.MouseWheel += TabPage_Grid_MouseWheel;
        }

        public int UploadCounter;
        public int DownloadCounter;
        private void GB_Check_Click(object sender, EventArgs e)
        {
            foreach (e6_GridItem e6_GridItemTemp in flowLayoutPanel_Grid.Controls)
            {
                e6_GridItemTemp.cCheckBox_UPDL.Checked = true;
            }
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                foreach (DataRow PicImageRow in Module_TableHolder.Database_Table.Rows)
                {
                    PicImageRow["UPDL_Queued"] = true;
                }
            }
        }

        private void GB_Inverse_Click(object sender, EventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                foreach (DataRow PicImageRow in Module_TableHolder.Database_Table.Rows)
                {
                    PicImageRow["UPDL_Queued"] = !(bool)PicImageRow["UPDL_Queued"];
                }
                foreach (e6_GridItem e6_GridItemTemp in flowLayoutPanel_Grid.Controls)
                {
                    e6_GridItemTemp.cCheckBox_UPDL.Checked = (bool)e6_GridItemTemp._DataRowReference["UPDL_Queued"];
                }
            }
            else
            {
                foreach (e6_GridItem e6_GridItemTemp in flowLayoutPanel_Grid.Controls)
                {
                    e6_GridItemTemp.cCheckBox_UPDL.Checked = !e6_GridItemTemp.cCheckBox_UPDL.Checked;
                }
            }
        }

        private void GB_Uncheck_Click(object sender, EventArgs e)
        {
            foreach (e6_GridItem e6_GridItemTemp in flowLayoutPanel_Grid.Controls)
            {
                e6_GridItemTemp.cCheckBox_UPDL.Checked = false;
            }
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                foreach (DataRow PicImageRow in Module_TableHolder.Database_Table.Rows)
                {
                    PicImageRow["UPDL_Queued"] = false;
                }
            }
        }

        private void GB_Clear_Click(object sender, EventArgs e)
        {
            if (cTreeView_UploadQueue.Nodes.Count > 0)
            {
                MessageBox.Show("Images can't be removed while they are queued for upload.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (Form_Tagger._FormReference != null)
                {
                    Form_Tagger._FormReference.Close();
                }
                if (Form_Preview._FormReference != null)
                {
                    Form_Preview._FormReference.Close();
                }

                bool ClearAll = true;
                if (ModifierKeys.HasFlag(Keys.Control))
                {
                    lock (Module_TableHolder.Database_Table)
                    {
                        for (int i = Module_TableHolder.Database_Table.Select().Length - 1; i >= 0; i--)
                        {
                            if (Module_TableHolder.Database_Table.Rows[i]["Uploaded_As"] != DBNull.Value)
                            {
                                Module_TableHolder.Database_Table.Rows.RemoveAt(i);
                            }
                        }
                    }
                    ClearAll = false;
                }

                if (ModifierKeys.HasFlag(Keys.Shift))
                {
                    lock (Module_TableHolder.Database_Table)
                    {
                        for (int i = (GridIndexTracker + flowLayoutPanel_Grid.Controls.Count - 1); i >= GridIndexTracker; i--)
                        {
                            Module_TableHolder.Database_Table.Rows.RemoveAt(i);
                        }
                    }
                    ClearAll = false;
                }

                if (ClearAll)
                {
                    lock (Module_TableHolder.Database_Table)
                    {
                        Module_TableHolder.Database_Table.Clear();
                    }
                }

                if (Module_TableHolder.Database_Table.Rows.Count == 0)
                {
                    GB_Left.Visible = false;
                    GB_Right.Visible = false;
                    Label_PageShower.Text = "1 / 1";
                    cTabControl_e621ReBot.SelectedIndex = 0;
                    Module_Grabber._Grabbed_MediaURLs.Clear();
                    ClearGrid();
                    GridIndexTracker = 0;
                    if (Form_Menu._FormReference != null)
                    {
                        Form_Menu._FormReference.MB_Grid.Visible = false;
                    }
                }
                else
                {
                    int CurrentPage = GridIndexTracker / Form_Loader._GridMaxControls;
                    int NewEndPage = (int)Math.Floor((float)Module_TableHolder.Database_Table.Rows.Count / Form_Loader._GridMaxControls - 0.01);
                    NewEndPage = NewEndPage < CurrentPage ? NewEndPage : CurrentPage;
                    GridIndexTracker = NewEndPage * Form_Loader._GridMaxControls;

                    UIDrawController.SuspendDrawing(flowLayoutPanel_Grid);
                    flowLayoutPanel_Grid.SuspendLayout();
                    ClearGrid();
                    PopulateGrid(GridIndexTracker);
                    Paginator();
                    GB_Left.Visible = GridIndexTracker >= Form_Loader._GridMaxControls;
                    GB_Right.Visible = Module_TableHolder.Database_Table.Rows.Count - Form_Loader._GridMaxControls > GridIndexTracker;
                    flowLayoutPanel_Grid.ResumeLayout();
                    UIDrawController.ResumeDrawing(flowLayoutPanel_Grid);
                }
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect();
            }
        }

        private void GB_Download_Click(object sender, EventArgs e)
        {
            GB_Download.Enabled = false;

            DataRow DataRowTemp = null;
            if (ModifierKeys.HasFlag(Keys.Shift))
            {
                foreach (e6_GridItem e6_GridItemTemp in Form_Loader._GridFLPHolder.Controls)
                {
                    DataRowTemp = e6_GridItemTemp._DataRowReference;
                    CreateDownloadQueueItem(ref DataRowTemp);
                }
            }
            else
            {
                foreach (DataRow DataRowTemp0 in Module_TableHolder.Database_Table.Rows)
                {
                    DataRowTemp = DataRowTemp0;
                    CreateDownloadQueueItem(ref DataRowTemp);
                }
            }
            Module_Downloader.UpdateTreeViewNodes();
            Module_Downloader.timer_Download.Start();
            BeginInvoke(new Action(() => { GB_Download.Enabled = true; }));
        }

        private void CreateDownloadQueueItem(ref DataRow DataRowRef)
        {
            if ((bool)DataRowRef["UPDL_Queued"])
            {
                if (!Module_TableHolder.DownloadQueueContainsURL((string)DataRowRef["Grab_MediaURL"]) && !Module_Downloader.Download_AlreadyDownloaded.Contains((string)DataRowRef["Grab_MediaURL"]))
                {
                    //Weasyl fix
                    if (DataRowRef["Grab_ThumbnailURL"] == DBNull.Value) DataRowRef["Grab_ThumbnailURL"] = "";
                    Module_Downloader.AddDownloadQueueItem(
                        DataRowRef: DataRowRef,
                        URL: (string)DataRowRef["Grab_URL"],
                        Media_URL: (string)DataRowRef["Grab_MediaURL"],
                        Thumbnail_URL: (string)DataRowRef["Grab_ThumbnailURL"],
                        MediaFormat: (string)DataRowRef["Info_MediaFormat"],
                        Artist: (string)DataRowRef["Artist"],
                        Grab_Title: (string)DataRowRef["Grab_Title"]
                        );
                }
            }
        }

        private void GB_Upload_Click(object sender, EventArgs e)
        {
            Module_Uploader.UploadBtnClicked(ModifierKeys.HasFlag(Keys.Shift));
        }

        #endregion









        #region "Download Tab"

        private void CCheckGroupBox_Download_Paint(object sender, PaintEventArgs e)
        {
            using (Pen TempPen = new Pen(Color.Black, 1))
            {
                Custom_CheckGroupBox cCGPCTemp = (Custom_CheckGroupBox)sender;
                e.Graphics.DrawLine(TempPen, new Point(0, 32), new Point(cCGPCTemp.Width, 32));
            }
        }

        private void BU_CancelAPIDL_Click(object sender, EventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                lock (Module_e621APIMinion._WorkQueue)
                {
                    Module_e621APIMinion._WorkQueue.Clear();
                }
            }
            bU_CancelAPIDL.Enabled = Module_e621APIMinion._WorkQueue.Any();
            Module_e621APIMinion.WorkerMinion.CancelAsync();
        }

        private void CCheckGroupBox_Download_CheckedChanged(object sender, EventArgs e)
        {
            if (cCheckGroupBox_Download.Checked)
            {
                Module_Downloader.timer_Download.Start();
            }

        }

        private void RadioButton_DL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton WhichRadioButton = (RadioButton)sender;
            if (!string.IsNullOrEmpty(WhichRadioButton.Text) && WhichRadioButton.Checked && cCheckGroupBox_Download.Checked)
            {
                int NewValue = int.Parse(WhichRadioButton.Text);
                if (Module_Downloader.DLThreadsCount != NewValue)
                {
                    Module_Downloader.DLThreadsCount = NewValue;
                    Properties.Settings.Default.DLThreadsCount = NewValue;
                    Properties.Settings.Default.Save();
                    //Module_Downloader.Download_Start();
                }
            }
        }

        private void BU_ClearDLHistory_Click(object sender, EventArgs e)
        {
            Module_Downloader.Download_AlreadyDownloaded.Clear();
            bU_ClearDLHistory.Enabled = false;
            UIDrawController.SuspendDrawing(DownloadFLP_Downloaded);
            DownloadFLP_Downloaded.SuspendLayout();
            DownloadFLP_Downloaded.Controls.Clear();
            DownloadFLP_Downloaded.ResumeLayout();
            UIDrawController.ResumeDrawing(DownloadFLP_Downloaded);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
        }

        private void BU_DownloadFolder_Click(object sender, EventArgs e)
        {
            string Path = Properties.Settings.Default.DownloadsFolderLocation;
            Directory.CreateDirectory(Path);
            Process.Start(Path);
        }

        private void DownloadFLP_Downloaded_ControlAdded(object sender, ControlEventArgs e)
        {
            bU_ClearDLHistory.Enabled = true;
        }

        private void ToolStripMenuItem_RemoveDLNode_Click(object sender, EventArgs e)
        {
            if (WhichNodeIsIt.TreeView != null)
            {
                Module_TableHolder.DownloadQueueRemoveURL(WhichNodeIsIt.Text);
                Module_Downloader.UpdateTreeViewText();
                Module_Downloader.UpdateTreeViewNodes();
            }
        }

        private void ToolStripMenuItem_RemoveDLAll_Click(object sender, EventArgs e)
        {
            lock (Module_TableHolder.Download_Table)
            {
                Module_TableHolder.Download_Table.Rows.Clear();
            }
            Module_Downloader.TreeViewPage = 0;
            Module_Downloader.UpdateTreeViewNodes();
            Module_Downloader.UpdateTreeViewText();
        }

        private void BU_DownloadPageUp_Click(object sender, EventArgs e)
        {
            Module_Downloader.TreeViewPage -= 1;
            Module_Downloader.UpdateTreeViewNodes();
        }

        private void BU_DownloadPageDown_Click(object sender, EventArgs e)
        {
            Module_Downloader.TreeViewPage += 1;
            Module_Downloader.UpdateTreeViewNodes();
        }

        private void BU_SkipDLCache_Click(object sender, EventArgs e)
        {
            Module_Downloader.Load_DownloadFolderCache();
        }

        private void BU_ReverseDownload_Click(object sender, EventArgs e)
        {
            if (cCheckGroupBox_Download.Checked | Module_e621APIMinion.WorkerMinion.IsBusy)
            {
                MessageBox.Show("Download queue and API should be stopped before attempting to reverse the order.", "e621 ReBot");
                return;
            }
            if (Module_TableHolder.Download_Table.Rows.Count > 1)
            {
                Module_TableHolder.Download_Table = Module_TableHolder.ReverseDataTable(Module_TableHolder.Download_Table);
                Module_Downloader.UpdateTreeViewNodes();
            }
        }

        #endregion









        #region "Jobs Tab"

        private void CCheckGroupBox_Grab_Paint(object sender, PaintEventArgs e)
        {
            using (Pen TempPen = new Pen(Color.Black, 1))
            {
                Custom_CheckGroupBox cCGPCTemp = (Custom_CheckGroupBox)sender;
                e.Graphics.DrawLine(TempPen, new Point(52, 32), new Point(52, cCGPCTemp.Height - 2));
            }
        }

        private void CCheckGroupBox_Jobs_Paint(object sender, PaintEventArgs e)
        {
            using (Pen TempPen = new Pen(Color.Black, 1))
            {
                Custom_CheckGroupBox cCGPCTemp = (Custom_CheckGroupBox)sender;
                e.Graphics.DrawLine(TempPen, new Point(0, 32), new Point(cCGPCTemp.Width, 32));
            }
        }

        private void ToolStripMenuItem_ClearReports_Click(object sender, EventArgs e)
        {
            textBox_Info.Clear();
        }

        private void ContextMenuStrip_cTreeView_Opening(object sender, CancelEventArgs e)
        {
            WhichNodeIsIt.ForeColor = Color.Orange;
        }

        private void ContextMenuStrip_cTreeView_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            WhichNodeIsIt.ForeColor = Color.LightSteelBlue;
        }

        private void CTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
            label_GrabStatus.Focus();
        }

        private TreeNode WhichNodeIsIt;
        private void CTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            WhichNodeIsIt = e.Node;
        }

        private void CTreeView_GrabQueue_AfterCheck(object sender, TreeViewEventArgs e)
        {
            cTreeView_GrabQueue.AfterCheck -= CTreeView_GrabQueue_AfterCheck;

            int ChildNodesTotal;
            int CheckedChildNodesCount;

            if (e.Node.Parent != null)
            {
                int ParentNodeTag = e.Node.Parent.Tag != null ? int.Parse(e.Node.Parent.Tag.ToString()) : 0;
                CheckedChildNodesCount = ParentNodeTag + (e.Node.Checked ? 1 : -1);
                e.Node.Parent.Tag = CheckedChildNodesCount;
                UpdateParentNode_Tooltip(e.Node.Parent);
                e.Node.Parent.Checked = CheckedChildNodesCount != 0;
            }
            else
            {
                ChildNodesTotal = e.Node.Nodes.Count;
                foreach (TreeNode ChildNode in e.Node.Nodes)
                {
                    ChildNode.Checked = e.Node.Checked;
                }
                CheckedChildNodesCount = e.Node.Checked ? ChildNodesTotal : 0;
                e.Node.Tag = CheckedChildNodesCount;
                UpdateParentNode_Tooltip(e.Node);
            }

            cTreeView_GrabQueue.AfterCheck += CTreeView_GrabQueue_AfterCheck;
        }

        private void ToolStripMenuItem_RemoveNode_Click(object sender, EventArgs e)
        {
            TreeNode ParentNode = WhichNodeIsIt.Parent;

            if (WhichNodeIsIt.TreeView.Name.Contains("Grab"))
            {
                if (ParentNode != null)
                {
                    if (ParentNode.Nodes.Count > 1)
                    {
                        if (WhichNodeIsIt.Checked)
                        {
                            WhichNodeIsIt.Checked = false;
                        }
                    }
                }
                if (WhichNodeIsIt.Nodes.Count > 1)
                {
                    lock (Module_Grabber._GrabQueue_URLs)
                    {
                        foreach (TreeNode TreeNodeTemp in WhichNodeIsIt.Nodes)
                        {
                            Module_Grabber._GrabQueue_URLs.Remove(TreeNodeTemp.Name);
                        }
                    }
                }
                else
                {
                    if (Module_Grabber._GrabQueue_URLs.Contains(WhichNodeIsIt.Name))
                    {
                        lock (Module_Grabber._GrabQueue_URLs)
                        {
                            Module_Grabber._GrabQueue_URLs.Remove(WhichNodeIsIt.Name);
                        }
                    }
                }
                WhichNodeIsIt.TreeView.Nodes.Remove(WhichNodeIsIt);
                if (ParentNode != null)
                {
                    UpdateParentNode_Tooltip(ParentNode);
                }
                return;
            }

            if (WhichNodeIsIt.TreeView.Name.Contains("Upload"))
            {
                DataRow DataRowTemp = (DataRow)ParentNode.Tag;
                if (ParentNode != null && ParentNode.Nodes.Count == 1)
                {
                    lock (Module_TableHolder.Upload_Table)
                    {
                        Module_TableHolder.Upload_Table.Rows.Remove(DataRowTemp);
                    }
                    ParentNode.Remove();
                }
                else
                {
                    lock (Module_TableHolder.Upload_Table)
                    {
                        DataRowTemp[WhichNodeIsIt.Text.Replace(" ", "")] = false;
                    }
                    WhichNodeIsIt.TreeView.Nodes.Remove(WhichNodeIsIt);
                }
                return;
            }

            WhichNodeIsIt.Remove();
        }

        private void ToolStripMenuItem_RemoveAll_Click(object sender, EventArgs e)
        {
            if (WhichNodeIsIt.TreeView.Name.Contains("Grab"))
            {
                lock (Module_Grabber._GrabQueue_URLs)
                {
                    Module_Grabber._GrabQueue_URLs.Clear();
                }
            }

            if (WhichNodeIsIt.TreeView.Name.Contains("Upload"))
            {
                lock (Module_TableHolder.Upload_Table)
                {
                    Module_TableHolder.Upload_Table.Rows.Clear();
                }
            }

            WhichNodeIsIt.TreeView.Nodes.Clear();
        }

        private void ToolStripMenuItem_ExpandAll_Click(object sender, EventArgs e)
        {
            if (WhichNodeIsIt.TreeView != null)
            {
                WhichNodeIsIt.TreeView.ExpandAll();
            }
        }

        private void ToolStripMenuItem_CollapseAll_Click(object sender, EventArgs e)
        {
            if (WhichNodeIsIt.TreeView != null)
            {
                WhichNodeIsIt.TreeView.CollapseAll();
            }
        }

        public void UpdateParentNode_Tooltip(TreeNode ParentNode)
        {
            int CheckedChildNodesCount = int.Parse(ParentNode.Tag.ToString());
            ParentNode.Tag = CheckedChildNodesCount;
            if (ParentNode.Nodes.Count != 0)
            {
                ParentNode.ToolTipText = $"Selected: {CheckedChildNodesCount} / {ParentNode.Nodes.Count}";
                // ^ doesnt update while hovered.
            }
        }

        private void CCheckGroupBox_Grab_CheckedChanged(object sender, EventArgs e)
        {
            ContextMenuStrip ContexMenuHolder = cCheckGroupBox_Grab.Checked ? null : contextMenuStrip_cTreeView;
            foreach (TreeNode ParentNode in cTreeView_GrabQueue.Nodes)
            {
                ParentNode.ContextMenuStrip = ContexMenuHolder;
                foreach (TreeNode Childnode in ParentNode.Nodes)
                {
                    Childnode.ContextMenuStrip = ContexMenuHolder;
                }
            }
        }

        private void CCheckGroupBox_Upload_CheckedChanged(object sender, EventArgs e)
        {
            if (Module_Uploader.timer_UploadDisable.Enabled)
            {
                cCheckGroupBox_Upload.CheckedChanged -= CCheckGroupBox_Upload_CheckedChanged;
                cCheckGroupBox_Upload.Checked = false;
                cCheckGroupBox_Upload.CheckedChanged += CCheckGroupBox_Upload_CheckedChanged;
                MessageBox.Show("There is no upload credit remaining, please wait for hourly limit to reset or for posts to be approved.", "e621 ReBot");
            }
            else
            {
                int NodeCount = cTreeView_UploadQueue.Nodes.Count;
                cCheckGroupBox_Upload.Text = $"Uploader{(NodeCount > 0 ? $" ({NodeCount})" : null)}";
                Module_Uploader.timer_Upload.Enabled = ((CheckBox)sender).Checked && !Module_Uploader.Upload_BGW.IsBusy;
            }
        }

        public void CCheckGroupBox_Retry_CheckedChanged(object sender, EventArgs e)
        {
            if (Module_Retry.timer_RetryDisable.Enabled)
            {
                Module_Retry.timer_Retry.Enabled = false;
                if (Module_Credits.Credit_Flag == 0 && Module_Credits.Credit_Notes == 0)
                {
                    Module_Retry.RetryDisable("Flag and Note limit reached, please wait for limits to reset.");
                    return;
                }

                if (Module_Credits.Credit_Flag == 0)
                {
                    Module_Retry.RetryDisable("Flag limit reached, please wait for limit to reset.");
                    return;
                }
                if (Module_Credits.Credit_Notes == 0)
                {
                    Module_Retry.RetryDisable("Note limit reached, please wait for limit to reset.");
                    return;
                }
            }
            else
            {
                Module_Retry.timer_Retry.Enabled = ((CheckBox)sender).Checked && !Module_Retry.Retry_BGW.IsBusy;
            }
        }

        private void RetryQueue_Save()
        {
            if (cTreeView_RetryQueue.Nodes.Count > 0)
            {
                Dictionary<string, object> Data = new Dictionary<string, object>();
                foreach (TreeNode RetryNode in cTreeView_RetryQueue.Nodes)
                {
                    Data.Add(RetryNode.Text, RetryNode.Tag);
                }
                string SerializedData = JsonConvert.SerializeObject(Data);
                Properties.Settings.Default.RetrySave = SerializedData;
            }
            else
            {
                Properties.Settings.Default.RetrySave = null;
            }
            Properties.Settings.Default.Save();
        }

        private void RetryQueue_Load()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.RetrySave))
            {
                cTreeView_RetryQueue.BeginUpdate();
                JObject TempJson = JObject.Parse(Properties.Settings.Default.RetrySave);
                foreach (JProperty cItem in TempJson.Children())
                {
                    TreeNode TempNode = new TreeNode
                    {
                        Text = cItem.Name
                    };
                    if (!string.IsNullOrEmpty(cItem.Value.ToString()))
                    {
                        TempNode.Tag = cItem.Value.Value<double>();
                    }
                    cTreeView_RetryQueue.Nodes.Add(TempNode);
                }

                cTreeView_RetryQueue.EndUpdate();
            }
        }

        private void CCheckGroupBox_Convert_CheckedChanged(object sender, EventArgs e)
        {
            if (cCheckGroupBox_Convert.Checked)
            {
                if (File.Exists("ffmpeg.exe"))
                {
                    Module_Converter.timer_Conversion.Enabled = !Module_Converter.Conversion_BGW.IsBusy;
                }
                else
                {
                    MessageBox.Show("ffmpeg.exe not found.\nYou need to add it before trying to enable Conversionist.", "e621 Rebot");
                }
            }
            else
            {
                Module_Converter.timer_Conversion.Stop();
            }
        }

        private void ToolStripMenuItem_RemoveConversionNode_Click(object sender, EventArgs e)
        {
            if (WhichNodeIsIt.Index == 0 && Module_Converter.Conversion_BGW.IsBusy)
            {
                MessageBox.Show("You can't remove task from queue when conversion is already in progress.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                WhichNodeIsIt.Remove();
                int NodesRemaining = cTreeView_ConversionQueue.Nodes.Count;
                if (NodesRemaining > 0)
                {
                    cCheckGroupBox_Convert.Text = $"Conversionist ({NodesRemaining})";
                }
                else
                {
                    cCheckGroupBox_Convert.Text = "Conversionist";
                }
            }
        }

        private void ToolStripMenuItem_RemoveConversionAll_Click(object sender, EventArgs e)
        {
            if (Module_Converter.Conversion_BGW.IsBusy)
            {
                MessageBox.Show("You can't remove tasks from queue while conversion is in progress.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                cTreeView_ConversionQueue.Nodes.Clear();
                cCheckGroupBox_Convert.Text = "Conversionist";
            }
        }

        private void BU_ReverseUpload_Click(object sender, EventArgs e)
        {
            if (cCheckGroupBox_Upload.Checked | Module_Uploader.Upload_BGW.IsBusy)
            {
                MessageBox.Show("Upload queue and API should be stopped before attempting to reverse the order.", "e621 ReBot");
                return;
            }
            if (Module_TableHolder.Upload_Table.Rows.Count > 1)
            {
                Module_TableHolder.Upload_Table = Module_TableHolder.ReverseDataTable(Module_TableHolder.Upload_Table);
                Module_Uploader.ReverseUploadNodes();
            }
        }

        #endregion









        #region "Info Tab"

        private void PictureBox_Discord_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/7ncEzah");
        }

        private void Label_Forum_Click(object sender, EventArgs e)
        {
            Process.Start("https://e621.net/forum_topics/25939");
        }

        private void PictureBox_GitHub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/e621-ReBot/e621-ReBot-v2");
        }

        private void PictureBox_KoFi_Click(object sender, EventArgs e)
        {
            Process.Start("https://ko-fi.com/e621rebot");
        }

        #endregion









        #region "Settings"

        //private void TabPage_Settings_Paint(object sender, PaintEventArgs e)
        //{
        //    using (Pen TempPen = new Pen(Color.Black, 1))
        //    {
        //        TempPen.DashStyle = DashStyle.DashDotDot;
        //        e.Graphics.DrawLine(TempPen, new Point(36, 129), new Point(1229, 129));
        //    }
        //}

        private void TabPage_Settings_Enter(object sender, EventArgs e)
        {
            TrackBar_Volume.Value = Module_VolumeControl.GetApplicationVolume();
        }

        private void BU_RefreshCredit_Click(object sender, EventArgs e)
        {
            bU_RefreshCredit.Enabled = false;
            new Thread(Module_Credits.Check_Credit_All).Start();
        }

        private void Panel_CheckBoxOptions_Paint(object sender, PaintEventArgs e)
        {
            int TempHeightHolder = panel_CheckBoxOptions.Height - 1;
            using (Pen TempPen = new Pen(Color.Black, 1))
            {
                e.Graphics.DrawLine(TempPen, new Point(0, 0), new Point(0, TempHeightHolder));
                e.Graphics.DrawLine(TempPen, new Point(0, 0), new Point(8, 0));
                e.Graphics.DrawLine(TempPen, new Point(0, TempHeightHolder), new Point(8, TempHeightHolder));
            }
        }

        private void BU_APIKey_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.API_Key))
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.UserName))
                {
                    MessageBox.Show("I still don't know your name so you will have to introduce yourself first.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Properties.Settings.Default.FirstRun = true;
                    panel_Browser.Visible = true;
                    Module_CefSharp.CefSharpBrowser.Load("https://e621.net/session/new");
                    cTabControl_e621ReBot.SelectedIndex = 0;
                }
                else
                {
                    if (Form_APIKey._FormReference == null) new Form_APIKey().Show();
                }          
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to remove API Key?", "e621 ReBot", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Properties.Settings.Default.API_Key = "";
                    Properties.Settings.Default.Save();
                    bU_APIKey.Text = "Add API Key";
                    Module_APIControler.ToggleStatus();
                    BU_CancelAPIDL_Click(null, null);
                    MessageBox.Show("Some functions will remain disabled until you add the API Key again.", "e621 ReBot");
                }
            }
        }

        private void BU_APIKey_TextChanged(object sender, EventArgs e)
        {
            if (bU_APIKey.Text.Equals("Add API Key"))
            {
                GB_Upload.Enabled = false;
                bU_PoolWatcher.Enabled = false;
                bU_RefreshCredit.Enabled = false;
                if (Form_Preview._FormReference != null)
                {
                    Form_Preview._FormReference.PB_IQDBQ.Enabled = false;
                }
            }
            else
            {
                GB_Upload.Enabled = UploadCounter > 0;
                bU_PoolWatcher.Enabled = true;
                bU_RefreshCredit.Enabled = true;
                if (Form_Preview._FormReference != null)
                {
                    Form_Preview._FormReference.PB_IQDBQ.Enabled = true;
                }
            }
        }

        private void BU_NoteAdd_Click(object sender, EventArgs e)
        {
            new Form_Notes();
            Form_Notes._FormReference.Delete_Note_btn.Tag = "Save";
            Form_Notes._FormReference.ShowDialog();
        }

        private void BU_NoteRemove_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Note = "";
            Properties.Settings.Default.Save();
            bU_NoteAdd.Text = "Add Note";
            toolTip_Display.SetToolTip(bU_NoteAdd, "Leave a note for yourself that will appear when application starts.");
            bU_NoteRemove.Enabled = false;
        }

        private void TrackBar_Volume_Scroll(object sender, EventArgs e)
        {
            uint vol = (uint)(ushort.MaxValue / 100 * TrackBar_Volume.Value);
            Module_VolumeControl.waveOutSetVolume(IntPtr.Zero, (vol & 0xFFFF) | (vol << 16));
        }

        private void TrackBar_Volume_ValueChanged(object sender, EventArgs e)
        {
            cGroupBoxColored_Volume.Text = $"Volume ({TrackBar_Volume.Value}%)";
        }

        private void UpdateDays_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton SenderTemp = (RadioButton)sender;
            if (SenderTemp.Checked)
            {
                Properties.Settings.Default.UpdateDays = int.Parse(SenderTemp.Text);
                Properties.Settings.Default.Save();
            }
        }

        //

        private void RadioButton_GrabDisplayOrder_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton RadioButtonHolder = (RadioButton)sender;
            if (RadioButtonHolder.Checked)
            {
                Properties.Settings.Default.GrabDisplayOrder = RadioButtonHolder.Name.Substring(RadioButtonHolder.Name.Length - 1);
                Properties.Settings.Default.Save();
            }
        }

        private void RadioButton_GridItemStyle_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton RadioButtonHolder = (RadioButton)sender;
            if (RadioButtonHolder.Checked)
            {
                Properties.Settings.Default.GridItemStyle = RadioButtonHolder.Name.Substring(RadioButtonHolder.Name.Length - 1);
                Properties.Settings.Default.Save();

                Form_Loader._customGIStyle = Properties.Settings.Default.GridItemStyle.Equals("0") ? GridItemStyle.Defult : GridItemStyle.Simple;

                if (flowLayoutPanel_Grid.Controls.Count > 0)
                {
                    foreach (e6_GridItem e6GridItemTemp in flowLayoutPanel_Grid.Controls)
                    {
                        e6GridItemTemp._Style = Form_Loader._customGIStyle;
                    }
                }
            }
        }

        private void RadioButton_ProgressBarStyle_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton RadioButtonHolder = (RadioButton)sender;
            if (RadioButtonHolder.Checked)
            {
                Properties.Settings.Default.ProgressBarStyle = RadioButtonHolder.Name.Substring(RadioButtonHolder.Name.Length - 1);
                Properties.Settings.Default.Save();

                Form_Loader._customPBStyle = Properties.Settings.Default.ProgressBarStyle.Equals("0") ? CustomPBStyle.Hex : CustomPBStyle.Round;

                if (cFlowLayoutPanel_ProgressBarHolder.Controls.Count > 0)
                {
                    foreach (Custom_ProgressBar cPBTemp in cFlowLayoutPanel_ProgressBarHolder.Controls)
                    {
                        cPBTemp.PBStyle = Form_Loader._customPBStyle;
                    }
                }

                if (flowLayoutPanel_Grid.Controls.Count > 0)
                {
                    foreach (e6_GridItem e6GridItemTemp in flowLayoutPanel_Grid.Controls)
                    {
                        e6GridItemTemp._Style = Form_Loader._customGIStyle;
                    }
                }
            }
        }

        //

        public List<string> Blacklist = new List<string>();
        private void BU_Blacklist_Click(object sender, EventArgs e)
        {
            new Form_Blacklist(bU_Blacklist.PointToScreen(Point.Empty));
            Form_Blacklist._FormReference.ShowDialog();
        }

        //

        private void TextBox_Delay_Enter(object sender, EventArgs e)
        {
            TextBox SenderTB = (TextBox)sender;
            SenderTB.Text = null;
        }

        private void TextBox_Delay_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter key pressed
            if (e.KeyCode == Keys.Enter)
            {
                cGroupBoxColored_ActionDelay.Focus();
                e.Handled = true;
                return;
            }
        }

        private void TextBox_Delay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                return;
            }

            // Don't allow anything that isn't a number
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            TextBox SenderTB = (TextBox)sender;
            // Don't allow 0 as first value
            if (SenderTB.Text.Length == 0 && e.KeyChar == '0')
            {
                e.Handled = true;
                return;
            }
        }

        private void TextBox_DelayGrabber_Leave(object sender, EventArgs e)
        {
            int TBValue;
            if (!int.TryParse(textBox_DelayGrabber.Text, out TBValue))
            {
                TBValue = 0;
            }
            if (TBValue < 250)
            {
                TBValue = 250;
            }
            textBox_DelayGrabber.Text = TBValue.ToString();
            Properties.Settings.Default.DelayGrabber = TBValue;
            Properties.Settings.Default.Save();

            bool RestartTimeCheck = Module_Grabber.timer_Grab.Enabled;
            Module_Grabber.timer_Grab.Stop();
            Module_Grabber.timer_Grab.Interval = TBValue;
            if (RestartTimeCheck)
            {
                Module_Grabber.timer_Grab.Start();
            }
        }

        private void TextBox_DelayUploader_Leave(object sender, EventArgs e)
        {
            int TBValue;
            if (!int.TryParse(textBox_DelayUploader.Text, out TBValue))
            {
                TBValue = 0;
            }
            if (TBValue < 1000)
            {
                TBValue = 1000;
            }
            textBox_DelayUploader.Text = TBValue.ToString();
            Properties.Settings.Default.DelayUploader = TBValue;
            Properties.Settings.Default.Save();

            bool RestartTimeCheck = Module_Uploader.timer_Upload.Enabled;
            Module_Uploader.timer_Upload.Stop();
            Module_Uploader.timer_Upload.Interval = TBValue;
            if (RestartTimeCheck)
            {
                Module_Uploader.timer_Upload.Start();
            }
        }

        private void TextBox_DelayDownload_Leave(object sender, EventArgs e)
        {
            int TBValue;
            if (!int.TryParse(textBox_DelayDownload.Text, out TBValue))
            {
                TBValue = 0;
            }
            if (TBValue < 250)
            {
                TBValue = 250;
            }
            textBox_DelayDownload.Text = TBValue.ToString();
            Properties.Settings.Default.DelayDownload = TBValue;
            Properties.Settings.Default.Save();

            bool RestartTimeCheck = Module_Downloader.timer_Download.Enabled;
            Module_Downloader.timer_Download.Stop();
            Module_Downloader.timer_Download.Interval = TBValue;
            if (RestartTimeCheck)
            {
                Module_Downloader.timer_Download.Start();
            }
        }

        //

        private void NamingE6_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton SenderTemp = (RadioButton)sender;
            if (SenderTemp.Checked)
            {
                Properties.Settings.Default.Naming_e6 = int.Parse(SenderTemp.Tag.ToString());
                Properties.Settings.Default.Save();
            }
        }

        private void NamingWeb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton SenderTemp = (RadioButton)sender;
            if (SenderTemp.Checked)
            {
                Properties.Settings.Default.Naming_web = int.Parse(SenderTemp.Tag.ToString());
                Properties.Settings.Default.Save();
            }
        }

        private readonly Timer FolderDialogFix;
        private void BU_DownloadFolderChange_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FolderDialogTemp = new FolderBrowserDialog()
            {
                SelectedPath = Application.StartupPath
            };

            //To make shitty FolderBrowserDialog scroll folder into view.
            FolderDialogFix.Start();
            if (FolderDialogTemp.ShowDialog() == DialogResult.OK)
            {
                label_DownloadsFolder.Text = $"{FolderDialogTemp.SelectedPath}\\Downloads\\";
                Properties.Settings.Default.DownloadsFolderLocation = label_DownloadsFolder.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void FolderDialogFix_Tick(object sender, EventArgs e)
        {
            FolderDialogFix.Stop();
            SendKeys.Send("{TAB}{TAB}{RIGHT}");
            FolderDialogFix.Dispose();
        }

        //

        private void CheckBox_ConverterKeepOriginal_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Converter_KeepOriginal = CheckBox_ConverterKeepOriginal.Checked;
            Properties.Settings.Default.Save();
        }

        private void CheckBox_ConverterDontConvertVideos_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Converter_DontConvertVideos = CheckBox_ConverterDontConvertVideos.Checked;
            Properties.Settings.Default.Save();
        }

        private void Label_DragDropConvert_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label_DragDropConvert.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void Label_DragDropConvert_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void Label_DragDropConvert_DragDrop(object sender, DragEventArgs e)
        {
            List<string> fileList = new List<string>();

            string[] DropList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string FilePath in DropList)
            {
                if (FilePath.ToLower().EndsWith(".mp4") || FilePath.ToLower().EndsWith(".swf"))
                {
                    fileList.Add(FilePath);
                }
            }

            switch (fileList.Count)
            {
                case 0:
                    {
                        MessageBox.Show("No supported video files detected.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }

                case 1:
                    {
                        Module_FFmpeg.DragDropConvert(fileList[0]);
                        break;
                    }

                default:
                    {
                        MessageBox.Show("Can only convert one video at a time.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }

            }
        }

        private MemoryStream DownloadDBExport(string URL, Button_Unfocusable ControlStatusUpdate)
        {
            MemoryStream DownloadedBytes = new MemoryStream();
            DateTime DateTimeTempUTC = DateTime.UtcNow;
            int RetryCount = 0;
        RetryDate:
            HttpWebRequest ExportDataDownloader = (HttpWebRequest)WebRequest.Create(string.Format(URL, DateTimeTempUTC.Year, DateTimeTempUTC.ToString("MM"), DateTimeTempUTC.ToString("dd")));
            if (RetryCount == 3)
            {
                return null;
            }
            try
            {
                using (HttpWebResponse DownloaderReponse = (HttpWebResponse)ExportDataDownloader.GetResponse())
                {
                    using (Stream DownloadStream = DownloaderReponse.GetResponseStream())
                    {
                        byte[] DownloadBuffer = new byte[65536]; // 64 kB buffer
                        while (DownloadedBytes.Length < DownloaderReponse.ContentLength)
                        {
                            int DownloadStreamPartLength = DownloadStream.Read(DownloadBuffer, 0, DownloadBuffer.Length);
                            if (DownloadStreamPartLength > 0)
                            {
                                DownloadedBytes.Write(DownloadBuffer, 0, DownloadStreamPartLength);
                                double ReportPercentage = DownloadedBytes.Length / (double)DownloaderReponse.ContentLength;
                                bU_DLTags.BeginInvoke(new Action(() => { ControlStatusUpdate.Text = $"Downloading: {ReportPercentage.ToString("P2")}"; }));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                RetryCount += 1;
                DateTimeTempUTC = DateTimeTempUTC.AddDays(-1);
                goto RetryDate;
            }
            return DownloadedBytes;
        }

        private void BU_DLTags_Click(object sender, EventArgs e)
        {
            bU_DLTags.Enabled = false;
            bU_DLPools.Enabled = false;
            new Thread(DLWork_Tags).Start();
        }

        private void DLWork_Tags()
        {
            MemoryStream DownloadedStream = DownloadDBExport("https://e621.net/db_export/tags-{0}-{1}-{2}.csv.gz", bU_DLTags);
            if (DownloadedStream != null)
            {
                List<string> TagList = new List<string>();
                bU_DLTags.BeginInvoke(new Action(() => { bU_DLTags.Text = "Processing..."; }));
                DownloadedStream.Position = 0;
                using (GZipStream TagsZip = new GZipStream(DownloadedStream, CompressionMode.Decompress))
                {
                    using (StreamReader StreamReaderTemp = new StreamReader(TagsZip))
                    {
                        StreamReaderTemp.ReadLine();
                        string ReadCSV = StreamReaderTemp.ReadToEnd();
                        using (TextFieldParser CSVParser = new TextFieldParser(new StringReader(ReadCSV)))
                        {
                            CSVParser.HasFieldsEnclosedInQuotes = true;
                            CSVParser.SetDelimiters(",");

                            List<Tuple<int, string>> TagListTemp = new List<Tuple<int, string>>();
                            string[] CSVFields;
                            while (!CSVParser.EndOfData)
                            {
                                CSVFields = CSVParser.ReadFields();
                                if (!CSVFields[3].Equals("0"))
                                {
                                    TagListTemp.Add(new Tuple<int, string>(int.Parse(CSVFields[3]), CSVFields[1]));
                                }
                            }
                            TagListTemp.Sort((x, y) => y.Item1.CompareTo(x.Item1));
                            TagList = TagListTemp.Select(x => x.Item2).ToList();
                            TagListTemp.Clear();
                        }
                    }
                    DownloadedStream.Dispose();
                }
                TagList = TagList.Distinct().ToList();

                DownloadedStream = DownloadDBExport("https://e621.net/db_export/tag_aliases-{0}-{1}-{2}.csv.gz", bU_DLTags);
                if (DownloadedStream != null)
                {
                    Dictionary<string, List<string>> TagAliases = new Dictionary<string, List<string>>();
                    bU_DLTags.BeginInvoke(new Action(() => { bU_DLTags.Text = "Processing..."; }));
                    DownloadedStream.Position = 0;
                    using (GZipStream TagsZip = new GZipStream(DownloadedStream, CompressionMode.Decompress))
                    {
                        using (StreamReader StreamReaderTemp = new StreamReader(TagsZip))
                        {
                            StreamReaderTemp.ReadLine();
                            string ReadCSV = StreamReaderTemp.ReadToEnd();
                            using (TextFieldParser CSVParser = new TextFieldParser(new StringReader(ReadCSV)))
                            {
                                CSVParser.HasFieldsEnclosedInQuotes = true;
                                CSVParser.SetDelimiters(",");

                                string[] CSVFields;
                                while (!CSVParser.EndOfData)
                                {
                                    CSVFields = CSVParser.ReadFields();
                                    if (CSVFields[4].Equals("active"))
                                    {
                                        if (TagAliases.ContainsKey(CSVFields[2]))
                                        {
                                            TagAliases[CSVFields[2]].Add(CSVFields[1]);
                                        }
                                        else
                                        {
                                            TagAliases.Add(CSVFields[2], new List<string> { CSVFields[1] });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    DownloadedStream.Dispose();

                    if (TagAliases != null)
                    {
                        StringBuilder StringBuilderTemp = new StringBuilder();
                        foreach (string StringTemp in TagList)
                        {
                            StringBuilderTemp.Append(StringTemp);
                            if (TagAliases.ContainsKey(StringTemp))
                            {
                                StringBuilderTemp.Append("," + string.Join(",", TagAliases[StringTemp]));
                            }
                            StringBuilderTemp.Append("✄");
                        }
                        File.WriteAllText("tags.txt", StringBuilderTemp.ToString());
                        StringBuilderTemp = null;
                    }
                    else
                    {
                        File.WriteAllText("tags.txt", string.Join("✄", TagList));
                    }

                    BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("Downloaded all tags.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bU_DLTags.Text = "DL Tags";
                        bU_DLTags.Enabled = true;
                        bU_DLPools.Enabled = true;
                    }));
                    AutoTagsList_Tags.Clear();
                    Read_AutoTags();
                }
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            else
            {
                BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Downloaded failed.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bU_DLTags.Text = "DL Tags";
                    bU_DLTags.Enabled = true;
                    bU_DLPools.Enabled = true;
                }));
            }
        }

        private void Read_AutoTags()
        {
            List<string> TempStringList = new List<string>();
            foreach (string StringTemp in File.ReadAllText("tags.txt").Split(new string[] { "✄" }, StringSplitOptions.RemoveEmptyEntries))
            {
                DC_AutocompleteItem DC_AutocompleteItemTemp;
                if (StringTemp.Contains(","))
                {
                    TempStringList = StringTemp.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    string TitleHolder = TempStringList[0];
                    TempStringList.RemoveAt(0);
                    DC_AutocompleteItemTemp = new DC_AutocompleteItem(TitleHolder, TempStringList.ToArray());
                    TempStringList.Clear();
                }
                else
                {
                    DC_AutocompleteItemTemp = new DC_AutocompleteItem(StringTemp);
                }
                AutoTagsList_Tags.Add(DC_AutocompleteItemTemp);
            }

            CustomTagsTableHolder.Clear();
            foreach (string StringTemp in Module_DB.DB_CT_ReadTable())
            {
                AutoTagsList_Tags.Add(new DC_AutocompleteItem(StringTemp));
                CustomTagsTableHolder.Add(StringTemp);
            }
            AutoTags.SetAutocompleteItems(AutoTagsList_Tags);
            CustomTagsTableHolder.Sort();
            AutoCompleteStringCollection TempACSC = new AutoCompleteStringCollection();
            TempACSC.AddRange(CustomTagsTableHolder.ToArray());
            BeginInvoke(new Action(() =>
            {
                textBox_AutoTagsEditor.AutoCompleteCustomSource = TempACSC;
                cGroupBoxColored_AutocompleteTagEditor.Enabled = true;
            }));
        }

        private void BU_DLPools_Click(object sender, EventArgs e)
        {
            bU_DLPools.Enabled = false;
            bU_DLTags.Enabled = false;
            new Thread(DLWork_Pools).Start();
        }

        private void DLWork_Pools()
        {
            MemoryStream DownloadedStream = DownloadDBExport("https://e621.net/db_export/pools-{0}-{1}-{2}.csv.gz", bU_DLPools);
            if (DownloadedStream != null)
            {
                List<string> PoolList = new List<string>();
                bU_DLPools.BeginInvoke(new Action(() => { bU_DLPools.Text = "Processing..."; }));
                DownloadedStream.Position = 0;
                using (GZipStream TagsZip = new GZipStream(DownloadedStream, CompressionMode.Decompress))
                {
                    using (StreamReader StreamReaderTemp = new StreamReader(TagsZip))
                    {
                        StreamReaderTemp.ReadLine();
                        string ReadCSV = StreamReaderTemp.ReadToEnd();
                        using (TextFieldParser CSVParser = new TextFieldParser(new StringReader(ReadCSV)))
                        {
                            CSVParser.HasFieldsEnclosedInQuotes = true;
                            CSVParser.SetDelimiters(",");

                            string[] CSVFields;
                            while (!CSVParser.EndOfData)
                            {
                                CSVFields = CSVParser.ReadFields();
                                PoolList.Add($"{CSVFields[0]},{CSVFields[1]}");
                            }
                        }
                    }
                }
                DownloadedStream.Dispose();

                PoolList = PoolList.Distinct().ToList();
                PoolList.Reverse();
                File.WriteAllText("pools.txt", string.Join("✄", PoolList));
                BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Downloaded all pools.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bU_DLPools.Text = "DL Pools";
                    bU_DLPools.Enabled = true;
                    bU_DLTags.Enabled = true;
                }));
                AutoTagsList_Pools.Clear();
                Read_AutoPools();
            }
            else
            {
                BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Download failed.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bU_DLPools.Text = "DL Pools";
                    bU_DLPools.Enabled = true;
                    bU_DLTags.Enabled = true;
                }));
            }
        }

        private void Read_AutoPools()
        {
            string[] DataSplitter;
            List<string> TempList = new List<string>(File.ReadAllText("pools.txt").Split(new string[] { "✄" }, StringSplitOptions.RemoveEmptyEntries));
            foreach (string PoolData in TempList)
            {
                DataSplitter = PoolData.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                AutoTagsList_Pools.Add(new DC_MultiCollumnAutocompleteItem(new string[] { "#" + DataSplitter[0], DataSplitter[1].Replace("_", " ") }, "pool:" + DataSplitter[0], new string[] { DataSplitter[1].ToLower() }) { CollumnWidth = new int[] { 48, 272 } });
            }
        }

        //

        private void Textbox_AutoTagsEditor_Enter(object sender, EventArgs e)
        {
            textBox_AutoTagsEditor.Text = null;
        }

        private void Textbox_AutoTagsEditor_Leave(object sender, EventArgs e)
        {
            textBox_AutoTagsEditor.Text = "Type here";
            bU_AutoTagsAdd.Enabled = false;
            bU_AutoTagsRemove.Enabled = false;
        }

        public DC_AutocompleteMenu AutoTags = new DC_AutocompleteMenu();
        public List<DC_AutocompleteItem> AutoTagsList_Tags = new List<DC_AutocompleteItem>();
        public List<DC_MultiCollumnAutocompleteItem> AutoTagsList_Pools = new List<DC_MultiCollumnAutocompleteItem>();

        private void Textbox_AutoTagsEditor_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    {
                        e.SuppressKeyPress = true;
                        break;
                    }

                case Keys.Escape:
                    {
                        if (!AutoTags.Visible)
                        {
                            textBox_AutoTagsEditor.Focus();
                        }
                        e.SuppressKeyPress = true;
                        break;
                    }
            }
        }

        private List<string> CustomTagsTableHolder = new List<string>();
        private void Textbox_AutoTagsEditor_TextChanged(object sender, EventArgs e)
        {
            if (textBox_AutoTagsEditor.TextLength > 0)
            {
                if (CustomTagsTableHolder == null)
                {
                    CustomTagsTableHolder = Module_DB.DB_CT_ReadTable();
                }

                if (CustomTagsTableHolder.Contains(textBox_AutoTagsEditor.Text))
                {
                    bU_AutoTagsAdd.Enabled = false;
                    bU_AutoTagsRemove.Enabled = true;
                }
                else
                {
                    bU_AutoTagsAdd.Enabled = true;
                    bU_AutoTagsRemove.Enabled = false;
                }
            }
            else
            {
                bU_AutoTagsAdd.Enabled = false;
                bU_AutoTagsRemove.Enabled = false;
            }
        }

        private void BU_AutoTagsAdd_Click(object sender, EventArgs e)
        {
            textBox_AutoTagsEditor.Text = textBox_AutoTagsEditor.Text.ToLower();
            Module_DB.DB_CT_CreateRecord(textBox_AutoTagsEditor.Text);
            CustomTagsTableHolder.Add(textBox_AutoTagsEditor.Text);
            AutoTagsList_Tags.Add(new DC_AutocompleteItem(textBox_AutoTagsEditor.Text));
            textBox_AutoTagsEditor.AutoCompleteCustomSource.Clear();
            AutoCompleteStringCollection TempACSC = new AutoCompleteStringCollection();
            TempACSC.AddRange(CustomTagsTableHolder.ToArray());
            textBox_AutoTagsEditor.AutoCompleteCustomSource = TempACSC;
            cGroupBoxColored_AutocompleteTagEditor.Focus();
        }

        private void BU_AutoTagsRemove_Click(object sender, EventArgs e)
        {
            textBox_AutoTagsEditor.Text = textBox_AutoTagsEditor.Text.ToLower();
            Module_DB.DB_CT_DeleteRecord(textBox_AutoTagsEditor.Text);
            CustomTagsTableHolder.Remove(textBox_AutoTagsEditor.Text);
            AutoTagsList_Tags.Clear();
            AutoTags.Dispose();
            AutoTags = new DC_AutocompleteMenu();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Read_AutoTags();
            cGroupBoxColored_AutocompleteTagEditor.Focus();
        }

        //

        private void CheckBox_BigMode_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LoadBigForm = CheckBox_BigMode.Checked;
            Properties.Settings.Default.Save();
        }

        private void CheckBox_AutocompleteTags_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutocompleteTags = CheckBox_AutocompleteTags.Checked;
            Properties.Settings.Default.Save();
        }

        private void CheckBox_ManualInferiorSave_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ManualInferiorSave = CheckBox_ManualInferiorSave.Checked;
            Properties.Settings.Default.Save();
        }

        private void CheckBox_ExpandedDescription_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ExpandedDescription = CheckBox_ExpandedDescription.Checked;
            Properties.Settings.Default.Save();
        }

        private void CheckBox_RemoveBVAS_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RemoveBVAS = CheckBox_RemoveBVAS.Checked;
            Properties.Settings.Default.Save();
        }

        private void CheckBox_ClearCache_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ClearCache = CheckBox_ClearCache.Checked;
            Properties.Settings.Default.Save();
        }

        private void CheckBox_DisableGPU_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DisableGPU = CheckBox_DisableGPU.Checked;
            Properties.Settings.Default.Save();
        }

        private void CheckBox_EnableReplacement_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ReplacementBeta = CheckBox_EnableReplacement.Checked;
            Properties.Settings.Default.Save();
        }

        //

        private void BU_PoolWatcher_Click(object sender, EventArgs e)
        {
            new Form_PoolWatcher(bU_PoolWatcher.PointToScreen(Point.Empty));
            Form_PoolWatcher._FormReference.ShowDialog();
        }

        //

        private void BU_GetGenders_Click(object sender, EventArgs e)
        {
            MemoryStream DownloadedStream = DownloadDBExport("https://e621.net/db_export/tag_aliases-{0}-{1}-{2}.csv.gz", bU_GetGenders);
            if (DownloadedStream != null)
            {
                List<string> GendersList = new List<string>() { "ambiguous_gender", "male", "female", "intersex" };
                string ReadCSVAliases = null;
                string ReadCSVImplications = null;

                bU_GetGenders.BeginInvoke(new Action(() => { bU_GetGenders.Text = "Processing..."; }));
                DownloadedStream.Position = 0;
                using (GZipStream TagsZip = new GZipStream(DownloadedStream, CompressionMode.Decompress))
                {
                    using (StreamReader StreamReaderTemp = new StreamReader(TagsZip))
                    {
                        StreamReaderTemp.ReadLine();
                        ReadCSVAliases = StreamReaderTemp.ReadToEnd();
                    }
                }
                DownloadedStream.Dispose();

                DownloadedStream = DownloadDBExport("https://e621.net/db_export/tag_implications-{0}-{1}-{2}.csv.gz", bU_GetGenders);
                if (DownloadedStream != null)
                {
                    bU_GetGenders.BeginInvoke(new Action(() => { bU_GetGenders.Text = "Processing..."; }));
                    DownloadedStream.Position = 0;
                    using (GZipStream TagsZip = new GZipStream(DownloadedStream, CompressionMode.Decompress))
                    {
                        using (StreamReader StreamReaderTemp = new StreamReader(TagsZip))
                        {
                            StreamReaderTemp.ReadLine();
                            ReadCSVImplications = StreamReaderTemp.ReadToEnd();
                        }
                    }
                    DownloadedStream.Dispose();
                }

                // = = = = =

                for (int repeat = 0; repeat < 4; repeat++)
                {
                    string[] CSVFields;
                    TextFieldParser CSVParser = new TextFieldParser(new StringReader(ReadCSVAliases))
                    {
                        HasFieldsEnclosedInQuotes = true
                    };
                    CSVParser.SetDelimiters(",");
                    while (!CSVParser.EndOfData)
                    {
                        CSVFields = CSVParser.ReadFields();
                        if (GendersList.Contains(CSVFields[2]) && CSVFields[4].Equals("active"))
                        {
                            GendersList.Add(CSVFields[1]);
                        }
                    }
                    CSVParser.Dispose();
                    CSVParser = new TextFieldParser(new StringReader(ReadCSVImplications))
                    {
                        HasFieldsEnclosedInQuotes = true
                    };
                    CSVParser.SetDelimiters(",");
                    while (!CSVParser.EndOfData)
                    {
                        CSVFields = CSVParser.ReadFields();
                        if (GendersList.Contains(CSVFields[2]) && CSVFields[4].Equals("active"))
                        {
                            GendersList.Add(CSVFields[1]);
                        }
                    }
                    CSVParser.Dispose();
                    GendersList = GendersList.Distinct().ToList();
                }
                GendersList.Sort();
                File.WriteAllText("Genders.txt", string.Join("✄", GendersList));
                MessageBox.Show("Downloaded all Genders.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Download failed.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            bU_GetGenders.Text = "Get Genders";
        }

        public List<string> Gender_Tags = new List<string>();
        private void Read_Genders()
        {
            Gender_Tags.AddRange(Properties.Resources.Genders.Split(new string[] { "✄" }, StringSplitOptions.RemoveEmptyEntries));
        }

        private void BU_GetDNPs_Click(object sender, EventArgs e)
        {
            MemoryStream DownloadedStream = DownloadDBExport("https://e621.net/db_export/tag_implications-{0}-{1}-{2}.csv.gz", bU_GetDNPs);
            if (DownloadedStream != null)
            {
                List<string> DNPList = new List<string>();
                bU_GetDNPs.BeginInvoke(new Action(() => { bU_GetDNPs.Text = "Processing..."; }));
                DownloadedStream.Position = 0;
                string[] DNPStrings = new string[] { "avoid_posting", "conditional_dnp" };
                using (GZipStream TagsZip = new GZipStream(DownloadedStream, CompressionMode.Decompress))
                {
                    using (StreamReader StreamReaderTemp = new StreamReader(TagsZip))
                    {
                        StreamReaderTemp.ReadLine();
                        string ReadCSV = StreamReaderTemp.ReadToEnd();
                        using (TextFieldParser CSVParser = new TextFieldParser(new StringReader(ReadCSV)))
                        {
                            CSVParser.HasFieldsEnclosedInQuotes = true;
                            CSVParser.SetDelimiters(",");

                            string[] CSVFields;
                            while (!CSVParser.EndOfData)
                            {
                                CSVFields = CSVParser.ReadFields();
                                if (DNPStrings.Any(str => CSVFields[2].Equals(str)) && CSVFields[4].Equals("active"))
                                {
                                    DNPList.Add(CSVFields[1]);
                                }
                            }
                        }
                    }
                }
                DownloadedStream.Dispose();

                DownloadedStream = DownloadDBExport("https://e621.net/db_export/tag_aliases-{0}-{1}-{2}.csv.gz", bU_GetDNPs);
                if (DownloadedStream != null)
                {
                    bU_GetDNPs.BeginInvoke(new Action(() => { bU_GetDNPs.Text = "Processing..."; }));
                    DownloadedStream.Position = 0;
                    using (GZipStream TagsZip = new GZipStream(DownloadedStream, CompressionMode.Decompress))
                    {
                        using (StreamReader StreamReaderTemp = new StreamReader(TagsZip))
                        {
                            StreamReaderTemp.ReadLine();
                            string ReadCSV = StreamReaderTemp.ReadToEnd();
                            using (TextFieldParser CSVParser = new TextFieldParser(new StringReader(ReadCSV)))
                            {
                                CSVParser.HasFieldsEnclosedInQuotes = true;
                                CSVParser.SetDelimiters(",");

                                string[] CSVFields;
                                while (!CSVParser.EndOfData)
                                {
                                    CSVFields = CSVParser.ReadFields();
                                    if (DNPList.Contains(CSVFields[2]) && CSVFields[4].Equals("active"))
                                    {
                                        DNPList.Add(CSVFields[1]);
                                    }
                                }
                            }
                        }
                    }
                    DownloadedStream.Dispose();
                }

                DNPList = DNPList.Distinct().ToList();
                DNPList.Sort();
                File.WriteAllText("DNPs.txt", string.Join("✄", DNPList));
                MessageBox.Show("Downloaded all DNPs.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Download failed.", "e621 ReBot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            bU_GetDNPs.Text = "Get DNPs";
        }

        public List<string> DNP_Tags = new List<string>();
        private void Read_DNPs()
        {
            DNP_Tags.AddRange(Properties.Resources.DNPs.Split(new string[] { "✄" }, StringSplitOptions.RemoveEmptyEntries));
        }

        //

        private void BU_AppData_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "e621_ReBot_v2"));
        }

        private void BU_ResetSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset all settings?", "e621 ReBot", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset();
                Cef.Shutdown();
                new Thread(() =>
                {
                    Thread.Sleep(500);
                    Thread.CurrentThread.IsBackground = true;
                    int FailCount = 0;
                    bool DeleteWorked = false;
                    do
                    {
                        try
                        {
                            Directory.Delete("CefSharp Cache", true);
                            DeleteWorked = true;
                        }
                        catch (Exception)
                        {
                            FailCount++;
                            Thread.Sleep(500);
                        }
                    } while (DeleteWorked == false && FailCount < 6);
                }).Start();
                Close();
            }
        }

        #endregion









        #region "Puzzle Game"

        private void Panel_GameStart_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel_GameStart.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void PB_GameThumb_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, pB_GameThumb.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void GB_StartGame_Click(object sender, EventArgs e)
        {
            GB_StartGame.Enabled = false;
            gamePanel_Main.Focus();
            gamePanel_Main.LoadPuzzle();
        }

        private void GB_RestartGame_Click(object sender, EventArgs e)
        {
            gamePanel_Main.Focus();
            gamePanel_Main.ResetPuzzle();
        }

        private void CC_GameThumb_CheckedChanged(object sender, EventArgs e)
        {
            pB_GameThumb.Visible = CC_GameThumb.Checked;
        }

        private void LabelPuzzle_SelectedPost_Click(object sender, EventArgs e)
        {
            string e6Post = "https://e621.net/post/show/" + labelPuzzle_SelectedPost.Tag.ToString();
            if (ModifierKeys.HasFlag(Keys.Alt))
            {
                Process.Start(e6Post);
            }
            else
            {
                QuickButtonPanel.Visible = false;
                panel_Browser.Visible = true;
                Form_Loader._FormReference.BringToFront();
                Form_Loader._FormReference.cTabControl_e621ReBot.SelectedIndex = 0;
                if (!Module_CefSharp.CefSharpBrowser.Address.Equals(e6Post))
                {
                    Module_CefSharp.CefSharpBrowser.Load(e6Post);
                }
            }
        }

        #endregion
    }
}