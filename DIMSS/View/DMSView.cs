using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace DIMSS.View
{
    /// <summary>
    /// Main form (DMS visualizer) class
    /// </summary>
    public partial class DMSView : Form, IDMSView
    {
        public System.Windows.Forms.ListView CompoundsListView
        {
            get { return compoundsListView;  }
            set { compoundsListView = value; }
        }

        public System.Windows.Forms.DataVisualization.Charting.Chart ChartDIMS
        {
            get { return chartDIMS; }
            set { chartDIMS = value; }
        }

        public string TotalFilesCountText
        {
            get { return labelTotalFiles.Text; }
            set { labelTotalFiles.Text = value; }
        }

        /// <summary>
        /// Currently selected style of the main chart. Options:
        /// 1.Line plot
        /// 2.Column plot
        /// 3.Point plot
        /// </summary>
        public SeriesChartType CurrentChartType { get; set; }

        public event EventHandler<EventArgs> FilesLoaded;
        public event EventHandler<ItemCheckedEventArgs> CompoundChecked;
        public event EventHandler<EventArgs> AllCleared;
        public event EventHandler<EventArgs> MzXmlOpened;
        

        public DMSView()
        {
            InitializeComponent();
        }
        
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FilesLoaded != null)
                FilesLoaded(sender, e);
        }

        private void openMzXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MzXmlOpened != null)
                MzXmlOpened(sender, e);
        }
        
        private void compoundsListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (CompoundChecked != null)
                CompoundChecked(sender, e);
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            if (AllCleared != null)
                AllCleared(sender, e);
        }

        #region data chart and radio buttons to switch between the view modes (not in presenter)

        private void ChangeChartStyle(SeriesChartType style)
        {
            foreach (var ser in chartDIMS.Series)
            {
                ser.ChartType = style;
            }

            CurrentChartType = style;
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

        private void Details_Click(object sender, EventArgs e)
        {
            MessageBox.Show( "In progress..." );
        }
        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
