using DIMSS.Model;
using DIMSS.View;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DIMSS.Presenter
{
    class ChromatogramPresenter
    {
        private readonly ChromatogramModel model = new ChromatogramModel();
        private readonly IChromatogramView view;

        public ChromatogramPresenter(IChromatogramView vw)
        {
            view = vw;
            view.ViewLoaded += OnLoad;
            view.ViewClosed += OnClose;
            view.PreviousScan += OnPreviousScan;
            view.NextScan += OnNextScan;
            view.NavigateScan += OnNavigateScan;
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
        public bool LoadMzXmlFile( string mzxmlFilename )
        {
            string parseResult = model.OpenParser(mzxmlFilename);

            if (parseResult != ChromatogramModel.LOAD_RESULT_OK)
            {
                MessageBox.Show(parseResult);
                return false;
            }

            if (model.ScanCount() <= 0)
            {
                MessageBox.Show("No spectral information in given file!");
                return false;
            }

            // retrieve the number of scans and fill a combobox with corresponding range of values for navigation
            for (int i = 1; i < model.ScanCount(); i++)
            {
                view.ScansView.Items.Add(i.ToString());
            }

            view.ScansView.SelectedIndex = 0;
            
            return true;
        }

        /// <summary>
        /// Optional procedure - map the value onto RGB-scale for plotting;
        /// value is given between 0 and 1 (percent) 
        /// </summary>
        /// <param name="value">Pixel intensity percent (value between 0 and 1)</param>
        /// <returns>Color of the pixel on a plot</returns>
        private Color GetRGBColor(double value)
        {
            // COLORMAP #1
            // int intvalue = (int)(value * 0xFFFFFF);
            // return Color.FromArgb((intvalue & 0xFF0000) >> 16, (intvalue & 0xFF00) >> 8, 0xFF);

            // COLORMAP #2
            byte r = 0, g = 0, b = 0;
            if (value < 1 / 6f)
            {
                r = 255;
                g = (byte)(r * (value - 0) / (2 / 6f - value));
            }
            else if (value < 2 / 6f)
            {
                g = 255;
                r = (byte)(g * (2 / 6f - value) / (value - 0));
            }
            else if (value < 3 / 6f)
            {
                g = 255;
                b = (byte)(g * (2 / 6f - value) / (value - 4 / 6f));
            }
            else if (value < 4 / 6f)
            {
                b = 255;
                g = (byte)(b * (value - 4 / 6f) / (2 / 6f - value));
            }
            else if (value < 5 / 6f)
            {
                b = 255;
                r = (byte)(b * (4 / 6f - value) / (value - 1f));
            }
            else
            {
                r = 255;
                b = (byte)(r * (value - 1f) / (4 / 6f - value));
            }

            return Color.FromArgb(r, g, b);
        }


        private void ShowChromatogram2D()
        {
            int scanCount = model.ScanCount();

            int width = 700, height = 600;

            // create empty bitmap and fill it with black color
            Bitmap chromatogram2D = new Bitmap(width, height);//scanCount - 1);

            for (int i = 0; i < chromatogram2D.Width; i++)
            {
                for (int j = 0; j < chromatogram2D.Height; j++)
                {
                    chromatogram2D.SetPixel(i, j, Color.FromArgb(0,0,255) );
                }
            }

            // get all spectra and blit them onto the bitmap
            int step = scanCount / height;

            for (int scanNo = 1, vPos = 0; vPos < height; scanNo += step, vPos++)
            {
                MZSpectrum spectrum = model.GetMZSpectrumByIndex(scanNo);

                for (int i = 0; i < spectrum.PeakCount(); i++)
                {
                    double x = spectrum.MZList[i];
                    double y = spectrum.IntensityList[i];

                    chromatogram2D.SetPixel((int)x, vPos, GetRGBColor(y/1000));
                }
            }

            // show current spectrum line on chromatogram
            MZSpectrum curSpectrum = model.GetMZSpectrumByIndex( model.CurrentMZSpectrum );

            for (int i = 0; i < width; i++)
            {
                chromatogram2D.SetPixel(i, model.CurrentMZSpectrum / step, Color.White);
            }

            // fit the entire chromatogram to the pictureBox area
            view.ChromatogramImage.Image = chromatogram2D;
        }

        private void UpdateChart()
        {
            MZSpectrum spectrum = model.GetCurrentMZSpectrum();

            // fill datagrid ...
            view.MZSpectraView.ColumnCount = spectrum.PeakCount() + 1;
            view.MZSpectraView.RowCount = 2;
            view.MZSpectraView.Rows[0].DefaultCellStyle.BackColor = Color.LightYellow;
            view.MZSpectraView.Rows[0].Cells[0].Value = String.Format("m/z peaks [ {0} ]", spectrum.PeakCount());
            view.MZSpectraView.Rows[1].Cells[0].Value = "intensity";

            // ... and data chart
            Series mzSpectrum = new Series();

            for (int i = 0; i < spectrum.PeakCount(); i++)
            {
                mzSpectrum.Points.AddXY(spectrum.MZList[i], spectrum.IntensityList[i]);

                view.MZSpectraView.Rows[0].Cells[i + 1].Value = spectrum.MZList[i].ToString();
                view.MZSpectraView.Rows[1].Cells[i + 1].Value = spectrum.IntensityList[i].ToString();
            }

            view.MZSpectraView.AutoResizeColumns();

            view.ChromatogramChart.Series.Clear();
            view.ChromatogramChart.Series.Add(mzSpectrum);

            view.ScansView.SelectedIndex = model.CurrentMZSpectrum - 1;

            ShowChromatogram2D();
        }
        
        private void OnClose(object sender, EventArgs e)
        {
            model.CloseParser();
            view.ViewLoaded -= OnLoad;
            view.ViewClosed -= OnClose;
            view.PreviousScan -= OnPreviousScan;
            view.NextScan -= OnNextScan;
            view.NavigateScan -= OnNavigateScan;
        }

        private void OnPreviousScan(object sender, EventArgs e)
        {
            model.CurrentMZSpectrum--;
            UpdateChart();
        }

        private void OnNextScan(object sender, EventArgs e)
        {
            model.CurrentMZSpectrum++;
            UpdateChart();
        }

        private void OnNavigateScan(object sender, EventArgs e)
        {
            model.CurrentMZSpectrum = view.ScansView.SelectedIndex + 1;
            UpdateChart();
        }
    }
}
