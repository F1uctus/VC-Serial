using System;
using System.Xml;

namespace Serial
{
    /// <summary>
    /// options will be loaded automatically when this is created
    /// </summary>
    public class PluginOptions
    {
        //define variable to hold the options you need to keep track of here
        public int Bauds;
        public string Port;
        public bool GenEventOnReceive;
        public bool ConcateStrings;
        public int ConcatenationInterval;
        public bool DTRenabled;

        //this is used to load and save options to the correct folder
        private readonly string OptionsPath;

        public PluginOptions()
        {
            //set default values
            Bauds = 9600;
            Port = "";
            GenEventOnReceive = true;
            ConcateStrings = false;
            ConcatenationInterval = 100;
            DTRenabled = false;

            //find the correct folder and store it for later use by the load and save methods
            OptionsPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Options.xml";
            if (System.IO.File.Exists(OptionsPath)) LoadOptionsFromXml();
        }
        /// <summary>
        /// saves all our options
        /// must create a case for each value to load
        /// </summary>
        private void LoadOptionsFromXml()
        {
            XmlDocument OptionsXML = new XmlDocument();
            OptionsXML.Load(OptionsPath);
            foreach (XmlNode Option in OptionsXML.DocumentElement.SelectNodes("Option"))
            {
                string OptionValue = Option.Attributes["value"].Value;
                switch (Option.Attributes["name"].Value)
                {
                    case "Bauds":
                        Bauds = Convert.ToInt32(OptionValue);
                        break;
                    case "Port":
                        Port = OptionValue;
                        break;
                    case "GenEventOnReceive":
                        GenEventOnReceive = Convert.ToBoolean(OptionValue);
                        break;
                    case "ConcateStrings":
                        ConcateStrings = Convert.ToBoolean(OptionValue);
                        break;
                    case "ConcatenationInterval":
                        ConcatenationInterval = Convert.ToInt32(OptionValue);
                        break;
                    case "DTRenabled":
                        DTRenabled = Convert.ToBoolean(OptionValue);
                        break;
                }
            }
        }
        public void SaveOptionsToXml()
        {
            // Create the XML document and write the comments and root node "AllOptions"
            using (XmlTextWriter writer = new XmlTextWriter(OptionsPath, new System.Text.ASCIIEncoding())
            {
                Formatting = Formatting.Indented,
                Indentation = 4
            })
            {
                writer.WriteStartDocument();
                writer.WriteComment("Serial plugin options");
                writer.WriteStartElement("Options");
                {
                    WriteOption(writer, "Bauds", Bauds.ToString());
                    WriteOption(writer, "Port", Port);
                    WriteOption(writer, "GenEventOnReceive", GenEventOnReceive.ToString());
                    WriteOption(writer, "ConcateStrings", ConcateStrings.ToString());
                    WriteOption(writer, "ConcatenationInterval", ConcatenationInterval.ToString());
                    WriteOption(writer, "DTRenabled", DTRenabled.ToString());
                }
                writer.WriteEndElement();//options
                writer.WriteEndDocument();
            }
        }
        private static void WriteOption(XmlWriter writer, string nodeName, string nodeValue)
        {
            writer.WriteStartElement("Option");
            writer.WriteAttributeString("name", nodeName);
            writer.WriteAttributeString("value", nodeValue);
            writer.WriteEndElement();
        }
    }
}
