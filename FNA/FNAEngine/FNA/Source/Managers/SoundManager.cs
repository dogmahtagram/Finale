using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using FNA.Components;
using FNA.Core;

namespace FNA.Managers
{   
    /// <summary>
    /// 
    /// </summary>
    public sealed class SoundManager_cl : BaseManager_cl
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly SoundManager_cl sInstance = new SoundManager_cl();

        /// <summary>
        /// 
        /// </summary>
        public static SoundManager_cl Instance
        {
            get
            {
                return sInstance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<SoundComponent_cl> mSoundComponentsList;

        /// <summary>
        /// 
        /// </summary>
        public List<SoundComponent_cl> SoundComponentsList
        {
            get
            {
                return mSoundComponentsList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private AudioEngine mProjectAudioEngine;

        /// <summary>
        /// 
        /// </summary>
        public AudioEngine ProjectAudioEngine
        {
            get
            {
                return mProjectAudioEngine;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private WaveBank mProjectWaveBank;

        /// <summary>
        /// 
        /// </summary>
        public WaveBank ProjectWaveBank
        {
            get
            {
                return mProjectWaveBank;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private SoundBank mProjectSoundBank;

        /// <summary>
        /// 
        /// </summary>
        public SoundBank ProjectSoundBank
        {
            get
            {
                return mProjectSoundBank;
            }
        }

        /// <summary>
        /// This should be the player entity
        /// </summary>
        private Entity_cl mListenerEntity;

        /// <summary>
        /// 
        /// </summary>
        public Entity_cl ListenerEntity
        {
            get
            {
                return mListenerEntity;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private AudioListener mListener;

        /// <summary>
        /// 
        /// </summary>
        public AudioListener Listener
        {
            get
            {
                return mListener;
            }
        }

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        private SoundManager_cl()
        {
            mSoundComponentsList = new List<SoundComponent_cl>();
        }

        /// <summary>
        /// Initialize audio engine, wave bank, sound bank, listenerEntity
        /// </summary>
        public void InitializeXACT(string audioEnginePath, string waveBankPath, string soundBankPath, Entity_cl listenerEntity)
        {
            mProjectAudioEngine = new AudioEngine(audioEnginePath);
            mProjectWaveBank = new WaveBank(mProjectAudioEngine, waveBankPath);
            mProjectSoundBank = new SoundBank(mProjectAudioEngine, soundBankPath);
            mListenerEntity = listenerEntity;
            CreateAudioListener();
        }

        /// <summary>
        /// Adds a Component to this manager's list of all SoundComponents.
        /// Overrides IManager's implementation.
        /// </summary>
        /// <param name="c">The Component to add.</param>
        public void RegisterComponent(SoundComponent_cl c)
        {
            mSoundComponentsList.Add((SoundComponent_cl)c);
        }

        /// <summary>
        /// Removes a Component from this manager's list of all PointLightComponents.
        /// Overrides IManager's implementation.
        /// </summary>
        /// <param name="c"></param>
        public void UnregisterComponent(SoundComponent_cl c)
        {
            mSoundComponentsList.Remove((SoundComponent_cl)c);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void PreUpdate()
        {
        }

        /// <summary>
        /// Updates all SoundComponents registered with this manager.
        /// Overrides IManager's implementation.
        /// </summary>
        public override void Update()
        {
            SetListenerPosition();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateAudioEngine()
        {
            mProjectAudioEngine.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        private Vector3 ListenerPosition
        {
            get
            {
                PositionComponent_cl component = (PositionComponent_cl)mListenerEntity.GetComponentOfType(typeof(PositionComponent_cl));
                Vector3 componentVector = component.Position3D;
                componentVector.Z = componentVector.Y;
                componentVector.Y = 0;
                return componentVector;
            }
        }

        /// <summary>
        /// Sets the position of the manager's AudioListener
        /// </summary>
        private void SetListenerPosition()
        {
            // the listener position should be set in the sound manager
            // there should be a player entity with an audio listener
            mListener.Position = Managers.SoundManager_cl.Instance.ListenerPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateAudioListener()
        {
            mListener = new AudioListener();
        }
    }
}
