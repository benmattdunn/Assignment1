using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    
    public partial class Assignment1 : Form
    {
        public Assignment1()
        {
            InitializeComponent();
            //init space holders into real values 
            this.supportedLanguages = new List<Dictionary<int, string>>();
            this.workingDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            this._loadLanguages(workingDir + "\\languages");
            this._setRadioButtonNames();

            // starts the threaded listener for updates in GUI components
            this.radioButtonThreadListener = new System.Threading.Thread(
                new ThreadStart(this._listenForRadioButtonChangesLanguageSelectionGroupSizing));
            this.radioButtonThreadListener.Start();
        }








        // Validation methods Private Methods


        /// <summary>
        /// Changes the value to a 0 if below 0 to prevent negitive numbers
        /// from being thrown into the system of calculation.
        /// </summary>
        /// <param name="value">any double</param>
        /// <returns>double greater then 0</returns>
        private double _checkForNegitive (double value)
        {
            if (value < 0 )
            { 
                return 0;
            } else { return value; }
        }




        // DISPLAY UPDATE PRIVATE METHODS 

        /// <summary>
        /// listens for changes on the value of the language change button group and updates as apropriately
        /// the text displays for the display. This method had no inputs or outputs as it's called by a 
        /// seperate thread for execution of the listener code. In addition, updates the location
        /// of the display components incase a user changes the size of the window (via method call);
        /// </summary>
        private void _listenForRadioButtonChangesLanguageSelectionGroupSizing()
        {
            //Console.WriteLine("listener running");
            do
            {
                if( this.EnglishSelectRadioBttn.Checked)
                {
                    //Console.WriteLine("reached english");
                    this._updateDisplayedLanguage(1);
                } else if (this.FrenchSelectRadioBttn.Checked)
                {
                    this._updateDisplayedLanguage(2);
                } else if (this.GermanSelectRadioBttn.Checked)
                {
                    //Console.WriteLine("reached german");
                    this._updateDisplayedLanguage(3);
                } else
                {
                    //default generic catch case is set to english
                    //Console.WriteLine("reached english");
                    this._updateDisplayedLanguage(1);
                }
                
                this._updateDisplay(); 
                Thread.Sleep(10);
            } while (true);
        }

        /// <summary>
        /// Pulls the supported languages from the folder in the project, this method 
        /// is very simply written as I really like keeping file path reads as simple
        /// as possible for debugging. 
        /// </summary>
        /// <param name="path"></param>
        private void _loadLanguages(String Dir)
        {
            Console.WriteLine(Dir);
            this.supportedLanguages.Add(this.EnglishLanguage);
            
            string[] filePaths = Directory.GetFiles(Dir);
            foreach (String filepath in filePaths)
            {
                Dictionary<int, string> tempDic = new Dictionary<int, string>();
                  Console.WriteLine(filepath);
                int counter = 0;
                string line;

                // Read the file and display it line by line.
                StreamReader file = new StreamReader(filepath);
                while ((line = file.ReadLine()) != null)
                {
                    tempDic.Add(counter, line);
                    Console.WriteLine(line);
                    counter++;
                }
                this.supportedLanguages.Add(tempDic);
            }


        }
 
        /// <summary>
        /// sets the radio buttons off the loaded first three language files in the language folder
        /// </summary>
        private void _setRadioButtonNames()
        {
            try {
                this._setText(this.supportedLanguages[0][0], this.EnglishSelectRadioBttn); }
            catch(Exception ex) {
                this._setText("", this.EnglishSelectRadioBttn);
            }
            try {
                this._setText(this.supportedLanguages[1][0], this.FrenchSelectRadioBttn);
            } catch(Exception ex)
            {
                this._setText("", this.FrenchSelectRadioBttn);
            }
            try {
                this._setText(this.supportedLanguages[2][0], this.GermanSelectRadioBttn);
            }
            catch (Exception ex)
            {
                this._setText("", this.GermanSelectRadioBttn);
            }
        }

        /// <summary>
        /// Updatereoil,ks the langue based on the case 1st language is first alphanum loaded
        /// reference. 
        /// </summary>
        /// <param name="language"> the language as an int</param>
        private void _updateDisplayedLanguage(int language)
        {
            switch (language) {
                case 1:
                    try {
                        this._updateTextLabelsForLanguage(this.supportedLanguages[0]); }
                    catch (Exception e) { }
                break;
                case 2:
                    //code language 1
                    try {
                        this._updateTextLabelsForLanguage(this.supportedLanguages[1]);
                    } catch (Exception e) { }
                    break;
                case 3:
                    //code language 2
                    try{
                        this._updateTextLabelsForLanguage(this.supportedLanguages[2]);
                    }catch (Exception e) { }
                    break;

            }
        }

        /// <summary>
        /// a blunt method for changing the label text of the entire control in
        /// a single pass, just used to save some programming time. Simply put
        /// due to this control, a language could be added to the program in seconds.
        /// </summary>
        /// <param name="empname">name label text</param>
       private void _updateTextLabelsForLanguage (Dictionary<int, string> language)
       {
            this._setText(language[1], this.EmpNameLabel);
            this._setText(language[2], this.EmployeeIDLabel);
            this._setText(language[3], this.HoursWorkedLabel);
            this._setText(language[4], this.TotalSaleslabel);
            this._setText(language[5], this.SalesBonusLabel);
            this._setText(language[6], this.CalculateButton);
            this._setText(language[7], this.NextButton);
            this._setText(language[8], this.PrintButton);
       }

        /// <summary>
        /// void method for updating new location of the various controls as the window size shifts if the user 
        /// changes it. Most of these methods are non-dynamic as they chain for generic functions such as
        /// the _setLabelAndBoxPositionsWidth methods. 
        /// </summary>
        private void _updateDisplay()
        {
            this._setLocation(new Point(this.Width - this.languageSelectionGroup.Width - paddingConst, paddingConst), this.languageSelectionGroup);
            this._setLocation(new Point(paddingConst, paddingConst), this.LogoPictureBox);

            //labels and text box positing

            //label and textbox for name
            this._setLabelAndBoxPositionsWidth(this.EmpNameLabel, this.EmployNameTextBox, 0); //first instance, no padding required
            //label and textbox for empID
            this._setLabelAndBoxPositionsWidth(this.EmployeeIDLabel, this.EmployeeIDTextBox, 
                this.EmployeeIDLabel.Height+paddingConst);
            //label and textbox for hours worked
            this._setLabelAndBoxPositionsWidth(this.HoursWorkedLabel, this.HoursWorkedTextBox,
                this.EmployeeIDLabel.Height * 2 + paddingConst * 2);
            //label and textbox for total Sales
            this._setLabelAndBoxPositionsWidth(this.TotalSaleslabel, this.TotalSalesTextBox,
                this.EmployeeIDLabel.Height * 3 + paddingConst * 3);
            //label and textbox for sales bonus, moved down +1 on padding
            this._setLabelAndBoxPositionsWidth(this.SalesBonusLabel, this.SalesBonusTextBox,
                this.EmployeeIDLabel.Height * 4 + paddingConst * 5);

            //button locations
            this._setLocation(new Point(0, 
                this.Height - paddingConst*2 - this.CalculateButton.Height), this.CalculateButton);

            this._setLocation(new Point(this.Width-this.Width*2/3,
                this.Height - paddingConst * 2 - this.NextButton.Height), this.NextButton);

            this._setLocation(new Point(this.Width - this.Width * 1 / 3,
                this.Height - paddingConst * 2 - this.PrintButton.Height), this.PrintButton);

            //button sizes
            this._setWidthHeight(new Size(this.Width * 1 / 3 -paddingConst, this.CalculateButton.Height), 
                this.CalculateButton);

            this._setWidthHeight(new Size(this.Width * 1 / 3-paddingConst, this.NextButton.Height),
                this.NextButton);

            this._setWidthHeight(new Size(this.Width * 1 / 3-paddingConst, this.PrintButton.Height),
                this.PrintButton);

        }

        /// <summary>
        /// does the blunt of the calculations for figuring out the relationship between the form, lable, and text boxes
        /// </summary>
        /// <param name="label">a lable</param>
        /// <param name="textBox">a text box</param>
        /// <param name="heightOffset"> an offset for height</param>
        private void _setLabelAndBoxPositionsWidth(Label label, TextBox textBox, int heightOffset)
        {
            this._setLocation(new Point(Convert.ToInt16(this.Width * .3 - label.Width / 2 - paddingConst),
                Convert.ToInt16(this.Height * .4 + heightOffset)), label); //dynamically centers control
            this._setLocation(new Point(Convert.ToInt16(this.Width * .66 - textBox.Width / 2 + paddingConst),
                Convert.ToInt16(this.Height * .4 + heightOffset)), textBox);
            this._setWidthHeight(new Size(this.Width / 3, label.Height), label); //dropped the convert for ease
            this._setWidthHeight(new Size(this.Width / 3, textBox.Height), textBox);
        }

        /// <summary>
        /// Takes a control and allows the generic update of the control via a thread or an outside 
        /// class. Allows the invoke of a new action aimed at the control of the text field.
        /// this class may be made more generic later on. Used a lambda to invoke a new
        /// action on the object (control). Added to the form to allow easy outside access of methods
        /// and display. However, set to private for the time being as I would want to validate this
        /// with another method if I made it public. 
        /// </summary>
        /// <param name="text">a text parrameter</param>
        /// <param name="control">Referenced control</param>
        private void _setText(string text, Control control)
        {
            if (control.InvokeRequired)
                control.Invoke(new Action(() => control.Text = text));
            else
                control.Text = text;
        }


        /// <summary>
        /// as above but with new location, (point); 
        /// shifts the position of objects as needed, generic call method for all control types. 
        /// </summary>
        /// <param name="location">new Point location</param>
        /// <param name="control">Referenced control</param>
        private void _setLocation(Point location, Control control)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => control.Location = location)); //invoke a lambda/anonymous/delegate
            }
            else {
                control.Location = location; //just change the location if an invoke is not required. 
            }
        }


        /// <summary>
        /// as above but with new size; in this assignment I only intend to streatch things horizontally. (labels,
        /// and text boxes); 
        /// </summary>
        /// <param name="size">new size</param>
        /// <param name="control">Referenced control</param>
        private void _setWidthHeight(Size size, Control control)
        {
            //Console.WriteLine(size.Height);
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => control.Size = size)); //invoke a lambda/anonymous/delegate
            }
            else {
                control.Size = size; //just change the location if an invoke is not required. 
            }
        }

    }


}
