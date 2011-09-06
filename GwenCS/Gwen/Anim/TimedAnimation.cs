using System;

namespace Gwen.Anim
{
    // Timed animation. Provides a useful base for animations.
    public class TimedAnimation : Animation
    {
        protected bool m_bStarted;
        protected bool m_bFinished;
        protected float m_fStart;
        protected float m_fEnd;
        protected float m_fEase;

        public override bool Finished { get { return m_bFinished; } }

        public TimedAnimation( float fLength, float fDelay = 0.0f, float fEase = 1.0f )
        {
            m_fStart = Platform.Windows.GetTimeInSeconds() + fDelay;
            m_fEnd = m_fStart + fLength;
            m_fEase = fEase;
            m_bStarted = false;
            m_bFinished = false;
        }

        protected override void Think()
        {
            base.Think();

            if (m_bFinished) 
                return;

            float fCurrent = Platform.Windows.GetTimeInSeconds();
            float fSecondsIn = fCurrent - m_fStart;
            if (fSecondsIn < 0.0) 
                return;

            if (!m_bStarted)
            {
                m_bStarted = true;
                onStart();
            }

            float fDelta = fSecondsIn/(m_fEnd - m_fStart);
            if (fDelta < 0.0f) 
                fDelta = 0.0f;
            if (fDelta > 1.0f) 
                fDelta = 1.0f;

            Run((float)Math.Pow(fDelta, m_fEase));

            if (fDelta == 1.0f)
            {
                m_bFinished = true;
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
