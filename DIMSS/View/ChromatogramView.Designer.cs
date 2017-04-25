namespace DIMSS.View
{
    partial class ChromatogramView
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.chartChromatogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonGoTo = new System.Windows.Forms.Button();
            this.comboBoxScans = new System.Windows.Forms.ComboBox();
            this.dataGridViewMZSpectra = new System.Windows.Forms.DataGridView();
            this.pictureBoxChromatogram = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.chartChromatogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMZSpectra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChromatogram)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartChromatogram
            // 
            this.chartChromatogram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea3.AxisX.Maximum = 500D;
            chartArea3.AxisX.Minimum = 0D;
            chartArea3.AxisY.Maximum = 30000D;
            chartArea3.AxisY.Minimum = 0D;
            chartArea3.Name = "ChartArea1";
            this.chartChromatogram.ChartAreas.Add(chartArea3);
            this.chartChromatogram.Location = new System.Drawing.Point(9, 9);
            this.chartChromatogram.Margin = new System.Windows.Forms.Padding(9);
            this.chartChromatogram.Name = "chartChromatogram";
            this.chartChromatogram.Size = new System.Drawing.Size(530, 474);
            this.chartChromatogram.TabIndex = 0;
            this.chartChromatogram.Text = "chartChromatogram";
            // 
            // buttonPrev
            // 
            this.buttonPrev.Location = new System.Drawing.Point(4, 4);
            this.buttonPrev.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(91, 46);
            this.buttonPrev.TabIndex = 2;
            this.buttonPrev.Text = "Prev";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(103, 4);
            this.buttonNext.Margin = new System.Windows.Forms.Padding(4);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(91, 46);
            this.buttonNext.TabIndex = 3;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonGoTo
            // 
            this.buttonGoTo.Location = new System.Drawing.Point(202, 4);
            this.buttonGoTo.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGoTo.Name = "buttonGoTo";
            this.buttonGoTo.Size = new System.Drawing.Size(91, 46);
            this.buttonGoTo.TabIndex = 5;
            this.buttonGoTo.Text = "Go To";
            this.buttonGoTo.UseVisualStyleBackColor = true;
            this.buttonGoTo.Click += new System.EventHandler(this.buttonGoTo_Click);
            // 
            // comboBoxScans
            // 
            this.comboBoxScans.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxScans.FormattingEnabled = true;
            this.comboBoxScans.Location = new System.Drawing.Point(301, 12);
            this.comboBoxScans.Margin = new System.Windows.Forms.Padding(4, 12, 4, 4);
            this.comboBoxScans.Name = "comboBoxScans";
            this.comboBoxScans.Size = new System.Drawing.Size(88, 33);
            this.comboBoxScans.TabIndex = 6;
            // 
            // dataGridViewMZSpectra
            // 
            this.dataGridViewMZSpectra.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMZSpectra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMZSpectra.Location = new System.Drawing.Point(4, 96);
            this.dataGridViewMZSpectra.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewMZSpectra.Name = "dataGridViewMZSpectra";
            this.dataGridViewMZSpectra.Size = new System.Drawing.Size(532, 103);
            this.dataGridViewMZSpectra.TabIndex = 7;
            // 
            // pictureBoxChromatogram
            // 
            this.pictureBoxChromatogram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxChromatogram.Location = new System.Drawing.Point(552, 4);
            this.pictureBoxChromatogram.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxChromatogram.Name = "pictureBoxChromatogram";
            this.tableLayoutPanel3.SetRowSpan(this.pictureBoxChromatogram, 2);
            this.pictureBoxChromatogram.Size = new System.Drawing.Size(540, 695);
            this.pictureBoxChromatogram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxChromatogram.TabIndex = 2;
            this.pictureBoxChromatogram.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel1.Controls.Add(this.buttonPrev);
            this.flowLayoutPanel1.Controls.Add(this.buttonNext);
            this.flowLayoutPanel1.Controls.Add(this.buttonGoTo);
            this.flowLayoutPanel1.Controls.Add(this.comboBoxScans);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 18);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(513, 70);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.chartChromatogram, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.pictureBoxChromatogram, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(16, 15);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1096, 703);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.dataGridViewMZSpectra, 0, 1);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(4, 496);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(540, 203);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // ChromatogramView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 726);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ChromatogramView";
            this.Text = "ChromatogramForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChromatogramForm_FormClosed);
            this.Load += new System.EventHandler(this.ChromatogramForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartChromatogram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMZSpectra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChromatogram)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartChromatogram;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonGoTo;
        private System.Windows.Forms.ComboBox comboBoxScans;
        private System.Windows.Forms.DataGridView dataGridViewMZSpectra;
        private System.Windows.Forms.PictureBox pictureBoxChromatogram;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}