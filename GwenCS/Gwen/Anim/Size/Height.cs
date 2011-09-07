using System;

namespace Gwen.Anim.Size
{
    class Height : TimedAnimation
    {
        protected int m_StartSize;
        protected int m_Delta;
        protected bool m_Hide;

        public Height(int startSize, int endSize, float length, bool hide = false, float delay = 0.0f, float ease = 1.0f) 
            : base( length, delay, ease )
        {
            m_StartSize = startSize;
            m_Delta = endSize - m_StartSize;
            m_Hide = hide;
        }

        protected override void onStart() { base.onStart(); m_Control.Height = m_StartSize; }
        protected override void Run(float delta) { base.Run(delta); m_Control.Height = Global.Trunc( m_StartSize + (m_Delta * delta)); }
        protected override void onFinish() { base.onFinish(); m_Control.Height = m_StartSize + m_Delta; m_Control.IsHidden = m_Hide; }
    }
}
