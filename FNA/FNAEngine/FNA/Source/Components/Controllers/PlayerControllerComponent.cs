using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using FNA.Core;
using FNA.Managers;
using FNA.Components.Cameras;
using FNA.Interfaces;

namespace FNA.Components
{
    /// <summary>
    /// The PlayerControllerComponent_cl holds player control specific logic.
    /// </summary>
    [Serializable]
    public class PlayerControllerComponent_cl : CharacterControllerComponent_cl, ISerializable, IUpdateAble
    {
        private static float mJoyStickSensitivity = 0.2f;

        private float NextDepthIn;
        private float NextDepthOut;
        private bool MovingIn = false;
        private bool MovingOut = false;
        private float hangTime = 0;

        private int mActionChannelFlags = 0;

        /// <summary>
        /// The different things we'll be checking in the controllers logic.
        /// By toggling channels on or off, the animation system can determine
        /// which actions are associated with specific animations and buttons.
        /// 
        /// Note: this enum shouldn't exceed 32 members currently due to the
        /// mActionChannelFlags bitfield.
        /// </summary>
        public enum ActionChannels : uint
        {
            ///
            STAND_IDLE = 0,
            ///
            WALK_LEFT,
            ///
            WALK_RIGHT,
            ///
            JUMP,
            ///
            ROLL,
            ///
            JUMP_MOVE,
            ///
            TURN,
            ///
            FIRST_PUNCH,
            ///
            GROUND_POUND,
            ///
            SECOND_PUNCH,
            ///
            FALL,
            ///
            CHECK_FALL,
            ///
            LANDED,
            ///
            GOUND_POUND_END,
            ///
            ACTION_CHANNEL_COUNT
        }

        /// <summary>
        /// Constructor which calls the overloaded constructor on the base class.
        /// </summary>
        /// <param name="parent">parent is the parent Entity of this component.</param>
        public PlayerControllerComponent_cl(Entity_cl parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">info is the serialization info to deserialize with</param>
        /// <param name="context">context is the context in which to deserialize...?</param>
        protected PlayerControllerComponent_cl(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
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
        }

        /// <summary>
        /// Main update loop.  On the player controller this is where we query input and update actions accordingly.
        /// </summary>
        public override void Update()
        {
            InputComponent_cl playerInput = ((InputComponent_cl)mParentEntity.GetComponentOfType(typeof(InputComponent_cl)));
            AnimatedComponent_cl animated = ((AnimatedComponent_cl)mParentEntity.GetComponentOfType(typeof(AnimatedComponent_cl)));
            RotationComponent rotation = ((RotationComponent)mParentEntity.GetComponentOfType(typeof(RotationComponent)));
            PositionComponent_cl position = (PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl));
            PhysicsComponent_cl physics = ((PhysicsComponent_cl)mParentEntity.GetComponentOfType(typeof(PhysicsComponent_cl)));

            /************************************************************************
             * HACK:
             * I'm gonna do an ugly switch case thing here because I just want to get
             * it working. We should think of a better way (array of Delegates?) to
             * process these different code blocks.
             *
             * Larsson Burch - 2011/11/11 - 17:57
             ************************************************************************/

            foreach (ActionChannels channel in Enum.GetValues(typeof(ActionChannels)))
            {
                if (DoesUseMotionDelta((int)channel))
                {
                    switch(channel)
                    {
                            /************************************************************************
                             * HACK/TODO:
                             * Just to test and see how people like the pipeline is a new iteration,
                             * the player controller will also be able to set animations on the animated.
                             * I'm gonna hard code the animation names in here for now, and then we'll
                             * decide what method we like best and how to change the tools accordingly.
                             *
                             * Larsson Burch - 2011/11/11 - 18:08
                             ************************************************************************/
                        case ActionChannels.STAND_IDLE:
                            if (playerInput.IsKeyDown("moveLeft") == false &&
                                playerInput.IsKeyDown("moveRight") == false &&
                                Math.Abs(playerInput.LeftStickPosition().X) < mJoyStickSensitivity)
                            {
                                animated.QueueAnimation("idle");
                            }
                            break;

                        case ActionChannels.WALK_LEFT:
                            if (playerInput.IsKeyDown("moveLeft") || (playerInput.LeftStickPosition().X < -mJoyStickSensitivity))
                            {
                                animated.QueueAnimation("run");
                                rotation.Direction = new Vector2(-1, 0);
                                rotation.Orientation = new Vector2(-1, 0);
                            }
                            break;

                        case ActionChannels.WALK_RIGHT:
                            if (playerInput.IsKeyDown("moveRight") || (playerInput.LeftStickPosition().X > mJoyStickSensitivity))
                            {
                                animated.QueueAnimation("run");
                                rotation.Direction = new Vector2(1, 0);
                                rotation.Orientation = new Vector2(1, 0);
                            }
                            break;

                        case ActionChannels.JUMP:
                            if (/*playerInput.WasButtonPressedThisFrame("jump") || */playerInput.WasKeyPressedThisFrame("jump"))
                            {
                                animated.QueueAnimation("jumpStart");

                                if (playerInput.IsKeyDown("moveIn"))
                                {
                                    SetLimits();
                                    SetMovingIn(true);
                                }
                                else if (playerInput.IsKeyDown("moveOut"))
                                {
                                    SetLimits();
                                    SetMovingOut(true);
                                }
                            }
                            break;

                        case ActionChannels.ROLL:
                            if (/*playerInput.WasButtonPressedThisFrame("roll") || */playerInput.WasKeyPressedThisFrame("roll"))
                            {
                                animated.QueueAnimation("roll");
                            }
                            break;

                        case ActionChannels.JUMP_MOVE:
                            if (playerInput.IsKeyDown("moveRight") && rotation.Direction.X < 0)
                            {
                                rotation.Direction = new Vector2(1, 0);
                                rotation.Orientation = new Vector2(1, 0);
                            }
                            if (playerInput.IsKeyDown("moveLeft") && rotation.Direction.X >0)
                            {
                                rotation.Direction = new Vector2(-1, 0);
                                rotation.Orientation = new Vector2(-1, 0);
                            }
                            break;

                        case ActionChannels.TURN:
                            if (playerInput.WasKeyPressedThisFrame("moveRight") && rotation.Direction.X < 0)
                            {
                                animated.QueueAnimation("turn");
                                rotation.Direction = new Vector2(1, 0);
                                rotation.Orientation = new Vector2(1, 0);
                            }
                            if (playerInput.WasKeyPressedThisFrame("moveLeft") && rotation.Direction.X > 0)
                            {
                                animated.QueueAnimation("turn");
                                rotation.Direction = new Vector2(-1, 0);
                                rotation.Orientation = new Vector2(-1, 0);
                            }
                            break;

                        case ActionChannels.FIRST_PUNCH:
                            if (playerInput.WasKeyPressedThisFrame("action"))
                            {
                                animated.QueueAnimation("firstPunch");
                            }
                            break;

                        case ActionChannels.GROUND_POUND:
                            if (playerInput.WasKeyPressedThisFrame("action"))
                            {
                                animated.QueueAnimation("groundPound");
                            }
                            break;

                        case ActionChannels.SECOND_PUNCH:
                            if (playerInput.WasKeyPressedThisFrame("action"))
                            {
                                animated.QueueAnimation("secondPunch");
                            }
                            break;

                        case ActionChannels.FALL:
                            if (physics.IsLanding == true)
                            {
                                animated.QueueAnimation("jumpLand");
                            }
                            break;

                        case ActionChannels.CHECK_FALL:
                            if (physics.currentVelocity.Y < -3.0f)
                            {
                                animated.QueueAnimation("fall");
                            }
                            break;

                        case ActionChannels.LANDED:
                            physics.IsLanding = false;
                            break;

                        case ActionChannels.GOUND_POUND_END:
                            hangTime += 0.02f;
                            if (physics.currentVelocity.Y >= -0.01f && hangTime != 0.01f)
                            {
                                float magnitude = Math.Max(hangTime, 0.2f);
                                physics.currentVelocity = Vector2.Zero;
                                ((SunburnCameraComponent_cl)CameraManager_cl.Instance.ActiveCamera).Shake(magnitude, 0.5f + hangTime);
                                animated.QueueAnimation("groundPoundEnd");
                                Console.WriteLine("HangTime: " + hangTime);
                                hangTime = 0;
                            }
                            break;
                    }
                }
            }

            CheckDepthMovement();

        }

