using System.Collections.Generic;
using System.Windows;

namespace EquipRentalPointApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static List<Window> OpenedWindows { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            OpenedWindows = new List<Window>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            foreach (var window in OpenedWindows)
            {
                window.Close();
            }
        }
    }
}
