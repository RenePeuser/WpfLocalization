using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using WpfLocalization;
using System.Threading;

namespace Demo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        int _languageChanges;

        System.Timers.Timer _timer;

        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            labelCodeBehind_Resource.Property(TextBlock.TextProperty).SetResourceValue("Text_CodeBehind_Resource");
            labelCodeBehind_Format.Property(TextBlock.TextProperty).SetFormattedValue("{0:d}", DateTime.Now);
            labelCodeBehind_ResourceFormat.Property(TextBlock.TextProperty).SetResourceFormattedValue(
                "Text_CodeBehind_ResourceFormat", 
                DateTime.Now,
                DateTime.Now.AddDays(1)
                );

            // Multiple languages demo

            labelMultilingual_CodeBehind_Resource.Property(TextBlock.TextProperty).SetResourceValue("Text_CodeBehind_Resource");
            labelMultilingual_CodeBehind_Format.Property(TextBlock.TextProperty).SetFormattedValue("{0:d}", DateTime.Now);
            labelMultilingual_CodeBehind_ResourceFormat.Property(TextBlock.TextProperty).SetResourceFormattedValue(
                "Text_CodeBehind_ResourceFormat",
                DateTime.Now,
                DateTime.Now.AddDays(1)
                );

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _timer.Dispose();
        }

        private void comboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = comboBoxLanguage.SelectedItem as ComboBoxItem;

            var culture = CultureInfo.GetCultureInfo(selectedItem.Tag as string);

            Dispatcher.Thread.CurrentCulture = culture;
            Dispatcher.Thread.CurrentUICulture = culture;

            LocalizationManager.UpdateValues();
        }

        private object Callback(CultureInfo culture, CultureInfo uiCulture, object parameter)
        {
            // The "culture" and "uiCulture" parameters must be always respected if:
            // - The application calls the "LocalizationManager.UpdateValues()" method from threads
            //   other than the UI thread. In this case this callback will be executed in the context
            //   of the thread calling the "UpdateValues()" method.
            // - The application uses multiple languages at the same time and this callback is used to 
            //   localize a control that uses a language different than the default language of the
            //   application

            return string.Format(
                culture,
                Properties.Resources.ResourceManager.GetString("Text_LanguageChanges", uiCulture),
                ++_languageChanges
                );
        }

        private object Multilingual_Callback(CultureInfo culture, CultureInfo uiCulture, object parameter)
        {
            // The "culture" and "uiCulture" parameters must be always respected if:
            // - The application calls the "LocalizationManager.UpdateValues()" method from threads
            //   other than the UI thread. In this case this callback will be executed in the context
            //   of the thread calling the "UpdateValues()" method.
            // - The application uses multiple languages at the same time and this callback is used to 
            //   localize a control that uses a language different than the default language of the
            //   application

            return string.Format(
                culture,
                Properties.Resources.ResourceManager.GetString("Text_CurrentCulture", uiCulture),
                uiCulture.Name
                );
        }

        private void comboBoxMultilingual_Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = comboBoxMultilingual_Language.SelectedItem as ComboBoxItem;

            var cultureName = selectedItem.Tag as string;

            CultureInfo culture;

            if (string.IsNullOrEmpty(cultureName))
            {
                // Use the culture of the parent
                culture = null;
            }
            else
            {
                culture = CultureInfo.GetCultureInfo(cultureName);
            }

            LocalizationScope.SetCulture(panelMultilingual, culture);
            LocalizationScope.SetUICulture(panelMultilingual, culture);

            LocalizationManager.UpdateValues();
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Set the value of a property directly from a non-UI thread

            labelMultithreading_CurrentTime.Property(TextBlock.TextProperty).SetResourceFormattedValue(
                "Text_Multithreading_CurrentTime",
                DateTime.Now
                );
        }
    }
}
