//using System;
//using System.Xml;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;

//using FNA.Graphics;

//namespace FNA.AnimationEditor
//{
//    public static class XMLFunctions
//    {
//        public static void ExportAnimation(string filename, Dictionary<string, Animation> animations)
//        {
//            XmlWriterSettings settings = new XmlWriterSettings();
//            settings.Indent = true;
//            settings.NewLineOnAttributes = true;
//            settings.Encoding = Encoding.UTF8;
//            XmlWriter writer = XmlWriter.Create(filename, settings);

//            foreach (KeyValuePair<string, Animation> animationPair in animations)
//            {
//                Animation animation = animationPair.Value;

//                writer.WriteStartElement("Animation");

//                writer.WriteAttributeString("Name", animation.Name);
//                //string shortFilename = animation.Filename.Substring(animation.Filename.IndexOf("\\Content\\")+9);
//                //writer.WriteAttributeString("Filename", animation.Filename);
//                //string motion = animation.MotionDelta.X.ToString() + "," + animation.MotionDelta.Y.ToString();
//                //writer.WriteAttributeString("Motion", motion);

//                //foreach (KeyValuePair<int, KeyFrame> pair in animation.Frames)
//                //{
//                    //writer.WriteStartElement("Direction", pair.Key);

//                    //List<Keyframe> kf = pair.Value;
//                    //for (int i = 0; i < kf.Count; i++)
//                    //{
//                    //    writer.WriteStartElement("Keyframe");

//                    //    writer.WriteAttributeString("Duration", kf[i].FrameDuration.ToString());

//                    //    string rectangle = kf[i].Rect.X.ToString() + "," + kf[i].Rect.Y.ToString() + "," +
//                    //                       kf[i].Rect.Width.ToString() + "," + kf[i].Rect.Height.ToString();
//                    //    writer.WriteAttributeString("Rectangle", rectangle);
                        
//                    //    writer.WriteEndElement();
//                    //}

//                    //writer.WriteEndElement();
//                //}
//                writer.WriteEndElement();
//            }

//            writer.Close();
//        }

//        public static Dictionary<string, Animation> LoadAnimationsFromFile(string filename)
//        {
//            Dictionary<string, Animation> result = new Dictionary<string, Animation>();

//            // Load the document
//            XmlDocument doc = new XmlDocument();
//            XmlTextReader reader = new XmlTextReader(filename);
//            try
//            {
//                doc.Load(reader);
//            }
//            catch (System.Exception ex)
//            {
//                string s = ex.Message;
//                return null;
//            }

//            // Get elements
//            XmlNodeList animationList = doc.GetElementsByTagName("Animation");

//            foreach (XmlNode node in animationList)
//            {
//                Animation newAnimation = new Animation();

//                newAnimation.Name = node.Attributes["Name"].InnerText;
//                //newAnimation.Filename = node.Attributes["Filename"].InnerText;

//                string motion = node.Attributes["Motion"].InnerText;
//                char token = ',';
//                string[] values = motion.Split(token);
//                float motionX = (float)Convert.ToDouble(values[0]);
//                float motionY = (float)Convert.ToDouble(values[1]);
//                //newAnimation.SetMotionDelta(motionX, motionY);

//                //    for (int i = 0; i < enemyList.Count; i++)
//                //    {
//                //        EnemyRepresentation newEnemy = new EnemyRepresentation(i);

//                //        //EnemyType newEnemyType = (EnemyType)Enum.Parse(typeof(EnemyType), enemyList[i].Attributes["Type"].InnerText, true);
//                //        string newEnemyType = enemyList[i].Attributes["Type"].InnerText;
//                //        newEnemy.Type = newEnemyType;

//                //        string newEnemyPositionVector = enemyList[i].Attributes["Offset"].InnerText;
//                //        string[] values = newEnemyPositionVector.Split(new char[] { ',' });

//                //        float newEnemyXPosition = (float)Convert.ToSingle(values[0]);
//                //        newEnemy.XUnitPosition = newEnemyXPosition;
//                //        newEnemyXPosition *= 200.0f;
//                //        newEnemyXPosition += 200.0f;

//                //        float newEnemyYPosition = (float)Convert.ToSingle(values[1]);
//                //        newEnemy.YUnitPosition = newEnemyYPosition;
//                //        newEnemyYPosition *= 200.0f;
//                //        newEnemyYPosition = 200.0f - newEnemyYPosition;

//                //        newEnemy.X = newEnemyXPosition;
//                //        newEnemy.Y = newEnemyYPosition;

//                //        float newEnemyAngle = (float)Convert.ToSingle(enemyList[i].Attributes["Angle"].InnerText);
//                //        newEnemy.Angle = newEnemyAngle;

//                //        newAnimation.Enemies.Add(newEnemy);
//                //    }

//                result.Add(newAnimation.Name, newAnimation);
//            }

//            reader.Close();
//            return result;
//        }
//    }
//}