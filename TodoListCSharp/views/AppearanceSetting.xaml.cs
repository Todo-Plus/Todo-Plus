using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TodoListCSharp.core;

namespace TodoListCSharp.views {
    /// <summary>
    /// AppearanceSetting.xaml 的交互逻辑
    /// </summary>
    public partial class AppearanceSetting : Window {
        private bool bDragging = false;
        public int iShowPercent { get; set; }
        public Setting setting;

        // !! delegate & events
        public delegate void ClosedCallbackFunc();

        public event ClosedCallbackFunc closedCallbackFunc;

        public delegate void SliderValueChangeCallbackFunc(int value);

        public event SliderValueChangeCallbackFunc SliderValueChangeCallback;

        public delegate void AppearanceSettingChangeCallback(Setting setting);

        public event AppearanceSettingChangeCallback AppearanceSettingChange;

        public delegate void ConfirmButtonCallbackFunc();

        public event ConfirmButtonCallbackFunc ConfirmButtonCallback;

        public delegate void CancelButtonCallbackFunc();

        public event CancelButtonCallbackFunc CancelButtonCallback;

        // !! Functions
        public AppearanceSetting() {
            InitializeComponent();
        }

        private void AppearanceSetting_onLoaded(object sender, EventArgs e) {
            this.AppearanceColorPicker.SelectColorCallback += AppearanceSetting_onColorChange;
        }

        private void AppearanceSetting_OnClosed(object sender, EventArgs e) {
            if (closedCallbackFunc != null) {
                closedCallbackFunc();
            }
        }

        private void SetShowPercentValue(int value) {
            iShowPercent = value;
            this.PercentLabel.Content = iShowPercent;

            setting.Alpha = value;
            SettingTemporaryChange();
        }

        private void SettingTemporaryChange() {
            if (AppearanceSettingChange != null) {
                AppearanceSettingChange(setting);
            }
        }

        private void AppearanceSetting_onDragOver(object sender, DragCompletedEventArgs e) {
            SetShowPercentValue((int) ((Slider) sender).Value);
            bDragging = false;
        }

        private void AppearanceSetting_onDragStart(object sender, DragStartedEventArgs e) {
            bDragging = true;
        }

        private void AppearanceSetting_onValueChange(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (!bDragging) {
                SetShowPercentValue((int) e.NewValue);
            }
        }

        private void AppearanceSetting_onColorChange(Color color) {
            setting.BackgroundColor = color;
            SettingTemporaryChange();
        }

        public void ConfirmButton_onClicked(object sender, RoutedEventArgs e) {
            if (ConfirmButtonCallback != null) {
                ConfirmButtonCallback();
            }

            this.CloseMessageWindow(sender, e);
        }

        public void CancelButton_onClicked(object sender, RoutedEventArgs e) {
            if (CancelButtonCallback != null) {
                CancelButtonCallback();
            }

            this.CloseMessageWindow(sender, e);
        }

        public void CloseMessageWindow(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void CloseButton_onClicked(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}