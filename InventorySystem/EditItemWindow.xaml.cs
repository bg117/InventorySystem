using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InventorySystem.Models;
using MahApps.Metro.Controls;

namespace InventorySystem
{
    /// <summary>
    /// Interaction logic for EditItemWindow.xaml
    /// </summary>
    public partial class EditItemWindow : MetroWindow
    {
        public EditItemWindow()
        {
            InitializeComponent();
        }

        public EditItemWindow(ref Item item)
        {
            InitializeComponent();
            DataContext = item;
        }
    }
}
