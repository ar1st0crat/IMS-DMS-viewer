using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DIMSS.View
{
    interface IChromatogramView
    {
        ComboBox ScansView { get; set; }
        DataGridView MZSpectraView { get; set; }
        Chart ChromatogramChart { get; set; }
        PictureBox ChromatogramImage { get; set; }
        
        event EventHandler<EventArgs> ViewLoaded;
        event EventHandler<EventArgs> ViewClosed;
        event EventHandler<EventArgs> PreviousScan;
        event EventHandler<EventArgs> NextScan;
        event EventHandler<EventArgs> NavigateScan;
    }
}
