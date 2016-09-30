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

/// <summary>
/// Created by Ben mathew dunn
/// First final version created september 28, 2016
/// 
/// app is a simple calculation for sales bonus for the mail order 
/// companies employees... if that company actually exists.
/// it's assignment 1, but I added some background jazz.
/// 
/// Added functionality over and above the assignment:
/// -added a threaded exception handler, a threaded updater for the position of the 
/// components. It's next to impossible to crash because of this.
/// 
/// -Loads language files from a outside text file, and deals with it 
/// pretty well when you 
/// 
/// -aborts safely with no memmorry leaks (as far as I can tell). 
/// 
/// mostly did these updates for pratice, however I had a nightmare changing 
/// the name space, (leading to me thanking god github exists). So I couldn;t
/// get it to change the namespace to an aptly named program rather then
/// 'SharpMailOrderProject'. 
/// 
/// </summary>
namespace SharpMailOrderProject
{
    
    public partial class MailOrder : Form
    {
        public MailOrder()
        {
            //default boot balues to prevent null pointer errors amongst the error stack
            this._FirstIssue = new Exception("no error");
            this._SecondIssue = this._FirstIssue;

            InitializeComponent();
            //init space holders into real values 
            this.supportedLanguages = new List<Dictionary<int, string>>();
            this._workingDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            this._loadLanguages(_workingDir + "\\languages");
            this._setRadioButtonNames();

            // starts the threaded listener for error handling, as in this case I 
            // do not have any real error handling needed, I just catch them for
            // example. This is mostly so my console doesn't get flooded with 
            // error messages, and I can show the important cases to the user
            // once. 
            this._errorHandleThread = new System.Threading.Thread(
                new ThreadStart(this._listenAndInformErrors));
            this._errorHandleThread.Start();


            // starts the threaded listener for updates in GUI components
            this._radioButtonThreadListener = new System.Threading.Thread(
                new ThreadStart(this._listenForRadioButtonChangesLanguageSelectionGroupSizing));
            this._radioButtonThreadListener.Start();

        }

        /// <summary>
        /// generic error handling thread, moves the first exception into the second
        /// to not pop the error warning twice and writes a message to pop up for the
        /// user to see. 
        /// </summary>
        private void _listenAndInformErrors()
        {
            do
            {

                if (!(this._FirstIssue.Equals(this._SecondIssue)))
                {
                    this._SecondIssue = this._FirstIssue; // make them the same, to prevent further catching
                    MessageBox.Show(this._SecondIssue.Message, "ERROR!");
                    this._clearControlTextBox(this.SalesBonusTextBox); // clear the bonus text box;
                }
                Thread.Sleep(10);
            } while (this.appRunning);
        }

        // Validation methods Private Methods (Universal)

        /// <summary>
        /// Converts the string to a number, and throws an error to the error thread 
        /// if a conversion error happens.Includes a reference to the control to make it come to focus
        /// to easily allow the user to find the correct one. 
        /// </summary>
        /// <param name="number">a string that represents a double or another number value type</param>
        /// <param name="control">a string that represents the control, null means no error will be thrown</param>
        /// <returns>the double value</returns>
        /// <param name="targetControl">the actual targeted control</param>
        private double _convertToNumber(string number, string control, TextBox targetControl)
        {
            try
            {
                return Convert.ToDouble(number);
            }
            catch (Exception)
            {
                if (!control.Equals(""))
                {
                    this._FirstIssue = this._FirstIssue =
                        new System.FormatException("Value was not a valid value in following field: "
                        + control);
                    this._bringToFocusAndClear(targetControl);

                }
                return 0; //return nothing on failed catch.
            }
        }


        /// <summary>
        /// returns a boolean representing if the value is less then zero.
        /// </summary>
        /// <param name="value">any double</param>
        /// <returns>check if less then zero greater then 0</returns>
        private bool _checkForNegitiveBool(double value, string control, TextBox textbox)
        {
            if (value < 0)
            {
                this._bringToFocusAndClear(textbox);
                this._FirstIssue = new System.ArgumentException("Value of " + control + " was less then 0");
                return false;
            }
            else { return true; }
        }

        /// <summary>
        /// Makes sure that the value is within bounds
        /// </summary>
        /// <param name="low">lowest value</param>
        /// <param name="high">highest value</param>
        /// <param name="value"> the value</param>
        /// <param name="value"> control name for error throw (if null no error throw)</param>
        /// <returns></returns>
        private bool _valueIsWithinBounds(double low, double high, double value, string target, TextBox textbox)
        {
            if (value > high || value < low)
            {
                if (!target.Equals(""))
                {
                    this._bringToFocusAndClear(textbox);
                    this._FirstIssue = new Exception("The format of " + target + " field is invalid! Values must be greater then 0 for all fields, and less then 160 for hours worked!");
                }

                return false;
            }
            return true;
        }


