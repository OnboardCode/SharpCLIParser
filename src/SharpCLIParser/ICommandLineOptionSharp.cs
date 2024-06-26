﻿#region License
// ICommandLineOptionSharp.cs
// Copyright (c) 2024, Weslley Luiz
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provide
// d that the following conditions are met:
// 
// Redistributions of source code must retain the above copyright notice, this list of conditions and the
// following disclaimer.
// 
// Redistributions in binary form must reproduce the above copyright notice, this list of conditions and
// the following disclaimer in the documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
// PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Collections.Generic;
using SharpCLIParser.Exceptions;

namespace SharpCLIParser
{
    /// <summary>
    /// Provides the fluent interface for a <see cref="ICommandLineOptionSharp{T}"/> object.
    /// </summary>
    public interface ICommandLineOptionSharp<T>
	{
		/// <summary>
		/// Adds the specified description to the <see cref="ICommandLineOptionSharp{T}"/>.
		/// </summary>
		/// <param name="description">The <see cref="System.String"/> representing the description to use. This should be localised text.</param>
		/// <returns>A <see cref="ICommandLineOptionSharp{T}"/>.</returns>
		ICommandLineOptionSharp<T> WithDescription(string description);

		/// <summary>
		/// Declares that this <see cref="ICommandLineOptionSharp{T}"/> is required.
		/// </summary>
		/// <returns>A <see cref="ICommandLineOptionSharp{T}"/>.</returns>
		ICommandLineOptionSharp<T> Required();

		/// <summary>
		/// Specifies the method to invoke when the <see cref="ICommandLineOptionSharp{T}"/>. 
		/// is parsed. If a callback is not required either do not call it, or specify <c>null</c>.
		/// Do no use this if you are using the Generic Fluent Command Line Parser.
		/// </summary>
		/// <param name="callback">The return callback to execute with the parsed value of the Option.</param>
		/// <returns>A <see cref="ICommandLineOptionSharp{T}"/>.</returns>
		ICommandLineOptionSharp<T> Callback(Action<T> callback);

        /// <summary>
        /// Set default option and specifies the default value to use if no value is found whilst parsing this <see cref="ICommandLineOptionSharp{T}"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        /// <returns>A <see cref="ICommandLineOptionSharp{T}"/>.</returns>
        ICommandLineOptionSharp<T> SetDefault(T value);

		/// <summary>
		/// Specified the method to invoke with any addition arguments parsed with the Option.
		/// If additional arguments are not required either do not call it, or specify <c>null</c>.
		/// </summary>
		/// <param name="callback">The return callback to execute with the parsed addition arguments found for this Option.</param>
		/// <returns>A <see cref="ICommandLineOptionSharp{T}"/>.</returns>
		ICommandLineOptionSharp<T> CaptureAdditionalArguments(Action<IEnumerable<string>> callback);

        /// <summary>
        /// Specifies a command to attached the option too.
        /// </summary>
        /// <param name="command">The command to attach the option too. This must not be <c>null</c> and already be setup with the parser.</param>
        /// <returns>A <see cref="ICommandLineOptionSharp{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> is <c>null</c>.</exception>
        /// <exception cref="CommandNotFoundException">Thrown if the specified <paramref name="command"/> does not exist in the parser.</exception>
        ICommandLineOptionSharp<T> AssignToCommand(ICommandLineCommand command);

        /// <summary>
        /// If values are found before an option then treat this option as the default and apply those values against it!
        /// </summary>
        /// <returns>A <see cref="ICommandLineOptionSharp{T}"/>.</returns>
	    ICommandLineOptionSharp<T> UseForOrphanArguments();
	}
}
