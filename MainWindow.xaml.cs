using Microsoft.Win32;
using Sciensano.CovidJson.Parser.ISciensanoParsers;
using Sciensano.CovidJson.Parser.Models;
using Sciensano.CovidJson.Parser.SciensanoModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openDialog = new OpenFileDialog
                {
                    AddExtension = true,
                    DefaultExt = ".json",
                    Filter = "JSON Data (.json)|*.json",
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    Title = $"Open {((TabItem)Tabs.SelectedItem).Header} File"
                };

                if (!(openDialog.ShowDialog(this) ?? false))
                    return;

                DataGrid dataGrid = null;
                Type targetType = null;
                if (tabCases.IsSelected)
                {
                    dataGrid = CasesGrid;
                    targetType = typeof(CasesModel);
                }
                else if (tabTests.IsSelected)
                {
                    dataGrid = TestsGrid;
                    targetType = typeof(TestsModel);
                }                    
                else if (tabHospitalisations.IsSelected)
                {
                    dataGrid = HospitalisationGrid;
                    targetType = typeof(HospitalisationModel);
                }  

                IList<ILocalModel> list;
                using (var stream = File.Open(openDialog.FileName, FileMode.Open))
                    list = GetData(stream, targetType);

                dataGrid.ItemsSource = list.Count > 0 ? list : null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open file.{Environment.NewLine}Error : {Environment.NewLine}{ex.Message}");
            }
        }

        private void DownloadFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = null;
                Type sourceType = null;
                if (tabCases.IsSelected)
                {
                    dataGrid = CasesGrid;                  
                    sourceType = typeof(SciensanoCasesModel);
                }
                else if (tabTests.IsSelected)
                {
                    dataGrid = TestsGrid;
                    sourceType = typeof(SciensanoTestsModel);
                }
                else if (tabHospitalisations.IsSelected)
                {
                    dataGrid = HospitalisationGrid;
                    sourceType = typeof(SciensanoHospitalisationModel);
                }

                var data = SciensanoCovidDownloader.GetCovidData(sourceType);
                IList<ILocalModel> list;
                using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(data)))
                    list = GetData(stream, sourceType);

                dataGrid.ItemsSource = list.Count > 0 ? list : null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to download file.{Environment.NewLine}Error : {Environment.NewLine}{ex.Message}");
            }
        }

        private IList<ILocalModel> GetData(Stream stream, Type targetType)
        {        
            try
            {
                if (stream == null || targetType == null)
                    throw new ArgumentException("Invalid stream or targetType.");

                var parser = SciensanoParserSelector.GetParser(targetType);
                return parser.Parse(stream);
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception trying to parse JSON file.{Environment.NewLine}Error : {Environment.NewLine}{ex.Message}");
            }
        }
    }
}