        /// <summary>
        /// calculates the bonus for the employee
        /// </summary>
        /// <param name="totalHoursWorked"> total employee hours</param>
        /// <param name="maxMonthWork">max month hours possible</param>
        /// <param name="monthlySales">monthly sales</param>
        /// <param name="percentBonus">the bonus as a percent</param>
        /// <returns></returns>
        private double _calculateSalesBonus(double totalHoursWorked, double maxMonthWork, double monthlySales, double percentBonus)
        {
            double percentHoursWorked = totalHoursWorked / maxMonthWork;
            double totalBonusAmount = percentBonus * monthlySales;
            return percentHoursWorked * totalBonusAmount;
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
                if (this.EnglishSelectRadioBttn.Checked)
                {
                    //Console.WriteLine("reached english");
                    this._updateDisplayedLanguage(1);
                }
                else if (this.FrenchSelectRadioBttn.Checked)
                {
                    this._updateDisplayedLanguage(2);
                }
                else if (this.GermanSelectRadioBttn.Checked)
                {
                    //Console.WriteLine("reached german");
                    this._updateDisplayedLanguage(3);
                }
                else
                {
                    //default generic catch case is set to english
                    //Console.WriteLine("reached english");
                    this._updateDisplayedLanguage(1);
                }

                this._updateDisplay();
                Thread.Sleep(10);
            } while (appRunning);
        }

