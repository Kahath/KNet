/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Database.Model.KNet.Command;
using KNetFramework.Enums;
using KNetFramework.Network.Session;
using System;

namespace KNetFramework.Commands.Base
{
	public sealed class Command
	{
		#region Fields

		private string				_name;
		private CommandLevel		_commandLevel;
		private Command				_baseCommand;
		private CommandModel		_model;
		private Command[]			_subCommands;
		private CommandHandler		_script;
		private string				_description;
		private CommandValidation	_validation;

		#endregion

		#region Properties

		public string Description
		{
			get { return _description; }
			internal set { _description = value; }
		}

		internal CommandHandler Script
		{
			get { return _script; }
			set { _script = value; }
		}

		internal Command BaseCommand
		{
			get { return _baseCommand; }
			set { _baseCommand = value; }
		}

		internal CommandModel Model
		{
			get { return _model; }
			set { _model = value; }
		}

		internal Command[] SubCommands
		{
			get { return _subCommands; }
			set { _subCommands = value; }
		}

		public CommandLevel CommandLevel
		{
			get { return _commandLevel; }
			internal set { _commandLevel = value; }
		}

		public string Name
		{
			get { return _name; }
			internal set { _name = value; }
		}

		public string FullName
		{
			get
			{
				string retVal = String.Empty;

				retVal = BaseCommand != null ? $"{BaseCommand.FullName} {Name}" : Name;

				return retVal;
			}
		}

		internal CommandValidation Validation
		{
			get { return _validation; }
			set { _validation = value; }
		}

		public bool IsValid
		{
			get { return Validation == CommandValidation.Successful; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new object of <see cref="Command"/> type.
		/// </summary>
		/// <param name="name">Command name.</param>
		/// <param name="commandLevel">Command level.</param>
		/// <param name="subCommands">Command sub commands.</param>
		/// <param name="script">Command script.</param>
		/// <param name="description">Command description.</param>
		public Command(string name, CommandLevel commandLevel
			, Command[] subCommands, CommandHandler script, string description)
		{
			Name = name;
			CommandLevel = commandLevel;
			SubCommands = subCommands;
			Script = script;
			Description = description;
		}

		#endregion

		#region Methods

		#region Invoke

		/// <summary>
		/// Executes command script.
		/// </summary>
		/// <param name="user">Client who executes script.</param>
		/// <param name="args">Command arguments.</param>
		/// <returns></returns>
		public bool Invoke(Client user, params string[] parameters)
		{
			return Script(user, parameters);
		}

		#endregion

		#endregion
	}
}
