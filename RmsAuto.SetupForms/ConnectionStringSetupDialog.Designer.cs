namespace SetupForms
{
    partial class ConnectionStringSetupDialog
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
            this.StoreConnectionLabel = new System.Windows.Forms.Label();
            this.LogConnectionLabel = new System.Windows.Forms.Label();
            this.CommonConnectionLabel = new System.Windows.Forms.Label();
            this.ShowDialogBtnForStoreConnection = new System.Windows.Forms.Button();
            this.ShowDialogBtnForLogConnection = new System.Windows.Forms.Button();
            this.ShowDialogBtnForCommonConnection = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StoreConnectionLabel
            // 
            this.StoreConnectionLabel.AutoSize = true;
            this.StoreConnectionLabel.Location = new System.Drawing.Point(73, 13);
            this.StoreConnectionLabel.Name = "StoreConnectionLabel";
            this.StoreConnectionLabel.Size = new System.Drawing.Size(68, 13);
            this.StoreConnectionLabel.TabIndex = 0;
            this.StoreConnectionLabel.Text = "к базе Store";
            // 
            // LogConnectionLabel
            // 
            this.LogConnectionLabel.AutoSize = true;
            this.LogConnectionLabel.Location = new System.Drawing.Point(73, 43);
            this.LogConnectionLabel.Name = "LogConnectionLabel";
            this.LogConnectionLabel.Size = new System.Drawing.Size(61, 13);
            this.LogConnectionLabel.TabIndex = 1;
            this.LogConnectionLabel.Text = "к базе Log";
            // 
            // CommonConnectionLabel
            // 
            this.CommonConnectionLabel.AutoSize = true;
            this.CommonConnectionLabel.Location = new System.Drawing.Point(73, 72);
            this.CommonConnectionLabel.Name = "CommonConnectionLabel";
            this.CommonConnectionLabel.Size = new System.Drawing.Size(84, 13);
            this.CommonConnectionLabel.TabIndex = 2;
            this.CommonConnectionLabel.Text = "к базе Common";
            // 
            // ShowDialogBtnForStoreConnection
            // 
            this.ShowDialogBtnForStoreConnection.Location = new System.Drawing.Point(12, 8);
            this.ShowDialogBtnForStoreConnection.Name = "ShowDialogBtnForStoreConnection";
            this.ShowDialogBtnForStoreConnection.Size = new System.Drawing.Size(55, 23);
            this.ShowDialogBtnForStoreConnection.TabIndex = 3;
            this.ShowDialogBtnForStoreConnection.Text = "Задать";
            this.ShowDialogBtnForStoreConnection.UseVisualStyleBackColor = true;
            this.ShowDialogBtnForStoreConnection.Click += new System.EventHandler(this.ShowDialogBtnForStoreConnection_Click);
            // 
            // ShowDialogBtnForLogConnection
            // 
            this.ShowDialogBtnForLogConnection.Location = new System.Drawing.Point(12, 38);
            this.ShowDialogBtnForLogConnection.Name = "ShowDialogBtnForLogConnection";
            this.ShowDialogBtnForLogConnection.Size = new System.Drawing.Size(55, 23);
            this.ShowDialogBtnForLogConnection.TabIndex = 4;
            this.ShowDialogBtnForLogConnection.Text = "Задать";
            this.ShowDialogBtnForLogConnection.UseVisualStyleBackColor = true;
            this.ShowDialogBtnForLogConnection.Click += new System.EventHandler(this.ShowDialogBtnForLogConnection_Click);
            // 
            // ShowDialogBtnForCommonConnection
            // 
            this.ShowDialogBtnForCommonConnection.Location = new System.Drawing.Point(12, 67);
            this.ShowDialogBtnForCommonConnection.Name = "ShowDialogBtnForCommonConnection";
            this.ShowDialogBtnForCommonConnection.Size = new System.Drawing.Size(55, 23);
            this.ShowDialogBtnForCommonConnection.TabIndex = 5;
            this.ShowDialogBtnForCommonConnection.Text = "Задать";
            this.ShowDialogBtnForCommonConnection.UseVisualStyleBackColor = true;
            this.ShowDialogBtnForCommonConnection.Click += new System.EventHandler(this.ShowDialogBtnForCommonConnection_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.Location = new System.Drawing.Point(595, 127);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(55, 23);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "Отмена";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Location = new System.Drawing.Point(534, 127);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(55, 23);
            this.OkButton.TabIndex = 7;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // ConnectionStringSetupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 158);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ShowDialogBtnForCommonConnection);
            this.Controls.Add(this.ShowDialogBtnForLogConnection);
            this.Controls.Add(this.ShowDialogBtnForStoreConnection);
            this.Controls.Add(this.CommonConnectionLabel);
            this.Controls.Add(this.LogConnectionLabel);
            this.Controls.Add(this.StoreConnectionLabel);
            this.Name = "ConnectionStringSetupDialog";
            this.Text = "Строки соединения";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label StoreConnectionLabel;
        private System.Windows.Forms.Label LogConnectionLabel;
        private System.Windows.Forms.Label CommonConnectionLabel;
        private System.Windows.Forms.Button ShowDialogBtnForStoreConnection;
        private System.Windows.Forms.Button ShowDialogBtnForLogConnection;
        private System.Windows.Forms.Button ShowDialogBtnForCommonConnection;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OkButton;

    }
}

