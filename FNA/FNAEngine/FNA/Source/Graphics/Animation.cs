using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;

using Microsoft.Xna.Framework;

using FNA.Scripts;
using FNA.Components;
using FNA.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.Graphics
{
    /// <summary>
    /// A class that handles the playback of a collection of AnimationFrames.
    /// </summary>
    [Serializable]
    public class Animation : ISerializable
    {
        /// <summary>
        /// The script that is in effect for this entire animation.
        /// </summary>
        protected Script mWholeAnimationScript = new Script();
        
        /// <summary>
        /// 
        /// </summary>
        public Script AnimationScripts
        {
            get
            {
                return mWholeAnimationScript;
            }
            set
            {
                mWholeAnimationScript = value;
            }
        }

        /// <summary>
        /// A list of all the durations for each frame.
        /// </summary>
        private Dictionary<int, int> mFrameDurations = new Dictionary<int, int>();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, int> FrameDurations
        {
            get
            {
                return mFrameDurations;
            }
            set
            {
                mFrameDurations = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int NumFrames()
        {
            return mFrames[DirectionMap[RotationComponent.CardinalDirections.N]].Count;
        }

        private string mTextureName;

        /// <summary>
        /// 
        /// </summary>
        public string TextureName
        {
            get
            {
                return mTextureName;
            }
            set
            {
                mTextureName = value;
            }
        }

        /// <summary>
        /// A dictionary that matches frame numbers with lists of scripts for those specific frames only.
        /// </summary>
        protected Dictionary<int, Script> mFrameScripts = new Dictionary<int, Script>();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, Script> FrameScripts
        {
            get
            {
                return mFrameScripts;
            }
            set
            {
                mFrameScripts = value;
            }
        }

        /// <summary>
        /// A dictionary whose keys are frame indices and whose values are themselves dictionaries which map motion delta channels to vectors.
        /// </summary>
        protected Dictionary<int, Dictionary<int, Vector2>> mFrameMotionDeltas = new Dictionary<int,Dictionary<int,Vector2>>();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, Dictionary<int, Vector2>> FrameMotionDeltas
        {
            get
            {
                return mFrameMotionDeltas;
            }
        }

        /// <summary>
        /// The name associated with this animation.
        /// </summary>
        protected string mName;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        /// <summary>
        /// The number of frames in this animation.
        /// </summary>
        private Dictionary<RotationComponent.CardinalDirections, KeyFrameDictionary> mFrames = new Dictionary<RotationComponent.CardinalDirections, KeyFrameDictionary>();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<RotationComponent.CardinalDirections, KeyFrameDictionary> Frames
        {
            get
            {
                return mFrames;
            }
        }

        private Dictionary<RotationComponent.CardinalDirections, RotationComponent.CardinalDirections> mDirectionMap = new Dictionary<RotationComponent.CardinalDirections, RotationComponent.CardinalDirections>();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<RotationComponent.CardinalDirections, RotationComponent.CardinalDirections> DirectionMap
        {
            get
            {
                return mDirectionMap;
            }
            set
            {
                mDirectionMap = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Animation()
        {
            foreach (RotationComponent.CardinalDirections direction in Enum.GetValues(typeof(RotationComponent.CardinalDirections)))
            {
                mDirectionMap.Add(direction, direction);
            }
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected Animation(SerializationInfo info, StreamingContext context)
        {
            mTextureName = info.GetString("Texture");

            if (Game_cl.BaseInstance != null)
            {
                string[] tokens = mTextureName.Split('\\');
                string resourceName = tokens[tokens.Length - 1];
                string[] tokens2 = resourceName.Split('.');
                mTextureName = tokens2[0];
                //mTexture = Game_cl.BaseInstance.Content.Load<Texture2D>(tokens2[0]);
            }

            mName = info.GetString("Name");
            mWholeAnimationScript = (Script)info.GetValue("AnimationScripts", typeof(Script));

            List<KeyValuePair<int, int>> frameDurationPairs = (List<KeyValuePair<int, int>>)(info.GetValue("FrameDurations", typeof(List<KeyValuePair<int, int>>)));
            mFrameDurations = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> pair in frameDurationPairs)
            {
                mFrameDurations.Add(pair.Key, pair.Value);
            }

            List<KeyValuePair<int, Script>> frameScriptPairs = (List<KeyValuePair<int, Script>>)(info.GetValue("FrameScriptPairs", typeof(List<KeyValuePair<int, Script>>)));
            mFrameScripts = new Dictionary<int, Script>();
            foreach (KeyValuePair<int, Script> pair in frameScriptPairs)
            {
                mFrameScripts.Add(pair.Key, pair.Value);
            }

            List<KeyValuePair<int, List<KeyValuePair<int, Vector2>>>> motionDeltas = (List<KeyValuePair<int, List<KeyValuePair<int, Vector2>>>>)(info.GetValue("MotionDeltas", typeof(List<KeyValuePair<int, List<KeyValuePair<int, Vector2>>>>)));
            mFrameMotionDeltas = new Dictionary<int, Dictionary<int, Vector2>>();
            foreach (KeyValuePair<int, List<KeyValuePair<int, Vector2>>> pair in motionDeltas)
            {
                mFrameMotionDeltas.Add(pair.Key, new Dictionary<int, Vector2>());
                foreach (KeyValuePair<int, Vector2> channelDeltas in pair.Value)
                {
                    mFrameMotionDeltas[pair.Key].Add(channelDeltas.Key, channelDeltas.Value);
                }
            }

            mFrames = new Dictionary<RotationComponent.CardinalDirections, KeyFrameDictionary>();
            int dirCount = info.GetInt32("NumDirections");
            for (int fc = 0; fc < dirCount; fc++)
            {
                string dirString = "Dir" + fc;
                RotationComponent.CardinalDirections dir = (RotationComponent.CardinalDirections)info.GetValue(dirString, typeof(RotationComponent.CardinalDirections));
                mFrames[dir] = new KeyFrameDictionary();

                KeyFrameDictionary directionalFrames = new KeyFrameDictionary();
                string dirFrameCountString = dir.ToString() + "Count";
                int dirFrameCount = info.GetInt32(dirFrameCountString);                
                for (int dc = 0; dc < dirFrameCount; dc++)
                {
                    string innerString = dir.ToString() + "Frame" + dc;
                    FloatRectangle floatRect = (FloatRectangle)info.GetValue(innerString, typeof(FloatRectangle));
                    directionalFrames.Add(dc, floatRect);
                }

                KeyValuePair<RotationComponent.CardinalDirections, KeyFrameDictionary> frameDirectionPair = 
                    new KeyValuePair<RotationComponent.CardinalDirections, KeyFrameDictionary>(dir, directionalFrames);

                mFrames[frameDirectionPair.Key] = frameDirectionPair.Value;
            }
            
            List<KeyValuePair<RotationComponent.CardinalDirections, RotationComponent.CardinalDirections>> directionMapPairs = (List<KeyValuePair<RotationComponent.CardinalDirections, RotationComponent.CardinalDirections>>)(info.GetValue("DirectionMapPairs", typeof(List<KeyValuePair<RotationComponent.CardinalDirections, RotationComponent.CardinalDirections>>)));
            mDirectionMap = new Dictionary<RotationComponent.CardinalDirections, RotationComponent.CardinalDirections>();
            foreach (KeyValuePair<RotationComponent.CardinalDirections, RotationComponent.CardinalDirections> pair in directionMapPairs)
            {
                mDirectionMap.Add(pair.Key, pair.Value);
            }

            if (ScriptNodeManager_cl.Instance.FnNodesLoaded == false)
            {
                ScriptNodeManager_cl.Instance.DeserializeNodes();
            }
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand,
        SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Texture", mTextureName);
            info.AddValue("Name", mName);
            info.AddValue("AnimationScripts", mWholeAnimationScript);

            List<KeyValuePair<int, int>> frameDurationPairs = mFrameDurations.ToList<KeyValuePair<int, int>>();
            info.AddValue("FrameDurations", frameDurationPairs);

            List<KeyValuePair<int, Script>> frameScriptPairs = mFrameScripts.ToList<KeyValuePair<int, Script>>();
            info.AddValue("FrameScriptPairs", frameScriptPairs);

            List<KeyValuePair<int, List<KeyValuePair<int, Vector2>>>> motionDeltas = new List<KeyValuePair<int, List<KeyValuePair<int, Vector2>>>>();
            foreach (KeyValuePair<int, Dictionary<int, Vector2>> frameDeltas in mFrameMotionDeltas)
            {
                List<KeyValuePair<int, Vector2>> channelMotionDeltas = frameDeltas.Value.ToList<KeyValuePair<int, Vector2>>();
                motionDeltas.Add(new KeyValuePair<int, List<KeyValuePair<int, Vector2>>>(frameDeltas.Key, channelMotionDeltas));
            }
            info.AddValue("MotionDeltas", motionDeltas);

            info.AddValue("NumDirections", mFrames.Keys.Count);
            int dirCount = 0;
            foreach (KeyValuePair<RotationComponent.CardinalDirections, KeyFrameDictionary> directionPair in mFrames)
            {
                string dirString = "Dir" + dirCount; dirCount++;
                info.AddValue(dirString, directionPair.Key);
                List<KeyValuePair<int, FloatRectangle>> directionalFrames = directionPair.Value.ToList<KeyValuePair<int, FloatRectangle>>();
                string countString = directionPair.Key.ToString() + "Count";
                info.AddValue(countString, directionalFrames.Count);
                foreach (KeyValuePair<int, FloatRectangle> innerPair in directionalFrames)
                {
                    string numString = directionPair.Key.ToString() + "Frame" + innerPair.Key;
                    info.AddValue(numString, innerPair.Value);
                }
            }

            List<KeyValuePair<RotationComponent.CardinalDirections, RotationComponent.CardinalDirections>> directionMapPairs = mDirectionMap.ToList<KeyValuePair<RotationComponent.CardinalDirections, RotationComponent.CardinalDirections>>();
            info.AddValue("DirectionMapPairs", directionMapPairs);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentKey"></param>
        /// <param name="framesOnKey"></param>
        /// <param name="rect"></param>
        /// <param name="script"></param>
        /// <param name="direction"></param>
        /// <param name="spriteFlipped"></param>
        /// <param name="reversed"></param>
        public virtual void GetFrameInfo(ref int currentKey, ref int framesOnKey, ref FloatRectangle rect, ref Script script, RotationComponent.CardinalDirections direction, out bool spriteFlipped,bool reversed = false)
        {
            framesOnKey++;
            if (framesOnKey > mFrameDurations[currentKey])
            {
                framesOnKey = 0;

                if (reversed)
                {
                    currentKey--;
                    if (currentKey < 0)
                    {
                        currentKey = mFrames[DirectionMap[RotationComponent.CardinalDirections.N]].Count - 1;
                    }
                }
                else
                {
                    currentKey++;
                    if (currentKey > mFrames[DirectionMap[RotationComponent.CardinalDirections.N]].Count - 1)
                    {
                        currentKey = 0;
                    }
                }
            }

            if (mFrames.Count == 1)
            {
                rect = mFrames[RotationComponent.CardinalDirections.NONE][currentKey];
            }
            else
            {
                rect = mFrames[mDirectionMap[direction]][currentKey];
            }

            if (direction > RotationComponent.CardinalDirections.S && mDirectionMap[RotationComponent.CardinalDirections.SW] != RotationComponent.CardinalDirections.SW)
            {
                spriteFlipped = true;
            }
            else
            {
                spriteFlipped = false;
            }

            script.AddRange(mWholeAnimationScript);

            if (mFrameScripts.ContainsKey(currentKey))
            {
                script.AddRange(mFrameScripts[currentKey]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="deltaDictionary"></param>
        public virtual void GetMotionDeltas(int frame, out Dictionary<int, Vector2> deltaDictionary)
        {
            mFrameMotionDeltas.TryGetValue(frame, out deltaDictionary);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        public void AddScriptRoutine(ScriptRoutine script)
        {
            mWholeAnimationScript.Add(script);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="script"></param>
        public void AddFrameRoutine(int frame, ScriptRoutine script)
        {
            if (mFrameScripts.ContainsKey(frame))
            {
                mFrameScripts[frame].Add(script);
            }
            else
            {
                mFrameScripts.Add(frame, new Script());
                mFrameScripts[frame].Add(script);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="channel"></param>
        /// <param name="delta"></param>
        public void ChangeFrameMotionDelta(int frame, int channel, Vector2 delta)
        {
            if (mFrameMotionDeltas.ContainsKey(frame))
            {
                if (mFrameMotionDeltas[frame].ContainsKey(channel))
                {
                    mFrameMotionDeltas[frame][channel] = delta;
                }
                else
                {
                    mFrameMotionDeltas[frame].Add(channel, delta);
                }
            }
            else
            {
                mFrameMotionDeltas.Add(frame, new Dictionary<int, Vector2>());
                mFrameMotionDeltas[frame].Add(channel, delta);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="frame"></param>
        /// <param name="index"></param>
        public void AddFrame(RotationComponent.CardinalDirections direction, FloatRectangle frame, int index)
        {
            // Add a keyframe dictionary if one doesn't exist for the specified direction.
            if(mFrames.ContainsKey(direction) == false)
            {
                mFrames.Add(direction, new KeyFrameDictionary());
            }

            if (mFrames[direction].ContainsKey(index))
            {
                mFrames[direction][index] = frame;
            }
            else
            {
                mFrames[direction].Add(index, frame);
            }

            // Make sure a script exists for this keyframe index
            if(mFrameScripts.ContainsKey(index) == false)
            {
                mFrameScripts.Add(index, new Script());
            }

            if (direction == RotationComponent.CardinalDirections.NONE)
            {
                foreach (RotationComponent.CardinalDirections animDirection in Enum.GetValues(typeof(RotationComponent.CardinalDirections)))
                {
                    mDirectionMap[animDirection] = RotationComponent.CardinalDirections.NONE;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frameToMove"></param>
        /// <param name="movement"></param>
        public void MoveFrameScript(int frameToMove, int movement)
        {
            if (mFrameScripts.ContainsKey(frameToMove))
            {
                Script temp = new Script();
                if (mFrameScripts.ContainsKey(frameToMove + movement))
                {
                    temp = mFrameScripts[frameToMove + movement];
                    mFrameScripts[frameToMove + movement] = mFrameScripts[frameToMove];
                }
                else
                {
                    mFrameScripts.Add(frameToMove + movement, mFrameScripts[frameToMove]);
                }
                mFrameScripts[frameToMove] = temp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="duration"></param>
        public void SetFrameDuration(int frame, int duration)
        {
            if(mFrameDurations.ContainsKey(frame))
            {
                mFrameDurations[frame] = duration;
            }
            else
            {
                mFrameDurations.Add(frame, duration);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return mName;
        }
    }
}
