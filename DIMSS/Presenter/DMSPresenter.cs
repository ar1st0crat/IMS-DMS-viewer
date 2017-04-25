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
        private readonly DMS _model = new DMS();
        private readonly IDMSView _view;

        /// <summary>
        /// The indices of spectra specified by user in checked listview
        /// </summary>
        private List<int> _checkedList = new List<int>();

        /// <summary>
        /// Setting up presenter:
        /// subscribing to view events and synchronizing with its properties
        /// </summary>
        /// <param name="view">Particular view implementing IDMSView interface</param>
        public DMSPresenter(IDMSView view)
        {
            _view = view;
            _view.FilesLoaded += OnLoadFiles;
            _view.CompoundChecked += OnCheckCompound;
            _view.AllCleared += OnClearAll;
            _view.MzXmlOpened += OnOpenMzXml;
            _view.CurrentChartType = SeriesChartType.Column;
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
            var files = _model.LoadFolderContent(fbd.SelectedPath);

            for (int i = 0; i < files[0].Count; i++)
            {
                // add the short file name to the first column of the listview
                ListViewItem item = _view.CompoundsListView.Items.Add(files[0][i]);
                // add the file info to the second column of the listview
                item.SubItems.Add(files[1][i]);
            }

            _view.TotalFilesCountText = string.Format("Total: {0} files", _model.Spectra.Count);
        }

        /// <summary>
        /// Show all info regarding the spectrum we've just checked in the checked listview
        /// </summary>
        private void OnCheckCompound(object sender, ItemCheckedEventArgs e)
        {
            int serieNo = e.Item.Index;

            if (e.Item.Checked)
            {
                var series = new Series(e.Item.Text);
                series.ChartType = _view.CurrentChartType;

                try
                {
                    _view.ChartDIMS.Series.Add(series);
                }
                catch (ArgumentException argEx)
                {
                    MessageBox.Show(argEx.Message);
                    e.Item.Checked = false;
                    return;
                }

                _checkedList.Add(serieNo);

                // copy spectrum to the newly created serie of a .NET Chart Control

                // ...and annoying check for format of initial csv file 
                // (if it was given without x coordinates then we extract info from measurement parameters)
                if (_model.SpectralPoints[serieNo][0] == int.MaxValue)
                {
                    // we have only y coordinates
                    // however we can approximately evaluate x coordinates based on the info we have in measureParams, 
                    // namely: (from,V) and (To,V)
                    float fromV = 0.0f;
                    float toV = 0.0f;

                    _model.ParseMeasureParams(serieNo, ref fromV, ref toV);

                    // ... and map x coordinates into range [ fromV, toV ]
                    for (int j = 0; j < DMS.SpecSize; j++)
                    {
                        _view.ChartDIMS.Series[_checkedList.Count - 1].Points.AddXY(
                            fromV + j * (toV - fromV) / DMS.SpecSize, _model.Spectra[serieNo][j]);
                    }
                }

                // if we have both x and y coordinates then everything is pretty simple
                else
                {
                    for (int j = 0; j < DMS.SpecSize; j++)
                    {
                        _view.ChartDIMS.Series[_checkedList.Count - 1].Points.AddXY(
                            _model.SpectralPoints[serieNo][j], _model.Spectra[serieNo][j]);
                    }
                }
            }
            else
            {
                int checkedPos = _checkedList.IndexOf(serieNo);

                if (checkedPos == -1)
                {
                    return;
                }

                _view.ChartDIMS.Series.RemoveAt(checkedPos);
                _checkedList.RemoveAt(checkedPos);
            }

            _view.ChartDIMS.Update();
        }

        /// <summary>
        /// Clear all selections in checked listview
        /// </summary>
        private void OnClearAll(object sender, EventArgs e)
        {
            foreach (ListViewItem checkedItem in _view.CompoundsListView.CheckedItems)
            {
                checkedItem.Checked = false;
            }

            _view.ChartDIMS.Series.Clear();
            _checkedList.Clear();
        }

        /// <summary>
        /// Choose and load MzXml chromatogram
        /// </summary>
        private void OnOpenMzXml(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var chromatogramView = new ChromatogramView();
            var chromatogramPresenter = new ChromatogramPresenter(chromatogramView);
            if (chromatogramPresenter.LoadMzXmlFile(ofd.FileName))
            {
                chromatogramView.ShowDialog();
            }
        }
    }
}
