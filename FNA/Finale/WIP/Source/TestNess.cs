using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using FNA.Triggers;

namespace FNA.WorldEditor
{
    class TestNess : Entity
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

        public TestNess(int playerNumber = 0)
        {
            mPlayerNumber = playerNumber;

            mPosition = new PositionComponent(this, 100, 100);
            mRotation = new RotationComponent(this);
            //mRenderable = new RenderableComponent(this);
            mPhysics = new PhysicsComponent(this);
            mAnimated = new AnimatedComponent(this);
            mInput = new InputComponent(this);
            mController = new PlayerControllerComponent(this);

            //switch (mPlayerNumber)
            //{
            //    case 0:
            //        // Player 0 means no human control.
            //        break;

            //    case 1:
            //        mInput.PlayerIndex = PlayerIndex.One;
            //        break;

            //    case 2:
            //        mInput.PlayerIndex = PlayerIndex.Two;
            //        break;

            //    case 3:
            //        mInput.PlayerIndex = PlayerIndex.Three;
            //        break;

            //    case 4:
            //        mInput.PlayerIndex = PlayerIndex.Four;
            //        break;
            //}

            mHealth = new HealthComponent(this, 100.0f);
        }

        public static void InitAnimations()
        {
            Texture2D nessTexture = WorldEditor.Instance.Content.Load<Texture2D>("commando");

            Animation walkAnimation = new Animation();
            walkAnimation.Name = "walk";
            walkAnimation.Texture = nessTexture;
            for (int index = 0; index < 4; index++)
            {
                walkAnimation.ChangeFrameMotionDelta(index, 0, new Vector2(0, 30.0f));
                walkAnimation.SetFrameDuration(index, 5);
            }
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(0, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(0, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(0, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(0, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(64, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(64, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(64, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(64, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(128, 64, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(128, 128, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(128, 64, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(128, 0, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(192, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(192, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(192, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(192, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(256, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(256, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(256, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(256, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(320, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(320, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(320, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(320, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(384, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(384, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(384, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(384, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(448, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(448, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(448, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(448, 64, 64, 64), 3);

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

            //ScriptNodeManager.Instance.SerializeNodes("ness.nodes");

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
        }

        public static void SerializeAnimations()
        {
            Texture2D nessTexture = WorldEditor.Instance.Content.Load<Texture2D>("ness");

            Animation walkAnimation = new Animation();
            walkAnimation.Name = "walk";
            walkAnimation.Texture = nessTexture;
            walkAnimation.TextureName = "ness";
            for (int index = 0; index < 4; index++)
            {
                walkAnimation.ChangeFrameMotionDelta(index, 0, new Vector2(0, 30.0f));
                walkAnimation.SetFrameDuration(index, 5);
            }
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(0, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(0, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(0, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(0, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(64, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(64, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(64, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(64, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(128, 64, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(128, 128, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(128, 64, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(128, 0, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(192, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(192, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(192, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(192, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(256, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(256, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(256, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(256, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(320, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(320, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(320, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.SW, new Rectangle(320, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(384, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(384, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(384, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.W, new Rectangle(384, 64, 64, 64), 3);

            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(448, 0, 64, 64), 0);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(448, 64, 64, 64), 1);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(448, 128, 64, 64), 2);
            walkAnimation.AddFrame(RotationComponent.CardinalDirections.NW, new Rectangle(448, 64, 64, 64), 3);

            Animation attackAnimation = new Animation();
            attackAnimation.Name = "attack";
            attackAnimation.Texture = nessTexture;
            attackAnimation.TextureName = "ness";
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
            idleAnimation.TextureName = "ness";

            idleAnimation.AddFrame(RotationComponent.CardinalDirections.N, idleFrames[0], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.NE, idleFrames[1], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.E, idleFrames[2], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.SE, idleFrames[3], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.S, idleFrames[4], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.SW, idleFrames[5], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.W, idleFrames[6], 0);
            idleAnimation.AddFrame(RotationComponent.CardinalDirections.NW, idleFrames[7], 0);

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

            ScriptRoutine inputToWalkScript = new ScriptRoutine();
            inputToWalkScript.Name = "Input to walk";
            inputToWalkScript.Add("Is left stick active");
            inputToWalkScript.Add("Play walk animation");
            idleAnimation.AddScriptRoutine(inputToWalkScript);

            ScriptRoutine noInputToIdleScript = new ScriptRoutine();
            noInputToIdleScript.Name = "No input to idle";
            noInputToIdleScript.Add("Is left stick active");
            noInputToIdleScript.Add("Play idle animation");
            walkAnimation.AddScriptRoutine(noInputToIdleScript);

            ScriptRoutine toggleMotionDeltaScript = new ScriptRoutine();
            toggleMotionDeltaScript.Name = "Toggle motion delta";
            toggleMotionDeltaScript.Add("Is left stick active");
            toggleMotionDeltaScript.Add("Toggle motion delta");
            walkAnimation.AddScriptRoutine(toggleMotionDeltaScript);
            attackAnimation.AddScriptRoutine(toggleMotionDeltaScript);

            ScriptRoutine playAttackScript = new ScriptRoutine();
            playAttackScript.Name = "Play attack";
            playAttackScript.Add("If attack button hit");
            playAttackScript.Add("Play attack animation");
            walkAnimation.AddScriptRoutine(playAttackScript);
            idleAnimation.AddScriptRoutine(playAttackScript);

            ScriptRoutine keepAttackingScript = new ScriptRoutine();
            keepAttackingScript.Name = "Keep attacking";
            keepAttackingScript.Add("If attack button hit");
            keepAttackingScript.Add("Play attack animation");
            attackAnimation.AddFrameRoutine(1, keepAttackingScript);

            ScriptRoutine backToIdleScript = new ScriptRoutine();
            backToIdleScript.Name = "Back to idle";
            backToIdleScript.Add("If attack button hit");
            backToIdleScript.Add("Play idle animation");
            attackAnimation.AddFrameRoutine(1, backToIdleScript);

            //crispy
            //List<string> attackTrigger = new List<string>();
            //TriggerNode newTrig = new TriggerNode(TrigType.HITBOX, new Object[] { new Rectangle(-20, -20, 20, 20), Vector2.Zero, 0f, AffectFlags.ALL});
            //newTrig.Name = "Hitbox";
            //ScriptNodeManager.Instance.RegisterNode((INode)attackTrigger);
            //attackTrigger.Add(newTrig.Name);
            //attackAnimation.AddFrameScript(1, attackTrigger);
            //crispy



            ScriptNodeManager.Instance.SerializeNodes("ness.nodes");

            string filePath = Application.StartupPath;
            filePath += "\\Content\\" + "ness" + ".fap";

            Stream stream = File.Create(filePath);
            BinaryFormatter formatter = new BinaryFormatter();
            BinaryWriter writer = new BinaryWriter(stream);

            int numAnimations = 3;
            writer.Write(numAnimations);
            formatter.Serialize(stream, idleAnimation);
            formatter.Serialize(stream, walkAnimation);
            formatter.Serialize(stream, attackAnimation);

            stream.Close();
        }

        public void Initialize()
        {
            //mPosition.X = WorldEditor.Instance.Random.Next(600) - 300;
            //mPosition.Y = WorldEditor.Instance.Random.Next(600) - 300;
            mPosition.SetPosition2D(WorldEditor.Instance.Random.Next(600) - 300, WorldEditor.Instance.Random.Next(600) - 300);

            //mInput.AddKey("moveN", Microsoft.Xna.Framework.Input.Keys.I);
            //mInput.AddKey("moveW", Microsoft.Xna.Framework.Input.Keys.J);
            //mInput.AddKey("moveS", Microsoft.Xna.Framework.Input.Keys.K);
            //mInput.AddKey("moveE", Microsoft.Xna.Framework.Input.Keys.L);
            mInput.AddButton("attack", Buttons.RightTrigger);

            SerializeNess("ness", this);

            //Entity newNess = new Entity();
            //DeserializeNess("ness", newNess);

            mAnimated.QueueAnimation("idle");
        }

        public void SerializeNess(string name, Entity ness)
        {
            string filePath = Application.StartupPath;
            filePath += "\\Content\\" + name + ".playa";
            
            Stream stream = File.Create(filePath);
            BinaryFormatter formatter = new BinaryFormatter();
            BinaryWriter writer = new BinaryWriter(stream);

            int numComponents = ness.Components.Count;

            writer.Write(numComponents);

            foreach (Component component in ness.Components.Values)
            {
                writer.Write(component.Name);
                formatter.Serialize(stream, component);
            }

            stream.Close();
        }

        public void DeserializeNess(string filename, Entity ness)
        {
            string filePath = Application.StartupPath;
            filePath += "\\Content\\" + filename + ".playa";

            if (File.Exists(filePath))
            {
                FileStream testNessFile = new FileStream(filePath, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                BinaryReader reader = new BinaryReader(testNessFile);

                try
                {
                    int numComponents = reader.ReadInt32();
                    for (int index = 0; index < numComponents; index++)
                    {
                        string componentName = reader.ReadString();
                        Component component = (Component)formatter.Deserialize(testNessFile);
                        component.AssignOwnership(ness, componentName);
                    }
                    ((AnimatedComponent)ness.GetComponentOfType("Animated")).QueueAnimation("idle");
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    testNessFile.Close();
                }
            }
        }
    }
}
