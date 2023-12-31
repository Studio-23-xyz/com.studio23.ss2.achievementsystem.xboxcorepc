// Copyright (c) Microsoft and Contributors. All rights reserved. Licensed under the University of Illinois/NCSA Open Source License. See LICENSE.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace XGamingRuntime.Interop
{
    [Conditional("DEBUG")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
    internal sealed class NativeTypeNameAttribute : Attribute
    {
        public NativeTypeNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}