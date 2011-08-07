using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gwen.Renderer
{
    public interface ICacheToTexture
    {
        void Initialize();
        void ShutDown();
        void SetupCacheTexture(Controls.Base control);
        void FinishCacheTexture(Controls.Base control);
        void DrawCachedControlTexture(Controls.Base control);
        void CreateControlCacheTexture(Controls.Base control);
        void UpdateControlCacheTexture(Controls.Base control);
        void SetRenderer(Base renderer);
    }
}
