using System;

namespace Gwen.Anim
{
    // Timed animation. Provides a useful base for animations.
    public class TimedAnimation : Animation
    {
        protected bool m_Started;
        protected bool m_Finished;
        protected float m_Start;
        protected float m_End;
        protected float m_Ease;

        public override bool Finished { get { return m_Finished; } }

        public TimedAnimation( float length, float delay = 0.0f, float ease = 1.0f )
        {
            m_Start = Platform.Windows.GetTimeInSeconds() + delay;
            m_End = m_Start + length;
            m_Ease = ease;
            m_Started = false;
            m_Finished = false;
        }

        protected override void Think()
        {
            base.Think();

            if (m_Finished) 
                return;

            float fCurrent = Platform.Windows.GetTimeInSeconds();
            float fSecondsIn = fCurrent - m_Start;
            if (fSecondsIn < 0.0) 
                return;

            if (!m_Started)
            {
                m_Started = true;
                onStart();
            }

            float fDelta = fSecondsIn/(m_End - m_Start);
            if (fDelta < 0.0f) 
                fDelta = 0.0f;
            if (fDelta > 1.0f) 
                fDelta = 1.0f;

            Run((float)Math.Pow(fDelta, m_Ease));

            if (fDelta == 1.0f)
            {
                m_Finished = true;
                onFinish();
            }
        }

        // These are the magic functions you should be overriding

        protected virtual void onStart()
        {}

        protected virtual void Run(float delta)
        {}

        protected virtual void onFinish()
        {}
    }
}
