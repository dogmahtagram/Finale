using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using FNA.Components;
using FNA.Core;

namespace FNA.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PrefabManager_cl : BaseManager_cl
    {
        /// <summary>
        /// The static instance of this class.
        /// </summary>
        private static readonly PrefabManager_cl mInstance = new PrefabManager_cl();

        /// <summary>
        /// Accessor for the static instance.
        /// </summary>
        public static PrefabManager_cl Instance
        {
            get
            {
                return mInstance;
            }
        }

        private static string mCurrentPrefabDirectory;

        /// <summary>
        /// 
        /// </summary>
        public static string CurrentPrefabDirectory
        {
            get
            {
                return mCurrentPrefabDirectory;
            }
        }

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        private PrefabManager_cl()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public Entity_cl LoadPrefab(string filePath, bool fullPath = true)
        {
            Entity_cl prefab = null;

            if (filePath == "") return null;

            string currentDirectory = filePath;
            int pathEnd = filePath.LastIndexOf("\\");
            mCurrentPrefabDirectory = currentDirectory.Remove(pathEnd + 1);

            if (File.Exists(filePath))
            {
                FileStream prefabFile = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                BinaryReader reader = new BinaryReader(prefabFile);

                try
                {
                    prefab = (Entity_cl)formatter.Deserialize(prefabFile);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize a prefab. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    prefabFile.Close();
                }
            }

            return prefab;
        }

        /// <summary>
        /// Save an entity as a prefab.
        /// </summary>
        /// <param name="entity">The entity which we want to save as a prefab.</param>
        public void SavePrefab(Entity_cl entity)
        {
            SaveFileDialog savePrefabDialog = new SaveFileDialog();
            savePrefabDialog.Filter = "Prefab File|*.fnPrefab";
            savePrefabDialog.Title = "Save a Prefab";
            savePrefabDialog.ShowDialog();

            if (savePrefabDialog.FileName != "")
            {
                Stream stream = File.Create(savePrefabDialog.FileName);
                BinaryFormatter formatter = new BinaryFormatter();
                BinaryWriter writer = new BinaryWriter(stream);

                formatter.Serialize(stream, entity);
                stream.Close();
            }
        }
    }
}
