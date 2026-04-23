// Copyright 2026 Code Philosophy
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
public class PreservedTypes
{
    // Base runtime types
    public object Object;
    public System.ValueType ValueType;
    public string String;
    public System.Enum Enum;
    public System.Array Array;
    public System.Delegate Delegate;
    public System.MulticastDelegate MulticastDelegate;
    //public System.TypedReference TypedReference;
    public System.Type SystemType;
    public System.Type RuntimeType;
    public object Nullable;
    public System.Collections.ICollection ICollection;
    public System.Collections.IEnumerable IEnumerable;
    public System.Collections.IList IList;
    public System.Collections.IEnumerator IEnumerator;
    public System.Collections.Generic.IList<object> IListGeneric;
    public System.Collections.Generic.ICollection<object> ICollectionGeneric;
    public System.Collections.Generic.IEnumerable<object> IEnumerableGeneric;
    public System.Collections.Generic.IReadOnlyList<object> IReadOnlyListGeneric;
    public System.Collections.Generic.IReadOnlyCollection<object> IReadOnlyCollectionGeneric;
    public System.Collections.Generic.IEnumerator<object> IEnumeratorGeneric;

    // Exceptions
    public System.Exception Exception;
    public System.ArithmeticException ArithmeticException;
    public System.DivideByZeroException DivisionByZeroException;
    public System.OverflowException OverflowException;
    public System.StackOverflowException StackOverflowException;
    public System.ArgumentException ArgumentException;
    public System.ArgumentNullException ArgumentNullException;
    public System.ArgumentOutOfRangeException ArgumentOutOfRangeException;
    public System.TypeLoadException TypeLoadException;
    public System.IndexOutOfRangeException IndexOutOfRangeException;
    public System.InvalidCastException InvalidCastException;
    public System.MissingFieldException MissingFieldException;
    public System.MissingMethodException MissingMethodException;
    public System.NullReferenceException NullReferenceException;
    public System.ArrayTypeMismatchException ArrayTypeMismatchException;
    public System.OutOfMemoryException OutOfMemoryException;
    public System.BadImageFormatException BadImageFormatException;
    public System.EntryPointNotFoundException EntryPointNotFoundException;
    public System.MissingMemberException MissingMemberException;
    public System.NotSupportedException NotSupportedException;
    public System.NotImplementedException NotImplementedException;
    public System.TypeInitializationException TypeInitializationException;
    public System.Reflection.TargetException TargetException;
    public System.Reflection.TargetInvocationException TargetInvocationException;
    public System.Reflection.TargetParameterCountException TargetParameterCountException;

    // Attributes / custom attributes
    public System.Attribute Attribute;
    public System.Reflection.CustomAttributeData CustomAttributeData;
    public System.Reflection.CustomAttributeTypedArgument CustomAttributeTypedArgument;
    public System.Reflection.CustomAttributeNamedArgument CustomAttributeNamedArgument;
    //public System.Runtime.CompilerServices.IntrinsicAttribute Intrinsic;

    // Reflection runtime types
    public System.Reflection.Assembly ReflectionAssembly;
    public System.Reflection.Module ReflectionModule;
    public System.Reflection.FieldInfo ReflectionField;
    public System.Reflection.MethodInfo ReflectionMethod;
    public System.Reflection.ConstructorInfo ReflectionConstructor;
    public System.Reflection.PropertyInfo ReflectionProperty;
    public System.Reflection.EventInfo ReflectionEvent;
    public System.Reflection.ParameterInfo ReflectionParameter;
    public System.Reflection.MemberInfo ReflectionMemberInfo;
    public System.Reflection.MethodBody ReflectionMethodBody;
    public System.Reflection.ExceptionHandlingClause ReflectionExceptionHandlingClause;
    public System.Reflection.LocalVariableInfo ReflectionLocalVariableInfo;

    // App/thread/context
    public System.AppDomain AppDomain;
    public System.AppDomainSetup AppDomainSetup;
    public object AppContext;
    public System.Threading.Thread Thread;
    public object InternalThread;

    // Misc attributes/types
    public System.Runtime.InteropServices.MarshalAsAttribute MarshalAs;
    public System.Runtime.CompilerServices.IsByRefLikeAttribute ByRefLike;

    // Globalization/internal framework types (some are internal, keep object refs)
    public object CultureData;
    public System.Globalization.CultureInfo CultureInfo;
    public System.Globalization.DateTimeFormatInfo DateTimeFormatInfo;
    public System.Globalization.NumberFormatInfo NumberFormatInfo;
    public System.Globalization.RegionInfo RegionInfo;
    public object CalendarData;

    // Diagnostics
    public System.Diagnostics.StackFrame StackFrame;
    public System.Diagnostics.StackTrace StackTrace;
}
