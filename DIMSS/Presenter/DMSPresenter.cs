using DIMSS.Model;
using DIMSS.View;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DIMSS.Presenter
{
    class DMSPresenter
    {
        private readonly DMSModel model = new DMSModel();
        private readonly IDMSView view;

        // The indices of spectra specified by user in checked listview
        private List<int> checkedList = new List<int>();

        /// <summary>
        /// Setting up presenter:
        /// subscribing to view events and synchronizing with its properties
        /// </summary>
        /// <param name="vw">Particular view implementing IDMSView interface</param>
        public DMSPresenter(IDMSView vw)
        {
            view = vw;
            view.FilesLoaded += OnLoadFiles;
            view.CompoundChecked += OnCheckCompound;
            view.AllCleared += OnClearAll;
            view.MzXmlOpened += OnOpenMzXml;
            view.CurrentChartType = SeriesChartType.Column;
        }

        /// <summary>
        /// Open csv files with DMS spectra in folder specified by user
        /// </summary>
        private void OnLoadFiles(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // iterate through the selected folder and its subfolders and load all DMS contents found in csv files 
            var files = model.LoadFolderContent(fbd.SelectedPath);

            for (int i = 0; i < files[0].Count; i++)
            {
                // add the short file name to the first column of the listview
                ListViewItem item = view.CompoundsListView.Items.Add(files[0][i]);

                // add the file info to the second column of the listview
                item.SubItems.Add(files[1][i]);
            }

            view.TotalFilesCountText = String.Format("Total: {0} files", model.Spectra.Count);
        }

        /// <summary>
        /// Show all info regarding the spectrum we've just checked in the checked listview
        /// </summary>
        private void OnCheckCompound(object sender, ItemCheckedEventArgs e)
        {
            int nSerie = e.Item.Index;

            if (e.Item.Checked)
            {
                Series s = new Series(e.Item.Text);
                s.ChartType = view.CurrentChartType;

                try
                {
                    view.ChartDIMS.Series.Add(s);
                }
                catch (ArgumentException argEx)
                {
                    MessageBox.Show(argEx.Message);
                    e.Item.Checked = false;
                    return;
                }

                checkedList.Add(nSerie);

                // copy spectrum to the newly created serie of a .NET Chart Control

                // ...and annoying check for format of initial csv file (if it was given without x coordinates then we extract info from measurement parameters)
                if (model.SpectralPoints[nSerie][0] == int.MaxValue)
                {
                    // we have only y coordinates
                    // however we can approximately evaluate x coordinates based on the info we have in measureParams, namely: (from,V) and (To,V)
                    float fromV = 0.0f;
                    float toV = 0.0f;

                    model.ParseMeasureParams(nSerie, ref fromV, ref toV);

                    // ... and map x coordinates into range [ fromV, toV ]
                    for (int j = 0; j < DMSModel.SPECSIZE; j++)
                    {
                        view.ChartDIMS.Series[checkedList.Count - 1].Points.AddXY(
                            fromV + j * (toV - fromV) / DMSModel.SPECSIZE, model.Spectra[nSerie][j]);
                    }
                }

                // if we have both x and y coordinates then everything is pretty simple
                else
                {
                    for (int j = 0; j < DMSModel.SPECSIZE; j++)
                    {
                        view.ChartDIMS.Series[checkedList.Count - 1].Points.AddXY(
                            model.SpectralPoints[nSerie][j], model.Spectra[nSerie][j]);
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

                view.ChartDIMS.Series.RemoveAt(checkedPos);
                checkedList.RemoveAt(checkedPos);
            }

            view.ChartDIMS.Update();
        }

        /// <summary>
        /// Clear all selections in checked listview
        /// </summary>
        private void OnClearAll(object sender, EventArgs e)
        {
            foreach (ListViewItem checkedItem in view.CompoundsListView.CheckedItems)
            {
                checkedItem.Checked = false;
            }

            view.ChartDIMS.Series.Clear();
            checkedList.Clear();
        }

        /// <summary>
        /// Choose and load MzXml chromatogram
        /// </summary>
        private void OnOpenMzXml(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            ChromatogramView chromView = new ChromatogramView();
            if (chromView.SetMzxmlFile(ofd.FileName))
            {
                chromView.ShowDialog();
            }
        }
    }
}
