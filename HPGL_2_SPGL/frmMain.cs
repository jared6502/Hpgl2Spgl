using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HPGL_2_SPGL
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.FileName = txtInputFile.Text;

            DialogResult result = ofd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtInputFile.Text = ofd.FileName;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(txtOutputFile.Text))
            {
                DialogResult result = MessageBox.Show("Replace existing file?", "File Already Exists", MessageBoxButtons.OKCancel);
                if (result != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
            }

            Convert();
        }

        void Convert()
        {
            try
            {
                using (System.IO.StreamReader infile = new System.IO.StreamReader(new System.IO.FileStream(txtInputFile.Text, System.IO.FileMode.Open)))
                using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(new System.IO.FileStream(txtOutputFile.Text, System.IO.FileMode.Create)))
                {
                    string input;
                    string output;
                    bool absolute = true;
                    bool up = true;

                    while (!infile.EndOfStream)
                    {
                        input = infile.ReadLine();

                        switch(input.Substring(0, 2))
                        {
                            case "IN":
                                output = "IN;";
                                outfile.WriteLine(output);
                                break;

                            case "PU":
                                if (input == "PU;")
                                {
                                    output = input;
                                    outfile.WriteLine(output);
                                }
                                else
                                {
                                    output = "PU;";
                                    outfile.WriteLine(output);
                                    output = (absolute ? "MA " : "MR ") + input.Substring(2, input.Length - 2);
                                    outfile.WriteLine(output);
                                }

                                up = true;
                                break;

                            case "PD":
                                if (input == "PD;")
                                {
                                    output = input;
                                    outfile.WriteLine(output);
                                }
                                else
                                {
                                    output = "PD;";
                                    outfile.WriteLine(output);
                                    output = (absolute ? "DA " : "DR ") + input.Substring(2, input.Length - 2);
                                    outfile.WriteLine(output);
                                }

                                up = false;
                                break;

                            case "PA":
                                absolute = true;

                                if (input == "PA;")
                                {
                                    //swallow this command
                                }
                                else
                                {
                                    output = (up ? "M" : "D") + (absolute ? "A " : "R ") + input.Substring(2, input.Length - 2);
                                    outfile.WriteLine(output);
                                }
                                break;

                            case "PR":
                                absolute = false;

                                if (input == "PR;")
                                {
                                    //swallow this command
                                }
                                else
                                {
                                    output = (up ? "M" : "D") + (absolute ? "A " : "R ") + input.Substring(2, input.Length - 2);
                                    outfile.WriteLine(output);
                                }
                                break;

                            case "AA":
                                //TODO
                                break;

                            case "AR":
                                //TODO
                                break;

                            case "VS":
                                output = input;
                                outfile.WriteLine(output);
                                break;

                            default:
                                //command dropped
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception thrown:\n" + ex.Message, "Error during conversion");
            }
        }
    }
}
