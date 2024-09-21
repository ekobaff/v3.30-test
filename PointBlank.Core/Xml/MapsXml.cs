﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Xml.MapsXml
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Models.Map;
using System.IO;
using System.Xml;

namespace PointBlank.Core.Xml
{
  public class MapsXml
  {
    public static void Load()
    {
      MapsXml.MatchParse();
      MapsXml.RuleParse();
    }

    private static void MatchParse()
    {
      string path = "Data/Maps/Matchs.xml";
      if (File.Exists(path))
      {
        XmlDocument xmlDocument = new XmlDocument();
        using (FileStream inStream = new FileStream(path, FileMode.Open))
        {
          if (inStream.Length == 0L)
          {
            Logger.error("File is empty: " + path);
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
                    if ("Map".Equals(xmlNode2.Name))
                    {
                      XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                      MapModel.Matchs.Add(new MapMatch()
                      {
                        Mode = int.Parse(attributes.GetNamedItem("Mode").Value),
                        Id = int.Parse(attributes.GetNamedItem("Id").Value),
                        Limit = int.Parse(attributes.GetNamedItem("Limit").Value),
                        Tag = int.Parse(attributes.GetNamedItem("Tag").Value)
                      });
                    }
                  }
                }
              }
            }
            catch (XmlException ex)
            {
              Logger.error(ex.ToString());
            }
          }
          inStream.Dispose();
          inStream.Close();
        }
      }
      else
        Logger.error("File not found: " + path);
    }

    private static void RuleParse()
    {
      string path = "Data/Maps/Rules.xml";
      if (File.Exists(path))
      {
        XmlDocument xmlDocument = new XmlDocument();
        using (FileStream inStream = new FileStream(path, FileMode.Open))
        {
          if (inStream.Length == 0L)
          {
            Logger.error("File is empty: " + path);
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
                    if ("Map".Equals(xmlNode2.Name))
                    {
                      XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                      MapModel.Rules.Add(new MapRule()
                      {
                        Id = int.Parse(attributes.GetNamedItem("Id").Value),
                        Rule = int.Parse(attributes.GetNamedItem("Rule").Value),
                        StageOptions = int.Parse(attributes.GetNamedItem("StageOptions").Value),
                        Conditions = int.Parse(attributes.GetNamedItem("Conditions").Value),
                        Name = attributes.GetNamedItem("Name").Value
                      });
                    }
                  }
                }
              }
            }
            catch (XmlException ex)
            {
              Logger.error(ex.ToString());
            }
          }
          inStream.Dispose();
          inStream.Close();
        }
      }
      else
        Logger.error("File not found: " + path);
    }
  }
}
