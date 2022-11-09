namespace IslandScadaServiceReader
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tmrStart = new System.Windows.Forms.Timer(this.components);
            this.NIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tmrRepaint = new System.Windows.Forms.Timer(this.components);
            this.rtbResult = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // tmrStart
            // 
            this.tmrStart.Enabled = true;
            this.tmrStart.Interval = 50;
            this.tmrStart.Tick += new System.EventHandler(this.tmrStart_Tick);
            // 
            // NIcon
            // 
            this.NIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.NIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NIcon.Icon")));
            this.NIcon.Text = "Island Scada драйвер для контроллеров Beckhoff";
            this.NIcon.Visible = true;
            this.NIcon.Click += new System.EventHandler(this.NIcon_Click);
            // 
            // tmrRepaint
            // 
            this.tmrRepaint.Enabled = true;
            this.tmrRepaint.Interval = 10;
            this.tmrRepaint.Tick += new System.EventHandler(this.tmrRepaint_Tick);
            // 
            // rtbResult
            // 
            this.rtbResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbResult.Location = new System.Drawing.Point(0, 0);
            this.rtbResult.Name = "rtbResult";
            this.rtbResult.ReadOnly = true;
            this.rtbResult.Size = new System.Drawing.Size(368, 157);
            this.rtbResult.TabIndex = 0;
            this.rtbResult.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 157);
            this.Controls.Add(this.rtbResult);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "Драйвер чтения данных из контроллера Beckhoff";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrStart;
        private System.Windows.Forms.NotifyIcon NIcon;
        private System.Windows.Forms.Timer tmrRepaint;
        private System.Windows.Forms.RichTextBox rtbResult;
    }
}

