using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrimMIDI.Properties;
using TrimMIDI.Tool;

namespace TrimMIDI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        #region 基础逻辑

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.FirstStart)
            {
                Info.Welc();
                Settings.Default.FirstStart = false;
                Settings.Default.Save();
            }
        }

        /// <summary> 快捷键 </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
                Info.Help();
            else if (e.Key == Key.F3)
                Logger.Save();
            else if (e.Key == Key.F5)
                Clear();
        }

        /// <summary> 清除文本框 </summary>
        private void Clear()
        {
            Tx_Path.Clear();
            Tx_ShortCut.Clear();
            Tx_Gap.Clear();
            Tx_EQ1.Text = "1.000";
            Tx_EQ2.Text = "1.000";
            Tx_EQ3.Text = "1.000";
            Tx_EQ4.Text = "1.000";
            Bt_Run.IsEnabled = false;
        }

        /// <summary> MIDI文件存在，激活功能 </summary>
        private void ActivateProc()
        {
            Ch_ShortCut.IsEnabled = true;
            Ch_Gap.IsEnabled = true;
            Ch_EQ.IsEnabled = true;
            Bt_GarthSettings.IsEnabled = true;
            RunCheck();
        }

        /// <summary> MIDI文件不存在，禁用功能 </summary>
        private void DeactivateProc()
        {
            Ch_ShortCut.IsEnabled = false;
            Ch_Gap.IsEnabled = false;
            Ch_EQ.IsEnabled = false;
            Bt_GarthSettings.IsEnabled = false;
            Bt_Run.IsEnabled = false;
        }

        #endregion

        #region 个人配置

        private void Bt_GarthSettings_Click(object sender, RoutedEventArgs e)
        {
            Ch_ShortCut.IsChecked = true;
            Tx_ShortCut.Text = "15";
            Ch_Gap.IsChecked = true;
            Tx_Gap.Text = "60";
            Ch_EQ.IsChecked = true;
            Sl_EQ1.Value = 21;
            Tx_EQ1.Text = "1.111";
            Sl_EQ2.Value = 50;
            Tx_EQ2.Text = "1.000";
            Sl_EQ3.Value = 80;
            Tx_EQ3.Text = "1.000";
            Sl_EQ4.Value = 108;
            Tx_EQ4.Text = "0.900";
        }

        #endregion

        #region 载入文件

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
            e.Handled = true;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)
                && e.Data.GetData(DataFormats.FileDrop) is string[] fileOrDirectory)
            {
                var path = fileOrDirectory.Where(MIDIProc.IsMIDIFileOrDirectory)
                                          .FirstOrDefault();
                if (path is not null)
                    Tx_Path.Text = path;
                else MsgB.OkErr("未找到有效的MIDI文件。", "错误");
            }
        }

        private void Tx_Path_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Tx_Path.Text)
                && MIDIProc.IsMIDIFileOrDirectory(Tx_Path.Text))
            {
                Tool.Buffer.LoadMidiFiles(Tx_Path.Text);
                if (Tool.Buffer.MidiFiles.Length > 0)
                    ActivateProc();
                else DeactivateProc();
            }
            else DeactivateProc();
        }

        #endregion

        #region 模块开关

        private void Ch_ShortCut_Checked(object sender, RoutedEventArgs e)
        {
            Tx_ShortCut.IsEnabled = true;
            RunCheck();
        }

        private void Ch_Gap_Checked(object sender, RoutedEventArgs e)
        {
            Tx_Gap.IsEnabled = true;
            RunCheck();
        }

        private void Ch_EQ_Checked(object sender, RoutedEventArgs e)
        {
            Sl_EQ1.IsEnabled = true;
            Sl_EQ2.IsEnabled = true;
            Sl_EQ3.IsEnabled = true;
            Sl_EQ4.IsEnabled = true;
            Tx_EQ1.IsEnabled = true;
            Tx_EQ2.IsEnabled = true;
            Tx_EQ3.IsEnabled = true;
            Tx_EQ4.IsEnabled = true;
            RunCheck();
        }

        private void Ch_ShortCut_Unchecked(object sender, RoutedEventArgs e)
        {
            Tx_ShortCut.IsEnabled = false;
            RunCheck();
        }

        private void Ch_Gap_Unchecked(object sender, RoutedEventArgs e)
        {
            Tx_Gap.IsEnabled = false;
            RunCheck();
        }

        private void Ch_EQ_Unchecked(object sender, RoutedEventArgs e)
        {
            Sl_EQ1.IsEnabled = false;
            Sl_EQ2.IsEnabled = false;
            Sl_EQ3.IsEnabled = false;
            Sl_EQ4.IsEnabled = false;
            Tx_EQ1.IsEnabled = false;
            Tx_EQ2.IsEnabled = false;
            Tx_EQ3.IsEnabled = false;
            Tx_EQ4.IsEnabled = false;
            RunCheck();
        }

        #endregion

        #region 6个文本框的变动

        private void Tx_ShortCut_TextChanged(object sender, TextChangedEventArgs e)
            => RunCheck();

        private void Tx_Gap_TextChanged(object sender, TextChangedEventArgs e)
            => RunCheck();

        private void Tx_EQ1_TextChanged(object sender, TextChangedEventArgs e)
            => RunCheck();

        private void Tx_EQ2_TextChanged(object sender, TextChangedEventArgs e)
            => RunCheck();

        private void Tx_EQ3_TextChanged(object sender, TextChangedEventArgs e)
            => RunCheck();

        private void Tx_EQ4_TextChanged(object sender, TextChangedEventArgs e)
            => RunCheck();

        #endregion

        #region 滑条相互作用

        private void Sl_EQ1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Ch_EQ.IsChecked == true && Sl_EQ1.Value >= Sl_EQ2.Value)
                Sl_EQ2.Value = Sl_EQ1.Value + 1;
        }

        private void Sl_EQ2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Ch_EQ.IsChecked == true && Sl_EQ2.Value >= Sl_EQ3.Value)
                Sl_EQ3.Value = Sl_EQ2.Value + 1;
            if (Ch_EQ.IsChecked == true && Sl_EQ2.Value <= Sl_EQ1.Value)
                Sl_EQ1.Value = Sl_EQ2.Value - 1;
        }

        private void Sl_EQ3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Ch_EQ.IsChecked == true && Sl_EQ3.Value >= Sl_EQ4.Value)
                Sl_EQ4.Value = Sl_EQ3.Value + 1;
            if (Ch_EQ.IsChecked == true && Sl_EQ3.Value <= Sl_EQ2.Value)
                Sl_EQ2.Value = Sl_EQ3.Value - 1;
        }

        private void Sl_EQ4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Ch_EQ.IsChecked == true && Sl_EQ4.Value <= Sl_EQ3.Value)
                Sl_EQ3.Value = Sl_EQ4.Value - 1;
        }

        #endregion

        #region 在滑条上滚鼠标

        private void Sl_EQ1_MouseWheel(object sender, MouseWheelEventArgs e)
            => Sl_EQ1.Value = e.Delta > 0
                ? Sl_EQ1.Value + 1
                : Sl_EQ1.Value - 1;

        private void Sl_EQ2_MouseWheel(object sender, MouseWheelEventArgs e)
            => Sl_EQ2.Value = e.Delta > 0
                ? Sl_EQ2.Value + 1
                : Sl_EQ2.Value - 1;

        private void Sl_EQ3_MouseWheel(object sender, MouseWheelEventArgs e)
            => Sl_EQ3.Value = e.Delta > 0
                ? Sl_EQ3.Value + 1
                : Sl_EQ3.Value - 1;

        private void Sl_EQ4_MouseWheel(object sender, MouseWheelEventArgs e)
            => Sl_EQ4.Value = e.Delta > 0
                ? Sl_EQ4.Value + 1
                : Sl_EQ4.Value - 1;

        #endregion

        #region 运行前检查

        private void RunCheck()
        {
            bool fileValid = Tool.Buffer.MidiFiles.Length > 0;
            bool needShortCut = Ch_ShortCut.IsChecked == true
                                && Text.IsPosInt(Tx_ShortCut.Text);
            bool needGap = Ch_Gap.IsChecked == true
                           && Text.IsPosInt(Tx_Gap.Text);
            bool needEQ = Ch_EQ.IsChecked == true
                          && Text.IsNonNegDouble(Tx_EQ1.Text)
                          && Text.IsNonNegDouble(Tx_EQ2.Text)
                          && Text.IsNonNegDouble(Tx_EQ3.Text)
                          && Text.IsNonNegDouble(Tx_EQ4.Text);
            if (Bt_Run is not null)
                Bt_Run.IsEnabled = fileValid && (needShortCut || needGap || needEQ);
        }

        #endregion

        #region 执行修改

        private void Bt_Run_Click(object sender, RoutedEventArgs e)
        {
            if (Ch_ShortCut.IsChecked == true)
                Modifier.Process.Add(Modifier.ShortCut(GetShortCutParameters()));
            if (Ch_Gap.IsChecked == true)
                Modifier.Process.Add(Modifier.Gap(GetGapParameters()));
            if (Ch_EQ.IsChecked == true)
                Modifier.Process.Add(Modifier.EQ(GetEQParameters()));
            Modifier.Run();
            Bt_Run.IsEnabled = false;
        }

        private int GetShortCutParameters() => int.Parse(Tx_ShortCut.Text);

        private int GetGapParameters() => int.Parse(Tx_Gap.Text);

        private (int, int, int, int, double, double, double, double) GetEQParameters()
            => ((int)Sl_EQ1.Value,
                (int)Sl_EQ2.Value,
                (int)Sl_EQ3.Value,
                (int)Sl_EQ4.Value,
                double.Parse(Tx_EQ1.Text),
                double.Parse(Tx_EQ2.Text),
                double.Parse(Tx_EQ3.Text),
                double.Parse(Tx_EQ4.Text));

        #endregion
    }
}
