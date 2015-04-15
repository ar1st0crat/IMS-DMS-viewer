using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;


namespace DIMSS
{
    /*
     * The CSV file structure should be as follows:
     *          
     *          1) MetaInfo; spec1; spec2; spec3; spec4; ...;
     *          
     *          or
     * 
     *          2) MetaInfo;; spec1; x1; spec2; x2; spec3; x3; ...;
     *          
     * 
     *  This version of DIMSS deals with constant spectrum dimension which we define in SPECSIZE
     */
    
    public partial class MainForm : Form
    {
        private const int SPECSIZE = 2048;                                  // by default the size of a spectrum is 2^11 = 2048 samples

        private List<string> measure_params = new List<string>();           // here we store measurement parameters

        private List<List<int>> spectra = new List<List<int>>();                // here we store all our spectra (vector of vectors)

        private List<List<float>> spectral_points = new List<List<float>>();    // here we store x-coordinates of spectral points (in some cases they're not given)

        private List<int> checkedList = new List<int>();                    // here we store the indices of spectra specified by user in checkboxes

        private SeriesChartType curChartType = SeriesChartType.Column;      // and here we have the current style of the main chart 
                                                                            // (Options: 1.Line plot;  2.Column plot;  3.Point plot)

        public MainForm()
        {
            InitializeComponent();
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        // fix the dims spectrum: invert the sign of corrupted samples
        private void FixSpectrum( List<int> spectrum )
        {
            for (int i = 1; i < spectrum.Count; i++)
            {
                if ((spectrum[i] == -32768) && (spectrum[i-1] > 0))
                {
                    spectrum[i] = 32767;
                }
            }
        }


        // iterate across the files in a given directory
        private void FileWalk(string dir)
        {
            // FILTER CSV: 
            // select only csv files in the directory chosen by user
            var files = Directory.EnumerateFiles(dir).Where(t => t.EndsWith("csv"));

            // ITERATE:
            // checking each file in the "dir" directory
            foreach (string file in files)
            {
                string fileDescription = String.Format("{0} [ {1} ]", Path.GetFileNameWithoutExtension(file), dir.Remove(0, dir.LastIndexOf('\\') + 1));

                // add new item to the main listview
                ListViewItem item = this.compoundsListView.Items.Add(fileDescription);

                // simple parsing of the csv file
                var csv_samples = File.ReadAllText(file).Split(';');

                // add info about the measurements parameters (it is stored in the first row of the csv file)
                measure_params.Add( csv_samples.ElementAt(0) );

                // add this info to the listview
                item.SubItems.Add(csv_samples.ElementAt(0));

                // create new lists for new spectrum
                List<int> spec = new List<int>();
                List<float> spec_points = new List<float>();

                // if there is only one column of data in csv file (i.e. the second element isn't empty)
                if (csv_samples.ElementAt(1) != "")
                {
                    // fill current spectrum except x-axis values
                    for (int j = 0; j < SPECSIZE; j++)
                    {
                        spec.Add( Int32.Parse(csv_samples.ElementAt(j + 1)) );
                        spec_points.Add( int.MaxValue );
                    }
                }
                // otherwise include the x-axis values into spectral information
                else
                {
                    // fill current spectrum
                    for (int j = 0; j < SPECSIZE; j++)
                    {
                        spec.Add( Int32.Parse(csv_samples.ElementAt(j * 2 + 2)) );
                        spec_points.Add( float.Parse(csv_samples.ElementAt(j * 2 + 3),
                                                            System.Globalization.CultureInfo.InvariantCulture) );
                    }
                }

                // fix
                FixSpectrum(spec);

                // add spectrum to the list of spectra
                spectra.Add( spec );
                spectral_points.Add( spec_points );
            }
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // get all child directories of the directory specified by user
            var dirs = Directory.EnumerateDirectories(fbd.SelectedPath);

            // firstly, iterate through files in chosen directory
            FileWalk(fbd.SelectedPath);

            // secondly, read data from all files in each child directory
            foreach (string dir in dirs)
            {
                FileWalk(dir);
            }

            this.labelTotalFiles.Text = String.Format("Total: {0} files", spectra.Count);
        }


        private void compoundsListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            int nSerie = e.Item.Index;

            if (e.Item.Checked)
            {
                Series s = new Series(e.Item.Text);
                s.ChartType = curChartType;

                try
                {
                    chartDIMS.Series.Add(s);
                }
                catch ( ArgumentException argEx )
                {
                    MessageBox.Show( argEx.Message );
                    e.Item.Checked = false;
                    return;
                }

                checkedList.Add( nSerie );

                // copy spectrum to the newly created serie of a .NET Chart Control

                // ...and annoying check for format of initial csv file (if it was given without x coordinates then we extract info from measurement parameters)
                if (spectral_points[nSerie][0] == int.MaxValue)
                {
                    // we have only y coordinates
                    // however we can approximately evaluate x coordinates based on the info we have in measure_params, namely: (from,V) and (To,V)

                    // parse measure_params using RegExp
                    var col = System.Text.RegularExpressions.Regex.Matches(measure_params[nSerie], @"(?<key>\s*\w+[,.]*\w+\s*)=(?<val>\s*\d*[,.]?\d+\s*)");

                    float fromV = 0.0f;
                    float toV = 0.0f;

                    foreach (System.Text.RegularExpressions.Match m in col)
                    {
                        if (m.Groups["key"].Value.Contains("From,V"))
                        {
                            fromV = float.Parse(m.Groups["val"].Value);
                        }

                        if (m.Groups["key"].Value.Contains("To,V"))
                        {
                            toV = float.Parse(m.Groups["val"].Value);
                        }
                    }

                    // ... and map x coordinates into range [ fromV, toV ]
                    for (int j = 0; j < SPECSIZE; j++)
                    {
                        chartDIMS.Series[checkedList.Count() - 1].Points.AddXY(fromV + j * (toV - fromV) / SPECSIZE, spectra[nSerie][j]);
                    }
                }

                // if we have both x and y coordinates then everything is pretty simple
                else
                {
                    for (int j = 0; j < SPECSIZE; j++)
                    {
                        chartDIMS.Series[checkedList.Count() - 1].Points.AddXY(spectral_points[nSerie][j], spectra[nSerie][j]);
                    }
                }
            }
            else
            {
                int checkedPos = checkedList.IndexOf(nSerie);

                if (checkedPos == -1)
                {
                    return;
                }

                chartDIMS.Series.RemoveAt(checkedPos);
                checkedList.RemoveAt(checkedPos);
            }

            chartDIMS.Update();
        }


        private void ChangeChartStyle(SeriesChartType style)
        {
            foreach (var ser in chartDIMS.Series)
            {
                ser.ChartType = style;
            }

            curChartType = style;
        }

        private void radioButtonLinePlot_CheckedChanged(object sender, EventArgs e)
        {
            ChangeChartStyle(SeriesChartType.Line);
        }

        private void radioButtonColumnPlot_CheckedChanged(object sender, EventArgs e)
        {
            ChangeChartStyle(SeriesChartType.Column);
        }

        private void radioButtonPointPlot_CheckedChanged(object sender, EventArgs e)
        {
            ChangeChartStyle(SeriesChartType.Point);
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem checkedItem in compoundsListView.CheckedItems)
            {
                checkedItem.Checked = false;
            }

            chartDIMS.Series.Clear();
            checkedList.Clear();
        }


        private void Details_Click(object sender, EventArgs e)
        {
            MessageBox.Show( "In progress..." );
        }


        private void openMzXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            ChromatogramForm form = new ChromatogramForm();
            if (form.SetMzxmlFile(ofd.FileName))
            {
                form.ShowDialog();
            }
        }
    }
}
