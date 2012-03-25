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

namespace FNA.WorldEditor
{
    class TestOldMan : Entity
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

        public TestOldMan(int playerNumber = 0)
        {
            mPlayerNumber = playerNumber;

            mPosition = new PositionComponent(this, 100, 100);
            mRotation = new RotationComponent(this);
            //mRenderable = new RenderableComponent(this);
            mPhysics = new PhysicsComponent(this);
            mAnimated = new AnimatedComponent(this);
            mInput = new InputComponent(this);
            mController = new PlayerControllerComponent(this);

            mHealth = new HealthComponent(this, 100.0f);
        }

        public static void InitAnimations()
        {
            Texture2D oldManTexture = WorldEditor.Instance.Content.Load<Texture2D>("oldManSheet");

            Animation walkAnimation = new Animation();
            walkAnimation.Name = "oldManWalk";
            walkAnimation.Texture = oldManTexture;
            walkAnimation.TextureName = "oldManSheet";

            Animation attackAnimation = new Animation();
            attackAnimation.Name = "oldManAttack";
            attackAnimation.Texture = oldManTexture;
            attackAnimation.TextureName = "oldManSheet";

            Animation idleAnimation = new Animation();
            idleAnimation.Name = "oldManIdle";
            idleAnimation.Texture = oldManTexture;
            idleAnimation.TextureName = "oldManSheet";

            Animation takeCoverAnimation = new Animation();
            takeCoverAnimation.Name = "oldManTakeCover";
            takeCoverAnimation.Texture = oldManTexture;
            takeCoverAnimation.TextureName = "oldManSheet";

            int frameOffset = 0;
            for (int index = 0; index < 6; index++)
            {
                takeCoverAnimation.SetFrameDuration(index, 2);
                takeCoverAnimation.AddFrame(RotationComponent.CardinalDirections.NONE, new Rectangle(frameOffset, 640, 128, 128), index);

                walkAnimation.SetFrameDuration(index, 3);
                walkAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(frameOffset, 0, 128, 128), index);
                walkAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(frameOffset, 128, 128, 128), index);
                walkAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(frameOffset, 256, 128, 128), index);
                walkAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(frameOffset, 384, 128, 128), index);
                walkAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(frameOffset, 512, 128, 128), index);

                if (index < 2)
                {
                    attackAnimation.SetFrameDuration(index, 2);
                    attackAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(frameOffset, 768, 128, 128), index);
                    attackAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(frameOffset + 384, 768, 128, 128), index);
                    attackAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(frameOffset, 896, 128, 128), index);
                    attackAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(frameOffset + 384, 896, 128, 128), index);
                    attackAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(768, frameOffset, 128, 128), index);
                }

                if (index == 0)
                {
                    idleAnimation.SetFrameDuration(index, 10);
                    idleAnimation.AddFrame(RotationComponent.CardinalDirections.E, new Rectangle(frameOffset, 0, 128, 128), index);
                    idleAnimation.AddFrame(RotationComponent.CardinalDirections.S, new Rectangle(frameOffset, 128, 128, 128), index);
                    idleAnimation.AddFrame(RotationComponent.CardinalDirections.N, new Rectangle(frameOffset, 256, 128, 128), index);
                    idleAnimation.AddFrame(RotationComponent.CardinalDirections.NE, new Rectangle(frameOffset, 384, 128, 128), index);
                    idleAnimation.AddFrame(RotationComponent.CardinalDirections.SE, new Rectangle(frameOffset, 512, 128, 128), index);
                }

                frameOffset += 128;
            }

            walkAnimation.DirectionMap[RotationComponent.CardinalDirections.SW] = RotationComponent.CardinalDirections.SE;
            walkAnimation.DirectionMap[RotationComponent.CardinalDirections.W] = RotationComponent.CardinalDirections.E;
            walkAnimation.DirectionMap[RotationComponent.CardinalDirections.NW] = RotationComponent.CardinalDirections.NE;

            idleAnimation.DirectionMap[RotationComponent.CardinalDirections.SW] = RotationComponent.CardinalDirections.SE;
            idleAnimation.DirectionMap[RotationComponent.CardinalDirections.W] = RotationComponent.CardinalDirections.E;
            idleAnimation.DirectionMap[RotationComponent.CardinalDirections.NW] = RotationComponent.CardinalDirections.NE;

            attackAnimation.DirectionMap[RotationComponent.CardinalDirections.SW] = RotationComponent.CardinalDirections.SE;
            attackAnimation.DirectionMap[RotationComponent.CardinalDirections.W] = RotationComponent.CardinalDirections.E;
            attackAnimation.DirectionMap[RotationComponent.CardinalDirections.NW] = RotationComponent.CardinalDirections.NE;

            takeCoverAnimation.DirectionMap[RotationComponent.CardinalDirections.SW] = RotationComponent.CardinalDirections.SE;
            takeCoverAnimation.DirectionMap[RotationComponent.CardinalDirections.W] = RotationComponent.CardinalDirections.E;
            takeCoverAnimation.DirectionMap[RotationComponent.CardinalDirections.NW] = RotationComponent.CardinalDirections.NE;

            walkAnimation.ChangeFrameMotionDelta(0, 0, new Vector2(0, 0.0f));
            walkAnimation.ChangeFrameMotionDelta(1, 0, new Vector2(0, 0.0f));
            walkAnimation.ChangeFrameMotionDelta(2, 0, new Vector2(0, 0.0f));
            walkAnimation.ChangeFrameMotionDelta(3, 0, new Vector2(0, 5.5f));
            walkAnimation.ChangeFrameMotionDelta(4, 0, new Vector2(0, 10.5f));
            walkAnimation.ChangeFrameMotionDelta(5, 0, new Vector2(0, 8.0f));

            attackAnimation.ChangeFrameMotionDelta(0, 10, new Vector2(0, -50f));
            attackAnimation.ChangeFrameMotionDelta(1, 10, new Vector2(0, -50f));

            AnimatedManager.Instance.AddAnimation("oldManIdle", idleAnimation);
            AnimatedManager.Instance.AddAnimation("oldManWalk", walkAnimation);
            AnimatedManager.Instance.AddAnimation("oldManAttack", attackAnimation);
            AnimatedManager.Instance.AddAnimation("oldManTakeCover", takeCoverAnimation);

            InputScriptNode isLeftStickActiveNode = new InputScriptNode();
            isLeftStickActiveNode.ScriptType = InputScriptNode.InputScriptType.TYPE_LEFT_STICK_ACTIVE;

            PlayAnimationScriptNode playWalkAnimScriptNode = new PlayAnimationScriptNode();
            playWalkAnimScriptNode.AnimationToQueue = "oldManWalk";
            playWalkAnimScriptNode.FrameToQueue = 0;
            playWalkAnimScriptNode.PlayIfTrue = true;

            PlayAnimationScriptNode playIdleAnimScriptNode = new PlayAnimationScriptNode();
            playIdleAnimScriptNode.AnimationToQueue = "oldManIdle";
            playIdleAnimScriptNode.FrameToQueue = 0;
            playIdleAnimScriptNode.PlayIfTrue = false;

            PlayAnimationScriptNode playTakeCoverAnimScriptNode = new PlayAnimationScriptNode();
            playTakeCoverAnimScriptNode.AnimationToQueue = "oldManTakeCover";
            playTakeCoverAnimScriptNode.FrameToQueue = 1;
            playTakeCoverAnimScriptNode.PlayIfTrue = true;

            ToggleMotionDeltaScriptNode toggleMotionDeltaNode = new ToggleMotionDeltaScriptNode();

            InputScriptNode attackButtonNode = new InputScriptNode();
            attackButtonNode.ScriptType = InputScriptNode.InputScriptType.TYPE_BUTTON_DOWN;
            attackButtonNode.SetInputValue("attack");

            InputScriptNode secondaryButtonNode = new InputScriptNode();
            secondaryButtonNode.ScriptType = InputScriptNode.InputScriptType.TYPE_BUTTON_DOWN;
            secondaryButtonNode.SetInputValue("secondary");

            PlayAnimationScriptNode playAttackScriptNode = new PlayAnimationScriptNode();
            playAttackScriptNode.PlayIfTrue = true;
            playAttackScriptNode.AnimationToQueue = "oldManAttack";

            PlayAnimationScriptNode backToIdleNode = new PlayAnimationScriptNode();
            backToIdleNode.AnimationToQueue = "oldManIdle";
            backToIdleNode.PlayIfTrue = false;

            PlayAnimationScriptNode stopUnderCoverNode = new PlayAnimationScriptNode();
            stopUnderCoverNode.AnimationToQueue = "oldManTakeCover";
            stopUnderCoverNode.FrameToQueue = 5;
            stopUnderCoverNode.PlayIfTrue = true;

            PlayAnimationScriptNode getOutOfCoverNode = new PlayAnimationScriptNode();
            getOutOfCoverNode.AnimationToQueue = "oldManTakeCover";
            getOutOfCoverNode.FrameToQueue = 4;
            getOutOfCoverNode.SpeedToQueue = -1.0f;
            getOutOfCoverNode.PlayIfTrue = false;

            isLeftStickActiveNode.Name = "Is left stick active";
            playWalkAnimScriptNode.Name = "Play walk animation";
            playIdleAnimScriptNode.Name = "Play idle animation";
            playTakeCoverAnimScriptNode.Name = "Play take cover animation";
            toggleMotionDeltaNode.Name = "Toggle motion delta";
            playAttackScriptNode.Name = "Play attack animation";
            attackButtonNode.Name = "If attack button hit";
            secondaryButtonNode.Name = "If secondary button hit";
            stopUnderCoverNode.Name = "Stop under cover";
            getOutOfCoverNode.Name = "Get out of cover";

            ScriptNodeManager.Instance.RegisterNode(isLeftStickActiveNode);
            ScriptNodeManager.Instance.RegisterNode(playWalkAnimScriptNode);
            ScriptNodeManager.Instance.RegisterNode(playIdleAnimScriptNode);
            ScriptNodeManager.Instance.RegisterNode(playTakeCoverAnimScriptNode);
            ScriptNodeManager.Instance.RegisterNode(playAttackScriptNode);
            ScriptNodeManager.Instance.RegisterNode(toggleMotionDeltaNode);
            ScriptNodeManager.Instance.RegisterNode(attackButtonNode);
            ScriptNodeManager.Instance.RegisterNode(secondaryButtonNode);
            ScriptNodeManager.Instance.RegisterNode(stopUnderCoverNode);
            ScriptNodeManager.Instance.RegisterNode(getOutOfCoverNode);

            ScriptNodeManager.Instance.SerializeNodes("oldMan.nodes");

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

            ScriptRoutine playSecondaryScript = new ScriptRoutine();
            playSecondaryScript.Name = "Play take cover";
            playSecondaryScript.Add("If secondary button hit");
            playSecondaryScript.Add("Play take cover animation");
            walkAnimation.AddScriptRoutine(playSecondaryScript);
            idleAnimation.AddScriptRoutine(playSecondaryScript);

            ScriptRoutine stopUnderCoverScript = new ScriptRoutine();
            stopUnderCoverScript.Name = "Stop while in cover";
            stopUnderCoverScript.Add("If secondary button hit");
            stopUnderCoverScript.Add("Stop under cover");
            takeCoverAnimation.AddFrameRoutine(5, stopUnderCoverScript);

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

            ScriptRoutine stopTakingCoverScript = new ScriptRoutine();
            stopTakingCoverScript.Name = "Get out of cover";
            stopTakingCoverScript.Add("If secondary button hit");
            stopTakingCoverScript.Add("Get out of cover");
            takeCoverAnimation.AddFrameRoutine(5, stopTakingCoverScript);

            ScriptRoutine getOutOfCoverScript = new ScriptRoutine();
            getOutOfCoverScript.Name = "Idle after cover";
            getOutOfCoverScript.Add("Play idle animation");
            takeCoverAnimation.AddFrameRoutine(0, getOutOfCoverScript);

            ScriptNodeManager.Instance.SerializeNodes("nodes.MILF");

            string filePath = Application.StartupPath;
            filePath += "\\Content\\" + "oldMan" + ".fap";

            Stream stream = File.Create(filePath);
            BinaryFormatter formatter = new BinaryFormatter();
            BinaryWriter writer = new BinaryWriter(stream);

            int numAnimations = 4;
            writer.Write(numAnimations);
            formatter.Serialize(stream, idleAnimation);
            formatter.Serialize(stream, walkAnimation);
            formatter.Serialize(stream, attackAnimation);
            formatter.Serialize(stream, takeCoverAnimation);

            stream.Close();
        }

        public static void LoadAnimations()
        {
            List<Animation> animations = new List<Animation>();

            OpenFileDialog openFAPDialog = new OpenFileDialog();
            openFAPDialog.Filter = "FAP File|*.fap";
            openFAPDialog.Title = "Where is the old man FAP file?";
            openFAPDialog.ShowDialog();

            if (openFAPDialog.FileName != null)
            {
                animations = AnimatedManager.Instance.LoadFAP(openFAPDialog.FileName);
            }
            
            OpenFileDialog openNodesMILFDialog = new OpenFileDialog();
            openNodesMILFDialog.Filter = "MILF File|*.MILF";
            openNodesMILFDialog.Title = "Select the nodes.MILF file";
            openNodesMILFDialog.ShowDialog();

            if (openNodesMILFDialog.FileName != null)
            {
                ScriptNodeManager.Instance.DeserializeNodes(openNodesMILFDialog.FileName, true);
            }

            foreach (Animation anim in animations)
            {
                AnimatedManager.Instance.AddAnimation(anim.Name, anim);
            }
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
            mInput.AddButton("secondary", Buttons.LeftTrigger);

            SerializeNess("ness", this);

            //Entity newNess = new Entity();
            //DeserializeNess("ness", newNess);

            mAnimated.QueueAnimation("oldManIdle");
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

                //    Assembly assembly = typeof(InputScriptNode).Assembly;

                //    string typeName = "FNA.Scripts.";
                //    typeName += node.Attributes["NodeType"].InnerText;
                //    ScriptNode newNode = (ScriptNode)Activator.CreateInstance(assembly.GetType(typeName));
                //    newNode.Name = node.Attributes["NodeName"].InnerText;
            }
        }
    }
}
