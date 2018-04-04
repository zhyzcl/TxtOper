namespace TxtOper
{
    partial class FiltrateFile
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.bkWk = new System.ComponentModel.BackgroundWorker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richText = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rTBTitle = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rTBFilter = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tBsp = new System.Windows.Forms.TextBox();
            this.btcz = new System.Windows.Forms.Button();
            this.btyl = new System.Windows.Forms.Button();
            this.tBdir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bkWk
            // 
            this.bkWk.WorkerReportsProgress = true;
            this.bkWk.WorkerSupportsCancellation = true;
            this.bkWk.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkWk_DoWork);
            this.bkWk.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bkWk_RunWorkerCompleted);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.richText);
            this.groupBox2.Location = new System.Drawing.Point(19, 257);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(859, 158);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "监视窗口";
            // 
            // richText
            // 
            this.richText.Location = new System.Drawing.Point(7, 14);
            this.richText.Name = "richText";
            this.richText.Size = new System.Drawing.Size(846, 137);
            this.richText.TabIndex = 3;
            this.richText.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.rTBTitle);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.rTBFilter);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tBsp);
            this.groupBox1.Controls.Add(this.btcz);
            this.groupBox1.Controls.Add(this.btyl);
            this.groupBox1.Controls.Add(this.tBdir);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(19, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(859, 239);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "标题过滤：";
            // 
            // rTBTitle
            // 
            this.rTBTitle.Location = new System.Drawing.Point(106, 76);
            this.rTBTitle.Name = "rTBTitle";
            this.rTBTitle.Size = new System.Drawing.Size(741, 44);
            this.rTBTitle.TabIndex = 8;
            this.rTBTitle.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "内容过滤：";
            // 
            // rTBFilter
            // 
            this.rTBFilter.Location = new System.Drawing.Point(106, 126);
            this.rTBFilter.Name = "rTBFilter";
            this.rTBFilter.Size = new System.Drawing.Size(741, 65);
            this.rTBFilter.TabIndex = 6;
            this.rTBFilter.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "搜索：";
            // 
            // tBsp
            // 
            this.tBsp.Location = new System.Drawing.Point(106, 48);
            this.tBsp.Name = "tBsp";
            this.tBsp.Size = new System.Drawing.Size(121, 21);
            this.tBsp.TabIndex = 4;
            this.tBsp.Text = "*.txt";
            // 
            // btcz
            // 
            this.btcz.Location = new System.Drawing.Point(106, 199);
            this.btcz.Name = "btcz";
            this.btcz.Size = new System.Drawing.Size(81, 23);
            this.btcz.TabIndex = 3;
            this.btcz.Text = "开始操作";
            this.btcz.UseVisualStyleBackColor = true;
            this.btcz.Click += new System.EventHandler(this.btcz_Click);
            // 
            // btyl
            // 
            this.btyl.Location = new System.Drawing.Point(625, 18);
            this.btyl.Name = "btyl";
            this.btyl.Size = new System.Drawing.Size(81, 23);
            this.btyl.TabIndex = 2;
            this.btyl.Text = "浏览";
            this.btyl.UseVisualStyleBackColor = true;
            this.btyl.Click += new System.EventHandler(this.btyl_Click);
            // 
            // tBdir
            // 
            this.tBdir.Location = new System.Drawing.Point(106, 20);
            this.tBdir.Name = "tBdir";
            this.tBdir.Size = new System.Drawing.Size(512, 21);
            this.tBdir.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "目录：";
            // 
            // FiltrateFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 437);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FiltrateFile";
            this.Text = "文本过滤";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bkWk;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox richText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox rTBTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rTBFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBsp;
        private System.Windows.Forms.Button btcz;
        private System.Windows.Forms.Button btyl;
        private System.Windows.Forms.TextBox tBdir;
        private System.Windows.Forms.Label label1;
    }
}

