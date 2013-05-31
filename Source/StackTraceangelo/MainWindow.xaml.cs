/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace StackTraceangelo
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainWindowViewModel();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;
            button.ContextMenu.Visibility = Visibility.Visible;
            button.ContextMenu.PlacementTarget = button;
            button.ContextMenu.Placement = PlacementMode.Bottom;
            button.ContextMenu.IsOpen = true;
        }

        private void OnContextMenuClosed(object sender, RoutedEventArgs e)
        {
            ((ContextMenu)sender).Visibility = Visibility.Hidden;
        }
    }
}
