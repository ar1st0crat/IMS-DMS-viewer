using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DIMSS.View
{
    interface IDMSView
    {
        ListView CompoundsListView { get; set; }
        Chart ChartDIMS { get; set; }
        string TotalFilesCountText { get; set; }
        SeriesChartType CurrentChartType { get; set; }
        
        event EventHandler<EventArgs> FilesLoaded;
        event EventHandler<ItemCheckedEventArgs> CompoundChecked;
        event EventHandler<EventArgs> AllCleared;
        event EventHandler<EventArgs> MzXmlOpened;
    }
}