        /// <summary>
        /// Toggles an action channel that the player controller component processes.
        /// These channels may be things such as move left, run, and jump - things that
        /// the controller component has the logic for. In this way, the animation system
        /// determines which actions are processed at which time.
        /// </summary>
        /// <param name="toggle">whether to toggle an action channel on or off</param>
        /// <param name="channel">the action channel to toggle</param>
        public void ToggleActionChannel(bool toggle, int channel)
        {
            if (toggle)
            {
                mActionChannelFlags |= 1 << channel;
            }
            else
            {
                mActionChannelFlags &= ~(1 << channel);
            }
        }

        /// <summary>
        /// Returns whether the queried action channel is currently turned on for this component.
        /// </summary>
        /// <param name="channel">the action channel in question</param>
        /// <returns>true if the queried channel's motion delta is turned on</returns>
        private bool DoesUseMotionDelta(int channel)
        {
            return Convert.ToBoolean(mActionChannelFlags & (1 << channel));
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetLimits()
        {
            PositionComponent_cl playerPosition = (PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl));
            NextDepthIn = playerPosition.Position3D.Z + 20.0f;
            NextDepthOut = playerPosition.Position3D.Z - 20.0f;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckDepthMovement()
        {
            InputComponent_cl playerInput = ((InputComponent_cl)mParentEntity.GetComponentOfType(typeof(InputComponent_cl)));
            PositionComponent_cl playerPosition = ((PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl)));
            PhysicsComponent_cl playerPhysics = ((PhysicsComponent_cl)mParentEntity.GetComponentOfType(typeof(PhysicsComponent_cl)));
            Vector3 displacement = Vector3.Zero;

            if (MovingIn)
            {
                if (playerPosition.Position3D.Z < NextDepthIn)
                {
                    displacement = new Vector3(0, 0, 1.0f);
                    //playerPosition.SetPosition3D(playerPosition.Position3D + displacement);
                    playerPhysics.SetPosition(playerPosition.Position3D + displacement);
                }
                if (playerPosition.Position3D.Z >= NextDepthIn)
                {
                    Vector3 test = playerPosition.Position3D;
                    test.Z = NextDepthIn;
                    playerPosition.SetPosition3D(test);
                    SetMovingIn(false);
                }
            }

            else if (MovingOut)
            {
                if (playerPosition.Position3D.Z > NextDepthOut)
                {
                    displacement = new Vector3(0, 0, -1.0f);
                    //playerPosition.SetPosition3D(playerPosition.Position3D + displacement);
                    playerPhysics.SetPosition(playerPosition.Position3D + displacement);
                }
                if (playerPosition.Position3D.Z <= NextDepthOut)
                {
                    Vector3 test = playerPosition.Position3D;
                    test.Z = NextDepthOut;
                    playerPosition.SetPosition3D(test);
                    SetMovingOut(false);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        private void SetMovingIn(bool b)
        {
            MovingIn = b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        private void SetMovingOut(bool b)
        {
            MovingOut = b;
        }
    }
}
