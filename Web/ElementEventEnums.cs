using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderTreeBuildHelper
{
    public enum FocusEvent : int
    {
        Focus,
        FocusIn,
        FocusOut,
    }

    public static class FocusEventExt
    {
        /// <summary>
        /// Converts a touch event enum to a string for a JavaScript function.
        /// </summary>
        /// <param name="focusEvent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetEventName(this FocusEvent focusEvent)
        {
            return focusEvent switch
            {
                FocusEvent.Focus => "onfocus",
                FocusEvent.FocusIn => "onfocusin",
                FocusEvent.FocusOut => "onfocusout",
                _ => throw new NotImplementedException($"selected value '{(int)focusEvent}' is not in enum.'")
            };
        }
    }
    /// <summary>
    /// mouse event enums
    /// </summary>
    public enum MouseEvent : int
    {
        Click,
        Ctextmenu,
        Dblclick,
        Mousedown,
        Mouseenter,
        Mouseleave,
        Mousemove,
        Mouseout,
        Mouseover,
        Mouseup,
        Drag,
        Dragend,
        Dragenter,
        Dragleave,
        Dragover,
        Dragstart,
        Drop,
    }

    public static class MouseEventExt
    {
        /// <summary>
        /// Converts a mouse event enum to a string for a JavaScript function.
        /// </summary>
        /// <param name="mouseEvent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetEventName(this MouseEvent mouseEvent)
        {
            return mouseEvent switch
            {
                MouseEvent.Click => "onclick",
                MouseEvent.Ctextmenu => "onctextmenu",
                MouseEvent.Dblclick => "ondblclick",
                MouseEvent.Mousedown => "onmousedown",
                MouseEvent.Mouseenter => "onmouseenter",
                MouseEvent.Mouseleave => "onmouseleave",
                MouseEvent.Mousemove => "onmousemove",
                MouseEvent.Mouseout => "onmouseout",
                MouseEvent.Mouseover => "onmouseover",
                MouseEvent.Mouseup => "onmouseup",
                MouseEvent.Drag => "ondrag",
                MouseEvent.Dragend => "ondragend",
                MouseEvent.Dragenter => "ondragenter",
                MouseEvent.Dragleave => "ondragleave",
                MouseEvent.Dragover => "ondragover",
                MouseEvent.Dragstart => "ondragstart",
                MouseEvent.Drop => "ondrop",
                _ => throw new NotImplementedException($"selected value '{(int)mouseEvent}' is not in enum.'")
            };
        }
    }
    /// <summary>
    /// touch event enums
    /// </summary>
    public enum TouchEvent
    {
        Touchstart,
        Touchend,
        Touchmove,
        Touchcancel,
    }

    public static class TouchEventExt
    {
        /// <summary>
        /// Converts a touch event enum to a string for a JavaScript function.
        /// </summary>
        /// <param name="touchEvent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetEventName(this TouchEvent touchEvent)
        {
            return touchEvent switch
            {
                TouchEvent.Touchstart => "ontouchstart",
                TouchEvent.Touchend => "ontouchend",
                TouchEvent.Touchmove => "ontouchmove",
                TouchEvent.Touchcancel => "ontouchcancel",
                _ => throw new NotImplementedException($"selected value '{(int)touchEvent}' is not in enum.'")
            };
        }
    }
}
