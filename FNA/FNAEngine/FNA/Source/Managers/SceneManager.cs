using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using FNA.Core;
using FNA.Components;

namespace FNA.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneManager_cl : BaseManager_cl
    {
        /// <summary>
        /// The static instance of this class.
        /// </summary>
        private static readonly SceneManager_cl mInstance = new SceneManager_cl();

        /// <summary>
        /// Accessor for the static instance.
        /// </summary>
        public static SceneManager_cl Instance
        {
            get
            {
                return mInstance;
            }
        }

        private static string mCurrentSceneDirectory;

        /// <summary>
        /// 
        /// </summary>
        public static string CurrentSceneDirectory
        {
            get
            {
                return mCurrentSceneDirectory;
            }
        }

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        private SceneManager_cl()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="scene"></param>
        public static void SaveScene(string filePath, Scene_cl scene)
        {
            string currentDirectory = filePath;
            int pathEnd = filePath.LastIndexOf("\\");
            mCurrentSceneDirectory = currentDirectory.Remove(pathEnd + 1);

            Stream stream = File.Create(filePath);
            BinaryFormatter formatter = new BinaryFormatter();
            BinaryWriter writer = new BinaryWriter(stream);

            try
            {
                formatter.Serialize(stream, scene.AmbientLight);

                writer.Write(scene.Lights.Count);
                foreach (Entity_cl light in scene.Lights)
                {
                    formatter.Serialize(stream, light);
                }

                writer.Write(scene.Entities.Count);
                foreach (Entity_cl entity in scene.Entities)
                {
                    formatter.Serialize(stream, entity);
                }

                writer.Write(scene.Layers.Count);
                foreach (Entity_cl layer in scene.Layers)
                {
                    formatter.Serialize(stream, layer);
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show("Failed to save Scene: " + e.Message);
                throw;
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        /// Loads a Scene by deserializing a file.
        /// </summary>
        /// <param name="filePath">The path to the Scene file.</param>
        /// <param name="fullPath">Whether to use the full path of the file.</param>
        /// <returns></returns>
        public static Scene_cl LoadScene(string filePath, bool fullPath = true)
        {
            Scene_cl scene = new Scene_cl();

            string currentDirectory = filePath;
            int pathEnd = filePath.LastIndexOf("\\");
            mCurrentSceneDirectory = currentDirectory.Remove(pathEnd + 1);

            if (File.Exists(filePath))
            {
                // Clear current scene
                scene.Clear();

                FileStream sceneFile = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                BinaryReader reader = new BinaryReader(sceneFile);

                try
                {
                    scene.SetAmbientLight((Entity_cl)formatter.Deserialize(sceneFile));

                    int count;
                    int num = reader.ReadInt32();
                    for (count = 0; count < num; count++)
                    {
                        Entity_cl entity = (Entity_cl)formatter.Deserialize(sceneFile);

                        foreach (List<Component_cl> componentList in entity.Components.Values)
                        {
                            foreach (Component_cl component in componentList)
                            {
                                if (typeof(FNA.Interfaces.IInitializeAble).IsAssignableFrom(component.GetType()))
                                {
                                    ((FNA.Interfaces.IInitializeAble)component).Initialize();
                                }
                            }
                        }
                        scene.AddEntity(entity);
                    }
                    
                    // Entities
                    num = reader.ReadInt32();
                    for (count = 0; count < num; count++)
                    {
                        scene.AddEntity((Entity_cl)formatter.Deserialize(sceneFile));
                    }
                    
                    // Layers
                    num = reader.ReadInt32();
                    for (count = 0; count < num; count++)
                    {
                        scene.AddLayer((Entity_cl)formatter.Deserialize(sceneFile));
                    }
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize Scene: " + e.Message);
                    throw;
                }
                finally
                {
                    sceneFile.Close();
                    reader.Close();
                }
            }

            return scene;
        }
    }
}