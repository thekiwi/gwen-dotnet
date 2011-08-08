using System;

namespace Gwen.Anim
{
    // Timed animation. Provides a useful base for animations.
    public class TimedAnimation : Animation
    {
        protected bool m_bStarted;
        protected bool m_bFinished;
        protected double m_fStart;
        protected double m_fEnd;
        protected double m_fEase;

        public override bool Finished { get { return m_bFinished; } }

        public TimedAnimation( double fLength, double fDelay = 0.0, double fEase = 1.0 )
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

            double fCurrent = Platform.Windows.GetTimeInSeconds();
            double fSecondsIn = fCurrent - m_fStart;
            if (fSecondsIn < 0.0) 
                return;

            if (!m_bStarted)
            {
                m_bStarted = true;
                onStart();
            }

            double fDelta = fSecondsIn/(m_fEnd - m_fStart);
            if (fDelta < 0.0) 
                fDelta = 0.0;
            if (fDelta > 1.0) 
                fDelta = 1.0;

            Run(Math.Pow(fDelta, m_fEase));

            if (fDelta == 1.0)
            {
                m_bFinished = true;
                onFinish();
            }
        }

        // These are the magic functions you should be overriding

        protected virtual void onStart()
        {}

        protected virtual void Run(double delta)
        {}

        protected virtual void onFinish()
        {}
    }
}
