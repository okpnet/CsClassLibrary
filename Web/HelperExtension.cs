using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Xml.Schema;

namespace RenderTreeBuildHelper
{
    public static class HelperExtension
    {
        internal const string ATTR_CLASS = "class";
        internal const string ATTR_CHILD_CONTENT = "ChildContent";
        internal const string ATTR_VALUE = "Value";
        internal const string ATTR_VALUE_CHANGE = "ValueChanged";
        internal const string ATTR_VALUE_EXPRESSION = "ValueExpression";
        internal const string ATTR_NAME = "Name";

        public static EventCallback<T> CreateEventCallback<T>(this T? value,object receiver, Action<T> callback)=> EventCallback.Factory.Create<T>(receiver, callback);

        public static EventCallback<T> CreateEventCallback<T>(this T? value, object receiver, Func<T, Task> callback)=> EventCallback.Factory.Create<T>(receiver, callback);

        public static EventCallback<T> CreateEventCallback<T>(this T? value, object receiver, EventCallback<T> callback)=> EventCallback.Factory.Create<T>(receiver, callback);
        /// <summary>
        /// Close the element after the action of opening the  'select' element and inserting content.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="value"></param>
        /// <param name="valueChange"></param>
        /// <param name="expression"></param>
        /// <param name="options"></param>
        public static void OpenInputSelectHelper<T>(this RenderTreeBuilder builder, ref int sequence, T value, EventCallback<T> valueChange, Expression<Func<T>> expression, RenderFragment options)
        {
            int seq = sequence;
            builder.OpenComponentHelper<InputSelect<T>>(ref seq, () =>
            {
                builder.AddAttribute(seq++, ATTR_VALUE, value);
                builder.AddAttribute(seq++, ATTR_VALUE_CHANGE, valueChange);
                builder.AddAttribute(seq++, ATTR_VALUE_EXPRESSION, expression);
                builder.AddAttribute(seq++, ATTR_CHILD_CONTENT, options);
            });
            sequence = seq;
            return;
        }
        /// <summary>
        /// Add  class.
        /// Specify the class value string as an array.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="seq"></param>
        /// <param name="classes"></param>
        /// <returns></returns>
        public static void AddClassHelper(this RenderTreeBuilder builder,ref int sequence, params string[] classes)
        {
            var seq = sequence;
            if (classes.Length == 0) return;
            builder.AddAttribute(seq++, "class", string.Join(" ", classes));
            sequence = seq;
            return;
        }
        /// <summary>
        /// Close the element after the action of opening the element and inserting content.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="element"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static void OpenElementHelper(this RenderTreeBuilder builder,ref int sequence, string element, Action contents)
        {
            var seq = sequence;
            builder.OpenElement(seq++, element);
            contents.Invoke();
            builder.CloseElement();
            sequence = seq;
            return;
        }
        /// <summary>
        /// Close the component after the action of opening the component and inserting content.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static void OpenComponentHelper<TComponent>(this RenderTreeBuilder builder,ref int sequence, Action contents) where TComponent : notnull, Microsoft.AspNetCore.Components.IComponent
        {
            var seq = sequence;
            builder.OpenComponent<TComponent>(seq++);
            contents.Invoke();
            builder.CloseComponent();
            sequence = seq;
            return;
        }
        /// <summary>
        /// Close the component after the action of opening the cascade component and inserting content.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <param name="action"></param>
        public static void OpenCascaingParameter<TValue>(this RenderTreeBuilder builder, ref int sequence,TValue value,string parameterName, Action<RenderTreeBuilder> action)
        {
            var seq = sequence;
            builder.OpenComponentHelper<CascadingValue<TValue>>(ref seq, ()=>
            {
                builder.AddAttribute(seq++, ATTR_VALUE, value);
                builder.AddAttribute(seq++, ATTR_NAME, parameterName);
                builder.AddAttribute(seq++, ATTR_CHILD_CONTENT, (RenderFragment)(action.Invoke));
            });
            sequence = seq;
            return;
        }
        /// <summary>
        /// Close the component after the action of opening the cascade component and inserting content.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="value"></param>
        /// <param name="action"></param>
        public static void OpenCascaingParameter<TValue>(this RenderTreeBuilder builder, ref int sequence, TValue value, Action<RenderTreeBuilder> action)
        {
            var seq = sequence;
            builder.OpenComponentHelper<CascadingValue<TValue>>(ref seq, () =>
            {
                builder.AddAttribute(seq++, ATTR_VALUE, value);
                builder.AddAttribute(seq++, ATTR_CHILD_CONTENT, (RenderFragment)(action.Invoke));
            });
            sequence = seq;
            return;
        }
        /// <summary>
        /// &U+003C span class="icon" &U+003E &U+003Ci class="gg-play-list-add"/&U+003E &U+003C/span &U+003Eを生成
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="seq"></param>
        /// <param name="iconClass"></param>
        /// <returns></returns>
        public static void AddIcons(this RenderTreeBuilder builder,ref int sequence, string iconClass)
        {
            var seq = sequence;
            builder.OpenElementHelper(ref seq, "span", () =>
            {

                builder.AddClassHelper(ref seq, "icon");
                builder.OpenElementHelper(ref seq, "i", () => {
                    builder.AddClassHelper(ref seq, iconClass);
                });
            });
            sequence = seq;
            return;
        }
        /// <summary>
        /// Optional parameter attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="name"></param>
        /// <param name="addAttr"></param>
        public static void AddAttributeHelper(this RenderTreeBuilder builder, ref int sequence,string name,params string[] addAttr)
        {
            if (addAttr.Length>0 && string.Join("", addAttr).Trim() is string attrs && attrs.Length>0 )
            {
                var seq = sequence;

                builder.AddAttribute(seq++, name, attrs);
                sequence = seq;
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddAttributeHelper(this RenderTreeBuilder builder, ref int sequence, string name, object? value)
        {
            var seq = sequence;
            builder.AddAttribute(seq++, name, value);
            sequence = seq;
        }
        /// <summary>
        /// Specify the content to include RenderFragment.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="contentName"></param>
        /// <param name="action"></param>
        public static void AddContentsHelper(this RenderTreeBuilder builder, ref int sequence,string contentName,Action<RenderTreeBuilder> action)
        {
            var seq = sequence;
            builder.AddAttribute(seq++, contentName,(RenderFragment)(action.Invoke));
            sequence = seq;
        }
        /// <summary>
        /// Specify ‘ChildContent’ containing child elements.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="action"></param>
        public static void AddChildContentsHelper(this RenderTreeBuilder builder, ref int sequence,  Action<RenderTreeBuilder> action)
        {
            var seq = sequence;
            builder.AddAttribute(seq++, ATTR_CHILD_CONTENT, (RenderFragment)(action.Invoke));
            sequence = seq;
        }
        /// <summary>
        /// Specify ‘ChildContent’ containing child elements.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="action"></param>
        public static void AddChildContentsHelper(this RenderTreeBuilder builder, ref int sequence,string contentName, Action<RenderTreeBuilder> action)
        {
            var seq = sequence;
            builder.AddAttribute(seq++, contentName, (RenderFragment)(action.Invoke));
            sequence = seq;
        }
        /// <summary>
        /// add mouse event to element and specify call back function for fire. 
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="events"></param>
        /// <param name="callback"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void AddMouseEvent(this RenderTreeBuilder builder, ref int sequence, MouseEvent events, EventCallback callback)
        {
            var seq = sequence;
            builder.AddAttribute(seq++, events.GetEventName(), callback);
            sequence = seq;
        }
        /// <summary>
        /// add mouse event to element and specify call back function for fire. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="events"></param>
        /// <param name="callback"></param>
        public static void AddMouseEvent<T>(this RenderTreeBuilder builder, ref int sequence, MouseEvent events, EventCallback<T> callback)
        {
            var seq = sequence;
            builder.AddAttribute(seq++, events.GetEventName(), callback);
            sequence = seq;
        }
        /// <summary>
        /// add touch event to element and specify call back function for fire. 
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="events"></param>
        /// <param name="callback"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void AddTouchEvent(this RenderTreeBuilder builder, ref int sequence, TouchEvent events, EventCallback callback)
        {
            var seq = sequence;
            builder.AddAttribute(seq++, events.GetEventName(), callback);
            sequence = seq;
        }
        /// <summary>
        /// add touch event to element and specify call back function for fire. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="events"></param>
        /// <param name="callback"></param>
        public static void AddTouchEvent<T>(this RenderTreeBuilder builder, ref int sequence, TouchEvent events, EventCallback<T> callback)
        {
            var seq = sequence;
            builder.AddAttribute(seq++, events.GetEventName(), callback);
            sequence = seq;
        }
        /// <summary>
        /// add "ValueExpression" attr to input contents.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <param name="expression"></param>
        public static void AddValueExpression<T>(this RenderTreeBuilder builder, ref int sequence,Expression<Func<T>> expression)
        {
            var seq = sequence;
            builder.AddAttribute(seq++, ATTR_VALUE_EXPRESSION, expression);
            sequence = seq;
        }
    }
}
