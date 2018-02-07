namespace BathymetryDbfToSQL
{
    partial class BathymetryDbfToSQL
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lblFileAfterDate = new System.Windows.Forms.Label();
            this.textBoxFileAfterDate = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lblStep1 = new System.Windows.Forms.Label();
            this.butStartTransfer = new System.Windows.Forms.Button();
            this.textBoxDbfFilesDirPath = new System.Windows.Forms.TextBox();
            this.lblCurrentFile = new System.Windows.Forms.Label();
            this.lblDbfFileDirPath = new System.Windows.Forms.Label();
            this.lblDoingFile = new System.Windows.Forms.Label();
            this.butListDBFFiles = new System.Windows.Forms.Button();
            this.lblStatusTxt = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.textBoxSoundStartAt = new System.Windows.Forms.TextBox();
            this.textBoxLineStartAt = new System.Windows.Forms.TextBox();
            this.butCreateIndexKMZ = new System.Windows.Forms.Button();
            this.lblCurrentFile2 = new System.Windows.Forms.Label();
            this.lblDoingFile2 = new System.Windows.Forms.Label();
            this.lblStatusTxt2 = new System.Windows.Forms.Label();
            this.lblStatus2 = new System.Windows.Forms.Label();
            this.butCreateSoundKMZ = new System.Windows.Forms.Button();
            this.butCreateLineDepthKMZ = new System.Windows.Forms.Button();
            this.butListChartsInDB = new System.Windows.Forms.Button();
            this.lblStep2 = new System.Windows.Forms.Label();
            this.richTextBoxResults = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxResults);
            this.splitContainer1.Size = new System.Drawing.Size(977, 531);
            this.splitContainer1.SplitterDistance = 216;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lblFileAfterDate);
            this.splitContainer2.Panel1.Controls.Add(this.textBoxFileAfterDate);
            this.splitContainer2.Panel1.Controls.Add(this.button1);
            this.splitContainer2.Panel1.Controls.Add(this.lblStep1);
            this.splitContainer2.Panel1.Controls.Add(this.butStartTransfer);
            this.splitContainer2.Panel1.Controls.Add(this.textBoxDbfFilesDirPath);
            this.splitContainer2.Panel1.Controls.Add(this.lblCurrentFile);
            this.splitContainer2.Panel1.Controls.Add(this.lblDbfFileDirPath);
            this.splitContainer2.Panel1.Controls.Add(this.lblDoingFile);
            this.splitContainer2.Panel1.Controls.Add(this.butListDBFFiles);
            this.splitContainer2.Panel1.Controls.Add(this.lblStatusTxt);
            this.splitContainer2.Panel1.Controls.Add(this.lblStatus);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.textBoxSoundStartAt);
            this.splitContainer2.Panel2.Controls.Add(this.textBoxLineStartAt);
            this.splitContainer2.Panel2.Controls.Add(this.butCreateIndexKMZ);
            this.splitContainer2.Panel2.Controls.Add(this.lblCurrentFile2);
            this.splitContainer2.Panel2.Controls.Add(this.lblDoingFile2);
            this.splitContainer2.Panel2.Controls.Add(this.lblStatusTxt2);
            this.splitContainer2.Panel2.Controls.Add(this.lblStatus2);
            this.splitContainer2.Panel2.Controls.Add(this.butCreateSoundKMZ);
            this.splitContainer2.Panel2.Controls.Add(this.butCreateLineDepthKMZ);
            this.splitContainer2.Panel2.Controls.Add(this.butListChartsInDB);
            this.splitContainer2.Panel2.Controls.Add(this.lblStep2);
            this.splitContainer2.Size = new System.Drawing.Size(977, 216);
            this.splitContainer2.SplitterDistance = 116;
            this.splitContainer2.TabIndex = 9;
            // 
            // lblFileAfterDate
            // 
            this.lblFileAfterDate.AutoSize = true;
            this.lblFileAfterDate.Location = new System.Drawing.Point(30, 86);
            this.lblFileAfterDate.Name = "lblFileAfterDate";
            this.lblFileAfterDate.Size = new System.Drawing.Size(77, 13);
            this.lblFileAfterDate.TabIndex = 11;
            this.lblFileAfterDate.Text = "File After Date:";
            // 
            // textBoxFileAfterDate
            // 
            this.textBoxFileAfterDate.Location = new System.Drawing.Point(113, 84);
            this.textBoxFileAfterDate.Name = "textBoxFileAfterDate";
            this.textBoxFileAfterDate.Size = new System.Drawing.Size(125, 20);
            this.textBoxFileAfterDate.TabIndex = 10;
            this.textBoxFileAfterDate.Text = "2013/09/18";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(867, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblStep1
            // 
            this.lblStep1.AutoSize = true;
            this.lblStep1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStep1.Location = new System.Drawing.Point(20, 10);
            this.lblStep1.Name = "lblStep1";
            this.lblStep1.Size = new System.Drawing.Size(80, 25);
            this.lblStep1.TabIndex = 8;
            this.lblStep1.Text = "Step 1";
            // 
            // butStartTransfer
            // 
            this.butStartTransfer.Location = new System.Drawing.Point(38, 47);
            this.butStartTransfer.Name = "butStartTransfer";
            this.butStartTransfer.Size = new System.Drawing.Size(108, 30);
            this.butStartTransfer.TabIndex = 0;
            this.butStartTransfer.Text = "Start Transfer";
            this.butStartTransfer.UseVisualStyleBackColor = true;
            this.butStartTransfer.Click += new System.EventHandler(this.butStartTransfer_Click);
            // 
            // textBoxDbfFilesDirPath
            // 
            this.textBoxDbfFilesDirPath.Location = new System.Drawing.Point(370, 17);
            this.textBoxDbfFilesDirPath.Name = "textBoxDbfFilesDirPath";
            this.textBoxDbfFilesDirPath.Size = new System.Drawing.Size(380, 20);
            this.textBoxDbfFilesDirPath.TabIndex = 1;
            this.textBoxDbfFilesDirPath.Text = "C:\\CSSP\\CHS Bathymetry\\File_dbf";
            // 
            // lblCurrentFile
            // 
            this.lblCurrentFile.AutoSize = true;
            this.lblCurrentFile.Location = new System.Drawing.Point(273, 56);
            this.lblCurrentFile.Name = "lblCurrentFile";
            this.lblCurrentFile.Size = new System.Drawing.Size(60, 13);
            this.lblCurrentFile.TabIndex = 7;
            this.lblCurrentFile.Text = "Current File";
            // 
            // lblDbfFileDirPath
            // 
            this.lblDbfFileDirPath.AutoSize = true;
            this.lblDbfFileDirPath.Location = new System.Drawing.Point(204, 21);
            this.lblDbfFileDirPath.Name = "lblDbfFileDirPath";
            this.lblDbfFileDirPath.Size = new System.Drawing.Size(125, 13);
            this.lblDbfFileDirPath.TabIndex = 2;
            this.lblDbfFileDirPath.Text = "DBF Files Directory Path:";
            // 
            // lblDoingFile
            // 
            this.lblDoingFile.AutoSize = true;
            this.lblDoingFile.Location = new System.Drawing.Point(204, 56);
            this.lblDoingFile.Name = "lblDoingFile";
            this.lblDoingFile.Size = new System.Drawing.Size(57, 13);
            this.lblDoingFile.TabIndex = 6;
            this.lblDoingFile.Text = "Doing File:";
            // 
            // butListDBFFiles
            // 
            this.butListDBFFiles.Location = new System.Drawing.Point(827, 13);
            this.butListDBFFiles.Name = "butListDBFFiles";
            this.butListDBFFiles.Size = new System.Drawing.Size(88, 26);
            this.butListDBFFiles.TabIndex = 3;
            this.butListDBFFiles.Text = "List DBF Files";
            this.butListDBFFiles.UseVisualStyleBackColor = true;
            this.butListDBFFiles.Click += new System.EventHandler(this.butListDBFFiles_Click);
            // 
            // lblStatusTxt
            // 
            this.lblStatusTxt.AutoSize = true;
            this.lblStatusTxt.Location = new System.Drawing.Point(456, 56);
            this.lblStatusTxt.Name = "lblStatusTxt";
            this.lblStatusTxt.Size = new System.Drawing.Size(372, 13);
            this.lblStatusTxt.TabIndex = 5;
            this.lblStatusTxt.Text = "This portion will indicate the status of the reading and saving in the Database.";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(409, 56);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Status:";
            // 
            // textBoxSoundStartAt
            // 
            this.textBoxSoundStartAt.Location = new System.Drawing.Point(477, 32);
            this.textBoxSoundStartAt.Name = "textBoxSoundStartAt";
            this.textBoxSoundStartAt.Size = new System.Drawing.Size(115, 20);
            this.textBoxSoundStartAt.TabIndex = 21;
            this.textBoxSoundStartAt.Text = "CA176030SOUNDG";
            // 
            // textBoxLineStartAt
            // 
            this.textBoxLineStartAt.Location = new System.Drawing.Point(136, 32);
            this.textBoxLineStartAt.Name = "textBoxLineStartAt";
            this.textBoxLineStartAt.Size = new System.Drawing.Size(115, 20);
            this.textBoxLineStartAt.TabIndex = 20;
            this.textBoxLineStartAt.Text = "CA176030";
            // 
            // butCreateIndexKMZ
            // 
            this.butCreateIndexKMZ.Location = new System.Drawing.Point(660, 13);
            this.butCreateIndexKMZ.Name = "butCreateIndexKMZ";
            this.butCreateIndexKMZ.Size = new System.Drawing.Size(116, 29);
            this.butCreateIndexKMZ.TabIndex = 19;
            this.butCreateIndexKMZ.Text = "Create Index.kmz";
            this.butCreateIndexKMZ.UseVisualStyleBackColor = true;
            this.butCreateIndexKMZ.Click += new System.EventHandler(this.butCreate_indexKMZ_Click);
            // 
            // lblCurrentFile2
            // 
            this.lblCurrentFile2.AutoSize = true;
            this.lblCurrentFile2.Location = new System.Drawing.Point(191, 55);
            this.lblCurrentFile2.Name = "lblCurrentFile2";
            this.lblCurrentFile2.Size = new System.Drawing.Size(60, 13);
            this.lblCurrentFile2.TabIndex = 18;
            this.lblCurrentFile2.Text = "Current File";
            // 
            // lblDoingFile2
            // 
            this.lblDoingFile2.AutoSize = true;
            this.lblDoingFile2.Location = new System.Drawing.Point(122, 55);
            this.lblDoingFile2.Name = "lblDoingFile2";
            this.lblDoingFile2.Size = new System.Drawing.Size(57, 13);
            this.lblDoingFile2.TabIndex = 17;
            this.lblDoingFile2.Text = "Doing File:";
            // 
            // lblStatusTxt2
            // 
            this.lblStatusTxt2.AutoSize = true;
            this.lblStatusTxt2.Location = new System.Drawing.Point(374, 55);
            this.lblStatusTxt2.Name = "lblStatusTxt2";
            this.lblStatusTxt2.Size = new System.Drawing.Size(372, 13);
            this.lblStatusTxt2.TabIndex = 16;
            this.lblStatusTxt2.Text = "This portion will indicate the status of the reading and saving in the Database.";
            // 
            // lblStatus2
            // 
            this.lblStatus2.AutoSize = true;
            this.lblStatus2.Location = new System.Drawing.Point(327, 55);
            this.lblStatus2.Name = "lblStatus2";
            this.lblStatus2.Size = new System.Drawing.Size(40, 13);
            this.lblStatus2.TabIndex = 15;
            this.lblStatus2.Text = "Status:";
            // 
            // butCreateSoundKMZ
            // 
            this.butCreateSoundKMZ.Location = new System.Drawing.Point(468, 3);
            this.butCreateSoundKMZ.Name = "butCreateSoundKMZ";
            this.butCreateSoundKMZ.Size = new System.Drawing.Size(146, 29);
            this.butCreateSoundKMZ.TabIndex = 12;
            this.butCreateSoundKMZ.Text = "Create Sound Depth KMZ";
            this.butCreateSoundKMZ.UseVisualStyleBackColor = true;
            this.butCreateSoundKMZ.Click += new System.EventHandler(this.butCreateSoundDepthKMZ_Click);
            // 
            // butCreateLineDepthKMZ
            // 
            this.butCreateLineDepthKMZ.Location = new System.Drawing.Point(125, 3);
            this.butCreateLineDepthKMZ.Name = "butCreateLineDepthKMZ";
            this.butCreateLineDepthKMZ.Size = new System.Drawing.Size(146, 29);
            this.butCreateLineDepthKMZ.TabIndex = 12;
            this.butCreateLineDepthKMZ.Text = "Create Line Depth KMZ";
            this.butCreateLineDepthKMZ.UseVisualStyleBackColor = true;
            this.butCreateLineDepthKMZ.Click += new System.EventHandler(this.butCreateLineDepthKMZ_Click);
            // 
            // butListChartsInDB
            // 
            this.butListChartsInDB.Location = new System.Drawing.Point(801, 13);
            this.butListChartsInDB.Name = "butListChartsInDB";
            this.butListChartsInDB.Size = new System.Drawing.Size(127, 29);
            this.butListChartsInDB.TabIndex = 9;
            this.butListChartsInDB.Text = "List Charts in DB";
            this.butListChartsInDB.UseVisualStyleBackColor = true;
            this.butListChartsInDB.Click += new System.EventHandler(this.butListChartsInDB_Click);
            // 
            // lblStep2
            // 
            this.lblStep2.AutoSize = true;
            this.lblStep2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStep2.Location = new System.Drawing.Point(20, 13);
            this.lblStep2.Name = "lblStep2";
            this.lblStep2.Size = new System.Drawing.Size(80, 25);
            this.lblStep2.TabIndex = 8;
            this.lblStep2.Text = "Step 2";
            // 
            // richTextBoxResults
            // 
            this.richTextBoxResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxResults.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxResults.Name = "richTextBoxResults";
            this.richTextBoxResults.Size = new System.Drawing.Size(973, 307);
            this.richTextBoxResults.TabIndex = 0;
            this.richTextBoxResults.Text = "";
            // 
            // BathymetryDbfToSQL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 531);
            this.Controls.Add(this.splitContainer1);
            this.Name = "BathymetryDbfToSQL";
            this.Text = "Transfering Bathymetry DBF files to SQL Database";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblStatusTxt;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button butListDBFFiles;
        private System.Windows.Forms.Label lblDbfFileDirPath;
        private System.Windows.Forms.TextBox textBoxDbfFilesDirPath;
        private System.Windows.Forms.Button butStartTransfer;
        private System.Windows.Forms.RichTextBox richTextBoxResults;
        private System.Windows.Forms.Label lblCurrentFile;
        private System.Windows.Forms.Label lblDoingFile;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lblStep1;
        private System.Windows.Forms.Button butCreateLineDepthKMZ;
        private System.Windows.Forms.Button butListChartsInDB;
        private System.Windows.Forms.Label lblStep2;
        private System.Windows.Forms.Label lblCurrentFile2;
        private System.Windows.Forms.Label lblDoingFile2;
        private System.Windows.Forms.Label lblStatusTxt2;
        private System.Windows.Forms.Label lblStatus2;
        private System.Windows.Forms.Button butCreateIndexKMZ;
        private System.Windows.Forms.Button butCreateSoundKMZ;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxLineStartAt;
        private System.Windows.Forms.TextBox textBoxSoundStartAt;
        private System.Windows.Forms.Label lblFileAfterDate;
        private System.Windows.Forms.TextBox textBoxFileAfterDate;
    }
}

