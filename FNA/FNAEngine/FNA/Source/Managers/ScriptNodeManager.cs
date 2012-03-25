using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

using FNA.Scripts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using FNA.Triggers;

namespace FNA.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class ScriptNodeManager_cl : BaseManager_cl
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly ScriptNodeManager_cl sInstance = new ScriptNodeManager_cl();

        /// <summary>
        /// 
        /// </summary>
        public static ScriptNodeManager_cl Instance
        {
            get
            {
                return sInstance;
            }
        }

        private bool mFnNodesLoaded;

        /// <summary>
        /// 
        /// </summary>
        public bool FnNodesLoaded
        {
            get
            {
                return mFnNodesLoaded;
            }
        }

        private Dictionary<string, INode> mScriptNodes = new Dictionary<string, INode>();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, INode> ScriptNodes
        {
            get
            {
                return mScriptNodes;
            }
        }

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        private ScriptNodeManager_cl()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public INode GetNode(string name)
        {
            return mScriptNodes[name];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool RegisterNode(INode node)
        {
            if (mScriptNodes.ContainsKey(node.GetName()))
            {
                return false;
            }
            else
            {
                mScriptNodes.Add(node.GetName(), node);
                mScriptNodes = (from entry in mScriptNodes orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fullPath"></param>
        public void SerializeNodes(string filename, bool fullPath = false)
        {
            string filePath = filename;
            if (fullPath == false)
            {
                filePath = Application.StartupPath;
                filePath += "\\Content\\" + filename;
            }

            Stream stream = File.Create(filePath);
            BinaryFormatter formatter = new BinaryFormatter();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(mScriptNodes.Count);

            foreach (KeyValuePair<string, INode> nodePair in mScriptNodes)
            {
                formatter.Serialize(stream, nodePair.Value);
            }

            stream.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fullPath"></param>
        public void DeserializeNodes(string filename = "nodes.fnNodes", bool fullPath = false)
        {
            mFnNodesLoaded = true;

            Instance.mScriptNodes = new Dictionary<string, INode>();

            string filePath = filename;
            if (fullPath == false)
            {
                filePath = Application.StartupPath;
                filePath += "\\Content\\Animations\\" + filename;
            }

            if (File.Exists(filePath))
            {
                FileStream scriptFile = new FileStream(filePath, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                BinaryReader reader = new BinaryReader(scriptFile);
                INode tempNode;

                try
                {
                    if (reader.BaseStream.Length > 0)
                    {
                        int numNodes = reader.ReadInt32();

                        for (int index = 0; index < numNodes; index++)
                        {
                            tempNode = (INode)formatter.Deserialize(scriptFile);
                            mScriptNodes.Add(tempNode.GetName(), tempNode);
                        }

                        mScriptNodes = (from entry in mScriptNodes orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                    }
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    scriptFile.Close();
                }        
            }
        }
    }
}
