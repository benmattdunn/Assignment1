using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace WindowsFormsApplication5
{
    partial class Assignment1
    {
        //initialize namespaces
        private System.ComponentModel.IContainer components = null;
        private Thread radioButtonThreadListener;

        //generic constants for quick changes. 
        private const int paddingConst = 20;
        private string workingDir;
        private List<Dictionary<int, string>> supportedLanguages;

        //Default language file for english, by default the program loads it instead of it being a call from a text file.
        private Dictionary<int, string> EnglishLanguage = new Dictionary<int, string>()
        {
            { 0, "English" },
            { 1, "Employee Name:" },
            { 2, "Employee ID:" },
            { 3, "Hours Worked:" },
            { 4, "Total Sales:" },
            { 5, "Sales Bonus:" },
            { 6, "Calculate" },
            { 7, "Next" },
            { 8, "Print" }

        };

        /// <summary>
        /// Clean up any resources being used, including the listener thread.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            radioButtonThreadListener.Abort(); //kill the thread in addition to standard procs, no exception handled
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
            this.languageSelectionGroup = new System.Windows.Forms.GroupBox();
            this.GermanSelectRadioBttn = new System.Windows.Forms.RadioButton();
            this.FrenchSelectRadioBttn = new System.Windows.Forms.RadioButton();
            this.EnglishSelectRadioBttn = new System.Windows.Forms.RadioButton();
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            this.EmpNameLabel = new System.Windows.Forms.Label();
            this.EmployNameTextBox = new System.Windows.Forms.TextBox();
            this.EmployeeIDLabel = new System.Windows.Forms.Label();
            this.EmployeeIDTextBox = new System.Windows.Forms.TextBox();
            this.HoursWorkedLabel = new System.Windows.Forms.Label();
            this.HoursWorkedTextBox = new System.Windows.Forms.TextBox();
            this.TotalSalesTextBox = new System.Windows.Forms.TextBox();
            this.TotalSaleslabel = new System.Windows.Forms.Label();
            this.SalesBonusLabel = new System.Windows.Forms.Label();
            this.SalesBonusTextBox = new System.Windows.Forms.TextBox();
            this.CalculateButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.PrintButton = new System.Windows.Forms.Button();
            this.languageSelectionGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // languageSelectionGroup
            // 
            this.languageSelectionGroup.Controls.Add(this.GermanSelectRadioBttn);
            this.languageSelectionGroup.Controls.Add(this.FrenchSelectRadioBttn);
            this.languageSelectionGroup.Controls.Add(this.EnglishSelectRadioBttn);
            this.languageSelectionGroup.Location = new System.Drawing.Point(180, 10);
            this.languageSelectionGroup.Name = "languageSelectionGroup";
            this.languageSelectionGroup.Size = new System.Drawing.Size(90, 100);
            this.languageSelectionGroup.TabIndex = 0;
            this.languageSelectionGroup.TabStop = false;
            // 
            // GermanSelectRadioBttn
            // 
            this.GermanSelectRadioBttn.AutoSize = true;
            this.GermanSelectRadioBttn.Location = new System.Drawing.Point(7, 68);
            this.GermanSelectRadioBttn.Name = "GermanSelectRadioBttn";
            this.GermanSelectRadioBttn.Size = new System.Drawing.Size(71, 17);
            this.GermanSelectRadioBttn.TabIndex = 2;
            this.GermanSelectRadioBttn.TabStop = true;
            this.GermanSelectRadioBttn.Text = "Deutsche";
            this.GermanSelectRadioBttn.UseVisualStyleBackColor = true;
            // 
            // FrenchSelectRadioBttn
            // 
            this.FrenchSelectRadioBttn.AutoSize = true;
            this.FrenchSelectRadioBttn.Location = new System.Drawing.Point(7, 44);
            this.FrenchSelectRadioBttn.Name = "FrenchSelectRadioBttn";
            this.FrenchSelectRadioBttn.Size = new System.Drawing.Size(65, 17);
            this.FrenchSelectRadioBttn.TabIndex = 1;
            this.FrenchSelectRadioBttn.TabStop = true;
            this.FrenchSelectRadioBttn.Text = "Français";
            this.FrenchSelectRadioBttn.UseVisualStyleBackColor = true;
            // 
            // EnglishSelectRadioBttn
            // 
            this.EnglishSelectRadioBttn.AutoSize = true;
            this.EnglishSelectRadioBttn.Location = new System.Drawing.Point(7, 20);
            this.EnglishSelectRadioBttn.Name = "EnglishSelectRadioBttn";
            this.EnglishSelectRadioBttn.Size = new System.Drawing.Size(59, 17);
            this.EnglishSelectRadioBttn.TabIndex = 0;
            this.EnglishSelectRadioBttn.TabStop = true;
            this.EnglishSelectRadioBttn.Text = "English";
            this.EnglishSelectRadioBttn.UseVisualStyleBackColor = true;
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.Image = global::WindowsFormsApplication5.Properties.Resources.Happy_Face_100x100;
            this.LogoPictureBox.InitialImage = global::WindowsFormsApplication5.Properties.Resources.Happy_Face_100x100;
            this.LogoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.LogoPictureBox.Size = new System.Drawing.Size(100, 100);
            this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LogoPictureBox.TabIndex = 2;
            this.LogoPictureBox.TabStop = false;
            // 
            // EmpNameLabel
            // 
            this.EmpNameLabel.AutoSize = true;
            this.EmpNameLabel.Location = new System.Drawing.Point(13, 153);
            this.EmpNameLabel.Name = "EmpNameLabel";
            this.EmpNameLabel.Size = new System.Drawing.Size(87, 13);
            this.EmpNameLabel.TabIndex = 3;
            this.EmpNameLabel.Text = "Employee Name:";
            // 
            // EmployNameTextBox
            // 
            this.EmployNameTextBox.Location = new System.Drawing.Point(136, 153);
            this.EmployNameTextBox.Name = "EmployNameTextBox";
            this.EmployNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.EmployNameTextBox.TabIndex = 4;
            // 
            // EmployeeIDLabel
            // 
            this.EmployeeIDLabel.AutoSize = true;
            this.EmployeeIDLabel.Location = new System.Drawing.Point(16, 183);
            this.EmployeeIDLabel.Name = "EmployeeIDLabel";
            this.EmployeeIDLabel.Size = new System.Drawing.Size(70, 13);
            this.EmployeeIDLabel.TabIndex = 5;
            this.EmployeeIDLabel.Text = "Employee ID:";
            // 
            // EmployeeIDTextBox
            // 
            this.EmployeeIDTextBox.Location = new System.Drawing.Point(136, 180);
            this.EmployeeIDTextBox.Name = "EmployeeIDTextBox";
            this.EmployeeIDTextBox.Size = new System.Drawing.Size(100, 20);
            this.EmployeeIDTextBox.TabIndex = 6;
            // 
            // HoursWorkedLabel
            // 
            this.HoursWorkedLabel.AutoSize = true;
            this.HoursWorkedLabel.Location = new System.Drawing.Point(16, 211);
            this.HoursWorkedLabel.Name = "HoursWorkedLabel";
            this.HoursWorkedLabel.Size = new System.Drawing.Size(79, 13);
            this.HoursWorkedLabel.TabIndex = 7;
            this.HoursWorkedLabel.Text = "Hours Worked:";
            // 
            // HoursWorkedTextBox
            // 
            this.HoursWorkedTextBox.Location = new System.Drawing.Point(136, 211);
            this.HoursWorkedTextBox.Name = "HoursWorkedTextBox";
            this.HoursWorkedTextBox.Size = new System.Drawing.Size(100, 20);
            this.HoursWorkedTextBox.TabIndex = 8;
            // 
            // TotalSalesTextBox
            // 
            this.TotalSalesTextBox.Location = new System.Drawing.Point(136, 238);
            this.TotalSalesTextBox.Name = "TotalSalesTextBox";
            this.TotalSalesTextBox.Size = new System.Drawing.Size(100, 20);
            this.TotalSalesTextBox.TabIndex = 9;
            // 
            // TotalSaleslabel
            // 
            this.TotalSaleslabel.AutoSize = true;
            this.TotalSaleslabel.Location = new System.Drawing.Point(19, 238);
            this.TotalSaleslabel.Name = "TotalSaleslabel";
            this.TotalSaleslabel.Size = new System.Drawing.Size(63, 13);
            this.TotalSaleslabel.TabIndex = 10;
            this.TotalSaleslabel.Text = "Total Sales:";
            // 
            // SalesBonusLabel
            // 
            this.SalesBonusLabel.AutoSize = true;
            this.SalesBonusLabel.Location = new System.Drawing.Point(19, 263);
            this.SalesBonusLabel.Name = "SalesBonusLabel";
            this.SalesBonusLabel.Size = new System.Drawing.Size(69, 13);
            this.SalesBonusLabel.TabIndex = 11;
            this.SalesBonusLabel.Text = "Sales Bonus:";
            // 
            // SalesBonusTextBox
            // 
            this.SalesBonusTextBox.Location = new System.Drawing.Point(136, 265);
            this.SalesBonusTextBox.Name = "SalesBonusTextBox";
            this.SalesBonusTextBox.ReadOnly = true;
            this.SalesBonusTextBox.Size = new System.Drawing.Size(100, 20);
            this.SalesBonusTextBox.TabIndex = 12;
            // 
            // CalculateButton
            // 
            this.CalculateButton.Location = new System.Drawing.Point(13, 377);
            this.CalculateButton.Name = "CalculateButton";
            this.CalculateButton.Size = new System.Drawing.Size(75, 23);
            this.CalculateButton.TabIndex = 13;
            this.CalculateButton.Text = "Calculate";
            this.CalculateButton.UseVisualStyleBackColor = true;
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(95, 377);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 23);
            this.NextButton.TabIndex = 14;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            // 
            // PrintButton
            // 
            this.PrintButton.Location = new System.Drawing.Point(180, 377);
            this.PrintButton.Name = "PrintButton";
            this.PrintButton.Size = new System.Drawing.Size(75, 23);
            this.PrintButton.TabIndex = 15;
            this.PrintButton.Text = "Print";
            this.PrintButton.UseVisualStyleBackColor = true;
            // 
            // Assignment1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 412);
            this.Controls.Add(this.PrintButton);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.CalculateButton);
            this.Controls.Add(this.SalesBonusTextBox);
            this.Controls.Add(this.SalesBonusLabel);
            this.Controls.Add(this.TotalSaleslabel);
            this.Controls.Add(this.TotalSalesTextBox);
            this.Controls.Add(this.HoursWorkedTextBox);
            this.Controls.Add(this.HoursWorkedLabel);
            this.Controls.Add(this.EmployeeIDTextBox);
            this.Controls.Add(this.EmployeeIDLabel);
            this.Controls.Add(this.EmployNameTextBox);
            this.Controls.Add(this.EmpNameLabel);
            this.Controls.Add(this.LogoPictureBox);
            this.Controls.Add(this.languageSelectionGroup);
            this.MinimumSize = new System.Drawing.Size(300, 450);
            this.Name = "Assignment1";
            this.Text = "Form1";
            this.languageSelectionGroup.ResumeLayout(false);
            this.languageSelectionGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox languageSelectionGroup;
        private System.Windows.Forms.RadioButton GermanSelectRadioBttn;
        private System.Windows.Forms.RadioButton FrenchSelectRadioBttn;
        private System.Windows.Forms.RadioButton EnglishSelectRadioBttn;
        private System.Windows.Forms.PictureBox LogoPictureBox;
        private System.Windows.Forms.Label EmpNameLabel;
        private System.Windows.Forms.TextBox EmployNameTextBox;
        private System.Windows.Forms.Label EmployeeIDLabel;
        private System.Windows.Forms.TextBox EmployeeIDTextBox;
        private System.Windows.Forms.Label HoursWorkedLabel;
        private System.Windows.Forms.TextBox HoursWorkedTextBox;
        private System.Windows.Forms.TextBox TotalSalesTextBox;
        private System.Windows.Forms.Label TotalSaleslabel;
        private System.Windows.Forms.Label SalesBonusLabel;
        private System.Windows.Forms.TextBox SalesBonusTextBox;
        private System.Windows.Forms.Button CalculateButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button PrintButton;
    }
}

