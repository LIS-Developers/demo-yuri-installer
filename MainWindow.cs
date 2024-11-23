using net_yuri_installer.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using SevenZip;
using System.Media;
using AxWMPLib;
using System.Diagnostics;
using WMPLib;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Runtime.InteropServices.ComTypes;

namespace net_yuri_installer
{
    // 程序主窗口类
    public sealed partial class _StartWindow : Form
    {
        // 程序的常量
        #region Consts

        // 表示安装包的开始页面。
        public const int _开始界面 = 0;
        public const int _许可协议 = 1;
        public const int _CD验证 = 2;
        public const int _安装选项 = 3;
        public const int _路径选择 = 4;
        public const int _安装中 = 5;
        public const int _完成安装 = 6;

        // 进度条长度。
        public const int progressbarLength = 619;

        // 右侧按钮左侧距离。
        public const int bottomButtonX = 644;

        // 注册游戏的程序。
        public const string _RegSetMD = "RegSetMD.exe";

        // 默认安装路径（路径框默认显示的路径）。
        public const string _defaultPath = @"C:\Program Files (x86)\";

        // 游戏主程序。
        public const string gameExeName = "gamemd.exe";

        // 游戏启动器。
        public const string gameLauncher = "RA2MD.EXE";

        // 自述文件。
        public const string readmeFile = "readmemd.doc";

        // 兼容性设置语句。
        public const string jrxStr = "~ DWM8And16BitMitigation RUNASADMIN 16BITCOLOR WINXPSP2 HIGHDPIAWARE";

        // 制作人员名单。
        private const string creditFull = "製作人員（安裝程序） - Credits for this installer\n\nEnderseven Tina： 主要製作貢獻\n君悅OwO：技術顧問\n贊助商：Flactine\n部分代碼支持：豆包AI\n部分功能資料來源：CSDN\n\n感謝 Westwood Studios 製作了偉大的遊戲《終極動員令：尤里的復仇》。";

        // 游戏官网。
        public const string gameHomepage = "www.westwood.com";

        // 游戏作者主页。
        public const string authorHomepage = "https://dtsm.mqmrx.cn";

        // 卸载程序。
        public const string unsExe = "uninstall.exe";

        #endregion

        // 程序的主要窗口逻辑
        #region MainWindow

        public static readonly string runPath = Application.StartupPath;
        // public static string[] 运参;

        private readonly Bitmap bg1 = Resources.Binary_bg;
        private readonly Bitmap bg2 = Resources.Binary_bg2;

        // 开头播放的视频。
        private string startVideoName = "";

        // 菜单按钮收缩展开的声音。
        private string menuUpdateAudio = "";

        // 背景播放的视频。
        private string backgroundVideoName = "";

        private bool tenkaiedTreeView = false;
        private bool switchMusic = true;

        private static DriveInfo drive;
        private static long freeSpace = 0;
        private static long spaceNeeded = 0;
        private static long setupSize = 0;
        private static long currentSize = 0;

        private int 当前进度;
        private bool touchedButton;
        private bool hasBackgroundVideo;
        private string installTo;

        // 是否显示U盘、移动硬盘之类的设备。
        private const bool displayRemovable = true;

        private bool firstLaunch = true;

        private static readonly string currentNamespace = Assembly.GetEntryAssembly().EntryPoint.DeclaringType.Namespace;

        private static readonly Assembly assembly = Assembly.GetExecutingAssembly();

        private int progressBase = 0;
        private string pathInfo_Text;

