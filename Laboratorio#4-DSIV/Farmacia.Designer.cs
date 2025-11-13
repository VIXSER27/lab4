namespace Laboratorio_4_DSIV
{
    partial class Farmacia
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
            this.flowLayoutPanelCatalogo = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanelCatalogo
            // 
            this.flowLayoutPanelCatalogo.AutoScroll = true;
            this.flowLayoutPanelCatalogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelCatalogo.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelCatalogo.Name = "flowLayoutPanelCatalogo";
            this.flowLayoutPanelCatalogo.Size = new System.Drawing.Size(600, 366);
            this.flowLayoutPanelCatalogo.TabIndex = 0;
            // 
            // Farmacia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.flowLayoutPanelCatalogo);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Farmacia";
            this.Text = "Farmacia ";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCatalogo;
    }
}