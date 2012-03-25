using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using FNA;
using FNA.Graphics;
using FNA.Components;
using FNA.Scripts;
using System.Reflection;
using FNA.Managers;

namespace FNA.WorldEditor
{
    class TestLink : Entity
    {
        private PositionComponent mPosition;
        private RotationComponent mRotation;
        //private RenderableComponent mRenderable;
        private AnimatedComponent mAnimated;
        private PhysicsComponent mPhysics;
        private InputComponent mInput;
        private PlayerControllerComponent mController;
        private HealthComponent mHealth;

        private int mPlayerNumber;

        public TestLink(int playerNumber = 0)
        {
            mPlayerNumber = playerNumber;

            mPosition = new PositionComponent(this, 100, 100);
            mRotation = new RotationComponent(this);
            //mRenderable = new RenderableComponent(this);
            mPhysics = new PhysicsComponent(this);
            mAnimated = new AnimatedComponent(this);
            mInput = new InputComponent(this);
            mController = new PlayerControllerComponent(this);

            switch (mPlayerNumber)
            {
                case 0:
                    // Player 0 means no human control.
                    break;

                case 1:
                    mInput.PlayerIndex = PlayerIndex.One;
                    break;

                case 2:
                    mInput.PlayerIndex = PlayerIndex.Two;
                    break;

                case 3:
                    mInput.PlayerIndex = PlayerIndex.Three;
                    break;

                case 4:
                    mInput.PlayerIndex = PlayerIndex.Four;
                    break;
            }

            mHealth = new HealthComponent(this, 100.0f);
        }

