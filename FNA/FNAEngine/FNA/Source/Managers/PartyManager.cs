using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FNA.Graphics;
using FNA.Components;
using FNA.Core;

namespace FNA.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class PartyManager_cl : BaseManager_cl
    {
        /// <summary>
        /// 
        /// </summary>
        private Renderer mParticleRenderer;
       
        private Vector2 ParticleLocation;
        private static PartyManager_cl sInstance = new PartyManager_cl();
        
        /// <summary>
        /// 
        /// </summary>
        public static PartyManager_cl Instance
        {
            get
            {
                return sInstance;
            }
        }

        private long mUniqueID = 0;
        
        private List<ParticleComponent_cl> mPartyList = new List<ParticleComponent_cl>();

        /// <summary>
        /// 
        /// </summary>
        public List<ParticleComponent_cl> PartyList
        {
            get
            {
                return mPartyList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PartyManager_cl()
        {
            mParticleRenderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = FNA.Game_cl.BaseInstance.Graphics
            };

            mParticleRenderer.LoadContent(FNA.Game_cl.BaseInstance.Content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="location"></param>
        public Entity_cl CreateNewParticle(string name, Vector2 location)
        {
            Entity_cl entity = new Entity_cl();

            ParticleLocation = location;

            ParticleComponent_cl partyEffect;
            partyEffect = new ParticleComponent_cl(entity,ParticleLocation);

            partyEffect.particle = FNA.Game_cl.BaseInstance.Content.Load<ParticleEffect>((name)).DeepCopy();
            partyEffect.particle.LoadContent(FNA.Game_cl.BaseInstance.Content);
            partyEffect.particle.Initialise();

            RegisterNewParty(partyEffect);

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public long RegisterNewParty(ParticleComponent_cl p)
        {
            mPartyList.Add(p);
            return mUniqueID++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="number"></param>
        public void ChangeParticleSpeed(ParticleComponent_cl p, int number)
        {
            foreach (Emitter e in p.particle)
            {
                e.ReleaseSpeed = number;
                
               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="number"></param>
        public void ChangeParticleNumber(ParticleComponent_cl p, int number)
        {
            foreach (Emitter e in p.particle)
            {
               
                e.ReleaseQuantity = number;
                
            }
        }

        //TODO change particle color with sound or other inputs.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pColor"></param>
        public void ChangeParticleColor(ParticleComponent_cl p, Color pColor)
        {
            foreach (Emitter e in p.particle)
            {
                e.ReleaseColour = pColor.ToVector3();
            }
        }

        // This isn't working not sure why yet
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="number"></param>
        public void ChangeParticleOpacity(ParticleComponent_cl p, VariableFloat number)
        {
            foreach (Emitter e in p.particle)
            {
                e.ReleaseOpacity += number;
                
            }
        }

        // Use for wind
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        public void ChangeParticleImpulse(ParticleComponent_cl p, float num1, float num2)
        {
            foreach (Emitter e in p.particle)
            {
                e.ReleaseImpulse = new Vector2(num1, num2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="number"></param>
        public void ChangeParticleScale(ParticleComponent_cl p,VariableFloat number)
        {

            foreach (Emitter e in p.particle)
            {
                e.ReleaseScale = number;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="number"></param>
        public void ChangeParticleRotation(ParticleComponent_cl p, VariableFloat number)
        {
            foreach (Emitter e in p.particle)
            {
                e.ReleaseRotation = number;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public void KillParticles(ParticleComponent_cl p)
        {
            foreach (Emitter e in p.particle)
            {
                e.Terminate();
                
            }
        }

        


        /// <summary>
        /// 
        /// </summary>
        /// 
        public override void PreUpdate()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            List<ParticleComponent_cl> KillParticleList = new List<ParticleComponent_cl>();

            foreach(ParticleComponent_cl p in mPartyList)
            {

                /************************************************************************/
                /* Test Area                                                            */
                /************************************************************************/

                KeyboardState keystate = Keyboard.GetState();



                if (InputManager_cl.Instance.WasKeyPressed(Keys.K))
                {

                    
                    
                    //ChangeParticleColor(p,Color.SeaGreen);
                    //ChangeParticleOpacity(p,(float)10);
                    //ChangeParticleImpulse(p,(float)200,(float)50);
                    //ChangeParticleScale(p, (float)50);
                    //ChangeParticleRotation(p, (float)500);

                    // This creates a list of particles that are to be deleted on the next pass.
                    KillParticleList.Add(p);
                    
                    //PartyList.Remove(p);
                    

                    
                }

                else if(keystate.IsKeyDown(Keys.L))
                {
                    //ChangeParticleColor(p, Color.SteelBlue);
                    //ChangeParticleOpacity(p, (float)-10);
                    //ChangeParticleImpulse(p, (float)-200, (float)-50);
                    //ChangeParticleScale(p, (float)10);
                    //ChangeParticleRotation(p, (float)-500);
                    
                }

                /************************************************************************/

                p.particle.Trigger(p.particlelocation);

                float SecondsPassed = (float)FNA.Game_cl.BaseInstance.Timer.ActualTime.ElapsedGameTime.TotalSeconds;

                p.particle.Update(SecondsPassed);                
            }

            foreach(ParticleComponent_cl p in KillParticleList)
            {
                mPartyList.Remove(p);
            }

            KillParticleList.Clear();



        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            foreach (ParticleComponent_cl p in PartyManager_cl.Instance.PartyList)
            {
                mParticleRenderer.RenderEffect(p.particle);
            }
        }
    }
}
