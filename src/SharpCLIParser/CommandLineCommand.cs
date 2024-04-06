using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using SharpCLIParser.Internals;
using SharpCLIParser.Internals.Validators;

namespace SharpCLIParser
{
    class CommandLineCommand<TBuildType> : ICommandLineOptionSetupFactory, ICommandLineCommandT<TBuildType>, ICommandLineCommandFluent<TBuildType> where TBuildType : new()
    {
        List<ICommandLineOption> _options;
        ICommandLineOptionFactory _optionFactory;
        ICommandLineOptionValidator _optionValidator;

        public CommandLineCommand(ISharpCLIParser parser)
        {
            Parser = parser;
            Object = new TBuildType();
        }

        /// <summary>
        /// Gets the <see cref="ISharpCLIParser"/>.
        /// </summary>
        public ISharpCLIParser Parser { get; set; }

        /// <summary>
        /// Gets the constructed object.
        /// </summary>
        public TBuildType Object { get; private set; }

        /// <summary>
        /// The callback to execute with the results of this command if used.
        /// </summary>
        public Action<TBuildType> SuccessCallback { get; set; }

        /// <summary>
        /// Gets or sets the command name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the list of Options setup for this command.
        /// </summary>
        public IEnumerable<ICommandLineOption> Options
        {
            get { return _options ?? (_options = new List<ICommandLineOption>()); }
        }

        /// <summary>
        /// Gets whether the command has a callback
        /// </summary>
        public bool HasSuccessCallback
        {
            get { return SuccessCallback != null; }
        }

        /// <summary>
        /// Executes the callback
        /// </summary>
        public void ExecuteOnSuccess()
        {
            if (HasSuccessCallback)
                SuccessCallback(Object);
        }

        /// <summary>
        /// Gets or sets the <see cref="ICommandLineOptionValidator"/> used to validate each setup Option.
        /// </summary>
        public ICommandLineOptionValidator OptionValidator
        {
            get { return _optionValidator ?? (_optionValidator = new CommandLineOptionValidator(this, Parser.SpecialCharacters)); }
            set { _optionValidator = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="ICommandLineOptionFactory"/> to use for creating <see cref="ICommandLineOptionSharp{T}"/>.
        /// </summary>
        /// <remarks>If this property is set to <c>null</c> then the default <see cref="OptionFactory"/> is returned.</remarks>
        public ICommandLineOptionFactory OptionFactory
        {
            get { return _optionFactory ?? (_optionFactory = new CommandLineOptionFactory()); }
            set { _optionFactory = value; }
        }

        public ICommandLineCommandFluent<TBuildType> OnSuccess(Action<TBuildType> callback)
        {
            SuccessCallback = callback;
            return this;
        }

        public ICommandLineOptionBuilderSharp<TProperty> Setup<TProperty>(Expression<Func<TBuildType, TProperty>> propertyPicker)
        {
            return new CommandLineOptionBuilderSharp<TBuildType, TProperty>(this, Object, propertyPicker, this);
        }

        public ICommandLineOptionSharp<T> Setup<T>(char shortOption, string longOption)
        {
            return SetupInternal<T>(shortOption.ToString(CultureInfo.InvariantCulture), longOption);
        }

        public ICommandLineOptionSharp<T> Setup<T>(char shortOption)
        {
            return SetupInternal<T>(shortOption.ToString(CultureInfo.InvariantCulture), null);
        }

        public ICommandLineOptionSharp<T> Setup<T>(string longOption)
        {
            return SetupInternal<T>(null, longOption);
        }

        private ICommandLineOptionSharp<T> SetupInternal<T>(string shortOption, string longOption)
        {
            var argOption = this.OptionFactory.CreateOption<T>(shortOption, longOption);

            if (argOption == null)
                throw new InvalidOperationException("OptionFactory is producing unexpected results.");

            OptionValidator.Validate(argOption, Parser.IsCaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            _options.Add(argOption);

            return argOption;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TBuildType"></typeparam>
    public interface ICommandLineCommandT<TBuildType> : ICommandLineCommand, ICommandLineOptionContainer
    {
        /// <summary>
        /// Gets the constructed object.
        /// </summary>
        TBuildType Object { get; }

        /// <summary>
        /// The callback to execute with the results of this command if used.
        /// </summary>
        Action<TBuildType> SuccessCallback { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ICommandLineCommand
    {
        /// <summary>
        /// Gets the <see cref="ISharpCLIParser"/>.
        /// </summary>
        ISharpCLIParser Parser { get; set; }

        /// <summary>
        /// Gets or sets the command name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the list of Options setup for this command.
        /// </summary>
        IEnumerable<ICommandLineOption> Options { get; }

        /// <summary>
        /// Gets whether the command has a callback
        /// </summary>
        bool HasSuccessCallback { get; }

        /// <summary>
        /// Executes the callback
        /// </summary>
        void ExecuteOnSuccess();
    }
}