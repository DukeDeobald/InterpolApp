using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InterpolApp.ViewModels;

namespace InterpolApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
            
#endif
            
            this.DataContext = new MainWindowViewModel();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
    
}