        // 启动窗口。
        public _StartWindow()
        {
            BottomBtn1 = new string[7];
            BottomBtn2 = new string[7];
            BottomBtn3 = new string[7];
            BottomBarText = new string[7, 5];

            Icon = Resources.icon;
            StartPosition = FormStartPosition.CenterScreen;

            CustomControls();
            InitializeComponent();
            L18n();

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            ButtonTexture[0] = Resources.ButtonNormal;
            ButtonTexture[1] = Resources.ButtonLight;
            ButtonTexture[2] = Resources.ButtonNone;
            ButtonTexture[3] = Resources.ButtonNull;
            ButtonTexture[4] = Resources.ButtonClosed;

            bottomButton1.Left = bottomButton2.Left = bottomButtonX;

            // bottomButton1.Location = new Point(bottomButtonX, bottomButton1.Location.Y);

            // waitingCursor = new Cursor(this.GetType(), "Resources.waiting.cur");

            Text = WindowTitle;
            credits.Text = creditFull;

            startVideo.Size = new Size(800, 600);
            startVideo.uiMode = "none";
            startVideo.enableContextMenu = false;
            startVideo.Ctlenabled = false;

            SetForeColorAndFont();

            credits.BackColor = Color.Black;
            credits.MaximumSize = new Size(800, 65535);
            credits.MinimumSize = new Size(800, 600);

            menuUpdateAudio = MizukiTools.EmbedToOutside(Resources.MenuUpdate, "MenuUpdate.wav", menuUpdate);

            ChangeButtonText();

            chkAgreeLicenses.Text = "";

            pathBox.Text = Path.Combine(_defaultPath, @"RA2MD\");

            chkCreateDesktopBox.Location = new Point(67, pathBoxPanel.Bottom + 9);
            chkCreateStartMenuBox.Top = chkCreateDesktopBox.Bottom + 1;
            chkJianRongXingBox.Top = chkCreateStartMenuBox.Bottom + 1;

            progressDisplay.Parent = progressBar1;
            progressBar1.BackColor = themeColor;
            progressBar1.Text = string.Empty;

            firstLaunch = false;

            bottomButton1.BackgroundImage = ButtonTexture[0];
            bottomButton2.BackgroundImage = ButtonTexture[0];

            contentTree.LineColor = contentTree.ForeColor;
            progressDisplay.BackColor = Color.Transparent;
            _progressDisplay.BackColor = progressDisplay.BackColor;
            creditBtn.BackColor = Color.Transparent;

            startVideo.Visible = false;
            startVideo.Ctlcontrols.stop();
            if (System.IO.File.Exists(startVideoName))
                System.IO.File.Delete(startVideoName);

            MizukiTools.EmbedToOutside(Resources.drok, "drok.wav", startVideo);
            startVideo.settings.setMode("loop", true);
            startVideo.Ctlcontrols.play();

            installerVer.Visible = true;

            credits.BringToFront();
        }

        #endregion

        #region 功能函数

        // 安装尝试
        private bool TryToInstall()
        {
            try
            {
                bottomButton1.Enabled = false;
                bottomButton2.Enabled = false;

                installTo = pathBox.Text;
                MizukiTools.SetCursor(Resources.waiting, new Point(0, 0), this);

                // 解压文件
                // 原理：第一个参数为选择框选项，如果为false则不执行此操作。
                //      第二个参数为流文件名，也就是要解压的嵌入的资源。
                //      第三个是文件大小，用于计算进度条。
                //      第四个是安装位置。
                ExtractAFile(true, Resources.setup, setupSize, installTo, "setup.7z");
                foreach (var i in setuplbls)
                    ExtractAFile(i.Checked, i.File, i.FileSize, installTo, i.Filename);

                try
                {
                    _ = _DoWork(installTo);

                    // 创建快捷方式
                    if (chkCreateDesktopBox.Checked)
                    {
                        rightTopPanel.Text += CreatingShortcut + "...\n";
                        Application.DoEvents();
                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string shortcutName = $"{gameName}.lnk";
                        CreateShortcut(desktopPath, shortcutName, Path.Combine(installTo, gameLauncher));
                    }

                    // 创建开始菜单快捷方式
                    if (chkCreateStartMenuBox.Checked)
                    {
                        rightTopPanel.Text += CreStarShortcut + "...\n";
                        Application.DoEvents();
                        string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                        string shortcutName = $"{gameName}.lnk";
                        CreateShortcut(startMenuPath, shortcutName, Path.Combine(installTo, gameLauncher));
                    }
                }
                catch (Exception ex)
                {
                    MizukiTools.弹窗(ex.ToString());
                }

                progressBar1.Width = progressbarLength;
                _progressDisplay.Text = string.Format(Percents, 100);
                progressDisplay.Text = _progressDisplay.Text;

                // 强制更新
                Application.DoEvents();
                return true;
            }
            catch (Exception ex)
            {
                MizukiTools.弹窗(ex.ToString());
                return false;
            }
        }

        // 解压事件处理器

        private void Archive_Extracting(object sender, ProgressEventArgs e)
        {
            int i = (int)(currentSize * e.PercentDone / spaceNeeded);
            progressBar1.Width = progressbarLength * (progressBase + i) / 100;
            _progressDisplay.Text = string.Format(Percents, MizukiTools.GetValueInADuring(progressBase + i, 0, 99));
            progressDisplay.Text = _progressDisplay.Text;
            // 强制更新
            Application.DoEvents();
        }

        // 展开树状图。

        private void TenkaiTree()
        {
            if (!tenkaiedTreeView)
            {
                tenkaiedTreeView = true;

                Thread.Sleep(25);
                pathInfo.Top = 32;

                SetPathInfo();

                selectTips.Top = pathInfo.Top + 130;
                contentPanel.Top = selectTips.Bottom + 9;
                contentPanel.Height = 431 - contentPanel.Top;
                contentTree.Height = contentPanel.Height - 4;

                contentTree.Width = contentPanel.Width - 4;
                contentTree.Left = contentTree.Top = 2;

                pathBoxPanel.Top = contentPanel.Bottom + 9;

                contentPanel.Visible = true;
                selectTips.Visible = true;

                SetSettingChkLocation(new MizukiLabel[3] { chkCreateDesktopBox, chkCreateStartMenuBox, chkJianRongXingBox }, new Point(chkCreateDesktopBox.Left, pathBoxPanel.Bottom + 9), 25);

                Console.WriteLine(contentPanel.Bottom);
            }
        }

        #endregion

        #region 程序函数2

        // 退出程序之前关掉其他东西。

        public void Exits()
        {
            // 停掉音频服务，防止鬼按钮
            try
            {
                startVideo.close();
                menuUpdate.close();
                Visible = false;
            }
            catch
            {
            }
            finally
            {
                string[] tempFiles = { backgroundVideoName, startVideoName, menuUpdateAudio };

                try
                {
                    foreach (string i in tempFiles)
                    {
                        if (!string.IsNullOrEmpty(i) && System.IO.File.Exists(i))
                            System.IO.File.Delete(i);
                    }
                }
                catch (Exception ex)
                {
                    MizukiTools.弹窗(TempfileCleaningError + Environment.NewLine + Environment.NewLine + ex.ToString());
                }
                finally
                {
                    Application.Exit();
                }
            }
        }

        private void ChangeButtonText()
        {
            chkAgreeLicenses.Visible = false;
            pathBoxPanel.Visible = false;
            contentPanel.Visible = false;
            chkCreateDesktopBox.Visible = false;
            pathInfo.Visible = false;
            InstallSetting.Visible = false;
            chkInstallSet.Visible = false;
            chkInstallSetA.Visible = false;
            selectTips.Visible = false;
            chkJianRongXingBox.Visible = false;
            chkCreateStartMenuBox.Visible = false;

            bool azxx = 当前进度 == _安装选项;

            creditBtn.Text = Button_Credit;

            if (当前进度 != _完成安装)
                rightTopPanel.Text = RightTopPanelText;

            BackgroundImage = 当前进度 > _开始界面 ? bg2 : bg1;

            bottomBar.Text = string.Empty;
            bottomButton1.Text = BottomBtn1[当前进度];
            bottomButton2.Text = BottomBtn2[当前进度];

            licensePanel.Visible = 当前进度 == _许可协议;

            // 用Label代替按钮，避免尴尬。
            chkAgreeLicenses.Text = AgreeTitle;
            chkAgreeLicenses.Visible = 当前进度 == _许可协议;

            pathBoxPanel.Visible = 当前进度 == _路径选择;
            spaceNeeded = setupSize;
            foreach (var i in setuplbls)
                spaceNeeded += i.FileSize * MizukiTools.BoolToInt(i.Checked);

            installerVer.Text = string.Format(ApplicationVer, assembly.GetName().Version);

            chkCreateDesktopBox.Visible = chkCreateStartMenuBox.Visible = chkJianRongXingBox.Visible = 当前进度 == _路径选择;

            chkCreateDesktopBox.Text = CreateDesktopShortcut;
            chkCreateStartMenuBox.Text = CreateStartMenuShortcut;
            chkJianRongXingBox.Text = SetCompatibility;

            selectTips.Font = azxx ? new Font(arialFontFamily, generalFontSize - 3F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))) : generalFont;
            if (azxx)
            {
                selectTips.Text = TradeMark;
                selectTips.Top = Height - (selectTips.Height + 100);
                selectTips.Visible = true;

                pathInfo.Text = WelcomeInfo;
                pathInfo.Top = selectTips.Top - (pathInfo.Height + 16);
            }
            else if (当前进度 == _完成安装)
            {
                pathInfo.Text = MoreInfo;
                pathInfo.Top = Height - (pathInfo.Height + 100);
            }
            else
            {
                pathInfo.Top = tenkaiedTreeView ? 23 : 120;
                SetPathInfo();

                selectTips.Top = pathInfo.Top + 130;
                selectTips.Text = AbleFolder;
                selectTips.Visible = contentPanel.Visible = tenkaiedTreeView && 当前进度 == _路径选择;
            }
            pathInfo.Visible = 当前进度 == _路径选择 || 当前进度 == _完成安装 || azxx;

            InstallSetting.Text = azxx ? SelectParts : ExtraSetting;
            InstallSetting.Visible = azxx || 当前进度 == _完成安装;

            chkInstallSet.Visible = azxx;

            foreach (var i in setuplbls)
            {
                i.Visible = azxx;
            }

            bottomButton1.Enabled = 当前进度 != _安装中;
            bottomButton2.Enabled = (当前进度 == _许可协议 && chkAgreeLicenses.Checked) || 当前进度 != _许可协议;
        }

        #endregion

        #region 程序函数3

        private void SetPathInfo()
        {
            if (MizukiTools.IsGoodPath(pathBox.Text))
            {
                try
                {
                    drive = new DriveInfo(Path.GetPathRoot(pathBox.Text));
                    freeSpace = drive.IsReady ? drive.AvailableFreeSpace : 0;
                }
                catch
                {
                    freeSpace = 0;
                }
            }
            else
            {
                Console.WriteLine("Has invaild character.");
                freeSpace = 0;
            }

            pathInfo_Text = pathInfo.Text = string.Format(PathPage, spaceNeeded, spaceNeeded / 1024, (float)spaceNeeded / 1024 / 1024, (float)spaceNeeded / 1024 / 1024 / 1024, freeSpace, freeSpace / 1024, (float)freeSpace / 1024 / 1024, (float)freeSpace / 1024 / 1024 / 1024, tenkaiedTreeView ? string.Empty : LittleTip);
        }

