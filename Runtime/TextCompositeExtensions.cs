using UnityEngine.UI;
#if YVSIM_TMP
using TMPro;
#endif

namespace Yvsim.Extensions
{
    /// <summary>
    /// 存放由多个独立扩展组合而成的文本扩展方法：设置文本，并通过 <see cref="ActiveStateExtensions.YvSetActive"/> 按内容控制显隐。
    /// </summary>
    public static class TextCompositeExtensions
    {
        /// <summary>设置 UGUI Text 文本，并可根据内容是否为空控制物体显隐。</summary>
        /// <param name="target">目标 Text；为 null 时不执行任何操作。</param>
        /// <param name="value">要设置的文本；null 按空字符串处理。</param>
        /// <param name="hideWhenEmpty">为 true 时，文本为空则隐藏物体、非空则显示物体。</param>
        public static void YvSetText(this Text target, string value, bool hideWhenEmpty = false)
        {
            if (target == null)
            {
                return;
            }

            target.text = value ?? string.Empty;
            if (hideWhenEmpty)
            {
                target.YvSetActive(!string.IsNullOrEmpty(value));
            }
        }

#if YVSIM_TMP
        /// <summary>设置 TMP_Text 文本，并可根据内容是否为空控制物体显隐。仅在项目包含 TextMesh Pro 时可用。</summary>
        /// <param name="target">目标 TMP_Text；为 null 时不执行任何操作。</param>
        /// <param name="value">要设置的文本；null 按空字符串处理。</param>
        /// <param name="hideWhenEmpty">为 true 时，文本为空则隐藏物体、非空则显示物体。</param>
        public static void YvSetText(this TMP_Text target, string value, bool hideWhenEmpty = false)
        {
            if (target == null)
            {
                return;
            }

            target.text = value ?? string.Empty;
            if (hideWhenEmpty)
            {
                target.YvSetActive(!string.IsNullOrEmpty(value));
            }
        }
#endif
    }
}