        /// <summary>
        /// Pulls the supported languages from the folder in the project, this method 
        /// is very simply written as I really like keeping file path reads as simple
        /// as possible for debugging. 
        /// </summary>
        /// <param name="path"></param>
        private void _loadLanguages(String Dir)
        {
            //Console.WriteLine(Dir);
            this.supportedLanguages.Add(this._EnglishLanguage);

            try
            {
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
                        //Console.WriteLine(line);
                        counter++;
                    }
                    this.supportedLanguages.Add(tempDic);
                }
            }
            catch (Exception) { this._FirstIssue = new System.Exception("Could not find the language file folder!"); ; }

        }

        /// <summary>
        /// sets the radio buttons off the loaded first three language files in the language folder
        /// </summary>
        private void _setRadioButtonNames()
        {
            try
            {
                this._setText(this.supportedLanguages[0][0], this.EnglishSelectRadioBttn);
            }
            catch (Exception)
            {
                this._setText("", this.EnglishSelectRadioBttn);
                this._FirstIssue = new System.Exception("A language file is missing or Corrupt!");
            }
            try
            {
                this._setText(this.supportedLanguages[1][0], this.FrenchSelectRadioBttn);
            }
            catch (Exception)
            {
                this._setText("", this.FrenchSelectRadioBttn);
                this._FirstIssue = new System.Exception("A language file is missing or Corrupt!");
            }
            try
            {
                this._setText(this.supportedLanguages[2][0], this.GermanSelectRadioBttn);
            }
            catch (Exception)
            {
                this._setText("", this.GermanSelectRadioBttn);
                this._FirstIssue = new System.Exception("A language file is missing or Corrupt!");
            }
        }

        /// <summary>
        /// Updatereoil,ks the langue based on the case 1st language is first alphanum loaded
        /// reference. 
        /// </summary>
        /// <param name="language"> the language as an int</param>
        private void _updateDisplayedLanguage(int language)
        {
            switch (language)
            {
                case 1:
                    try
                    {
                        this._updateTextLabelsForLanguage(this.supportedLanguages[0]);
                    }
                    catch (Exception)
                    { this._FirstIssue = new System.SystemException("File for this language was not found!"); }
                    break;
                case 2:
                    //code language 1
                    try
                    {
                        this._updateTextLabelsForLanguage(this.supportedLanguages[1]);
                    }
                    catch (Exception)
                    { this._FirstIssue = new System.SystemException("File for this language was not found!"); }
                    break;
                case 3:
                    //code language 2
                    try
                    {
                        this._updateTextLabelsForLanguage(this.supportedLanguages[2]);
                    }
                    catch (Exception)
                    { this._FirstIssue = new System.SystemException("File for this language was not found!"); }
                    break;

            }
        }

        /// <summary>
        /// a blunt method for changing the label text of the entire control in
        /// a single pass, just used to save some programming time. Simply put
        /// due to this control, a language could be added to the program in seconds.
        /// </summary>
        /// <param name="empname">name label text</param>
        private void _updateTextLabelsForLanguage(Dictionary<int, string> language)
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
            this._setLocation(new Point(this.Width - this.languageSelectionGroup.Width - _paddingConst, _paddingConst), this.languageSelectionGroup);
            this._setLocation(new Point(_paddingConst, _paddingConst), this.LogoPictureBox);

            //labels and text box positing

            //label and textbox for name
            this._setLabelAndBoxPositionsWidth(this.EmpNameLabel, this.EmployNameTextBox, 0); //first instance, no padding required
            //label and textbox for empID
            this._setLabelAndBoxPositionsWidth(this.EmployeeIDLabel, this.EmployeeIDTextBox,
                this.EmployeeIDLabel.Height + _paddingConst);
            //label and textbox for hours worked
            this._setLabelAndBoxPositionsWidth(this.HoursWorkedLabel, this.HoursWorkedTextBox,
                this.EmployeeIDLabel.Height * 2 + _paddingConst * 2);
            //label and textbox for total Sales
            this._setLabelAndBoxPositionsWidth(this.TotalSaleslabel, this.TotalSalesTextBox,
                this.EmployeeIDLabel.Height * 3 + _paddingConst * 3);
            //label and textbox for sales bonus, moved down +1 on padding
            this._setLabelAndBoxPositionsWidth(this.SalesBonusLabel, this.SalesBonusTextBox,
                this.EmployeeIDLabel.Height * 4 + _paddingConst * 5);

            //button locations
            this._setLocation(new Point(0,
                this.Height - _paddingConst * 2 - this.CalculateButton.Height), this.CalculateButton);

            this._setLocation(new Point(this.Width - this.Width * 2 / 3,
                this.Height - _paddingConst * 2 - this.NextButton.Height), this.NextButton);

            this._setLocation(new Point(this.Width - this.Width * 1 / 3,
                this.Height - _paddingConst * 2 - this.PrintButton.Height), this.PrintButton);

            //button sizes
            this._setWidthHeight(new Size(this.Width * 1 / 3 - _paddingConst, this.CalculateButton.Height),
                this.CalculateButton);

            this._setWidthHeight(new Size(this.Width * 1 / 3 - _paddingConst, this.NextButton.Height),
                this.NextButton);

            this._setWidthHeight(new Size(this.Width * 1 / 3 - _paddingConst, this.PrintButton.Height),
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
            this._setLocation(new Point(Convert.ToInt16(this.Width * .3 - label.Width / 2 - _paddingConst),
                Convert.ToInt16(this.Height * .4 + heightOffset)), label); //dynamically centers control
            this._setLocation(new Point(Convert.ToInt16(this.Width * .66 - textBox.Width / 2 + _paddingConst),
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

        /// <summary>
        /// invokes a clear at the input text box, used for multithreading clears.
        /// </summary>
        /// <param name="textbox">The text box to clear</param>
        private void _clearControlTextBox(TextBox textbox)
        {
            if (textbox.InvokeRequired)
            {
                textbox.Invoke(new Action(() => textbox.Clear()));
            }
            else
            {
                textbox.Clear();
            }
        }

        /// <summary>
        /// Brings error textbox into focus and cleans it's text.
        /// </summary>
        /// <param name="control">the control</param>
        private void _bringToFocusAndClear(TextBox textBox)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new Action(() => textBox.Focus()));
                textBox.Invoke(new Action(() => textBox.Clear()));
            }
            else {
                textBox.Focus();
                textBox.Clear();
            }
        }

        //Interactive events triggered by button events.

        /// <summary>
        /// Interactive events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculateButton_Click(object sender, EventArgs e)
        {
            double valueHours = (this._convertToNumber(this.HoursWorkedTextBox.Text, "Hours worked", this.HoursWorkedTextBox));
            double valueSales = (this._convertToNumber(this.TotalSalesTextBox.Text, "Total Sales", this.TotalSalesTextBox));

            if (this._valueIsWithinBounds(0, 160, valueHours, "Hours worked", this.HoursWorkedTextBox) &&
                this._checkForNegitiveBool(valueSales, "Total Sales2", this.TotalSalesTextBox) && valueHours != 0 && valueHours != 0)
            {
                this._setText(this._calculateSalesBonus(valueHours,
                    160, valueSales, 0.02).ToString("C2"), this.SalesBonusTextBox);
            }
            else if (valueHours != 0 || valueHours != 0)
            {
                this._FirstIssue = new Exception("A value within the one of the fields was left blank or was invalid");
            }
        }



        private void NextButton_Click(object sender, EventArgs e)
        {
            this.EmployeeIDTextBox.Clear();
            this.EmployNameTextBox.Clear();
            this.HoursWorkedTextBox.Clear();
            this.SalesBonusTextBox.Clear();
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The file is being sent to a 'printer'", "printing");
        }
    }


}
