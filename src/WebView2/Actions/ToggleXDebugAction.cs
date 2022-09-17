#nullable enable
using System;
using System.Collections.Generic;

namespace WebView2.Actions;

public delegate void ToggleXDebugAction(List<String>? domains, bool isEnabled);