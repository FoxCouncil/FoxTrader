using FoxTrader.UI.Control;
using OpenTK.Input;

namespace FoxTrader.UI
{
    /// <summary>Delegate used for all control's event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void GameControlEventHandler(GameControl c_control);

    internal delegate void KeyboardEventHandler(GameControl c_control, KeyboardKeyEventArgs c_args);

    /// <summary>Delegate used for all button's event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void MouseButtonEventHandler(GameControl c_control, MouseButtonEventArgs c_args);

    /// <summary>Delegate used for all button's event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void MouseMoveEventHandler(GameControl c_control, MouseMoveEventArgs c_args);

    /// <summary>Delegate used for all button's event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void MouseWheelEventHandler(GameControl c_control, MouseWheelEventArgs c_args);

    /// <summary>Delegate used for all checkbox's event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void CheckBoxEventHandler(GameControl c_control);

    /// <summary>Delegate used for all collapsible category's event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void CollapsibleCategoryEventHandler(GameControl c_control);

    /// <summary>Delegate used for all collapsible list's event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void CollapsibleListEventHandler(GameControl c_control);

    /// <summary>Delegate used for all color's event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void ColorEventHandler(GameControl c_control);

    /// <summary>Delegate used for all selection based event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void SelectionEventHandler(GameControl c_control);

    /// <summary>Delegate used for all zoom based event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void ZoomEventHandler(GameControl c_control);

    /// <summary>Delegate used for all message box's event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void MessageBoxEventHandler(GameControl c_control);

    /// <summary>Delegate used for all value based event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void ValueEventHandler(GameControl c_control);

    /// <summary>Delegate used for all size based event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void SizeEventHandler(GameControl c_control);

    /// <summary>Delegate used for all scroll based event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void ScrollEventHandler(GameControl c_control);

    /// <summary>Delegate used for all tab based event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void TabEventHandler(GameControl c_control);

    /// <summary>Delegate used for all text based event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void TextEventHandler(GameControl c_control);

    /// <summary>Delegate used for all collapsible node's event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void CollapsibleNodeEventHandler(GameControl c_control);

    /// <summary>Delegate used for all drag based event handlers</summary>
    /// <param name="c_control">Event source</param>
    internal delegate void DragEventHandler(GameControl c_control);
}