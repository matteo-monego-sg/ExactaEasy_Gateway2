using SPAMI.Util.Logger;
using System;
using System.Windows.Forms;

namespace ExactaEasyEng.Utilities
{
    /// <summary>
    /// Collection of functions to safely invoke UI functions by checking the availability of the controls.
    /// </summary>
    public static class UIInvoker
    {
        /// <summary>
        /// Checks if the control parameter is a valid reference and if it is not disposed.
        /// </summary>
        public static bool IsControlUiReady(Control ctrl)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Warning, "UIInvoker.IsControlUiReady", $"Trying to access a null-reference to a control object.");
                return false;
            }

            if (ctrl.IsDisposed)
            {
                Log.Line(LogLevels.Warning, "UIInvoker.IsControlUiReady", $"Trying to access a disposed control object.");
                return false;
            }
            return true;
        }

        #region ACTIONS
        /// <summary>
        /// 0 params.
        /// </summary>
        public static void Invoke(Control ctrl, Action action)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return;
            }

            if (action is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Action parameter reference [{nameof(action)}] is null.");
                return;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    ctrl.Invoke(action);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return;
                    }
                    action();
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
            }
        }
        /// <summary>
        /// 1 params.
        /// </summary>
        public static void Invoke<P1>(Control ctrl, Action<P1> action, P1 p1)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return;
            }

            if (action is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Action parameter reference [{nameof(action)}] is null.");
                return;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    ctrl.Invoke(action, p1);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return;
                    }
                    action(p1);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
            }
        }
        /// <summary>
        /// 2 params.
        /// </summary>
        public static void Invoke<P1, P2>(Control ctrl, Action<P1, P2> action, P1 p1, P2 p2)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return;
            }

            if (action is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Action parameter reference [{nameof(action)}] is null.");
                return;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    ctrl.Invoke(action, p1, p2);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return;
                    }
                    action(p1, p2);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
            }
        }
        /// <summary>
        /// 3 params.
        /// </summary>
        public static void Invoke<P1, P2, P3>(Control ctrl, Action<P1, P2, P3> action, P1 p1, P2 p2, P3 p3)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return;
            }

            if (action is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Action parameter reference [{nameof(action)}] is null.");
                return;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    ctrl.Invoke(action, p1, p2, p3);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return;
                    }
                    action(p1, p2, p3);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
            }
        }
        /// <summary>
        /// 4 params.
        /// </summary>
        public static void Invoke<P1, P2, P3, P4>(Control ctrl, Action<P1, P2, P3, P4> action, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return;
            }

            if (action is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Action parameter reference [{nameof(action)}] is null.");
                return;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    ctrl.Invoke(action, p1, p2, p3, p4);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return;
                    }
                    action(p1, p2, p3, p4);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
            }
        }
        /// <summary>
        /// 5 params.
        /// </summary>
        public static void Invoke<P1, P2, P3, P4, P5>(Control ctrl, Action<P1, P2, P3, P4, P5> action, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return;
            }

            if (action is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Action parameter reference [{nameof(action)}] is null.");
                return;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    ctrl.Invoke(action, p1, p2, p3, p4, p5);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return;
                    }
                    action(p1, p2, p3, p4, p5);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
            }
        }
        /// <summary>
        /// 6 params.
        /// </summary>
        public static void Invoke<P1, P2, P3, P4, P5, P6>(Control ctrl, Action<P1, P2, P3, P4, P5, P6> action, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return;
            }

            if (action is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Action parameter reference [{nameof(action)}] is null.");
                return;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    ctrl.Invoke(action, p1, p2, p3, p4, p5, p6);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return;
                    }
                    action(p1, p2, p3, p4, p5, p6);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
            }
        }
        #endregion

        #region FUNCS
        /// <summary>
        /// 0 params.
        /// </summary>
        public static T Invoke<T>(Control ctrl, Func<T> func)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return default;
            }

            if (func is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Function parameter reference [{nameof(func)}] is null.");
                return default;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    return (T)ctrl.Invoke(func);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return default;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return default;
                    }
                    return func();
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
                return default;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
                return default;
            }
        }
        /// <summary>
        /// 1 params.
        /// </summary>
        public static T Invoke<P1, T>(Control ctrl, Func<P1, T> func, P1 p1)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return default;
            }

            if (func is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Function parameter reference [{nameof(func)}] is null.");
                return default;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    return (T)ctrl.Invoke(func, p1);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return default;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return default;
                    }
                    return func(p1);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
                return default;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
                return default;
            }
        }
        /// <summary>
        /// 2 params.
        /// </summary>
        public static T Invoke<P1, P2, T>(Control ctrl, Func<P1, P2, T> func, P1 p1, P2 p2)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return default;
            }

            if (func is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Function parameter reference [{nameof(func)}] is null.");
                return default;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    return (T)ctrl.Invoke(func, p1, p2);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return default;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return default;
                    }
                    return func(p1, p2);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
                return default;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
                return default;
            }
        }
        /// <summary>
        /// 3 params.
        /// </summary>
        public static T Invoke<P1, P2, P3, T>(Control ctrl, Func<P1, P2, P3, T> func, P1 p1, P2 p2, P3 p3)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return default;
            }

            if (func is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Function parameter reference [{nameof(func)}] is null.");
                return default;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    return (T)ctrl.Invoke(func, p1, p2, p3);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return default;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return default;
                    }
                    return func(p1, p2, p3);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
                return default;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
                return default;
            }
        }
        /// <summary>
        /// 4 params.
        /// </summary>
        public static T Invoke<P1, P2, P3, P4, T>(Control ctrl, Func<P1, P2, P3, P4, T> func, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return default;
            }

            if (func is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Function parameter reference [{nameof(func)}] is null.");
                return default;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    return (T)ctrl.Invoke(func, p1, p2, p3, p4);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return default;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return default;
                    }
                    return func(p1, p2, p3, p4);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
                return default;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
                return default;
            }
        }
        /// <summary>
        /// 5 params.
        /// </summary>
        public static T Invoke<P1, P2, P3, P4, P5, T>(Control ctrl, Func<P1, P2, P3, P4, P5, T> func, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return default;
            }

            if (func is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Function parameter reference [{nameof(func)}] is null.");
                return default;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    return (T)ctrl.Invoke(func, p1, p2, p3, p4, p5);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return default;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return default;
                    }
                    return func(p1, p2, p3, p4, p5);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
                return default;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
                return default;
            }
        }
        /// <summary>
        /// 6 params.
        /// </summary>
        public static T Invoke<P1, P2, P3, P4, P5, P6, T>(Control ctrl, Func<P1, P2, P3, P4, P5, P6, T> func, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
        {
            if (ctrl is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference [{nameof(ctrl)}] is null.");
                return default;
            }

            if (func is null)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Function parameter reference [{nameof(func)}] is null.");
                return default;
            }

            try
            {
                if (ctrl.InvokeRequired)
                {
                    return (T)ctrl.Invoke(func, p1, p2, p3, p4, p5, p6);
                }
                else
                {
                    if (ctrl.IsDisposed)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter reference has been disposed.");
                        return default;
                    }

                    if (!ctrl.IsHandleCreated)
                    {
                        Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Control parameter handle was not created.");
                        return default;
                    }
                    return func(p1, p2, p3, p4, p5, p6);
                }
            }
            catch (ObjectDisposedException odex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"ObjectDisposedException raised: {odex}");
                return default;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "UIInvoker.Invoke", $"Exception raised: {ex}");
                return default;
            }
        }
        #endregion
    }
}
