using System.Windows;

namespace Cactus.Views
{
    /// <summary>
    /// Interaction logic for EditView.xaml
    /// </summary>
    public partial class EditView : Window
    {
        public EditView()
        {
            InitializeComponent();
        }

        // Not pure MVVM but doing this for now to close the window.
        // The actual "side effects" are happening through a command
        // and it hits the view model.
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
