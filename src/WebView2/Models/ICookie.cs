using System;
using System.Collections.Generic;

namespace WebView2.Models;

public interface ICookie
{
    string Name { get; }

    string Value { get; }

    List<String> Domains { get; }

    string Path { get; }

    int Expires { get; }

    int Size { get; }

    bool HttpOnly { get; }

    bool Secure { get; set; }
}