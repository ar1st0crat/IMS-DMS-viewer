using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace DIMSS
{
    public partial class ChromatogramForm : Form
    {
        // TODO: refactor. This component is no longer needed (seems like it can't decompress mz-spectral data).
        // We can use simple XmlReader / XmlDocument instead. However, currently we use some of MSDataFileReader.clsMzXMLFileAccessor features
        private MSDataFileReader.clsMzXMLFileAccessor mzXMLParser = new MSDataFileReader.clsMzXMLFileAccessor();

        // we start with the spectrum #1. The scan #0 contains metainfo
        private int curMzSpectrum = 1;


        /// <summary>
        /// helping function : decompress a byte array compressed with zlib (mzXML 3.0 format!) 
        /// </summary>
        /// <param name="source">A byte array compressed with zlib</param>
        /// <returns>Decompressed byte array</returns>
        public static byte[] DecompressZlib(Stream source)
        {
            byte[] result = null;
            
            using (MemoryStream outStream = new MemoryStream())
            {
                using (InflaterInputStream inf = new InflaterInputStream(source))
                {
                    inf.CopyTo( outStream );
                }
                result = outStream.ToArray();
            }

            return result;
        }


        public ChromatogramForm()
        {
            InitializeComponent();
        }


        public bool SetMzxmlFile( string mzxmlFilename )
        {
            if (!mzXMLParser.OpenFile( mzxmlFilename) )
            {
                MessageBox.Show( mzXMLParser.ErrorMessage );
                return false;
            }

            // retrieve the number of scans and fill a combobox with corresponding range of values for navigation
            for (int i = 1; i < mzXMLParser.ScanCount; i++)
            {
                this.comboBoxScans.Items.Add( i.ToString() );
            }

            this.comboBoxScans.SelectedIndex = 0;

            return true;
        }


        /// <summary>
        /// parse the region of an mzxml file corresponding to the spectrum with scanId = <paramref name="idx"/> 
        /// </summary>
        /// <param name="idx">ScanID (scan index)</param>
        /// <returns>MZ spectrum by the specified ScanId</returns>
        private MZSpectrum GetMZSpectrumByIndex( int idx )
        {
            MZSpectrum spec = new MZSpectrum();

            string xmlString = "";

            mzXMLParser.GetSourceXMLByScanNumber(idx, ref xmlString);

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(xmlString);

            // find the encoded and compressed binary data in xml file: the <peaks> tag
            string peaks = doc.SelectSingleNode(@"//peaks").InnerText;

            // Firstly, decode byte array from base64 string
            byte[] byteArray = System.Convert.FromBase64String(peaks);

            // Secondly, decompress the decoded byte array using the SharpZipLib dll
            Stream stream = new MemoryStream(byteArray);

            byte[] decodedBytes = DecompressZlib(stream);

            // allocate memory for lists of m-z peaks positions and intensities
            spec.MZList = new double[ decodedBytes.Count() / 16 ];
            spec.intensityList = new double[ decodedBytes.Count() / 16 ];

            // each value is stored as 'double' and little-endian
            // hence we allocate 8 bytes for each value
            byte[] mzPeak = new byte[8];
            byte[] intensity = new byte[8];

            for (int i = 0, specPos = 0; i < decodedBytes.Count(); i += 16, specPos++)
            {
                // ... and rewrite bytes in reverse order
                for (int j = 0; j < 8; j++)
                {
                    mzPeak[j] = decodedBytes[7 + i - j];
                    intensity[j] = decodedBytes[15 + i - j];
                }

                // add new peak to spectrum info
                spec.MZList[ specPos ] = System.BitConverter.ToDouble(mzPeak, 0);
                spec.intensityList[ specPos ] = System.BitConverter.ToDouble(intensity, 0);
            }

            return spec;
        }


        // optional procedure - map the value onto RGB-scale for plotting;
        // value is given between 0 and 1 (percent)   
        private Color GetRGBColor( double value )
        {
            int intvalue = (int)( value * 0xFFFFFF );

            return Color.FromArgb( (intvalue & 0xFF0000) >> 16, (intvalue & 0xFF00) >> 8, intvalue & 0xFF);
        }

        
        private void ShowChromatogram2D()
        {
            int scanCount = mzXMLParser.ScanCount;

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
                MZSpectrum spectrum = GetMZSpectrumByIndex( scanNo );

                for (int i = 0; i < spectrum.PeakCount(); i++)
                {
                    double x = spectrum.MZList[i];
                    double y = spectrum.intensityList[i];

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
            MZSpectrum spectrum = GetMZSpectrumByIndex(curMzSpectrum);
            
            Series mzSpectrum = new Series();

            this.dataGridViewMZSpectra.ColumnCount = spectrum.PeakCount() + 1;
            this.dataGridViewMZSpectra.RowCount = 2;
            this.dataGridViewMZSpectra.Rows[0].DefaultCellStyle.BackColor = Color.LightYellow;
            this.dataGridViewMZSpectra.Rows[0].Cells[0].Value = String.Format("m/z peaks [ {0} ]", spectrum.PeakCount() );
            this.dataGridViewMZSpectra.Rows[1].Cells[0].Value = "intensity";
            
            for (int i = 0; i < spectrum.PeakCount(); i++)
            {
                mzSpectrum.Points.AddXY( spectrum.MZList[i], spectrum.intensityList[i] );

                this.dataGridViewMZSpectra.Rows[0].Cells[i + 1].Value = spectrum.MZList[i].ToString();
                this.dataGridViewMZSpectra.Rows[1].Cells[i + 1].Value = spectrum.intensityList[i].ToString();
            }

            this.dataGridViewMZSpectra.AutoResizeColumns();

            chartChromatogram.Series.Clear();
            chartChromatogram.Series.Add(mzSpectrum);

            this.comboBoxScans.SelectedIndex  = curMzSpectrum - 1;
        }


        private void buttonPrev_Click(object sender, EventArgs e)
        {
            curMzSpectrum--;
            UpdateChart();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            curMzSpectrum++;
            UpdateChart();
        }

        private void ChromatogramForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mzXMLParser.CloseFile();
        }

        private void ChromatogramForm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            UpdateChart();

            ShowChromatogram2D();

            this.Cursor = Cursors.Arrow;
        }

        private void buttonGoTo_Click(object sender, EventArgs e)
        {
            curMzSpectrum = this.comboBoxScans.SelectedIndex + 1;
            UpdateChart();
        }
    }
}