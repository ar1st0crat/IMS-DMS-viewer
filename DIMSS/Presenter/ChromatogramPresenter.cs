using DIMSS.Model;
using DIMSS.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading.Tasks;
using SciColorMaps;

namespace DIMSS.Presenter
{
    class ChromatogramPresenter
    {
        private readonly Chromatogram _model = new Chromatogram();
        private readonly IChromatogramView _view;

        /// <summary>
        /// In-memory (for better performance) bitmap containing chromatogram image
        /// </summary>
        private Bitmap _chromatogram2D;

        /// <summary>
        /// Main colormap used for chromatogram display
        /// </summary>
        private readonly ColorMap _colorMap = new ColorMap("viridis");

        public ChromatogramPresenter(IChromatogramView view)
        {
            _view = view;
            _view.ViewLoaded += OnChromatogramLoad;
            _view.PreviousScan += OnPreviousScan;
            _view.NextScan += OnNextScan;
            _view.NavigateScan += OnNavigateScan;
        }

        /// <summary>
        /// Method checks if everything is alright with mzxml
        /// </summary>
        /// <param name="mzxmlFilename">The full name of mzXml file to parse</param>
        /// <returns>true if mzXml was loaded and parsed succesfully; false - otherwise</returns>
        public bool CheckFileCorrect(string mzxmlFilename)
        {
            string parseResult = _model.Load(mzxmlFilename);

            if (parseResult != MZXMLParser.LoadSuccessMessage)
            {
                MessageBox.Show(parseResult);
                return false;
            }

            if (_model.ScanCount <= 0)
            {
                MessageBox.Show("No spectral information in given file!");
                return false;
            }

            return true;
        }

        private async void OnChromatogramLoad(object sender, EventArgs e)
        {
            await LoadChromatogram();
        }

        /// <summary>
        /// Asynchronous loading, showing and drawing of all things related to a chromatogram
        /// </summary>
        public async Task LoadChromatogram()
        {
            // Retrieve the number of scans and fill the combobox 
            // with corresponding range of values for navigation
            int scanCount = _model.ScanCount;

            for (int i = 1; i <= scanCount; i++)
            {
                _view.ScansView.Items.Add(i.ToString());
            }

            _view.ScansView.SelectedIndex = 0;

            // TAPping ))
            var spectra = await LoadAllSpectraAsync(scanCount);

            // keep TAPping
            await DrawChromatogramAsync(spectra);

            // update left panel
            UpdateChart();
        }

        private Task<List<MZSpectrum>> LoadAllSpectraAsync(int scanCount)
        {
            return Task.Run(() =>
            {
                List<MZSpectrum> spectra = new List<MZSpectrum>();

                for (int scanNo = 1; scanNo <= scanCount; scanNo++)
                {
                    MZSpectrum spectrum = _model.GetMZSpectrumByIndex(scanNo);
                    if (spectrum == null)
                    {
                        // unlikely, but if this happens then at least display upper part of chromatogram
                        // that was correctly parsed by mzxmlparser
                        break;
                    }
                    spectra.Add(spectrum);
                }

                return spectra;
            });
        }

        private Task DrawChromatogramAsync(List<MZSpectrum> spectra)
        {
            return Task.Run(() =>
            {
                int scanCount = spectra.Count;

                int width = (int)spectra.Max(s => s.MZList.Max()) + 1;
                int height = scanCount;

                // create empty bitmap and fill it first with default color...
                _chromatogram2D = new Bitmap(width, height);

                for (int i = 0; i < _chromatogram2D.Width; i++)
                {
                    for (int j = 0; j < _chromatogram2D.Height; j++)
                    {
                        _chromatogram2D.SetPixel(i, j, _colorMap.GetColorByNumber(0));
                    }
                }

                // ...now fill chromatogram with meaningful colors
                int colorScaleFactor = 500;

                for (int scanNo = 0; scanNo < scanCount; scanNo++)
                {
                    var spectrum = spectra[scanNo];

                    for (int i = 0; i < spectrum.PeakCount; i++)
                    {
                        int x = (int)spectrum.MZList[i];
                        float y = (float)spectrum.IntensityList[i];

                        _chromatogram2D.SetPixel(x, scanNo,
                            _colorMap.GetColor(y / colorScaleFactor));
                    }
                }

                _view.ChromatogramImage.Image = _chromatogram2D;
            });
        }

        /// <summary>
        /// Update right panel of the window:
        /// Show current spectrum line on chromatogram without redrawing the entire chromatogram
        /// </summary>
        private void UpdateChromatogram2D()
        {
            MZSpectrum curSpectrum = _model.GetMZSpectrumByIndex(_model.CurrentMZSpectrum);

            var markedChromatogram2D = new Bitmap(_chromatogram2D);

            for (int i = 0; i < markedChromatogram2D.Width; i++)
            {
                markedChromatogram2D.SetPixel(i, _model.CurrentMZSpectrum, Color.White);
            }

            _view.ChromatogramImage.Image = markedChromatogram2D;
        }

        /// <summary>
        /// Update left panel of the window (datagrid and chart with currently selected spectrum)
        /// </summary>
        private void UpdateChart()
        {
            MZSpectrum spectrum = _model.GetCurrentMZSpectrum();
            if (spectrum == null)
            {
                MessageBox.Show(MZXMLParser.ReadErrorMessage);
                return;
            }

            // fill datagrid ...
            _view.MZSpectraView.ColumnCount = spectrum.PeakCount + 1;
            _view.MZSpectraView.RowCount = 2;
            _view.MZSpectraView.Rows[0].DefaultCellStyle.BackColor = Color.LightYellow;
            _view.MZSpectraView.Rows[0].Cells[0].Value = string.Format("m/z peaks [ {0} ]", spectrum.PeakCount);
            _view.MZSpectraView.Rows[1].Cells[0].Value = "intensity";

            // ... and data chart
            Series mzSpectrum = new Series();

            for (int i = 0; i < spectrum.PeakCount; i++)
            {
                mzSpectrum.Points.AddXY(spectrum.MZList[i], spectrum.IntensityList[i]);

                _view.MZSpectraView.Rows[0].Cells[i + 1].Value = spectrum.MZList[i].ToString();
                _view.MZSpectraView.Rows[1].Cells[i + 1].Value = spectrum.IntensityList[i].ToString();
            }

            _view.MZSpectraView.AutoResizeColumns();
            _view.ChromatogramChart.Series.Clear();
            _view.ChromatogramChart.Series.Add(mzSpectrum);

            _view.ScansView.SelectedIndex = _model.CurrentMZSpectrum - 1;
        }
        
        private void OnPreviousScan(object sender, EventArgs e)
        {
            _model.CurrentMZSpectrum--;
            UpdatePanels();
        }

        private void OnNextScan(object sender, EventArgs e)
        {
            _model.CurrentMZSpectrum++;
            UpdatePanels();
        }

        private void OnNavigateScan(object sender, EventArgs e)
        {
            _model.CurrentMZSpectrum = _view.ScansView.SelectedIndex + 1;
            UpdatePanels();
        }

        private void UpdatePanels()
        {
            UpdateChart();
            UpdateChromatogram2D();
        }
    }
}
