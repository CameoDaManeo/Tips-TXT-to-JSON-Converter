using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TipsJSONConverter
{
    public partial class Form1 : Form
    {
		//Constants and Class-Scope Variables
		List<Tip> tipList = new List<Tip>(); //A list to store the tips to.
		List<string> typeList = new List<string>(); //A list to store the types of tips to.

		public Form1()
        {
            InitializeComponent();
			
			//Variables
			OpenFileDialog openFileDialog = new OpenFileDialog();
			StreamWriter logger;
			StreamReader reader;
			string streamFilePath, tipsFolderPath, individualTipPath;
			string line;
			string[] txtArray;
			string type, text;
			string[] formatArray;

			//Set the filter and title for the open file dialog.
			openFileDialog.Filter = "TXT Files|*.TXT|All Files|*.*";
			openFileDialog.Title = "Open TXT file with tips on each line.";

			//Check if the user has selected a stream file.
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				//Record the stream file path.
				streamFilePath = openFileDialog.FileName;
				
				//Create the tips folder path, based on the stream file path and name.
				tipsFolderPath = Path.GetDirectoryName(streamFilePath) + "\\" + Path.GetFileNameWithoutExtension(streamFilePath);
				
				//If the tips folder does not exist, create it.
				if (!Directory.Exists(tipsFolderPath))
				{
					Directory.CreateDirectory(tipsFolderPath);
				}
				
				//Open a log file in the tips folder.
				logger = new StreamWriter(tipsFolderPath + "\\" + "log.txt");

				//Write the path information to the log file.
				logger.WriteLine("Tips generated from txt file at: \"" + streamFilePath + "\"");

				//Open the selected file to stream.
				reader = File.OpenText(streamFilePath);

				//Repeat while it is not the end of the file.
				while (!reader.EndOfStream)
				{
					try
					{
						//Read a line from the file.
						line = reader.ReadLine();

						//Split the line.
						txtArray = line.Split('|');

						//Check the size of the array.
						if (txtArray.Length == 3)
						{
							//Get the data from txtArray.
							type = txtArray[0].ToLower();
							text = txtArray[1];
							formatArray = txtArray[2].Split(',');

							//If type is an empty string, change it to "tip".
							if (type == "")
							{
								type = "my";
							}
							
							//Create a new tip (if valid), add it to the tip list, and add any new tip types to the type list.
							AddTip(type, text, formatArray);
						}
						else if(txtArray.Length == 1)
                        {
							//Get the data from txtArray.
							type = "my";
							text = txtArray[0];
							formatArray = new string[] { "" };

							//Create a new tip (if valid), add it to the tip list, and add any new tip types to the type list.
							AddTip(type, text, formatArray);
						}
						else
						{
							logger.WriteLine("Line was not in the correct format: \"" + line + "\"");
						}
					}
					catch (Exception ex)
					{
						logger.WriteLine(ex.Message);
					}
				}

				//Close the stream file.
				reader.Close();
				
				//For each type in the type list.
				foreach (string t in typeList)
				{
					int j = 0;

					individualTipPath = tipsFolderPath + "\\" + "assets" + "\\" + t + "\\" + "tips";

					//Write the folder path of the current tips that are being written to the log file.
					logger.WriteLine();
					logger.WriteLine("\"" + individualTipPath + "\":");

					//For each tip in the tip list.
					foreach (Tip tip in tipList)
					{
						//If the type of the tip matches the current tip.
						if (tip.Type == t)
						{
							//Write the tip to a JSON file.
							tip.WriteJson(individualTipPath, j);

							//Write the tip to the log file.
							logger.WriteLine(tip.Text);

							//Increase j by 1.
							j++;
						}
					}
				}

				//Close the logger.
				logger.Close();

				//Open the directory in Windows Explorer.
				Process.Start(tipsFolderPath);
			}
		}

		/// <summary>
		/// Creates a new tip (if valid), adds it to the tip list, and adds any new tip types to the type list.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="text"></param>
		/// <param name="formatArray"></param>
		private void AddTip(string type, string text, string[] formatArray)
        {
			//Check if text is not an empty string.
			if (text != "")
			{
				//Create a new Tip and add it to the tip list.
				Tip tip = new Tip(type, text, formatArray);
				tipList.Add(tip);

				//If the tip's type is not contained in the types list, then add it.
				if (!typeList.Contains(type))
				{
					typeList.Add(type);
				}
			}
		}

		private void Form1_Load(object sender, EventArgs e)
        {
			Application.Exit();
        }
    }
}
