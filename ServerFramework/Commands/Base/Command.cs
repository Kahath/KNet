/*
 * This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using ServerFramework.Database.Model.Application.Command;
using ServerFramework.Enums;
using ServerFramework.Network.Session;
using System;

namespace ServerFramework.Commands.Base
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
		private string				_arguments;
		private CommandValidation	_validation;

		#endregion

		#region Properties

		internal string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		internal string Arguments
		{
			get { return _arguments; }
			set { _arguments = value; }
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

		internal CommandLevel CommandLevel
		{
			get { return _commandLevel; }
			set { _commandLevel = value; }
		}

		internal string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		internal string FullName
		{
			get
			{
				string retVal = String.Empty;

				if (BaseCommand != null)
					retVal = String.Format("{0} {1}", BaseCommand.FullName, Name);
				else
					retVal = Name;

				return retVal;
			}
		}

		internal CommandValidation Validation
		{
			get { return _validation; }
			set { _validation = value; }
		}

		internal bool IsValid
		{
			get { return Validation == CommandValidation.Successful; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new object of <see cref="ServerFramework.Constants.Entities.Console.Command"/> type.
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
		public bool Invoke(Client user)
		{
			return Script(user, Arguments.Split(' '));
		}

		#endregion

		#endregion
	}
}
