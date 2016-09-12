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

namespace MonitorWindows.Components
{

    public class ScenarioMagmetItem
    {

        public ScenarioMagmetItem(ScenarioMagmet operation)
        {
            this.operation = operation;
        }

        private ScenarioMagmet operation;

        private Label item;

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (item != null) item.Content = value;
                text = value;
            }
        }

        public Label GetItem()
        {
            if(item == null)
            {
                item = new Label();
                item.Style = operation.FindResource("ScenarioMagmetItemStyle") as Style;
                item.Content = text;
                ContextMenu menu = new ContextMenu();
                MenuItem openMenuItem = new MenuItem() { Header = item.FindResource("ScenarioMagmet_Menu_Open") };
                MenuItem editMenuItem = new MenuItem() { Header = item.FindResource("ScenarioMagmet_Menu_Edit") };
                editMenuItem.Click += (object sender, RoutedEventArgs e) =>
                {
                    new Windows.EditScenarioMagmet.EditScenarioMagmetWin(this.text, (string name) => {
                        this.Text = name;
                    }).ShowDialog();
                };
                MenuItem saveMenuItem = new MenuItem() { Header = item.FindResource("ScenarioMagmet_Menu_SaveThis") };
                MenuItem delMenuItem = new MenuItem() { Header = item.FindResource("ScenarioMagmet_Menu_Del") };
                delMenuItem.Click += (object sender, RoutedEventArgs e) =>
                {
                    operation.DelItem(this);
                };
                menu.Items.Add(openMenuItem);
                menu.Items.Add(editMenuItem);
                menu.Items.Add(saveMenuItem);
                menu.Items.Add(delMenuItem);
                item.ContextMenu = menu;
            }
            return item;
        }
    }

    /// <summary>
    /// ScenarioMagmet.xaml 的交互逻辑
    /// </summary>
    public partial class ScenarioMagmet : UserControl
    {
        private List<ScenarioMagmetItem> dataList = new List<ScenarioMagmetItem>();

        public ScenarioMagmet()
        {
            InitializeComponent();
        }

        public void AddItem(ScenarioMagmetItem item)
        {
            if (dataList.Contains(item) || item == null) return;
            dataList.Add(item);
            dataListDom.Children.Add(item.GetItem());
        }

        public void DelItem(ScenarioMagmetItem item)
        {
            if (!dataList.Contains(item) || item == null) return;
            dataList.Remove(item);
            dataListDom.Children.Remove(item.GetItem());
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            new Windows.AddScenarioMagmet.AddScenarioMagmetWin((string name) => {
                var item = new ScenarioMagmetItem(this) { Text = name };
                this.AddItem(item);
            }).ShowDialog();
        }


        Windows.ScenarioChannel.ScenarioChannelWin scenarioChannelWin;
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (scenarioChannelWin == null || !scenarioChannelWin.Activate())
            {
                scenarioChannelWin = new Windows.ScenarioChannel.ScenarioChannelWin();
                scenarioChannelWin.Show();
            }
        }
    }
}
