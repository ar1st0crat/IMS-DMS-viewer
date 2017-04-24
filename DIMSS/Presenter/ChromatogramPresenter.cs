using DIMSS.Model;
using DIMSS.View;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SciColorMaps;

namespace DIMSS.Presenter
{
    class ChromatogramPresenter
    {
        private readonly ChromatogramModel _model = new ChromatogramModel();
        private readonly IChromatogramView _view;

        /// <summary>
        /// Main colormap used for chromatogram display
        /// </summary>
        private readonly ColorMap colorMap = new ColorMap("afmhot");

        public ChromatogramPresenter(IChromatogramView view)
        {
            _view = view;
            _view.ViewLoaded += OnLoad;
            _view.PreviousScan += OnPreviousScan;
            _view.NextScan += OnNextScan;
            _view.NavigateScan += OnNavigateScan;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            UpdateChart();
            ShowChromatogram2D();
        }
        
        /// <summary>
        /// Method tries to load and parse mzXml file. In case of failure returns false.
        /// </summary>
        /// <param name="mzxmlFilename">The full name of mzXml file to parse</param>
        /// <returns>true if mzXml was loaded and parsed succesfully; false - otherwise</returns>
        public bool LoadMzXmlFile(string mzxmlFilename)
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

            // retrieve the number of scans and fill a combobox with corresponding range of values for navigation
            for (int i = 1; i < _model.ScanCount; i++)
            {
                _view.ScansView.Items.Add(i.ToString());
            }

            _view.ScansView.SelectedIndex = 0;
            
            return true;
        }
        
        private void ShowChromatogram2D()
        {
            int scanCount = _model.ScanCount;

            int width = 700, height = 600;

            // create empty bitmap and fill it with black color
            Bitmap chromatogram2D = new Bitmap(width, height);

            for (int i = 0; i < chromatogram2D.Width; i++)
            {
                for (int j = 0; j < chromatogram2D.Height; j++)
                {
                    chromatogram2D.SetPixel(i, j, colorMap.GetColorByNumber(0));
                }
            }

            // get all spectra and blit them onto the bitmap
            int step = scanCount / height;

            for (int scanNo = 1, vPos = 0; vPos < height; scanNo += step, vPos++)
            {
                MZSpectrum spectrum = _model.GetMZSpectrumByIndex(scanNo);
                if (spectrum == null)
                {
                    // unlikely, but who knows...
                    return;
                }

                for (int i = 0; i < spectrum.PeakCount; i++)
                {
                    double x = spectrum.MZList[i];
                    double y = spectrum.IntensityList[i];

                    chromatogram2D.SetPixel((int)x, vPos, colorMap.GetColor((float)y / 500));
                }
            }

            // show current spectrum line on chromatogram
            MZSpectrum curSpectrum = _model.GetMZSpectrumByIndex(_model.CurrentMZSpectrum);

            for (int i = 0; i < width; i++)
            {
                chromatogram2D.SetPixel(i, _model.CurrentMZSpectrum / step, Color.White);
            }

            // fit the entire chromatogram to the pictureBox area
            _view.ChromatogramImage.Image = chromatogram2D;
        }

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
            _view.MZSpectraView.Rows[0].Cells[0].Value = String.Format("m/z peaks [ {0} ]", spectrum.PeakCount);
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

            ShowChromatogram2D();
        }
        
        private void OnPreviousScan(object sender, EventArgs e)
        {
            _model.CurrentMZSpectrum--;
            UpdateChart();
        }

        private void OnNextScan(object sender, EventArgs e)
        {
            _model.CurrentMZSpectrum++;
            UpdateChart();
        }

        private void OnNavigateScan(object sender, EventArgs e)
        {
            _model.CurrentMZSpectrum = _view.ScansView.SelectedIndex + 1;
            UpdateChart();
        }
    }
}
