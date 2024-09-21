// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Xml.QuickStartXml
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Models.Servers;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace PointBlank.Core.Xml
{
  public class QuickStartXml
  {
    public static List<QuickStart> QucikStarts = new List<QuickStart>(3);

    public static void Load()
    {
      string str = "Data//QuickStart.xml";
      if (File.Exists(str))
        QuickStartXml.Parse(str);
      else
        Logger.error("File not found: " + str);
    }

    public static void Parse(string Path)
    {
      XmlDocument xmlDocument = new XmlDocument();
      FileStream inStream = new FileStream(Path, FileMode.Open);
      if (inStream.Length == 0L)
      {
        Logger.error("File is Empty: " + Path);
      }
      else
      {
        try
        {
          xmlDocument.Load((Stream) inStream);
          for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
          {
            if ("List".Equals(xmlNode1.Name))
            {
              for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
              {
                if ("QuickStart".Equals(xmlNode2.Name))
                {
                  XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                  QuickStartXml.QucikStarts.Add(new QuickStart()
                  {
                    MapId = int.Parse(attributes.GetNamedItem("MapId").Value),
                    Rule = int.Parse(attributes.GetNamedItem("Rule").Value),
                    StageOptions = int.Parse(attributes.GetNamedItem("StageOptions").Value),
                    Type = int.Parse(attributes.GetNamedItem("Type").Value)
                  });
                }
              }
            }
          }
        }
        catch (XmlException ex)
        {
          Logger.warning(ex.ToString());
        }
      }
      inStream.Dispose();
      inStream.Close();
    }
  }
}
