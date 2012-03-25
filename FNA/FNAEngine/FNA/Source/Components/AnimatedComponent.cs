using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.IO;

using Microsoft.Xna.Framework;

using FNA.Core;
using FNA.Managers;
using FNA.Graphics;
using FNA.Scripts;
using FNA.Interfaces;

namespace FNA.Components
{
    /// <summary>
    /// The AnimatedComponent_cl class ties the animation system together with the component system.  It primarily
    /// provides an interface for adding animations and then queueing them my name.
    /// </summary>
    [Serializable]
    public class AnimatedComponent_cl : Component_cl, ISerializable, IInitializeAble, IExclusiveComponent
    {
        /************************************************************************/
        /* Animated component managing members and functions                    */
        /************************************************************************/
        private static Dictionary<string, Animation> sAnimations = new Dictionary<string, Animation>();

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, Animation> Animations
        {
            get
            {
                return sAnimations;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="animation"></param>
        public static void AddAnimation(string name, Animation animation)
        {
            if (sAnimations.ContainsKey(name) == false)
            {
                sAnimations.Add(name, animation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveAnimation(string name)
        {
            if (sAnimations.ContainsKey(name))
            {
                sAnimations.Remove(name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<Animation> LoadAnimationFile(string file)
        {
            /************************************************************************
             * HACK:
             * Now that things are being loaded from a saved scene and aren't using the
             * prefab manager, we can't query the CurrentPrefabDirectory in order to get
             * the correct .fnAnim path.
             *
             * Larsson Burch - 2011/11/09 - 19:06
             ************************************************************************/
            //string filePath = PrefabManager_cl.CurrentPrefabDirectory + file;

            string filePath = Application.StartupPath + "\\Content\\Animations\\" + file;

            if (File.Exists(filePath))
            {
                FileStream animFile = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                BinaryReader reader = new BinaryReader(animFile);

                try
                {
                    List<Animation> animations = new List<Animation>();
                    int numAnimations = reader.ReadInt32();
                    for (int index = 0; index < numAnimations; index++)
                    {
                        animations.Add((Animation)formatter.Deserialize(animFile));
                    }
                    foreach (Animation anim in animations)
                    {
                        AddAnimation(anim.Name, anim);
                    }
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    animFile.Close();
                }
            }

            return sAnimations.Values.ToList();
        }

        /************************************************************************/
        /* Animated component members and functions                             */
        /************************************************************************/

        /// <summary>
        /// A series of flags that represent whether the different channels of motion delta should be processed or not.
        /// </summary>
        private int mProcessMotionDeltaFlags = int.MaxValue;

        /// <summary>
        /// The current frame that this AnimatedComponent_cl is on in its animation.
        /// </summary>
        private int mCurrentKey;

        /// <summary>
        /// The number of frames that we've been on the current key frame.
        /// </summary>
        private int mFramesOnKey;

        /// <summary>
        /// The amount of time that we have been on the current frame;
        /// </summary>
        private float mAccumulatedFrameTime;

        /// <summary>
        /// mCurrentAnimation is the string key to the current animation.
        /// </summary>
        private string mCurrentAnimation = "";

        /// <summary>
        /// 
        /// </summary>
        public string CurrentAnimation
        {
            get
            {
                return mCurrentAnimation;
            }
        }

        private string mInitialAnimation;

        /// <summary>
        /// 
        /// </summary>
        public string InitialAnimation
        {
            get
            {
                return mInitialAnimation;
            }
            set
            {
                mInitialAnimation = value;
            }
        }

        /************************************************************************
         * TODO:
         * Get this with a file picker when using the prefab editor.
         * 
         * Larsson Burch - 2011/11/01
         ************************************************************************/
        private string mAnimationFile;

        /// <summary>
        /// 
        /// </summary>
        public string AnimationFile
        {
            get
            {
                return mAnimationFile;
            }
            set
            {
                mAnimationFile = ((string)value).Split('.')[0];
            }
        }

        private float mSpeed = 1.0f;

        /// <summary>
        /// 
        /// </summary>
        public float Speed
        {
            get
            {
                return mSpeed;
            }
            set
            {
                mSpeed = value;
            }
        }
    
        /// <summary>
        /// Default constructor - registers this component with the system and adds to the entity.
        /// </summary>
        /// <param name="parent"></param>
        public AnimatedComponent_cl(Entity_cl parent) : base(parent)
        {
            mParentEntity.AddComponent(this);
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected AnimatedComponent_cl(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            mProcessMotionDeltaFlags = info.GetInt32("MotionDeltaFlags");
            mCurrentAnimation = info.GetString("InitialAnimation");
            mInitialAnimation = mCurrentAnimation;

            mAnimationFile = info.GetString("AnimationFile");
        }

        /// <summary>
        /// GetOjectData is a method to fill a serialization info object from this class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand,
        SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("AnimationFile", mAnimationFile);
            info.AddValue("InitialAnimation", mInitialAnimation);
            info.AddValue("MotionDeltaFlags", mProcessMotionDeltaFlags);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            LoadAnimationFile(mAnimationFile + ".fnAnim");
        }

        /// <summary>
        /// On the update, the animated component sets the image on the renderable component to the current frame of animation.
        /// </summary>
        public void Update()
        {
            Animation currentAnimation = Animations[mCurrentAnimation];

            RotationComponent rotation = (RotationComponent)(mParentEntity.GetComponentOfType(typeof(RotationComponent)));
            RotationComponent.CardinalDirections animDirection = (rotation == null) ? RotationComponent.CardinalDirections.NONE : rotation.GetCardinalOrientation();

            mAccumulatedFrameTime += Game_cl.BaseInstance.Timer.UnpausedElapsedMilliseconds * Math.Abs(mSpeed);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // TODO: Make the value '1000.0f / 30.0f' be a private static variable or something instead of always calculating it multiple times every frame. [JSD 09/27/11]
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (mAccumulatedFrameTime > 1000.0f / 30.0f /* The time needed to advance one frame at 30fps */)
            {
                mAccumulatedFrameTime -= 1000.0f / 30.0f;

                FloatRectangle rect = new FloatRectangle();
                Script script = new Script();

                bool playReversed = false;
                if (mSpeed < 0)
                {
                    playReversed = true;
                }

                bool spriteFlipped;

                currentAnimation.GetFrameInfo(ref mCurrentKey, ref mFramesOnKey, ref rect, ref script, animDirection, out spriteFlipped, playReversed);

                ProcessScript(script);

                SunburnSprite_cl currentSprite = ((RenderableComponent_cl)mParentEntity.GetComponentOfType(typeof(RenderableComponent_cl))).Sprite;
                currentSprite.LoadContent(currentAnimation.TextureName);
                currentSprite.Flipped = spriteFlipped;
                currentSprite.Rectangle = rect;


                // Process motion delta channels here.
                PhysicsComponent_cl physics = ((PhysicsComponent_cl)mParentEntity.GetComponentOfType(typeof(PhysicsComponent_cl)));
                Dictionary<int, Vector2> motionDeltas = null;
                currentAnimation.GetMotionDeltas(mCurrentKey, out motionDeltas);

                if (motionDeltas != null)
                {
                    foreach (KeyValuePair<int, Vector2> deltaPair in motionDeltas)
                    {
                        if (DoesUseMotionDelta(deltaPair.Key))
                        {
                            Vector2 motionDelta = deltaPair.Value;
                            float timeBasedConstant = Game_cl.BaseInstance.Timer.UnpausedElapsedMilliseconds / (currentAnimation.FrameDurations[mCurrentKey] * 1000.0f / 30.0f);

                            Vector2 forwardDelta;
                            Vector2 upwardDelta;

                            // Not a TODO, but what's going on here?  Just curious
                            //      - JSD
                            switch (deltaPair.Key)
                            {
                                /************************************************************************
                                 * HACK:
                                 * Hacking for Finale. Motion delta coming from the tool as (X, Y) will translate
                                 * to horizontal and vertical movement on the 2D plane. Positive X is in the
                                 * current direction the character is facing. Positive Y is always up.
                                 *
                                 * Larsson Burch - 2011/11/16 - 13:26
                                 ************************************************************************/
                                case 0:
                                    // Position
                                    motionDelta.X *= timeBasedConstant;
                                    motionDelta.Y *= timeBasedConstant;

                                    forwardDelta = rotation.Direction * motionDelta.X;
                                    upwardDelta = rotation.DirectionUp * motionDelta.Y;

                                    physics.AddedPosition = new Vector2((forwardDelta.X + upwardDelta.X), (forwardDelta.Y + upwardDelta.Y));
                                    break;

                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                    // Velocity
                                    motionDelta.X *= timeBasedConstant;
                                    motionDelta.Y *= timeBasedConstant; // tested values - motionDelta: 4.0, Density: 10, Area = 16, Mass = 160

                                    forwardDelta = rotation.Direction * motionDelta.X;
                                    upwardDelta = rotation.DirectionUp * motionDelta.Y;

                                    Vector2 newVelocity = new Vector2((forwardDelta.X + upwardDelta.X), (forwardDelta.Y + upwardDelta.Y));
                                    if (newVelocity.Length() > 0)
                                    {
                                        physics.Velocity = newVelocity;
                                    }
                                    break;

                                case 6:
                                case 7:
                                case 8:
                                case 9:
                                case 10:
                                    // Force
                                    motionDelta.X *= timeBasedConstant * 1000.0f; // scale, N to kN
                                    motionDelta.Y *= timeBasedConstant * 1000.0f; // tested values - motionDelta: 4.0-8.0, Density: 10, Area = 16, Mass = 160

                                    forwardDelta = rotation.Direction * motionDelta.X;
                                    upwardDelta = rotation.DirectionUp * motionDelta.Y;

                                    physics.Force = new Vector2((forwardDelta.X + upwardDelta.X), (forwardDelta.Y + upwardDelta.Y));
                                    break;

                                case 11:
                                case 12:
                                case 13:
                                case 14:
                                case 15:
                                    // Impulse
                                    motionDelta.X *= timeBasedConstant * 1000.0f; // scale, N to kN
                                    motionDelta.Y *= timeBasedConstant * 1000.0f; // tested values - motionDelta: 0.1, Density: 10, Area = 16, Mass = 160

                                    forwardDelta = rotation.Direction * motionDelta.X;
                                    upwardDelta = rotation.DirectionUp * motionDelta.Y;

                                    Vector2 newImpulse = new Vector2((forwardDelta.X + upwardDelta.X), (forwardDelta.Y + upwardDelta.Y));
                                    if (newImpulse.Length() > 0)
                                    {
                                        physics.Impulse = newImpulse;
                                    }

                                    break;

                                case 16:
                                case 17:
                                case 18:
                                case 19:
                                case 20:
                                    // Jump Impulse
                                    motionDelta.X *= 1000.0f; // scale, N to kN
                                    motionDelta.Y *= 1000.0f; // tested values - motionDelta: 0.1, Density: 10, Area = 16, Mass = 160

                                    forwardDelta = rotation.Direction * motionDelta.X;
                                    upwardDelta = rotation.DirectionUp * motionDelta.Y;

                                    physics.JumpImpulse = new Vector2((forwardDelta.X + upwardDelta.X), (forwardDelta.Y + upwardDelta.Y));

                                    break;

                                case 21:
                                case 22:
                                case 23:
                                case 24:
                                case 25:
                                case 26:
                                case 27:
                                case 28:
                                case 29:

                                default:
                                    forwardDelta = rotation.Orientation * motionDelta.Y;
                                    upwardDelta = rotation.OrientationRight * motionDelta.X;

                                    ((PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl))).SetPosition2D(((PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl))).Position2D.X + forwardDelta.X + upwardDelta.X,
                                                                                                                    ((PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl))).Position2D.Y + (forwardDelta.Y + upwardDelta.Y) * 0.57735f);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Processes each of the scripts returned for that animation and frame.
        /// </summary>
        /// <param name="script">scripts is a list of strings that represent which script nodes to process.</param>
        private void ProcessScript(Script script)
        {
            string nextNode = "";
            foreach (ScriptRoutine routine in script)
            {
                foreach (string node in routine)
                {
                    int currentNodeIndex = routine.IndexOf(node);
                    if (currentNodeIndex < routine.Count - 1)
                    {
                        nextNode = routine[currentNodeIndex + 1];
                    }
                    ScriptNodeManager_cl.Instance.GetNode(node).ProcessNode(mParentEntity, nextNode);
                }
            }
        }

        /// <summary>
        /// QueueAnimation is the method by which we tell a new animation to play from the beginning.
        /// </summary>
        /// <param name="animationName">animationName is the string name by which the animation we want to play is 
        /// indexed into the hashmap.</param>
        /// <param name="frame"></param>
        /// <param name="speed"></param>
        public void QueueAnimation(string animationName, int frame = 0, float speed = 1.0f)
        {
            // fuck - took out the check to see if this animation is already playing so that we can jump to frames in a current anim... is this okay?
            RotationComponent rotation = (RotationComponent)(mParentEntity.GetComponentOfType(typeof(RotationComponent)));
            RotationComponent.CardinalDirections animDirection = RotationComponent.CardinalDirections.NONE;
            if (rotation != null)
            {
                animDirection = rotation.GetCardinalOrientation();
            }

            mCurrentAnimation = animationName;
            Animation currentAnimation = Animations[mCurrentAnimation];

            FloatRectangle rect = new FloatRectangle();
            Script script = new Script();

            bool spriteFlipped;
            mCurrentKey = frame;
            mFramesOnKey = 0;
            currentAnimation.GetFrameInfo(ref mCurrentKey, ref mFramesOnKey, ref rect, ref script, animDirection, out spriteFlipped, false);
            mCurrentKey = frame;
            mFramesOnKey = 0;

            //ProcessScripts(scripts);

            SunburnSprite_cl currentSprite = ((RenderableComponent_cl)mParentEntity.GetComponentOfType(typeof(RenderableComponent_cl))).Sprite;
            currentSprite.Rectangle = rect;
            currentSprite.LoadContent(currentAnimation.TextureName);

            mSpeed = speed;
        }

        /// <summary>
        /// Sets whether a specific channel of motion delta should be used currently on this component.
        /// </summary>
        /// <param name="shouldUse">shouldUse is whether we want this channel on or not</param>
        /// <param name="channel">channel is the motion delta channel in question</param>
        public void SetUseMotionDelta(bool shouldUse, int channel)
        {
            if (shouldUse)
            {
                mProcessMotionDeltaFlags |= 1 << channel;
            }
            else
            {
                mProcessMotionDeltaFlags &= ~(1 << channel);
            }
        }

        /// <summary>
        /// Returns whether the queried motion delta channel is currently turned on for this component.
        /// </summary>
        /// <param name="channel">the motion delta channel in question</param>
        /// <returns>true if the queried channel's motion delta is turned on</returns>
        private bool DoesUseMotionDelta(int channel)
        {
            return Convert.ToBoolean(mProcessMotionDeltaFlags & (1 << channel));
        }
    }
}