        public void Initialize()
        {
            //mPosition.X = WorldEditor.Instance.Random.Next(600) - 300;
            //mPosition.Y = WorldEditor.Instance.Random.Next(600) - 300;
            mPosition.SetPosition2D(WorldEditor.Instance.Random.Next(600) - 300, WorldEditor.Instance.Random.Next(600) - 300);

            mAnimated.Initialize();

            Texture2D nessTexture = WorldEditor.Instance.Content.Load<Texture2D>("link");

            Animation walkAnimation = new Animation();
            walkAnimation.Name = "walk";
            walkAnimation.Texture = nessTexture;
            for (int index = 0; index < 8; index++)
            {
                walkAnimation.ChangeFrameMotionDelta(index, 0, new Vector2(0, 15.0f));
                walkAnimation.SetFrameDuration(index, 2);
            }
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(0, 192, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(64, 192, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(128, 192, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(192, 192, 64, 64), 3);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(256, 192, 64, 64), 4);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(320, 192, 64, 64), 5);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(384, 192, 64, 64), 6);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(448, 192, 64, 64), 7);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(0, 128, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(64, 128, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(128, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(192, 128, 64, 64), 3);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(256, 128, 64, 64), 4);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(320, 128, 64, 64), 5);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(384, 128, 64, 64), 6);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(448, 128, 64, 64), 7);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(0, 128, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(64, 128, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(128, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(192, 128, 64, 64), 3);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(256, 128, 64, 64), 4);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(320, 128, 64, 64), 5);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(384, 128, 64, 64), 6);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(448, 128, 64, 64), 7);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(0, 320, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(64, 320, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(128, 320, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(192, 320, 64, 64), 3);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(256, 320, 64, 64), 4);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(320, 320, 64, 64), 5);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(384, 320, 64, 64), 6);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(448, 320, 64, 64), 7);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(0, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(64, 0, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(128, 0, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(192, 0, 64, 64), 3);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(256, 0, 64, 64), 4);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(320, 0, 64, 64), 5);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(384, 0, 64, 64), 6);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(448, 0, 64, 64), 7);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(0, 256, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(64, 256, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(128, 256, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(192, 256, 64, 64), 3);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(256, 256, 64, 64), 4);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(320, 256, 64, 64), 5);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(384, 256, 64, 64), 6);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(448, 256, 64, 64), 7);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(0, 64, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(64, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(128, 64, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(192, 64, 64, 64), 3);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(256, 64, 64, 64), 4);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(320, 64, 64, 64), 5);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(384, 64, 64, 64), 6);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(448, 64, 64, 64), 7);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(0, 448, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(64, 448, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(128, 448, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(192, 448, 64, 64), 3);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(256, 448, 64, 64), 4);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(320, 448, 64, 64), 5);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(384, 448, 64, 64), 6);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(448, 448, 64, 64), 7);

            Animation attackAnimation = new Animation();
            attackAnimation.Name = "attack";
            attackAnimation.Texture = nessTexture;
            for (int index = 0; index < 2; index++)
            {
                attackAnimation.ChangeFrameMotionDelta(index, 0, new Vector2(0, 10.0f));
                attackAnimation.SetFrameDuration(index, 4);
            }

            attackAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(0, 192, 64, 64), 0);
            attackAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(0, 256, 64, 64), 1);

            attackAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(64, 192, 64, 64), 0);
            attackAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(64, 256, 64, 64), 1);

            attackAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(128, 192, 64, 64), 0);
            attackAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(128, 256, 64, 64), 1);

            attackAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(192, 192, 64, 64), 0);
            attackAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(192, 256, 64, 64), 1);

            attackAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(256, 192, 64, 64), 0);
            attackAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(256, 256, 64, 64), 1);

            attackAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(320, 192, 64, 64), 0);
            attackAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(320, 256, 64, 64), 1);

            attackAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(384, 192, 64, 64), 0);
            attackAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(384, 256, 64, 64), 1);

            attackAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(448, 192, 64, 64), 0);
            attackAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(448, 256, 64, 64), 1);

            int idleX = 0;
            Rectangle[] idleFrames = new Rectangle[8];
            for (int index = 0; index < 8; index++)
            {
                idleFrames[index] = new Rectangle(idleX, 0, 64, 64);
                idleX += 64;
            }

            Animation idleAnimation = new Animation();
            idleAnimation.Name = "idle";
            idleAnimation.SetFrameDuration(0, 1);
            idleAnimation.Texture = nessTexture;

            idleAnimation.AddFrame(RotationComponent.CardinalDirections.N, idleFrames[0], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.NE, idleFrames[1], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.E, idleFrames[2], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.SE, idleFrames[3], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.S, idleFrames[4], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.SW, idleFrames[5], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.W, idleFrames[6], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.NW, idleFrames[7], 0);

            AnimatedManager.Instance.AddAnimation("idle", idleAnimation);
            AnimatedManager.Instance.AddAnimation("walk", walkAnimation);
            AnimatedManager.Instance.AddAnimation("attack", attackAnimation);

            if (mPlayerNumber != 0)
            {
                InputScriptNode isLeftStickActiveNode = new InputScriptNode();
                isLeftStickActiveNode.ScriptType = InputScriptNode.InputScriptType.TYPE_LEFT_STICK_ACTIVE;

                PlayAnimationScriptNode playWalkAnimScriptNode = new PlayAnimationScriptNode();
                playWalkAnimScriptNode.AnimationToQueue = "walk";
                playWalkAnimScriptNode.FrameToQueue = 0;
                playWalkAnimScriptNode.PlayIfTrue = true;

                PlayAnimationScriptNode playIdleAnimScriptNode = new PlayAnimationScriptNode();
                playIdleAnimScriptNode.AnimationToQueue = "idle";
                playIdleAnimScriptNode.FrameToQueue = 0;
                playIdleAnimScriptNode.PlayIfTrue = false;

                ToggleMotionDeltaScriptNode toggleMotionDeltaNode = new ToggleMotionDeltaScriptNode();

                InputScriptNode attackButtonNode = new InputScriptNode();
                attackButtonNode.ScriptType = InputScriptNode.InputScriptType.TYPE_BUTTON_DOWN;
                object attackString = "attack";
                attackButtonNode.SetInputValue(attackString);

                PlayAnimationScriptNode playAttackScriptNode = new PlayAnimationScriptNode();
                playAttackScriptNode.PlayIfTrue = true;
                playAttackScriptNode.AnimationToQueue = "attack";

                PlayAnimationScriptNode backToIdleNode = new PlayAnimationScriptNode();
                backToIdleNode.AnimationToQueue = "idle";
                backToIdleNode.PlayIfTrue = false;

                isLeftStickActiveNode.Name = "Is left stick active";
                playWalkAnimScriptNode.Name = "Play walk animation";
                playIdleAnimScriptNode.Name = "Play idle animation";
                toggleMotionDeltaNode.Name = "Toggle motion delta";
                playAttackScriptNode.Name = "Play attack animation";
                attackButtonNode.Name = "If attack button hit";
                ScriptNodeManager.Instance.RegisterNode(isLeftStickActiveNode);
                ScriptNodeManager.Instance.RegisterNode(playWalkAnimScriptNode);
                ScriptNodeManager.Instance.RegisterNode(playIdleAnimScriptNode);
                ScriptNodeManager.Instance.RegisterNode(playAttackScriptNode);
                ScriptNodeManager.Instance.RegisterNode(toggleMotionDeltaNode);
                ScriptNodeManager.Instance.RegisterNode(attackButtonNode);

                ScriptNodeManager.Instance.SerializeNodes("ness.nodes");

                ScriptRoutine inputToWalkScript = new ScriptRoutine();
                inputToWalkScript.Add("Is left stick active");
                inputToWalkScript.Add("Play walk animation");
                idleAnimation.AddScriptRoutine(inputToWalkScript);

                ScriptRoutine noInputToIdleScript = new ScriptRoutine();
                noInputToIdleScript.Add("Is left stick active");
                noInputToIdleScript.Add("Play idle animation");
                walkAnimation.AddScriptRoutine(noInputToIdleScript);

                ScriptRoutine toggleMotionDeltaScript = new ScriptRoutine();
                toggleMotionDeltaScript.Add("Is left stick active");
                toggleMotionDeltaScript.Add("Toggle motion delta");
                walkAnimation.AddScriptRoutine(toggleMotionDeltaScript);
                attackAnimation.AddScriptRoutine(toggleMotionDeltaScript);

                ScriptRoutine playAttackScript = new ScriptRoutine();
                playAttackScript.Add("If attack button hit");
                playAttackScript.Add("Play attack animation");
                walkAnimation.AddScriptRoutine(playAttackScript);
                idleAnimation.AddScriptRoutine(playAttackScript);

                ScriptRoutine keepAttackingScript = new ScriptRoutine();
                keepAttackingScript.Add("If attack button hit");
                keepAttackingScript.Add("Play attack animation");
                attackAnimation.AddFrameRoutine(1, keepAttackingScript);

                ScriptRoutine backToIdleScript = new ScriptRoutine();
                backToIdleScript.Add("If attack button hit");
                backToIdleScript.Add("Play idle animation");
                attackAnimation.AddFrameRoutine(1, backToIdleScript);

                mInput.AddKey("moveN", Keys.I);
                mInput.AddKey("moveW", Keys.J);
                mInput.AddKey("moveS", Keys.K);
                mInput.AddKey("moveE", Keys.L);
                mInput.AddButton("attack", Buttons.RightTrigger);
            }

            mAnimated.QueueAnimation("idle");
        }
    }
}
