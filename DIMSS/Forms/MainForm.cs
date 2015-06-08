using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace DIMSS
{
    /// <summary>
    /// Main form (DMS visualizer) class
    /// </summary>
    public partial class MainForm : Form
    {
        private DMSModel model = new DMSModel();

        // here we store the indices of spectra specified by user in checkboxes
        private List<int> checkedList = new List<int>();

        // and here we have the current style of the main chart 
        // (Options: 1.Line plot;  2.Column plot;  3.Point plot)
        private SeriesChartType curChartType = SeriesChartType.Column;      


        public MainForm()
        {
            InitializeComponent();
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // iterate through the selected folder and its subfolders and load all DMS contents found in csv files 
            var files = model.LoadFolderContent( fbd.SelectedPath );

            for (int i = 0; i < files[0].Count; i++ )
            {
                // add the short file name to the first column of the listview
                ListViewItem item = this.compoundsListView.Items.Add( files[0][i] );

                // add the file info to the second column of the listview
                item.SubItems.Add( files[1][i] );
            }

            this.labelTotalFiles.Text = String.Format( "Total: {0} files", model.Spectra.Count );
        }


        /// <summary>
        /// Show all info regarding the spectrum we've just checked in the checked listbox
        /// </summary>
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
                if ( model.SpectralPoints[nSerie][0] == int.MaxValue )
                {
                    // we have only y coordinates
                    // however we can approximately evaluate x coordinates based on the info we have in measureParams, namely: (from,V) and (To,V)
                    float fromV = 0.0f;
                    float toV = 0.0f;

                    model.ParseMeasureParams( nSerie, ref fromV, ref toV );

                    // ... and map x coordinates into range [ fromV, toV ]
                    for (int j = 0; j < DMSModel.SPECSIZE; j++)
                    {
                        chartDIMS.Series[checkedList.Count - 1].Points.AddXY(
                            fromV + j * (toV - fromV) / DMSModel.SPECSIZE, model.Spectra[nSerie][j] );
                    }
                }

                // if we have both x and y coordinates then everything is pretty simple
                else
                {
                    for (int j = 0; j < DMSModel.SPECSIZE; j++)
                    {
                        chartDIMS.Series[checkedList.Count - 1].Points.AddXY( model.SpectralPoints[nSerie][j], model.Spectra[nSerie][j] );
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


        #region data chart and radio buttons to switch between the view modes

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

        #endregion


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
            if ( form.SetMzxmlFile(ofd.FileName) )
            {
                form.ShowDialog();
            }
        }
    }
}
