#region License
// ISharpCLIParser.cs
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
using SharpCLIParser.Internals;
using SharpCLIParser.Internals.Validators;

namespace SharpCLIParser
{
    /// <summary>
    /// Represents a command line parser which provides methods and properties 
    /// to easily and fluently parse command line arguments. 
    /// </summary>
    public interface ISharpCLIParser : ICommandLineOptionSetupFactory, ICommandLineOptionContainer
	{
		/// <summary>
		/// Setup a new <see cref="ICommandLineOptionSharp{T}"/> using the specified short and long Option name.
		/// </summary>
		/// <param name="shortOption">The short name for the Option. This must not be <c>whitespace</c> or a control character.</param>
		/// <param name="longOption">The long name for the Option. This must not be <c>null</c>, <c>empty</c> or only <c>whitespace</c>.</param>
		/// <returns></returns>
		/// <exception cref="OptionAlreadyExistsException">
		/// A Option with the same <paramref name="shortOption"/> name or <paramref name="longOption"/> name already exists in the <see cref="ISharpCLIParser"/>.
		/// </exception>
		/// <exception cref="InvalidOptionNameException">
		/// Either <paramref name="shortOption"/> or <paramref name="longOption"/> are not valid. <paramref name="shortOption"/> must not be <c>whitespace</c>
		/// or a control character. <paramref name="longOption"/> must not be <c>null</c>, <c>empty</c> or only <c>whitespace</c>.
		/// </exception>
		[Obsolete("Use new overload Setup<T>(char, string) to specify both a short and long option name instead.")]
		ICommandLineOptionSharp<T> Setup<T>(string shortOption, string longOption);
			
	    /// <summary>
	    /// Setup a new command using the specified name.
	    /// </summary>
	    /// <typeparam name="TBuildType">The type of arguments to be built for this command</typeparam>
	    /// <param name="name">The name for the Command. This must be unique, not <c>null</c>, <c>empty</c> or contain only <c>whitespace</c>.</param>
	    /// <returns></returns>
	    /// <exception cref="CommandAlreadyExistsException">
	    /// A Command with the same <paramref name="name"/> name already exists in the <see cref="ISharpCLIParser"/>.
	    /// </exception>
	    ICommandLineCommandFluent<TBuildType> SetupCommand<TBuildType>(string name) where TBuildType : new();

		/// <summary>
		/// Setup the help args.
		/// </summary>
		/// <param name="helpArgs">The help arguments to register.</param>
		IHelpCommandLineOptionSharp SetupHelp(params string[] helpArgs);

		/// <summary>
		/// Parses the specified <see><cref>T:System.String[]</cref></see> using the setup Options.
		/// </summary>
		/// <param name="args">The <see><cref>T:System.String[]</cref></see> to parse.</param>
		/// <returns>An <see cref="ICommandLineParserResult"/> representing the results of the parse operation.</returns>
		ICommandLineParserResult Parse(string[] args);

        /// <summary>
        /// Gets or sets the help option for this parser.
        /// </summary>
        IHelpCommandLineOption HelpOption { get; set; }

		/// <summary>
		/// Gets or sets whether values that differ by case are considered different. 
		/// </summary>
		bool IsCaseSensitive { get; set; }

        /// <summary>
        /// Gets the special characters used by the parser.
        /// </summary>
        SpecialCharacters SpecialCharacters { get; }

        /// <summary>
        /// Configures the <see cref="ISharpCLIParser"/> so that short and long options that differ by case are considered the same.
        /// </summary>
        /// <returns></returns>
	    ISharpCLIParser MakeCaseInsensitive();

        /// <summary>
        /// Configures the <see cref="ISharpCLIParser"/> so that short options are treated the same as long options, thus
        /// unique short option behaviour is ignored.
        /// </summary>
        /// <returns></returns>
	    ISharpCLIParser DisableShortOptions();

        /// <summary>
        /// Configures the <see cref="ISharpCLIParser"/> to use the specified option prefixes instead of the default.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
	    ISharpCLIParser UseOwnOptionPrefix(params string[] prefix);

	    /// <summary>
	    /// Configures the <see cref="ISharpCLIParser"/> to skip the first of the specified arguments.
	    /// This can be useful when Windows inserts the application name in the command line arguments for your application.
	    /// </summary>
	    /// <returns>this <see cref="ISharpCLIParser"/></returns>
	    ISharpCLIParser SkipFirstArg();
    }
}
