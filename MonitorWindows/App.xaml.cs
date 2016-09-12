using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace MonitorWindows
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
            {
                dictionaryList.Add(dictionary);
            }
            string requestedCulture = "StringResourceZh.xaml";
            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.Equals("zh-CN"))
            {
                requestedCulture = "StringResourceEn.xaml";
            }
            ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.IndexOf(requestedCulture) > -1);
            Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
        }
    }
}
