using System;
using System.Collections.Generic;
using Gwen.Controls;

namespace Gwen.Anim
{
    public class Animation
    {
        protected Base m_Control;

        //private static List<Animation> g_AnimationsListed = new List<Animation>(); // unused
        private static Dictionary<Base, List<Animation>> g_Animations = new Dictionary<Base, List<Animation>>();

        public virtual void Think()
        {
            throw new Exception("Pure virtual function call");
        }

        public virtual bool Finished
        {
            get { throw new Exception("Pure virtual function call"); }
        }

        public static void Add(Base control, Animation animation)
        {
            animation.m_Control = control;
            if (!g_Animations.ContainsKey(control))
                g_Animations[control] = new List<Animation>();
            g_Animations[control].Add(animation);
        }

        public static void Cancel(Base control)
        {
            if (g_Animations.ContainsKey(control))
            {
                g_Animations[control].Clear();
                g_Animations.Remove(control);
            }
        }

        public static void GlobalThink()
        {
            foreach (KeyValuePair<Base, List<Animation>> pair in g_Animations)
            {
                var valCopy = pair.Value.FindAll(x =>true); // list copy so foreach won't break when we remove elements
                foreach (Animation animation in valCopy)
                {
                    animation.Think();
                    if (animation.Finished)
                    {
                        pair.Value.Remove(animation);
                    }
                }
            }
        }
    }
}
