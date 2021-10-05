using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipsJSONConverter
{
    class Tip
    {
        //Fields
        private string _type; //The type of the tip.
        private string _text; //The text of the tip.
        private string[] _formatArray; //The format of the tip.

        /// <summary>
        /// Constructor: Initialises the fields to the values passed in.
        /// </summary>
        /// <param name="type">The name space of the tip.</param>
        /// <param name="text">The text to be displayed.</param>
        /// <param name="formatArray">An array of tip formatting instructions in JSON format.</param>
        public Tip(string type, string text, string[] formatArray)
        {
            _type = type;
            _text = text;
            
            if (formatArray[0] != "")
            {
                _formatArray = formatArray;
            }
        }

        /// <summary>
        /// Gets the type of the tip.
        /// </summary>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the text of the tip.
        /// </summary>
        public string Text
        {
            get { return _text; }
        }
        
        /// <summary>
        /// Writes the tip to a JSON file.
        /// </summary>
        /// <param name="path">Directory to write the JSON to.</param>
        /// <param name="i">Integer for file name.</param>
        public void WriteJson(string path, int i)
        {
            //If the directory for the tip's type doesn't exist, create it.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Create a new stream writer.
            StreamWriter writer;

            //Open a new file.
            writer = new StreamWriter(path + "\\" + _type + "tip" + i + ".json");
            
            //Write the tip in JSON format.
            writer.WriteLine("{");
            writer.WriteLine("\t\"tip\":");
            writer.WriteLine("\t{");
            writer.Write("\t\t\"text\":");
            writer.Write("\"" + _text + "\"");

            //Check if _formatArray is not null.
            if (_formatArray != null)
            {
                //For each format in _formatArray
                foreach (string format in _formatArray)
                {
                    //Write the current format.
                    writer.WriteLine(",");
                    writer.Write("\t\t" + format);
                }
            }
            
            writer.WriteLine();
            writer.WriteLine("\t}");
            writer.WriteLine("}");

            //Close the file.
            writer.Close();
        }
    }
}
