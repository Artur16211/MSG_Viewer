﻿namespace MSG_Viewer
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.openog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel.Location = new System.Drawing.Point(12, 34);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(852, 428);
            this.flowLayoutPanel.TabIndex = 0;
            this.flowLayoutPanel.Visible = false;
            this.flowLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel_Paint);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.Location = new System.Drawing.Point(687, 5);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(188, 24);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "Drag a .msg file here.";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // openog
            // 
            this.openog.Location = new System.Drawing.Point(93, 5);
            this.openog.Name = "openog";
            this.openog.Size = new System.Drawing.Size(75, 23);
            this.openog.TabIndex = 2;
            this.openog.Text = "Open OG File";
            this.openog.UseVisualStyleBackColor = true;
            this.openog.Click += new System.EventHandler(this.openog_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(876, 474);
            this.Controls.Add(this.openog);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.flowLayoutPanel);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Button openog;
    }
}

