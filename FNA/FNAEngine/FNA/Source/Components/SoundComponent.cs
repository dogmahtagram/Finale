using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using FNA.Core;
using FNA.Interfaces;

namespace FNA.Components
{
    /// <summary>
    /// The SoundComponent class enables an Entity to play sounds.
    /// </summary>
    public class SoundComponent_cl : Component_cl, IUpdateAble
    {
        /// <summary>
        /// The SoundComponent SoundBank
        /// </summary>
        protected SoundBank mComponentSoundBank
        {
            get
            {
                return Managers.SoundManager_cl.Instance.ProjectSoundBank;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SoundBank ComponentSoundBank
        {
            get
            {
                return mComponentSoundBank;
            }
        }
        /// <summary>
        /// List of all 3D sounds
        /// </summary>
        protected List<Cue> mSounds3DList;

        /// <summary>
        /// 
        /// </summary>
        public List<Cue> Sounds3DList
        {
            get
            {
                return mSounds3DList;
            }
        }

        /// <summary>
        /// Sound listener for 3D sounds
        /// </summary>
        private AudioListener mSoundListener
        {
            get
            {
                return Managers.SoundManager_cl.Instance.Listener;
            }
        }

        /// <summary>
        /// Sound emitter for 3D sounds
        /// </summary>
        private AudioEmitter mSoundEmitter;

        /// <summary>
        /// Base constructor - registers the component and adds it to the entity.
        /// </summary>
        /// <param name="parent"></param>
        public SoundComponent_cl(Entity_cl parent)
            : base(parent)
        {
            mSoundEmitter = new AudioEmitter();
            PositionComponent_cl emitterPosition = (PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl));
            mSoundEmitter.Position = emitterPosition.Position3D;

            Managers.SoundManager_cl.Instance.RegisterComponent(this);
            mParentEntity.AddComponent(this);

            mSounds3DList = new List<Cue>();
        }

        /// <summary>
        /// Update loop for component
        /// </summary>
        public void Update()
        {
            Update3DSoundsList();
            UpdateApply3D();
        }

        /// <summary>
        /// Method to get cue from component
        /// </summary>
        /// <param name="cueName"></param>
        public Cue GetSoundCue(string cueName)
        {
            return mComponentSoundBank.GetCue(cueName);
        }

        /// <summary>
        /// Play a 2D sound
        /// </summary>
        /// <param name="cueName"></param>
        public void PlaySound(string cueName)
        {
            Cue cue = mComponentSoundBank.GetCue(cueName);
            cue.Play();
        }

        /// <summary>
        /// Play a 3D sound
        /// </summary>
        /// <param name="cueName"></param>
        public void Play3DSound(string cueName)
        {
            Cue cue = mComponentSoundBank.GetCue(cueName);
            SetEmitterPosition();
            cue.Apply3D(mSoundListener, mSoundEmitter);
            mSounds3DList.Add(cue);
            cue.Play();
        }

        /// <summary>
        /// Update Apply3D transform of all 3D sounds
        /// </summary>
        private void UpdateApply3D()
        {
            SetEmitterPosition();

            foreach (Cue cue in mSounds3DList)
            {
                cue.Apply3D(mSoundListener, mSoundEmitter);
            }
        }

        /// <summary>
        /// Update 3D sounds list, removing those that finished playing
        /// </summary>
        private void Update3DSoundsList()
        {
            for (int i = 0; i < mSounds3DList.Count; )
            {
                if (mSounds3DList[i].IsStopped)
                {
                    mSounds3DList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
        
        /// <summary>
        /// Sets the position of this component's AudioEmitter
        /// </summary>
        private void SetEmitterPosition()
        {
            PositionComponent_cl emitterPosition = (PositionComponent_cl)mParentEntity.GetComponentOfType(typeof(PositionComponent_cl));
            Vector3 componentVector = emitterPosition.Position3D;
            componentVector.Z = componentVector.Y;
            componentVector.Y = 0;
            mSoundEmitter.Position = componentVector;
        }
    }
}
