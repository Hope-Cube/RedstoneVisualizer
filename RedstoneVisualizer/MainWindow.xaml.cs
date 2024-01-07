using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Button = System.Windows.Controls.Button;
using Orientation = System.Windows.Controls.Orientation;

namespace RedstoneVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool isX = x > y;
        private static int x = 6;
        private static int y = 6;
        private static SolidColorBrush on = new SolidColorBrush(new Color {A = 255, R = 255, G = 159, B = 63});
        private static SolidColorBrush off = new SolidColorBrush(new Color { A = 255, R = 137, G = 80, B = 49 });
        public MainWindow()
        {
            InitializeComponent();
            GridGen();
        }
        private void GridGen()
        {
            StackPanel vertAxis = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Name = "VertAxis"
            };
            Grid.SetRow(vertAxis, 0); // Row 0
            Grid.SetColumn(vertAxis, 0); // Column 0
            myGrid.Children.Add(vertAxis);

            for (int xx = 0; xx < x; xx++)
            {
                StackPanel stackPanel = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Orientation = Orientation.Vertical,
                    Name = $"p{xx}"
                };
                double s = (isX ? Width / x : Height / y) - 12;
                List<Button> buttonList = Enumerable.Range(0, y)
                                                        .Select(yy =>
                                                        {
                                                            Button button = new Button
                                                            {
                                                                Name = $"p{xx}{yy}",
                                                                Background = off,
                                                                Width = s,
                                                                Height = s
                                                            };

                                                            // Attach the Click event handler
                                                            button.Click += Button_Click;

                                                            return button;
                                                        })
                                                        .ToList();

                buttonList.ForEach(button => stackPanel.Children.Add(button));
                vertAxis.Children.Add(stackPanel);
            }
        }
        private void MainWin_StateChanged(object sender, EventArgs e)
        {
            if (MainWin.WindowState.HasFlag(WindowState.Normal))
            {
                double scaleFactor = .5; // Adjust this value based on your desired percentage

                // Display the primary screen resolution
                MainWin.Width = Screen.PrimaryScreen.Bounds.Width * scaleFactor;
                MainWin.Height = Screen.PrimaryScreen.Bounds.Height * scaleFactor;

                double s = (isX ? Width / x : Height / y) - 12;
                List<Button> buttonList = FindButtonsInVisualTree(this);
                foreach (Button button in buttonList)
                {
                    button.Height = s;
                    button.Width = s;
                }
            }
        }
        private List<Button> FindButtonsInVisualTree(DependencyObject root)
        {
            List<Button> buttonList = new List<Button>();
            Queue<DependencyObject> queue = new Queue<DependencyObject>();

            // Enqueue the root of the visual tree
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();

                // Check if the current object is a Button
                if (current is Button button)
                {
                    // Add the button to the list
                    buttonList.Add(button);
                }

                // Enqueue the children of the current object
                int count = VisualTreeHelper.GetChildrenCount(current);
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(current, i);
                    queue.Enqueue(child);
                }
            }

            return buttonList;
        }
        private void MainWin_Initialized(object sender, EventArgs e)
        {
            MainWin_StateChanged(sender, e);
            GridGen();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Color currentColor = ((SolidColorBrush)button.Background).Color;

            button.Background = currentColor == off.Color ? on : off;
        }
    }
}
