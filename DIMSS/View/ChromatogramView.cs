using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DIMSS.View
{
    public partial class ChromatogramView : Form, IChromatogramView
    {
        #region IChromatogramView interface implementation

        public ComboBox ScansView
        {
            get { return comboBoxScans; }
            set { comboBoxScans = value; }
        }

        public DataGridView MZSpectraView
        {
            get { return dataGridViewMZSpectra; }
            set { dataGridViewMZSpectra = value; }
        }

        public Chart ChromatogramChart
        {
            get { return chartChromatogram; }
            set { chartChromatogram = value; }
        }

        public PictureBox ChromatogramImage
        {
            get { return pictureBoxChromatogram; }
            set { pictureBoxChromatogram = value; }
        }

        public event EventHandler<EventArgs> ViewLoaded;
        public event EventHandler<EventArgs> ViewClosed;
        public event EventHandler<EventArgs> PreviousScan;
        public event EventHandler<EventArgs> NextScan;
        public event EventHandler<EventArgs> NavigateScan;

        #endregion

        public ChromatogramView()
        {
            InitializeComponent();
        }

        private void ChromatogramForm_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (ViewLoaded != null)
                ViewLoaded(sender, e);

            Cursor = Cursors.Arrow;
        }

        private void ChromatogramForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed(sender, e);
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (PreviousScan != null)
                PreviousScan(sender, e);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (NextScan != null)
                NextScan(sender, e);
        }

        private void buttonGoTo_Click(object sender, EventArgs e)
        {
            if (NavigateScan != null)
                NavigateScan(sender, e);
        }
    }
}