        #endregion

        // 程序的附加控件
        #region Extras

        // 各种自定义标签实例
        private static SetupLabel chkInstallSetA = new SetupLabel();
        private static MizukiLabel chkCreateDesktopBox = new MizukiLabel();
        private static MizukiLabel chkCreateStartMenuBox = new MizukiLabel();
        private static MizukiLabel chkJianRongXingBox = new MizukiLabel();
        private static MizukiLabel chkInstallSet = new MizukiLabel();
        private static MizukiLabel chkAgreeLicenses = new MizukiLabel();

        // 初始化所有自定义标签数组
        private static MizukiLabel[] mizukiLabels = new MizukiLabel[6]
        {
            chkAgreeLicenses, chkInstallSet, chkJianRongXingBox, chkCreateStartMenuBox, chkCreateDesktopBox, chkInstallSetA
        };
        // 初始化安装选项组
        private static SetupLabel[] setuplbls = new SetupLabel[1] { chkInstallSetA };

        // 用于初始化各种自定义控件及其属性的方法

        private void CustomControls()
        {
            // 设置各标签的初始选中状态
            chkAgreeLicenses.Checked = false;
            chkCreateDesktopBox.Checked = true;
            chkCreateStartMenuBox.Checked = true;
            chkJianRongXingBox.Checked = false;
            chkInstallSet.Checked = true;
            chkInstallSetA.Checked = false;

            // 设置各标签的通用属性
            foreach (var chk in mizukiLabels)
            {
                chk.ForeColor = hutsuuTextColor;
                chk.CheckSound = checkbox;
            }

            // 设置各标签的具体位置及事件处理（部分）
            chkAgreeLicenses.Location = new Point(67, 458);
            chkAgreeLicenses.Name = "agreeLicenses";
            chkAgreeLicenses.TabIndex = 14;
            chkAgreeLicenses.MouseClick += 同意协议_MouseClick;

            chkCreateDesktopBox.Name = "createDesktopBox";
            chkCreateDesktopBox.TabIndex = 22;

            chkCreateStartMenuBox.Name = "createStartMenuBox";
            chkCreateStartMenuBox.TabIndex = 23;

            chkJianRongXingBox.Name = "jianRongXingBox";
            chkJianRongXingBox.TabIndex = 24;

            chkInstallSet.Name = "chkInstallSet";
            chkInstallSet.TabIndex = 26;
            chkInstallSet.CheckDisabled = true;

            chkInstallSetA.Name = "chkInstallSetA";
            chkInstallSetA.TabIndex = 37;
            chkInstallSetA.Filename = "setup1.7z";
            chkInstallSetA.File = Resources.setup1;

            // 将所有自定义标签添加到控件集合中
            foreach (var chk in mizukiLabels)
            {
                Controls.Add(chk);
            }

            // 设置部分标签的位置
            SetSettingChkLocation(new MizukiLabel[3] { chkCreateDesktopBox, chkCreateStartMenuBox, chkJianRongXingBox }, new Point(67, 370), 25);
            SetSettingChkLocation(new MizukiLabel[2] { chkInstallSet, chkInstallSetA }, new Point(67, 80), 25);
        }

        #endregion

        // 这里定义了所有的事件。
        #region Events

        // 路径框得到焦点后变亮。

        private void PathBox_Enter(object sender, EventArgs e)
        {
            pathBox.ForeColor = lightTextColor;
        }

        private void PathBox_Leave(object sender, EventArgs e)
        {
            pathBox.ForeColor = hutsuuTextColor;
        }

        private void _StartWindow_Load(object sender, EventArgs e)
        {
            MizukiTools.SetCursor(Resources.release, new Point(0, 0), this);

            //循环遍历计算机所有逻辑驱动器名称(盘符)
            foreach (string drive in Environment.GetLogicalDrives())
            {
                //实例化DriveInfo对象 命名空间System.IO
                var dir = new DriveInfo(drive);
                if (dir.DriveType == DriveType.Fixed || (dir.DriveType == DriveType.Removable && displayRemovable))           //判断驱动器类型
                {
                    //Split仅获取盘符字母
                    TreeNode tNode = new TreeNode(dir.Name.Split(':')[0] + ":")
                    {
                        Name = dir.Name,
                        Tag = dir.Name
                    };
                    contentTree.Nodes.Add(tNode);                    //加载驱动节点
                    tNode.Nodes.Add("");
                }
            }

            // 必选项
            setupSize = GetUncompresswdSize(Resources.setup);
        }

        private void StartWindow_MouseEnter(object sender, EventArgs e)
        {
            if (touchedButton)
            {
                bottomBar.Text = BottomBarText[当前进度, 0];
            }
        }

        private void Painting(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            ControlPaint.DrawBorder(e.Graphics, panel.ClientRectangle, ctrlBorderColor, ButtonBorderStyle.Solid);//底边
            /*, //左边
            Color.DarkRed, 1, ButtonBorderStyle.Solid, //上边
　　　　　Color.DarkRed, 1, ButtonBorderStyle.Solid, //右边
　　　　　Color.DarkRed, 1, ButtonBorderStyle.Solid
            */
        }

        private void PathBox_TextChanged(object sender, EventArgs e)
        {
            SetPathInfo();
            pathInfo.ForeColor = hutsuuTextColor;
        }

        private void PathBox_MouseClick(object sender, MouseEventArgs e)
        {
            pathBox.ReadOnly = false;
            TenkaiTree();
        }

