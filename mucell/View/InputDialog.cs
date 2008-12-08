using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MuCell.View
{
    /// <summary>
    /// Pops up a dialog allowing the user to enter a string of input. Use ShowDialog() after creation.
    /// </summary>
    /// <owner>Jonathan</owner>
    public partial class InputDialog : Form
    {
        /// <summary>
        /// The input string that the user typed. Get this after calling ShowDialog()
        /// and verifying that the value returned was DialogResult.OK
        /// </summary>
        public string NewString { get { return textBox1.Text; } }

        /// <summary>
        /// A dialog allowing the user to enter a string of input. Use ShowDialog() after creation.
        /// </summary>
        /// <param name="label">A message to the user to inform them about what they're inputting</param>
        /// <param name="caption">The string to appear in the title bar of the dialog box</param>
        /// <param name="initialText">An initial string which appears in the text box when the dialog is shown</param>
        public InputDialog(string label, string caption, string initialText)
        {
            InitializeComponent();

            label1.Text = label;
            this.Text = caption;
            textBox1.Text = initialText;

            this.ClientSize = new Size(
                // As wide as the label requires, but not thinner than the width of the buttons
                Math.Max(label1.Size.Width + label1.Margin.Horizontal,
                btnOk.Size.Width + btnOk.Margin.Horizontal +
                btnCancel.Size.Width + btnCancel.Margin.Horizontal),

                // As tall as the label and other controls require
                label1.Size.Height + label1.Margin.Vertical +
                textBox1.Size.Height + textBox1.Margin.Vertical +
                btnOk.Size.Height + btnOk.Margin.Vertical);
        }

        private void InputDialog_Load(object sender, EventArgs e)
        {
            // Set focus to the text box so the user can begin typing right away
            textBox1.Focus();
            textBox1.SelectAll();
        }
    }
}