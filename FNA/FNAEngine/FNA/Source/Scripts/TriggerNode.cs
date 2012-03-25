//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using FNA.Components;
//using FNA.Core;
//using FNA.Managers;
//using FNA.Scripts;

//namespace FNA.Triggers
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public enum TriggerType
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        HITBOX
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    public class TriggerNode : INode
//    {
//        //protected Vector2 mPosition;
//        //public Vector2 Position
//        //{
//        //    get
//        //    {
//        //        return mPosition;
//        //    }
//        //}

//        //protected Vector2 mVelocity;

//        //protected float mLifeLeft;

//        //public bool mExists;

//        TrigType TriggerType;

//        Object[] Arguments;

//        /// <summary>
//        /// 
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="type"></param>
//        /// <param name="args"></param>
//        public TriggerNode(TrigType type, Object[] args) //Vector2 position, float duration = float.MaxValue, AffectFlags flags = 0)
//        {
//            TriggerType = type;
            
//            Arguments = args;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="input"></param>
//        public void SetInputValue(object input)
//        {
//            return;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public virtual void PreUpdate()
//        {
       
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public virtual void RemoveSelf()
//        {
            
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public void Draw()
//        {
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public string GetName()
//        {
//            return Name;
//        }
//        /// <summary>
//        /// Process node is the main script method which performs the function of a ScriptNode.
//        /// </summary>
//        /// <param name="entity">entity is the entity of whos components are relevant to the current script.</param>
//        /// <param name="next">next is the next ScriptNode in the current script of whos input we will set with the current node's output.</param>
//        public virtual void ProcessNode(Entity_cl entity, string next)
//        {
//            Rectangle area = (Rectangle)Arguments[0];
//            Vector2 pos = ((Components.PositionComponent)entity.GetComponentOfType(typeof(PositionComponent))).Position2D;
//            area.X += (int)pos.X;
//            area.Y += (int)pos.Y;
            

//            switch (TriggerType)
//            {
//                case TrigType.HITBOX:
//                    trigger = new HitBoxTrigger_cl(area, (Vector2)Arguments[1], (float)Arguments[2]);
//                    break;
//                default:
//                    return;
//            }
//            if(trigger != null)
//                TriggerManager_cl.Instance.AddTrigger(trigger);
//        }
//    }
//}