        private void _StartWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = 当前进度 == _安装中;
        }

        private void _StartWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Exits();
        }

        #endregion

        #region 目录事件

        private void ContentTree_Enter(object sender, EventArgs e)
        {
            selectTips.ForeColor = lightTextColor;
        }

        private void ContentTree_Leave(object sender, EventArgs e)
        {
            selectTips.ForeColor = hutsuuTextColor;
        }

        #endregion

        // 程序使用的函数
        #region Functions

        // 移动按钮。
        private void MoveButton(Button bottomButton, string targetText)
        {
            for (int i = 2; i <= 16; i++)
            {
                bottomButton.Left += i * 2;
                Thread.Sleep(3);
                Application.DoEvents();
            }
            bottomButton.BackgroundImage = ButtonTexture[4];
            for (int i = 16; i >= 2; i--)
            {
                bottomButton.Left -= i * 2;
                Thread.Sleep(5);
                Application.DoEvents();
            }
            bottomButton.Text = targetText;
            bottomButton.BackgroundImage = targetText == string.Empty ? ButtonTexture[2] : ButtonTexture[0];
        }

        private void MoveButton(Button bottomButton)
        {
            bottomButton.Text = string.Empty;
            bottomButton.BackgroundImage = ButtonTexture[4];
            for (int i = 2; i <= 16; i++)
            {
                bottomButton.Left += i * 2;
                Thread.Sleep(3);
                Application.DoEvents();
            }
            bottomButton.BackgroundImage = ButtonTexture[2];
            for (int i = 16; i >= 2; i--)
            {
                bottomButton.Left -= i * 2;
                Thread.Sleep(5);
                Application.DoEvents();
            }
        }

        private void HideButtons(Button[] bottomButton)
        {
            foreach (var bbtn in bottomButton)
            {
                bbtn.Text = string.Empty;
                bbtn.BackgroundImage = ButtonTexture[4];
            }
            for (int i = 2; i <= 16; i++)
            {
                foreach (var bbtn in bottomButton)
                {
                    bbtn.Left += i * 2;
                }
                Thread.Sleep(3);
                Application.DoEvents();
            }
            foreach (var bbtn in bottomButton)
            {
                bbtn.BackgroundImage = ButtonTexture[2];
            }
            for (int i = 16; i >= 2; i--)
            {
                foreach (var bbtn in bottomButton)
                {
                    bbtn.Left -= i * 2;
                }
                Thread.Sleep(5);
                Application.DoEvents();
            }
        }

        #endregion

        #region Functions2

        // 设置安装选项页面的选择框位置。
        private void SetSettingChkLocation(MizukiLabel[] mls, Point firstPoint, int distance, bool visible = false)
        {
            for (int i = 0; i < mls.Length; i++)
            {
                if (i == 0)
                {
                    mls[0].Location = firstPoint;
                }
                else
                {
                    mls[i].Location = new Point(mls[0].Left, mls[i - 1].Top + distance);
                }

                if (visible)
                    mls[i].Visible = visible;
            }
        }

        // 将游戏文件夹写入注册表。
        private bool _DoWork(string GameDir)
        {
            rightTopPanel.Text += Registering + "...\n";
            Application.DoEvents();

            // Generate serial number
            var rng = new Random();
            char[] randomedChars = "0123456789".ToCharArray();
            int snLength = 22;
            var sb = new StringBuilder();
            for (int i = 0; i < snLength; i++)
            {
                _ = sb.Append(randomedChars[rng.Next(randomedChars.Length)]);
            }
            string sn = sb.ToString();

            var HKLM_32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            var ra2Key = HKLM_32.CreateSubKey(@"SOFTWARE\Westwood\Red Alert 2");
            ra2Key.SetValue("Serial", sn);
            ra2Key.SetValue("Name", "Red Alert 2");
            ra2Key.SetValue("InstallPath", Path.Combine(GameDir, "RA2.EXE"));
            ra2Key.SetValue("SKU", 8448);
            ra2Key.SetValue("Version", 65542);

            var yrKey = HKLM_32.CreateSubKey(@"SOFTWARE\Westwood\Yuri's Revenge");
            yrKey.SetValue("Serial", sn);
            yrKey.SetValue("Name", "Yuri's Revenge");
            yrKey.SetValue("InstallPath", Path.Combine(GameDir, "RA2MD.EXE"));
            yrKey.SetValue("SKU", 10496);
            yrKey.SetValue("Version", 65537);

            //var unsKey = HKLM_32.CreateSubKey($@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{LongGameName}");
            //unsKey.SetValue("DisplayName", LongGameName);
            //unsKey.SetValue("UninstallString", Path.Combine(GameDir, unsExe));
            //unsKey.SetValue("Publisher", ((AssemblyCompanyAttribute)assembly.GetCustomAttribute(typeof(AssemblyCompanyAttribute))).Company);

            if (chkJianRongXingBox.Checked)
            {
                var customKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers", true);
                customKey.SetValue(Path.Combine(installTo, gameExeName), jrxStr);
                // customKey.SetValue(Path.Combine(installTo, gameLauncher), jrxStr);
            }

            string blowfishPath = Path.Combine(GameDir, "Blowfish.dll");

            if (!System.IO.File.Exists(blowfishPath))
            {
                throw new Exception("找不到 Blowfish.dll 文件。");
            }
;
            ConsoleCommandManager.RunConsoleCommand("regsvr32.exe", $"/s \"{blowfishPath}\"", out int exitCode, out string stdOut, out string stdErr);

            if (!string.IsNullOrWhiteSpace(stdOut))
            {
                MizukiTools.弹窗(stdOut.Trim());
            }

            if (!string.IsNullOrWhiteSpace(stdErr))
            {
                MizukiTools.弹窗(stdErr.Trim());
            }

            if (exitCode != 0)
            {
                MizukiTools.弹窗($"进程返回值 {exitCode}。执行失败。");
            }

            return true;
        }

        private static void CreateShortcut(string folderPath, string shortcutName, string targetPath)
        {
            WshShell shell = new WshShell();
            string shortcutPath = Path.Combine(folderPath, shortcutName);
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetPath;
            shortcut.Save();
        }

        private void ExtractAFile(bool installs, byte[] file, long fileSize, string targetPath, string filename)
        {
            if (installs)
            {
                using (var resourceStream = new MemoryStream(file))
                {
                    if (resourceStream != null)
                    {
                        resourceStream.Seek(0, SeekOrigin.Begin);
                        progressBar1.Visible = progressDisplay.Visible = _progressDisplay.Visible = true;
                        currentSize = fileSize;
                        SevenZipExtractor extractor = new SevenZipExtractor(resourceStream);
                        extractor.Extracting += Archive_Extracting;
                        rightTopPanel.Text += string.Format(UnPacking, filename) + "...\n";
                        extractor.ExtractArchive(targetPath);
                    }
                }
                progressBase = (int)(currentSize * 100 / spaceNeeded);
            }
        }

        #endregion

        #region Functions3

        // 获取嵌入的资源大小。
        public static long GetEmbeddedResourceSize(Stream stream)
        {
            long originalPosition = stream.Position;
            stream.Seek(0, SeekOrigin.End);
            long size = stream.Position;
            stream.Seek(originalPosition, SeekOrigin.Begin);
            return size;
        }

        public static long GetUncompresswdSize(byte[] file)
        {
            using (Stream resourceStream = new MemoryStream(file))
            {
                if (resourceStream != null)
                {
                    SevenZipExtractor extractor = new SevenZipExtractor(resourceStream);
                    long totalSizeAfterExtraction = 0;
                    for (int i = 0; i < extractor.ArchiveFileData.Count; i++)
                    {
                        totalSizeAfterExtraction += (long)extractor.ArchiveFileData[i].Size;
                    }
                    Console.WriteLine($"解压后的总大小: {totalSizeAfterExtraction} 字节");
                    return totalSizeAfterExtraction;
                }
                else
                {
                    Console.WriteLine("Embedded resource not found.");
                }
                return 0;
            }
        }

        #endregion

        // 这里用于程序的配置，不是程序主逻辑。
        #region Configs

        // 全局使用的字体类型、大小、样式。
        public static Font generalFont { get; private set; }

        // 普通的文字颜色。
        public static readonly Color hutsuuTextColor = Color.Red;

        // 醒目的文字颜色，一般用于需要对比度大的地方。
        public static readonly Color lightTextColor = Color.Yellow;

        // 表示程序的主题颜色。
        public static readonly Color themeColor = Color.Red;

        // 边框的颜色，如编辑框、树形框等。
        public static readonly Color ctrlBorderColor = Color.FromArgb(0xAD, 0x08, 0x08);

        // 被点亮后的边框颜色，用于突出显示。
        public static readonly Color lightBorderColor = Color.Red;

        public static int SpaceNumber;

        // 用于缓存每一层级是否需要绘制连接线的状态
        private bool[] shouldDrawConnector;

        private Brush hutsuuTreeBrush;
        private Brush lightTreeBrush;

        // 本地化。
        public void L18n()
        {
            SpaceNumber = 4;

            generalFontFamily = "Batang";
            generalFontSize = 12F;
            arialFontFamily = "Microsoft JhengHei UI";
            ApplicationVer = "安裝器版本：{0}";

            BottomBarText[0, 0] = "歡迎回來，指揮官。";
            BottomBarText[0, 1] = "退出安裝程式。";
            BottomBarText[0, 2] = "開始安裝尤里的復仇。";
            BottomBarText[0, 3] = string.Empty;
            BottomBarText[0, 4] = string.Empty;

            BottomBarText[1, 0] = string.Empty;
            BottomBarText[1, 1] = BottomBarText[0, 1];
            BottomBarText[1, 2] = "同意協議并安裝。";
            BottomBarText[1, 3] = "返回到上一步。";
            BottomBarText[1, 4] = string.Empty;

            BottomBarText[2, 0] = string.Empty;
            BottomBarText[2, 1] = BottomBarText[0, 1];
            BottomBarText[2, 2] = "輸入許可證密鑰並繼續。";
            BottomBarText[2, 3] = BottomBarText[1, 3];
            BottomBarText[2, 4] = string.Empty;

            BottomBarText[3, 0] = string.Empty;
            BottomBarText[3, 1] = BottomBarText[0, 1];
            BottomBarText[3, 2] = "選擇完畢，進入下一步。";
            BottomBarText[3, 3] = BottomBarText[1, 3];
            BottomBarText[3, 4] = string.Empty;

            BottomBarText[4, 0] = string.Empty;
            BottomBarText[4, 1] = BottomBarText[0, 1];
            BottomBarText[4, 2] = "立即安裝尤里的復仇。";
            BottomBarText[4, 3] = BottomBarText[1, 3];
            BottomBarText[4, 4] = "瀏覽安裝文件夾。";

            BottomBarText[6, 0] = string.Empty;
            BottomBarText[6, 1] = "完成并打開游戲。";
            BottomBarText[6, 2] = "完成安裝程式。";
            BottomBarText[6, 3] = string.Empty;
            BottomBarText[6, 4] = string.Empty;

            BottomBtn1[0] = "退出安裝程式";
            BottomBtn1[1] = BottomBtn1[2] = BottomBtn1[3] = BottomBtn1[4] = BottomBtn1[5] = "退出";
            BottomBtn1[6] = "結束";

            BottomBtn2[0] = "開始安裝";
            BottomBtn2[1] = "繼續";
            BottomBtn2[2] = "验证";
            BottomBtn2[3] = "確認";
            BottomBtn2[4] = "安裝";
            BottomBtn2[5] = BottomBtn2[4];
            BottomBtn2[6] = string.Empty;

            BottomBtn3[0] = string.Empty;
            BottomBtn3[1] = BottomBtn3[2] = BottomBtn3[3] = BottomBtn3[4] = BottomBtn3[5] = "返回";
            BottomBtn3[6] = BottomBtn3[0];

            WindowTitle = "尤里的復仇安裝程式";
            gameName = "尤里復仇";
            LongGameName = "終極動員令：尤里的復仇";
            description = "終極動員令：尤里的復仇";

            AgreeTitle = "我已仔細閱讀并同意以上協議";

            CreateDesktopShortcut = "創建桌面快捷方式";
            CreateStartMenuShortcut = "創建開始菜單快捷方式";
            SetCompatibility = "設置遊戲兼容性";

            InstallSetting1 = "安裝尤里的復仇";
            InstallSettingA = "安裝過場動畫包";
            Selectable = "（可選）";

            Button_Skip = "跳過";
            Button_Browse = "瀏覽";
            Button_OK = "確定";
            Button_Cancel = "取消";
            Button_Credit = "製作人員";
            Button_Close = "關閉";

            FileNotFound = "找不到文件：{0}\n啓動目標程式失敗。";

            PathPage = $"設定尤里的復仇安裝路徑。\n{{8}}\n占用空間：{{0}}字節（{{1}} KB，{{2:0.00}} MB，{{3:0.00}} GB）\n剩餘空間：{{4}}字節（{{5}} KB，{{6:0.00}} MB，{{7:0.00}} GB）";
            LittleTip = $"\n本安裝程式目前沒有卸載功能，運行{_RegSetMD}點擊Uninstall再删除文件夾即可卸載。";
            FileExplorer = "請選擇要安裝到的目錄：";
            Percents = "{0}% 已完成";
            AbleFolder = "選擇要安裝到的文件夾：";
            InvaildPath = "{0}\n\n選擇的安裝路徑不合法，請重新選擇。";
            DisknotEnough = "磁盤空間不足，請更換目錄或清理磁盤後再試。";
            CreateDirFailed = "目錄創建失敗：";
            DisknotEmpty = "目錄不爲空，請選擇空目錄。";

            License = "暫無許可證";

            MoreInfo = $"請查看自述文件瞭解最新更改及注意事項。\n\n訪問 {gameHomepage} 瞭解最新策略、錦標賽和賽事。";

            QuitConfirm = "確定要退出本安裝程式？您的軟件將不會安裝。";
            VideoOpenFail = "展開視頻文件失敗，請檢查臨時目錄權限或剩餘空間。";
            NoSetupBin = "安裝文件若非遺失，即為無效，請重啟或重新下載再試。";
            UnpackError = "解壓文件失敗，請檢查目錄權限，以及文件是否已損壞。";

            RightTopPanelText = "感謝使用本尤里的復仇安裝程式。\n您正在安裝的是：尤里的復仇1.001。\n\nEA、Westwood 版權所有";

            ExtraSetting = $"{LongGameName}安裝完成。"; // \n\n敬请期待。";
            TempfileCleaningError = "刪除臨時文件目錄失敗。";
            FileSetError = "文件信息不正確。";
            FromsCopyToFailed = "從{0}複製文件到{1}失敗！";
            SelectParts = "選擇要安裝的部分：";

            WelcomeInfo = $"歡迎使用 {LongGameName} 安裝程序。\n\n安裝前最好先暫時關閉其他Windows程序。\n\n您可以退出安裝程序以關閉其他程序，也可以按“下一步”繼續安裝。\n\n警告：本程序受版權法和國際條約保護。未經授權複製或分發本程序或其任何部分，可能會導致嚴重的民事和刑事責任，並將在法律允許的最大范圍內起訴。";
            TradeMark = "(c)2001 Electronic Arts. Westwood Studios is a trademark or registered trademark of Electronic Arts in the U.S. and/or other countries. All rights reserved. Westwood Studios is an Electronic Arts(tm) company. Command & Conquer(tm) and Yuri's Revenge(tm) are trademarks or registered trademarks of Electronic Arts Inc. in the U.S. and/or other countries.";

            LICENSE = Resources.LICENSE;

            Installing = "安裝中";
            CreStarShortcut = "正在創建開始菜單捷徑";
            UnPacking = "正在提取{0}";
            ExceptionDiff = "單選框{0}的選項與其他不一致。";
            CreatingShortcut = "正在創建桌面捷徑";
            Registering = "正在寫入註冊表";
            ExceptionRound = "ChildItem、EnemyItem有共同元素。";

            generalFont = new Font(generalFontFamily, generalFontSize, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));

            chkInstallSet.Text = InstallSetting1;
            chkInstallSetA.Text = InstallSettingA;

            foreach (var i in setuplbls)
            {
                i.Text += Selectable;
            }

            SetFont();
            ChangeButtonText();

            licensePanel.Size = new Size(495, 400);
            try
            {
                license.Text = LICENSE;
            }
            catch
            {
                license.Text = License;
            }
            license.Size = new Size(licensePanel.Width - 4, licensePanel.Height - 4);

        }

        #endregion

        #region Config2

        // 设置控件的字体。

        private void SetFont()
        {
            installerVer.Font = new Font(arialFontFamily, generalFontSize - 3F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            rightTopPanel.Font = new Font(arialFontFamily, generalFontSize - 3F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            contentTree.Font = new Font("MS Sans Serif", generalFontSize - 1F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            progressDisplay.Font = new Font(arialFontFamily, generalFontSize - 2F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            _progressDisplay.Font = progressDisplay.Font;
            pathBox.Font = new Font("MS Sans Serif", generalFontSize, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            license.Font = new Font(generalFontFamily, generalFontSize - 1F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            creditBtn.Font = new Font(arialFontFamily, generalFontSize - 3F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));

            InstallSetting.Font = generalFont;
            bottomBar.Font = generalFont;
            bottomButton1.Font = generalFont;
            bottomButton2.Font = generalFont;
            credits.Font = generalFont;
            pathInfo.Font = generalFont;
            selectTips.Font = generalFont;

            foreach (var i in mizukiLabels)
            {
                i.Font = generalFont;
            }
        }

        // 设置控件的文本颜色。

        public void SetForeColorAndFont()
        {
            // 高亮的文字是这些。
            bottomButton1.ForeColor = lightTextColor;
            bottomButton2.ForeColor = lightTextColor;
            bottomBar.ForeColor = lightTextColor;
            rightTopPanel.ForeColor = lightTextColor;
            installerVer.ForeColor = lightTextColor;
            progressDisplay.ForeColor = lightTextColor;
            credits.ForeColor = lightTextColor;
            creditBtn.ForeColor = lightTextColor;
            _progressDisplay.ForeColor = progressDisplay.ForeColor;

            // 普通的文字是这些。
            license.ForeColor = hutsuuTextColor;
            pathBox.ForeColor = hutsuuTextColor;
            pathInfo.ForeColor = hutsuuTextColor;
            contentTree.ForeColor = hutsuuTextColor;
            selectTips.ForeColor = hutsuuTextColor;
            InstallSetting.ForeColor = hutsuuTextColor;

            bottomButton1.FlatAppearance.BorderSize = 0;
            bottomButton2.FlatAppearance.BorderSize = 0;

            hutsuuTreeBrush = new SolidBrush(hutsuuTextColor);
            lightTreeBrush = new SolidBrush(lightTextColor);
        }
        #endregion

        // 定义承载客户端内文本的变量
        #region 客户端内文本
        public static string generalFontFamily { get; private set; }
        public static float generalFontSize { get; private set; }
        public static string arialFontFamily { get; private set; }
        public static string ApplicationVer { get; private set; }
        public static string[] BottomBtn1 { get; private set; }
        public static string[] BottomBtn2 { get; private set; }
        public static string[] BottomBtn3 { get; private set; }
        public static string[,] BottomBarText { get; private set; }

        // 窗口标题
        public string WindowTitle { get; private set; }
        // 游戏名称
        public string gameName { get; private set; }
        // 长游戏名称
        public string LongGameName { get; private set; }
        // 描述
        public string description { get; private set; }

        // 同意协议
        public string AgreeTitle { get; private set; }

        // 创建桌面快捷方式
        public string CreateDesktopShortcut { get; private set; }
        // 创建开始菜单快捷方式
        public string CreateStartMenuShortcut { get; private set; }
        // 设置兼容性
        public string SetCompatibility { get; private set; }

        // 必选项尤复
        public string InstallSetting1 { get; private set; }
        // 选择项
        public string InstallSettingA { get; private set; }
        // 可选
        public string Selectable { get; private set; }

        // 表示跳过当前操作或步骤的按钮文本，例如在视频播放等场景下可让用户跳过该环节。
        public string Button_Skip { get; private set; }
        // 浏览文件夹
        public string Button_Browse { get; private set; }
        // 确定
        public static string Button_OK { get; private set; }
        // 取消
        public static string Button_Cancel { get; private set; }
        // 显示制作人员
        public string Button_Credit { get; private set; }
        // 关闭视频
        public string Button_Close { get; private set; }

        // 找不到指定文件时显示的错误文本，包含具体找不到的文件名占位符{0}，并提示启动目标程序失败。
        public string FileNotFound { get; private set; }

        // 用于设置《尤里的复仇》安装路径时展示的文本信息，包含占用空间和剩余空间等详细情况的展示格式。
        public string PathPage { get; private set; }
        // 提供的一个小提示信息，说明当前安装程序暂无卸载功能及一种可行的卸载方式。
        public string LittleTip { get; private set; }
        // 引导用户选择要安装到的目录的文本提示，用于安装程序中选择安装目录的环节。
        public string FileExplorer { get; private set; }
        // 用于展示完成进度百分比的文本格式，包含占位符{0}用于填入具体的完成百分比数值。
        public string Percents { get; private set; }
        // 引导用户选择要安装到的文件夹的文本提示，类似于选择安装目录但更强调文件夹层面。
        public string AbleFolder { get; private set; }
        // 选择的安装路径不合法时显示的错误文本，包含具体的错误信息占位符{0}，提示用户重新选择。
        public string InvaildPath { get; private set; }
        // 磁盘空间不足时显示的错误文本，告知用户需更换目录或清理磁盘后再试。
        public string DisknotEnough { get; private set; }
        // 目录创建失败时显示的错误文本，仅提示目录创建失败的情况。
        public string CreateDirFailed { get; private set; }
        // 所选目录不为空时显示的错误文本，提示用户选择空目录。
        public string DisknotEmpty { get; private set; }

        // 暂无许可证的占位符
        public string License { get; private set; }

        // 提示用户查看自述文件以了解最新更改及注意事项，并告知可访问游戏官网了解相关信息的文本。
        public string MoreInfo { get; private set; }

        // 用于确认是否要退出安装程序的文本提示，在用户执行退出操作时展示供用户确认。
        public string QuitConfirm { get; private set; }
        // 展开视频文件失败时显示的错误文本，提示用户检查临时目录权限或剩余空间。
        public string VideoOpenFail { get; private set; }
        // 安装文件若不是遗失就是无效时显示的错误文本，提示用户重启或重新下载再试。
        public string NoSetupBin { get; private set; }
        // 解压缩文件失败时显示的错误文本，提示用户检查目录权限以及文件是否已损坏。
        public string UnpackError { get; private set; }

        // 显示在右上角面板的文本信息，包含感谢使用安装程序、正在安装的游戏版本及版权所有等相关内容。
        public string RightTopPanelText { get; private set; }

        // 表示《尤里的复仇》（由LongGameName指定具体名称）安装完成的文本提示。
        public string ExtraSetting { get; private set; }
        // 删除临时文件目录失败时显示的错误文本，用于相关清理临时文件操作失败的提示。
        public string TempfileCleaningError { get; private set; }
        // 文件信息不正确时显示的错误文本，用于文件相关验证环节提示错误情况。
        public string FileSetError { get; private set; }
        // 从某个位置复制文件到另一个位置失败时显示的错误文本，包含源位置和目标位置的占位符{0}、{1}。
        public string FromsCopyToFailed { get; private set; }
        // 引导用户选择要安装的部分的文本提示，用于安装程序中可选择安装部分的环节。
        public string SelectParts { get; private set; }

        // 欢迎用户使用《尤里的复仇》安装程序的文本提示，还包含安装前建议及版权相关的警告信息。
        public string WelcomeInfo { get; private set; }
        // 包含版权相关的商标及注册信息等内容的文本，用于展示相关游戏及公司的商标等情况。
        public string TradeMark { get; private set; }

        // 许可证相关资源
        public string LICENSE { get; private set; }

        // 安装中
        public string Installing { get; private set; }
        // 正在创建开始菜单
        public string CreStarShortcut { get; private set; }
        // 正在解压文件
        public string UnPacking { get; private set; }
        // 单选项项目不同
        public static string ExceptionDiff { get; private set; }
        // 创建快捷方式
        public string CreatingShortcut { get; private set; }
        // 写入注册表中...
        public string Registering { get; private set; }
        // 内部循环
        public static string ExceptionRound { get; private set; }

        #endregion

        // 选择路径框事件
        #region 路径选择框有关函数

        // 在结点展开后发生 展开子结点

        private void DirectoryTree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            checkbox.Play();
            e.Node.Expand();
        }

        // 在将要展开结点时发生 加载子结点

        private void DirectoryTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeViewItems.Add(e.Node);
        }

        // 将路径加载到路径框里。

        private void DirectoryTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                pathBox.Text = Path.Combine(e.Node.Name, @"RA2MD\");
            }
        }

        #endregion

        #region 选择框事件

        private void 同意协议_MouseClick(object sender, MouseEventArgs e)
        {
            bottomButton2.Enabled = chkAgreeLicenses.Checked;
        }

        #endregion

        // 底部按钮点击的事件
        #region 按钮事件

        private void 底部按钮1_MouseClick(object sender, EventArgs e)
        {
            button.Play();
            bottomButton1.BackgroundImage = ButtonTexture[0];
            Application.DoEvents();
            Exits();
        }

        private void 底部按钮2_MouseClick(object sender, EventArgs e)
        {
            bottomButton1.Enabled = false;
            bottomButton2.Enabled = false;

            // 延时防止误触
            Thread.Sleep(100);

            button.Play();
            bottomButton2.BackgroundImage = ButtonTexture[0];
            Application.DoEvents();
            if (当前进度 == _路径选择)
            {
                if (!MizukiTools.IsGoodPath(pathBox.Text))
                {
                    pathInfo.Text = string.Format(InvaildPath, pathInfo_Text);
                    pathInfo.ForeColor = lightTextColor;
                    pengci.Play();
                }
                else if (spaceNeeded > freeSpace)
                {
                    MizukiTools.弹窗(DisknotEnough);
                }
                else if (spaceNeeded <= 0)
                {
                    MizukiTools.弹窗(NoSetupBin);
                }
                else if (Directory.Exists(pathBox.Text) && (Directory.GetDirectories(pathBox.Text).Length > 0 || Directory.GetFiles(pathBox.Text).Length > 0))
                {
                    MizukiTools.弹窗(DisknotEmpty);
                }
                else
                {
                    当前进度 += 1 + MizukiTools.BoolToInt(当前进度 + 1 == _CD验证);
                    ChangeButtonText();
                    rightTopPanel.ForeColor = hutsuuTextColor;
                    rightTopPanel.Text = $"{Installing}...\n";
                    Application.DoEvents();
                    bool end = TryToInstall();
                    MizukiTools.SetCursor(Resources.release, new Point(0, 0), this);
                    progressBar1.Visible = end;
                    _progressDisplay.Visible = end;
                    progressDisplay.Visible = _progressDisplay.Visible;
                    if (end)
                    {
                        当前进度 += 1;
                    }
                    else
                    {
                        Directory.Delete(installTo, true);
                        当前进度 -= 1;
                        rightTopPanel.ForeColor = lightTextColor;
                    }
                }
                Application.DoEvents();
                ChangeButtonText();
            }
            else
            {
                当前进度 += 1 + MizukiTools.BoolToInt(当前进度 + 1 == _CD验证);
                ChangeButtonText();
            }
        }

        private void 底部按钮3_MouseClick(object sender, EventArgs e)
        {
            // 延时防止误触
            Thread.Sleep(100);

            bottomButton1.Enabled = false;
            bottomButton2.Enabled = false;
            button.Play();
            // bottomButton3.BackgroundImage = ButtonTexture[0];
            Application.DoEvents();
            当前进度 -= 1 + MizukiTools.BoolToInt(当前进度 - 1 == _CD验证);
            ChangeButtonText();
        }

        private void 底部按钮4_MouseClick(object sender, EventArgs e)
        {
            button.Play();
            Application.DoEvents();
            TenkaiTree();
        }

        private void Rbtn_MouseEnter(object sender, EventArgs e)
        {
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;
            if (btn.Enabled)
            {
                if (btn == bottomButton1)
                    bottomBar.Text = BottomBarText[当前进度, 1];
                else if (btn == bottomButton2)
                    bottomBar.Text = BottomBarText[当前进度, 2];
                touchedButton = true;
            }
        }

        // 跳过按钮单击事件。

        private void Credits_MouseClick(object sender, MouseEventArgs e)
        {
            credits.Visible = false;
        }

        private void CreditBtn_MouseClick(object sender, MouseEventArgs e)
        {
            credits.Visible = true;
        }

        #endregion

        // 按钮亮起熄灭的特效
        #region 按钮点击特效

        private void Rbtn_MouseDown(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;
            if (btn.Enabled)
                btn.BackgroundImage = ButtonTexture[1];
        }

        private void Rbtn_MouseLeave(object sender, EventArgs e)
        {
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;
            if (btn.Text != string.Empty)
                btn.BackgroundImage = ButtonTexture[0];
            bottomBar.Text = string.Empty;
        }

        private void Rbtn_MouseUp(object sender, MouseEventArgs e)
        {
            // System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;
            if (btn.Text != string.Empty)
                btn.BackgroundImage = ButtonTexture[0];
        }

        #endregion

        #region Sounds

        // 选择框点击声。
        public static SoundPlayer checkbox = new SoundPlayer(Resources.checkbox);

        // 按钮按下声。
        private SoundPlayer button = new SoundPlayer(Resources.ButtonClick);

        // 验证失败声。
        private SoundPlayer pengci = new SoundPlayer(Resources.pengci);

        // 按钮的图片。
        public Bitmap[] ButtonTexture = new Bitmap[5];

        #endregion

        private void Rbtn_Enter(object sender, EventArgs e)
        {
            MizukiTools.TenIFocus(bottomBar);
        }
    }

    // 附加类
    #region Extra Controls

    // 继承自Label的自定义标签类，用于作为选择框
    public class MizukiLabel : Label, IComparable<MizukiLabel>
    {
        private bool _checked;
        private string _text;
        private int _spaceNumber = 4;
        protected bool _checket;
        private MizukiLabel[] _singlesArray;
        private MizukiLabel[] _childItem;

        public MizukiLabel[] Singles
        {
            get
            {
                return _singlesArray;
            }
            set
            {
                _checket = false;
                _singlesArray = value;
            }
        }
        public bool CheckDisabled = false;
        public SoundPlayer CheckSound;
        public MizukiLabel[] ChildItem
        {
            get
            {
                return _childItem;
            }
            set
            {
                if (value != null)
                {
                    foreach (var i in value)
                    {
                        if (Array.IndexOf(i.ChildItem, this) > -1)
                        {
                            throw new LRXDontLoveMeException(_StartWindow.ExceptionRound);
                        }
                    }
                }
                _childItem = value;
            }
        }

        public int SpaceNumber
        {
            get
            {
                return _spaceNumber;
            }
            set
            {
                _spaceNumber = value;
                _text = new string(' ', value) + base.Text;
                // base.Text = value;
            }
        }

        public Image[] CheckImages = new Image[2] { Resources.Unselected, Resources.Selected };

        public MizukiLabel()
        {
            AutoSize = true;
            BackColor = Color.Transparent;
            ImageAlign = TextAlign = ContentAlignment.MiddleLeft;
            Text = "label1";
            Visible = false;
            Checked = Checked;
            MouseClick += CheckIndex;
        }

        // 获取或设置标签的选中状态

        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                Image = value ? CheckImages[1] : CheckImages[0];
                if (AutoSize && Image != null)
                {
                    int i = 65535;
                    if (Parent != null)
                        i = Parent.Width;
                    MaximumSize = new Size(i, Image.Height);
                    MinimumSize = new Size(0, Image.Height);
                }
                else
                {
                    MaximumSize = MinimumSize = Size.Empty;
                }
                if (ChildItem != null)
                {
                    foreach (var item in ChildItem)
                    {
                        item.Enabled = Checked;
                        item.Checked = false;
                    }
                }
            }
        }

        // 获取或设置标签的文本内容，自动添加空格
        public override string Text
        {
            get => _text;
            set
            {
                _text = new string(' ', _spaceNumber) + value.TrimStart();
                base.Text = value;
            }
        }

        public int CompareTo(MizukiLabel other)
        {
            return this.Name.CompareTo(other.Name);
        }

        private void CheckIndex(object sender, EventArgs e)
        {
            if (!CheckDisabled & Visible)
            {
                CheckSound?.Play();
                MizukiLabel control = (MizukiLabel)sender;
                if (Singles != null && Singles.Length > 1)
                {
                    foreach (var i in control.Singles)
                    {
                        if (!i._checket || _checket)
                        {
                            Console.WriteLine("Checks");
                            if (!i.Singles.OrderBy(p => p).SequenceEqual(Singles.OrderBy(p => p)))
                            {
                                throw new LRXDontLoveMeException(string.Format(_StartWindow.ExceptionDiff, i.Name));
                            }
                        }

                        i.Checked = i == control;
                        _checket = true;
                        i._checket = true;
                    }
                }
                else
                {
                    Checked = !Checked;
                }
            }
        }
    }

    // 继承自MizukiLabel的类，用于安装选项相关的标签
    public class SetupLabel : MizukiLabel
    {
        private byte[] _file;
        public long FileSize { get; private set; }

        // 获取或设置文件名，同时设置文件大小
        public string Filename;

        public byte[] File
        {
            get => _file;
            set
            {
                _file = value;
                FileSize = _StartWindow.GetUncompresswdSize(_file);
            }
        }
    }

    // 自己写的小函数。
    public class MizukiTools
    {
        // 设置鼠标指针。
        public static void SetCursor(Bitmap cursor, Point hotPoint, Form form)
        {
            int hotX = hotPoint.X;
            int hotY = hotPoint.Y;
            Bitmap myNewCursor = new Bitmap(cursor.Width * 2 - hotX, cursor.Height * 2 - hotY);
            Graphics g = Graphics.FromImage(myNewCursor);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawImage(cursor, cursor.Width - hotX, cursor.Height - hotY, cursor.Width,
            cursor.Height);
            form.Cursor = new Cursor(myNewCursor.GetHicon());

            g.Dispose();
            myNewCursor.Dispose();
        }

        // 将布尔值转化为数字。
        public static int BoolToInt(bool Bool)
        {
            return Bool ? 1 : 0;
        }

        // 将嵌入的资源引入本地，并返回本地地址。
        public static string EmbedToOutside(Stream stream, string filename, AxWindowsMediaPlayer player)
        {
            if (stream != null)
            {
                filename = Path.Combine(Path.GetTempPath(), filename);
                using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                }
                // 设置 Windows Media Player 控件的 URL 为临时文件路径
                player.uiMode = "invisible";
                player.URL = filename;
                player.Ctlcontrols.stop();
                return filename;
            }
            return string.Empty;
        }

        // 将焦点转移到标签上去，防止按钮显示焦点。
        public static void TenIFocus(Label label)
        {
            label.Focus();
        }

        // 将错误信息弹出。
        public static void 弹窗(string ex)
        {
            MessageBox.Show(ex);
        }

        // 将值限制在一个区间。
        public static int GetValueInADuring(int value, int minimum, int maximum)
        {
            if (value < minimum)
            {
                return minimum;
            }
            else if (value > maximum)
            {
                return maximum;
            }
            return value;
        }

        public static bool IsGoodPath(string a)
        {
            return !Regex.IsMatch(a, "[\\*\"<>\\|\\?\t\n\f\r]") && a.Length > 2 && !Regex.IsMatch(a.Substring(2), ":");
        }

    }

    // 自定义类TreeViewItems 调用其Add(TreeNode e)方法加载子目录
    public static class TreeViewItems
    {
        public static void Add(TreeNode e)
        {
            // try..catch异常处理
            try
            {
                e.Nodes.Clear();                     // 清除空节点再加载子节点
                TreeNode tNode = e;                  // 获取选中\展开\折叠结点
                string path = tNode.Name;            // 路径  

                // 获取指定目录中的子目录名称并加载结点
                string[] dics = Directory.GetDirectories(path);
                foreach (string dic in dics)
                {
                    DirectoryInfo dicr = new DirectoryInfo(dic);
                    TreeNode subNode = new TreeNode(dicr.Name); // 实例化
                    subNode.Name = dicr.FullName;               // 完整目录
                    subNode.Tag = subNode.Name;
                    try
                    {
                        // 有子目录再添加占位符
                        if (Directory.GetDirectories(dicr.FullName).Length > 0)
                        {
                            subNode.Nodes.Add("");  // 加载空节点 实现+号
                        }
                    }
                    catch (Exception ex)
                    {
                        subNode.Text = string.Empty;
                        Console.WriteLine(ex.ToString());
                    }
                    if (subNode.Text != string.Empty)
                    {
                        tNode.Nodes.Add(subNode);
                    }
                }
            }
            catch (Exception msg)
            {
                MizukiTools.弹窗(msg.Message);                  //异常处理
            }
        }
    }

    // 一个用于命令行的类。
    public static class ConsoleCommandManager
    {
        // 运行命令行。
        public static void RunConsoleCommand(string command, string argument, out int exitCode, out string stdOut, out string stdErr)
        {
            var process = new System.Diagnostics.Process()
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = command,
                    Arguments = argument,
                    RedirectStandardInput = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            _ = process.Start();
            process.WaitForExit();

            stdOut = process.StandardOutput.ReadToEnd();
            stdErr = process.StandardError.ReadToEnd();
            exitCode = process.ExitCode;
        }
    }

    public class LRXDontLoveMeException : Exception
    {
        public LRXDontLoveMeException() : base()
        {
        }

        public LRXDontLoveMeException(string message) : base(message)
        {
        }

        public LRXDontLoveMeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    #endregion

}
