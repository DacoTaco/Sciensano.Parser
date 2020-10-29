using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Sciensano.CovidJson.Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //download json from https://epistat.wiv-isp.be/covid/
            InitializeComponent();
            //var list = SciensanoCovidDeserializer.GetHospitalisationTotals("");
            //HospitalisationGrid.ItemsSource = list;
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var list = new List<object>();
            DataGrid datagrid = null;

            var openDialog = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = ".json",
                Filter = "JSON Data (.json)|*.json",
                InitialDirectory = Directory.GetCurrentDirectory()
            };

            if (! (openDialog.ShowDialog(this) ?? false))
                return;

            var filepath = openDialog.FileName;
            try
            {
                if(tabCases.IsSelected)
                {
                    datagrid = CasesGrid;
                    list = SciensanoCovidDeserializer.GetCases(filepath).ToList<object>();
                }
                else if (tabTests.IsSelected)
                {
                    datagrid = TestsGrid;
                    list = SciensanoCovidDeserializer.GetTests(filepath).ToList<object>();
                }
                else if (tabHospitalisations.IsSelected)
                {
                    datagrid = HospitalisationGrid;
                    list = SciensanoCovidDeserializer.GetHospitalisation(filepath).ToList<object>();                   
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Exception trying to parse JSON file.{Environment.NewLine}Error : {Environment.NewLine}{ex.Message}");
            }

            if (datagrid == null)
                return;

            datagrid.ItemsSource = list.Count > 0 ? list : null;
        }
    }
}
