# WebView2MoveFocusCrash

Example repository to reproduct crash in Webview2 internals when focus is moved.

## Steps

1) Build the application and then start `WebViewExample`
2) In the first tab where wikipedia is shown, press CTRL+F to show the search dialog.
3) Now click the close button of _that_ tab to close that tab, this will close tab and activate the next tab.
4) Exception will be thrown:

```
System.InvalidOperationException: CoreWebView2 members cannot be accessed after the WebView2 control is disposed.
 ---> System.Runtime.InteropServices.COMException (0x8007139F): The group or resource is not in the correct state to perform the requested operation. (0x8007139F)
   at Microsoft.Web.WebView2.Core.Raw.ICoreWebView2Controller.MoveFocus(COREWEBVIEW2_MOVE_FOCUS_REASON reason)
   at Microsoft.Web.WebView2.Core.CoreWebView2Controller.MoveFocus(CoreWebView2MoveFocusReason reason)
   --- End of inner exception stack trace ---
   at Microsoft.Web.WebView2.Core.CoreWebView2Controller.MoveFocus(CoreWebView2MoveFocusReason reason)
   at Microsoft.Web.WebView2.Wpf.WebView2.WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, Boolean& handled)
   at System.Windows.Interop.HwndHost.SubclassWndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, Boolean& handled)
   at MS.Win32.HwndSubclass.DispatcherCallbackOperation(Object o)
   at System.Windows.Threading.ExceptionWrapper.InternalRealCall(Delegate callback, Object args, Int32 numArgs)
   at System.Windows.Threading.ExceptionWrapper.TryCatchWhen(Object source, Delegate callback, Object args, Int32 numArgs, Delegate catchHandler)
   at System.Windows.Threading.Dispatcher.LegacyInvokeImpl(DispatcherPriority priority, TimeSpan timeout, Delegate method, Object args, Int32 numArgs)
   at MS.Win32.HwndSubclass.SubclassWndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam)
```
and
```
System.Runtime.InteropServices.COMException (0x8007139F): The group or resource is not in the correct state to perform the requested operation. (0x8007139F)
   at Microsoft.Web.WebView2.Core.Raw.ICoreWebView2Controller.MoveFocus(COREWEBVIEW2_MOVE_FOCUS_REASON reason)
   at Microsoft.Web.WebView2.Core.CoreWebView2Controller.MoveFocus(CoreWebView2MoveFocusReason reason)
```

## System
1) Windows 10
2) Microsoft Edge: 105.0.1343.33
3) WebView2: 1.0.1340-prerelease
