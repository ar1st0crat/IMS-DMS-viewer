using DIMSS.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace DIMSS.View
{
    public partial class ChromatogramView : Form
    {
        ChromatogramModel model = new ChromatogramModel();

        public ChromatogramView()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Single-threaded loading. Just wait until entire file is loaded and chart is updated
        /// </summary>
        private void ChromatogramForm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            UpdateChart();
            ShowChromatogram2D();
            this.Cursor = Cursors.Arrow;
        }

        private void ChromatogramForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            model.CloseParser();
        }


        /// <summary>
        /// Method tries to load and parse mzXml file. In case of failure returns false.
        /// </summary>
        /// <param name="mzxmlFilename">The full name of mzXml file to parse</param>
        /// <returns>true if mzXml was loaded and parsed succesfully; false - otherwise</returns>
        public bool SetMzxmlFile( string mzxmlFilename )
        {
            string parseResult = model.OpenParser(mzxmlFilename);

            if ( parseResult != ChromatogramModel.LOAD_RESULT_OK )
            {
                MessageBox.Show( parseResult );
                return false;
            }

            if ( model.ScanCount() <= 0 )
            {
                MessageBox.Show( "No spectral information in given file!" );
                return false;
            }

            // retrieve the number of scans and fill a combobox with corresponding range of values for navigation
            for (int i = 1; i < model.ScanCount(); i++)
            {
                this.comboBoxScans.Items.Add( i.ToString() );
            }

            this.comboBoxScans.SelectedIndex = 0;

            return true;
        }


        /// <summary>
        /// Optional procedure - map the value onto RGB-scale for plotting;
        /// value is given between 0 and 1 (percent) 
        /// </summary>
        /// <param name="value">Pixel intensity percent (value between 0 and 1)</param>
        /// <returns>Color of the pixel on a plot</returns>
        private Color GetRGBColor( double value )
        {
            int intvalue = (int)( value * 0xFFFFFF );

            return Color.FromArgb( (intvalue & 0xFF0000) >> 16, (intvalue & 0xFF00) >> 8, intvalue & 0xFF);
        }


        private void ShowChromatogram2D()
        {
            int scanCount = model.ScanCount();

            // create empty bitmap and fill it with black color
            Bitmap chromatogram2D = new Bitmap(700, scanCount - 1);
            for (int i = 0; i < chromatogram2D.Width; i++)
            {
                for (int j = 0; j < chromatogram2D.Height; j++)
                {
                    chromatogram2D.SetPixel(i, j, Color.Black);
                }
            }

            // get all spectra and blit them onto the bitmap
            for (int scanNo = 1; scanNo < scanCount; scanNo++)
            {
                MZSpectrum spectrum = model.GetMZSpectrumByIndex( scanNo );

                for (int i = 0; i < spectrum.PeakCount(); i++)
                {
                    double x = spectrum.MZList[i];
                    double y = spectrum.IntensityList[i];

                    int redLevel = (int)(y * 1.5);
                    if (redLevel > 255)
                    {
                        redLevel = 255;
                    }

                    chromatogram2D.SetPixel( (int)x, scanNo - 1, GetRGBColor(y / 7500) );//Color.FromArgb( redLevel, 0, 0));
                }
            }

            // fit the entire chromatogram to the pictureBox area
            this.pictureBoxChromatogram.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxChromatogram.Image = chromatogram2D;
        }


        private void UpdateChart()
        { 
            MZSpectrum spectrum = model.GetCurrentMZSpectrum();
            
            // fill datagrid ...
            this.dataGridViewMZSpectra.ColumnCount = spectrum.PeakCount() + 1;
            this.dataGridViewMZSpectra.RowCount = 2;
            this.dataGridViewMZSpectra.Rows[0].DefaultCellStyle.BackColor = Color.LightYellow;
            this.dataGridViewMZSpectra.Rows[0].Cells[0].Value = String.Format("m/z peaks [ {0} ]", spectrum.PeakCount() );
            this.dataGridViewMZSpectra.Rows[1].Cells[0].Value = "intensity";

            // ... and data chart
            Series mzSpectrum = new Series();

            for (int i = 0; i < spectrum.PeakCount(); i++)
            {
                mzSpectrum.Points.AddXY( spectrum.MZList[i], spectrum.IntensityList[i] );

                this.dataGridViewMZSpectra.Rows[0].Cells[i + 1].Value = spectrum.MZList[i].ToString();
                this.dataGridViewMZSpectra.Rows[1].Cells[i + 1].Value = spectrum.IntensityList[i].ToString();
            }

            this.dataGridViewMZSpectra.AutoResizeColumns();

            chartChromatogram.Series.Clear();
            chartChromatogram.Series.Add(mzSpectrum);

            this.comboBoxScans.SelectedIndex  = model.CurrentMZSpectrum - 1;
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            model.CurrentMZSpectrum--;
            UpdateChart();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            model.CurrentMZSpectrum++;
            UpdateChart();
        }

        private void buttonGoTo_Click(object sender, EventArgs e)
        {
            model.CurrentMZSpectrum = this.comboBoxScans.SelectedIndex + 1;
            UpdateChart();
        }
    }
}