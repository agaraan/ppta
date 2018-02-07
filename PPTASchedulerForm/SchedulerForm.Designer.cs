namespace PPTASchedulerForm
{
    partial class SchedulerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDataFile = new System.Windows.Forms.Label();
            this.btnSchedule = new System.Windows.Forms.Button();
            this.txtDataFile = new System.Windows.Forms.TextBox();
            this.btnCreateTeacher = new System.Windows.Forms.Button();
            this.btnAssignRoom = new System.Windows.Forms.Button();
            this.btnStudent = new System.Windows.Forms.Button();
            this.btnPerformance = new System.Windows.Forms.Button();
            this.btnJudgeAvail = new System.Windows.Forms.Button();
            this.btnAssignLunch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDataFile
            // 
            this.lblDataFile.AutoSize = true;
            this.lblDataFile.Location = new System.Drawing.Point(147, 43);
            this.lblDataFile.Name = "lblDataFile";
            this.lblDataFile.Size = new System.Drawing.Size(73, 20);
            this.lblDataFile.TabIndex = 0;
            this.lblDataFile.Text = "Data File";
            // 
            // btnSchedule
            // 
            this.btnSchedule.Location = new System.Drawing.Point(342, 241);
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(109, 39);
            this.btnSchedule.TabIndex = 1;
            this.btnSchedule.Text = "Schedule";
            this.btnSchedule.UseVisualStyleBackColor = true;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            // 
            // txtDataFile
            // 
            this.txtDataFile.Location = new System.Drawing.Point(239, 40);
            this.txtDataFile.Name = "txtDataFile";
            this.txtDataFile.Size = new System.Drawing.Size(186, 26);
            this.txtDataFile.TabIndex = 2;
            this.txtDataFile.Text = "C:\\ppta\\PPTAEval.csv";
            // 
            // btnCreateTeacher
            // 
            this.btnCreateTeacher.Location = new System.Drawing.Point(93, 121);
            this.btnCreateTeacher.Name = "btnCreateTeacher";
            this.btnCreateTeacher.Size = new System.Drawing.Size(139, 38);
            this.btnCreateTeacher.TabIndex = 3;
            this.btnCreateTeacher.Text = "Create Teacher";
            this.btnCreateTeacher.UseVisualStyleBackColor = true;
            this.btnCreateTeacher.Click += new System.EventHandler(this.btnCreateTeacher_Click);
            // 
            // btnAssignRoom
            // 
            this.btnAssignRoom.Location = new System.Drawing.Point(263, 121);
            this.btnAssignRoom.Name = "btnAssignRoom";
            this.btnAssignRoom.Size = new System.Drawing.Size(135, 38);
            this.btnAssignRoom.TabIndex = 4;
            this.btnAssignRoom.Text = "Assign Room";
            this.btnAssignRoom.UseVisualStyleBackColor = true;
            this.btnAssignRoom.Click += new System.EventHandler(this.btnAssignRoom_Click);
            // 
            // btnStudent
            // 
            this.btnStudent.Location = new System.Drawing.Point(427, 121);
            this.btnStudent.Name = "btnStudent";
            this.btnStudent.Size = new System.Drawing.Size(131, 38);
            this.btnStudent.TabIndex = 5;
            this.btnStudent.Text = "Create Student";
            this.btnStudent.UseVisualStyleBackColor = true;
            this.btnStudent.Click += new System.EventHandler(this.btnStudent_Click);
            // 
            // btnPerformance
            // 
            this.btnPerformance.Location = new System.Drawing.Point(151, 174);
            this.btnPerformance.Name = "btnPerformance";
            this.btnPerformance.Size = new System.Drawing.Size(139, 37);
            this.btnPerformance.TabIndex = 6;
            this.btnPerformance.Text = "Load Perf.";
            this.btnPerformance.UseVisualStyleBackColor = true;
            this.btnPerformance.Click += new System.EventHandler(this.btnPerformance_Click);
            // 
            // btnJudgeAvail
            // 
            this.btnJudgeAvail.Location = new System.Drawing.Point(342, 174);
            this.btnJudgeAvail.Name = "btnJudgeAvail";
            this.btnJudgeAvail.Size = new System.Drawing.Size(128, 37);
            this.btnJudgeAvail.TabIndex = 7;
            this.btnJudgeAvail.Text = "Judge Avail.";
            this.btnJudgeAvail.UseVisualStyleBackColor = true;
            this.btnJudgeAvail.Click += new System.EventHandler(this.btnJudgeAvail_Click);
            // 
            // btnAssignLunch
            // 
            this.btnAssignLunch.Location = new System.Drawing.Point(175, 245);
            this.btnAssignLunch.Name = "btnAssignLunch";
            this.btnAssignLunch.Size = new System.Drawing.Size(115, 35);
            this.btnAssignLunch.TabIndex = 8;
            this.btnAssignLunch.Text = "Sche. Break";
            this.btnAssignLunch.UseVisualStyleBackColor = true;
            this.btnAssignLunch.Click += new System.EventHandler(this.btnAssignLunch_Click);
            // 
            // SchedulerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 430);
            this.Controls.Add(this.btnAssignLunch);
            this.Controls.Add(this.btnJudgeAvail);
            this.Controls.Add(this.btnPerformance);
            this.Controls.Add(this.btnStudent);
            this.Controls.Add(this.btnAssignRoom);
            this.Controls.Add(this.btnCreateTeacher);
            this.Controls.Add(this.txtDataFile);
            this.Controls.Add(this.btnSchedule);
            this.Controls.Add(this.lblDataFile);
            this.Name = "SchedulerForm";
            this.Text = "Scheduler Form";
            this.Load += new System.EventHandler(this.SchedulerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDataFile;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.TextBox txtDataFile;
        private System.Windows.Forms.Button btnCreateTeacher;
        private System.Windows.Forms.Button btnAssignRoom;
        private System.Windows.Forms.Button btnStudent;
        private System.Windows.Forms.Button btnPerformance;
        private System.Windows.Forms.Button btnJudgeAvail;
        private System.Windows.Forms.Button btnAssignLunch;
    }
}

