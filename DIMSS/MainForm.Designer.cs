namespace DIMSS
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMzXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compoundsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chartDIMS = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.radioButtonLinePlot = new System.Windows.Forms.RadioButton();
            this.radioButtonColumnPlot = new System.Windows.Forms.RadioButton();
            this.radioButtonPointPlot = new System.Windows.Forms.RadioButton();
            this.buttonClearAll = new System.Windows.Forms.Button();
            this.Details = new System.Windows.Forms.Button();
            this.labelTotalFiles = new System.Windows.Forms.Label();
            this.mainMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDIMS)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(1038, 24);
            this.mainMenuStrip.TabIndex = 1;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openMzXMLToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.openToolStripMenuItem.Text = "&Add Directory...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openMzXMLToolStripMenuItem
            // 
            this.openMzXMLToolStripMenuItem.Name = "openMzXMLToolStripMenuItem";
            this.openMzXMLToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.openMzXMLToolStripMenuItem.Text = "&Open mzXML...";
            this.openMzXMLToolStripMenuItem.Click += new System.EventHandler(this.openMzXMLToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // compoundsListView
            // 
            this.compoundsListView.CheckBoxes = true;
            this.compoundsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.compoundsListView.FullRowSelect = true;
            this.compoundsListView.Location = new System.Drawing.Point(12, 36);
            this.compoundsListView.Name = "compoundsListView";
            this.compoundsListView.Size = new System.Drawing.Size(846, 174);
            this.compoundsListView.TabIndex = 2;
            this.compoundsListView.UseCompatibleStateImageBehavior = false;
            this.compoundsListView.View = System.Windows.Forms.View.Details;
            this.compoundsListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.compoundsListView_ItemChecked);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Parameters";
            this.columnHeader3.Width = 620;
            // 
            // chartDIMS
            // 
            chartArea1.AxisX.Title = "VComp";
            chartArea1.AxisY.Title = "Intensity";
            chartArea1.Name = "ChartArea1";
            this.chartDIMS.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartDIMS.Legends.Add(legend1);
            this.chartDIMS.Location = new System.Drawing.Point(12, 231);
            this.chartDIMS.Name = "chartDIMS";
            this.chartDIMS.Size = new System.Drawing.Size(1014, 382);
            this.chartDIMS.TabIndex = 0;
            this.chartDIMS.Text = "chart1";
            // 
            // radioButtonLinePlot
            // 
            this.radioButtonLinePlot.AutoSize = true;
            this.radioButtonLinePlot.Location = new System.Drawing.Point(787, 622);
            this.radioButtonLinePlot.Name = "radioButtonLinePlot";
            this.radioButtonLinePlot.Size = new System.Drawing.Size(65, 17);
            this.radioButtonLinePlot.TabIndex = 4;
            this.radioButtonLinePlot.Text = "Line plot";
            this.radioButtonLinePlot.UseVisualStyleBackColor = true;
            this.radioButtonLinePlot.CheckedChanged += new System.EventHandler(this.radioButtonLinePlot_CheckedChanged);
            // 
            // radioButtonColumnPlot
            // 
            this.radioButtonColumnPlot.AutoSize = true;
            this.radioButtonColumnPlot.Checked = true;
            this.radioButtonColumnPlot.Location = new System.Drawing.Point(878, 622);
            this.radioButtonColumnPlot.Name = "radioButtonColumnPlot";
            this.radioButtonColumnPlot.Size = new System.Drawing.Size(61, 17);
            this.radioButtonColumnPlot.TabIndex = 5;
            this.radioButtonColumnPlot.TabStop = true;
            this.radioButtonColumnPlot.Text = "Bar plot";
            this.radioButtonColumnPlot.UseVisualStyleBackColor = true;
            this.radioButtonColumnPlot.CheckedChanged += new System.EventHandler(this.radioButtonColumnPlot_CheckedChanged);
            // 
            // radioButtonPointPlot
            // 
            this.radioButtonPointPlot.AutoSize = true;
            this.radioButtonPointPlot.Location = new System.Drawing.Point(957, 622);
            this.radioButtonPointPlot.Name = "radioButtonPointPlot";
            this.radioButtonPointPlot.Size = new System.Drawing.Size(69, 17);
            this.radioButtonPointPlot.TabIndex = 6;
            this.radioButtonPointPlot.Text = "Point plot";
            this.radioButtonPointPlot.UseVisualStyleBackColor = true;
            this.radioButtonPointPlot.CheckedChanged += new System.EventHandler(this.radioButtonPointPlot_CheckedChanged);
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Location = new System.Drawing.Point(878, 173);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(148, 36);
            this.buttonClearAll.TabIndex = 7;
            this.buttonClearAll.Text = "Clear All";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // Details
            // 
            this.Details.Location = new System.Drawing.Point(878, 131);
            this.Details.Name = "Details";
            this.Details.Size = new System.Drawing.Size(148, 36);
            this.Details.TabIndex = 8;
            this.Details.Text = "Details...";
            this.Details.UseVisualStyleBackColor = true;
            this.Details.Click += new System.EventHandler(this.Details_Click);
            // 
            // labelTotalFiles
            // 
            this.labelTotalFiles.AutoSize = true;
            this.labelTotalFiles.Location = new System.Drawing.Point(878, 36);
            this.labelTotalFiles.Name = "labelTotalFiles";
            this.labelTotalFiles.Size = new System.Drawing.Size(64, 13);
            this.labelTotalFiles.TabIndex = 9;
            this.labelTotalFiles.Text = "Total: 0 files";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 651);
            this.Controls.Add(this.labelTotalFiles);
            this.Controls.Add(this.Details);
            this.Controls.Add(this.buttonClearAll);
            this.Controls.Add(this.radioButtonPointPlot);
            this.Controls.Add(this.radioButtonColumnPlot);
            this.Controls.Add(this.radioButtonLinePlot);
            this.Controls.Add(this.compoundsListView);
            this.Controls.Add(this.chartDIMS);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.Text = "DIMS Statistics";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDIMS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ListView compoundsListView;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDIMS;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.RadioButton radioButtonLinePlot;
        private System.Windows.Forms.RadioButton radioButtonColumnPlot;
        private System.Windows.Forms.RadioButton radioButtonPointPlot;
        private System.Windows.Forms.Button buttonClearAll;
        private System.Windows.Forms.Button Details;
        private System.Windows.Forms.Label labelTotalFiles;
        private System.Windows.Forms.ToolStripMenuItem openMzXMLToolStripMenuItem;
    }
}

