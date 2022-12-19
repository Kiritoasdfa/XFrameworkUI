using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework
{
    public abstract class GraphicComponent<T> : UIComponent<T> where T : Graphic
    {
        public void SetColor(Color color)
        {
            this.Get().SetColor(color);
        }

        public void SetColor(string hexColor)
        {
            this.Get().SetColor(hexColor);
        }

        public void SetAlpha(float a)
        {
            this.Get().SetAlpha(a);
        }

        public Color Color()
        {
            return this.Get().color;
        }

        public MiniTween DoFade(float endValue, float duration)
        {
            return this.Get().DoFade(this, endValue, duration);
        }

        public MiniTween DoFade(float startValue, float endValue, float duration)
        {
            return this.Get().DoFade(this, startValue, endValue, duration);
        }
    }

    public static class GraphicComponentExtensions
    {
        #region MiniTween

        public static MiniTween DoFloat(this Graphic self, XObject parent, float startValue, float endValue, float duration, Action<float> setter)
        {
            var tweenMgr = Common.Instance.Get<MiniTweenManager>();
            if (tweenMgr is null)
                return null;

            var tween = tweenMgr.To(parent, startValue, endValue, duration);
            tween.AddListener(v =>
            {
                if (!self)
                {
                    tween.Cancel(parent);
                    return;
                }

                setter.Invoke(v);
            });

            return tween;
        }

        public static MiniTween DoFade(this Graphic self, XObject parent, float endValue, float duration)
        {
            return self.DoFade(parent, self.color.a, endValue, duration);
        }

        public static MiniTween DoFade(this Graphic self, XObject parent, float startValue, float endValue, float duration)
        {
            return self.DoFloat(parent, startValue, endValue, duration, self.SetAlpha);
        }

        public static void SetColor(this Graphic self, Color color)
        {
            self.color = color;
        }

        public static void SetColor(this Graphic self, string hexColor)
        {
            if (ColorUtility.TryParseHtmlString(hexColor, out var color))
            {
                self.SetColor(color);
            }
        }

        public static void SetAlpha(this Graphic self, float a)
        {
            a = Mathf.Clamp01(a);
            Color color = self.color;
            color.a = a;
            self.SetColor(color);
        }

        #endregion
    }
}
