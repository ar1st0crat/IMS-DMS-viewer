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
            this.pictureBoxChromatogram = new System.Windows.Forms.PictureBox();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonGoTo = new System.Windows.Forms.Button();
            this.comboBoxScans = new System.Windows.Forms.ComboBox();
            this.dataGridViewMZSpectra = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.chartChromatogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChromatogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMZSpectra)).BeginInit();
            this.SuspendLayout();
            // 
            // chartChromatogram
            // 
            chartArea1.AxisX.Maximum = 500D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.Maximum = 30000D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.Name = "ChartArea1";
            this.chartChromatogram.ChartAreas.Add(chartArea1);
            this.chartChromatogram.Location = new System.Drawing.Point(12, 15);
            this.chartChromatogram.Name = "chartChromatogram";
            this.chartChromatogram.Size = new System.Drawing.Size(593, 409);
            this.chartChromatogram.TabIndex = 0;
            this.chartChromatogram.Text = "chartChromatogram";
            // 
            // pictureBoxChromatogram
            // 
            this.pictureBoxChromatogram.Location = new System.Drawing.Point(612, 15);
            this.pictureBoxChromatogram.Name = "pictureBoxChromatogram";
            this.pictureBoxChromatogram.Size = new System.Drawing.Size(369, 563);
            this.pictureBoxChromatogram.TabIndex = 1;
            this.pictureBoxChromatogram.TabStop = false;
            // 
            // buttonPrev
            // 
            this.buttonPrev.Location = new System.Drawing.Point(11, 440);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(68, 37);
            this.buttonPrev.TabIndex = 2;
            this.buttonPrev.Text = "Prev";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(85, 440);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(68, 37);
            this.buttonNext.TabIndex = 3;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonGoTo
            // 
            this.buttonGoTo.Location = new System.Drawing.Point(159, 440);
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
            this.comboBoxScans.Location = new System.Drawing.Point(234, 445);
            this.comboBoxScans.Name = "comboBoxScans";
            this.comboBoxScans.Size = new System.Drawing.Size(67, 28);
            this.comboBoxScans.TabIndex = 6;
            // 
            // dataGridViewMZSpectra
            // 
            this.dataGridViewMZSpectra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMZSpectra.Location = new System.Drawing.Point(12, 493);
            this.dataGridViewMZSpectra.Name = "dataGridViewMZSpectra";
            this.dataGridViewMZSpectra.Size = new System.Drawing.Size(593, 85);
            this.dataGridViewMZSpectra.TabIndex = 7;
            // 
            // ChromatogramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 590);
            this.Controls.Add(this.dataGridViewMZSpectra);
            this.Controls.Add(this.comboBoxScans);
            this.Controls.Add(this.buttonGoTo);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrev);
            this.Controls.Add(this.pictureBoxChromatogram);
            this.Controls.Add(this.chartChromatogram);
            this.Name = "ChromatogramForm";
            this.Text = "ChromatogramForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChromatogramForm_FormClosed);
            this.Load += new System.EventHandler(this.ChromatogramForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartChromatogram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChromatogram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMZSpectra)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartChromatogram;
        private System.Windows.Forms.PictureBox pictureBoxChromatogram;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonGoTo;
        private System.Windows.Forms.ComboBox comboBoxScans;
        private System.Windows.Forms.DataGridView dataGridViewMZSpectra;
    }
}