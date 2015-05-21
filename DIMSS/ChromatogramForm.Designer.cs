namespace DIMSS
{
    partial class ChromatogramForm
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
            chartArea1.AxisX.Maximum = 500D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.Maximum = 30000D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.Name = "ChartArea1";
            this.chartChromatogram.ChartAreas.Add(chartArea1);
            this.chartChromatogram.Location = new System.Drawing.Point(7, 7);
            this.chartChromatogram.Margin = new System.Windows.Forms.Padding(7);
            this.chartChromatogram.Name = "chartChromatogram";
            this.chartChromatogram.Size = new System.Drawing.Size(397, 385);
            this.chartChromatogram.TabIndex = 0;
            this.chartChromatogram.Text = "chartChromatogram";
            // 
            // buttonPrev
            // 
            this.buttonPrev.Location = new System.Drawing.Point(3, 3);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(68, 37);
            this.buttonPrev.TabIndex = 2;
            this.buttonPrev.Text = "Prev";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(77, 3);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(68, 37);
            this.buttonNext.TabIndex = 3;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonGoTo
            // 
            this.buttonGoTo.Location = new System.Drawing.Point(151, 3);
            this.buttonGoTo.Name = "buttonGoTo";
            this.buttonGoTo.Size = new System.Drawing.Size(68, 37);
            this.buttonGoTo.TabIndex = 5;
            this.buttonGoTo.Text = "Go To";
            this.buttonGoTo.UseVisualStyleBackColor = true;
            this.buttonGoTo.Click += new System.EventHandler(this.buttonGoTo_Click);
            // 
            // comboBoxScans
            // 
            this.comboBoxScans.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxScans.FormattingEnabled = true;
            this.comboBoxScans.Location = new System.Drawing.Point(225, 3);
            this.comboBoxScans.Name = "comboBoxScans";
            this.comboBoxScans.Size = new System.Drawing.Size(67, 28);
            this.comboBoxScans.TabIndex = 6;
            // 
            // dataGridViewMZSpectra
            // 
            this.dataGridViewMZSpectra.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMZSpectra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMZSpectra.Location = new System.Drawing.Point(3, 79);
            this.dataGridViewMZSpectra.Name = "dataGridViewMZSpectra";
            this.dataGridViewMZSpectra.Size = new System.Drawing.Size(399, 84);
            this.dataGridViewMZSpectra.TabIndex = 7;
            // 
            // pictureBoxChromatogram
            // 
            this.pictureBoxChromatogram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxChromatogram.Location = new System.Drawing.Point(414, 3);
            this.pictureBoxChromatogram.Name = "pictureBoxChromatogram";
            this.tableLayoutPanel3.SetRowSpan(this.pictureBoxChromatogram, 2);
            this.pictureBoxChromatogram.Size = new System.Drawing.Size(405, 565);
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
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(385, 57);
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
            this.tableLayoutPanel3.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(822, 571);
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
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 402);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(405, 166);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // ChromatogramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 590);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "ChromatogramForm";
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