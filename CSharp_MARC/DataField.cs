/**
 * Parser for MARC records
 *
 * This project is based on the File_MARC package
 * (http://pear.php.net/package/File_MARC) by Dan Scott , which was based on PHP
 * MARC package, originally called "php-marc", that is part of the Emilda
 * Project (http://www.emilda.org). Both projects were released under the LGPL
 * which allowed me to port the project to C# for use with the .NET Framework.
 *
 *  This program is free software: you can redistribute it and/or modify
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
 *
 * @author    Matt Schraeder <mschraeder@csharpmarc.net> <mschraeder@btsb.com>
 * @copyright 2009-2012 Matt Schraeder and Bound to Stay Bound Books <http://www.btsb.com>
 * @license   http://www.gnu.org/copyleft/lesser.html  LGPL License 3
 */

using System;
using System.Collections.Generic;

namespace MARC
{
    /// <summary>
    /// The MARC DataField class represents a single field in a MARC record.
    /// A MARC data field consists of a tag name, two indicators which may be
    /// null, and zero or more subfields represented by MARC Subfield objects.
    /// Subfields are held within a List structure.
    /// </summary>
    public class DataField : Field
    {
        //Private member variables and properties
        #region Private member variables and properties

        private char _ind1;
        private char _ind2;

        /// <summary>
        /// Gets or sets the first indicator.
        /// </summary>
        /// <value>The first indicator.</value>
        public char Indicator1
        {
            get { return _ind1; }
            set
            {
                if (ValidateIndicator(value))
                    _ind1 = value;
                else
                    throw new ArgumentException("Invalid indicator.");
            }
        }

        /// <summary>
        /// Gets or sets the second indicator.
        /// </summary>
        /// <value>The second indicator.</value>
        public char Indicator2
        {
            get { return _ind2; }
            set
            {
                if (ValidateIndicator(value))
                    _ind2 = value;
                else
                    throw new ArgumentException("Invalid indicator.");
            }
        }

        /// <summary>
        /// Gets or sets the subfields.
        /// </summary>
        /// <value>The subfields.</value>
        public List<Subfield> Subfields { get; set; }

        /// <summary>
        /// Gets the first <see cref="MARC.Subfield"/> with the specified code.
        /// </summary>
        /// <value>The first matching subfield or null if not found</value>
        public Subfield this[char code]
        {
            get
            {
                Subfield foundSubfield = null;

                foreach (Subfield subfield in Subfields)
                {
                    if (subfield.Code == code)
                    {
                        foundSubfield = subfield;
                        break;
                    }
                }

                return foundSubfield;
            }
        }

        #endregion

        //Constructors
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataField"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="subfields">The subfields.</param>
        /// <param name="ind1">The first indicator.</param>
        /// <param name="ind2">The second indicator.</param>
        public DataField(string tag, List<Subfield> subfields, char ind1, char ind2) : base(tag)
        {
            Subfields = subfields;

            if (ValidateIndicator(ind1))
                _ind1 = ind1;
            else
                throw new ArgumentException("Invalid indicator.");

            if (ValidateIndicator(ind2))
                _ind2 = ind2;
            else
                throw new ArgumentException("Invalid indicator.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataField"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="subfields">The subfields.</param>
        /// <param name="ind1">The first indicator.</param>
        public DataField(string tag, List<Subfield> subfields, char ind1) : this(tag, subfields, ind1, ' ') { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataField"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="subfields">The subfields.</param>
        public DataField(string tag, List<Subfield> subfields) : this(tag, subfields, ' ') { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataField"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public DataField(string tag) : this(tag, new List<Subfield>()) { }

        #endregion

        /// <summary>
        /// Returns a List of subfield objects that match a requested code,
        /// or a cloned List that contains all the subfield objects if the
        /// requested code is null.
        /// </summary>
        /// <param name="code">The code, or null.</param>
        /// <returns></returns>
        public List<Subfield> GetSubfields(char? code)
        {
            List<Subfield> results = new List<Subfield>();

            foreach (Subfield subfield in Subfields)
            {
                if (code == null || subfield.Code == code)
                    results.Add(subfield);
            }

            return results;
        }

		/// <summary>
		/// Returns a List of all subfield objects within the field.
		/// </summary>
		/// <returns></returns>
		public List<Subfield> GetSubfields()
		{
			return GetSubfields(null);
		}

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsEmpty()
        {
            return Subfields == null || Subfields.Count == 0;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            string lines = string.Empty;
            string prefix = Tag.PadLeft(3) + " " + _ind1 + _ind2 + " ";

            foreach (Subfield subfield in Subfields)
            {
                if (lines != string.Empty)
                    lines += Environment.NewLine + "       ";
                else
                    lines += prefix;

                lines += subfield.ToString();
            }

            return lines;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>
        /// in raw USMARC format.
        /// </summary>
        /// <returns></returns>
        public override string ToRaw()
        {
            string raw = string.Empty;

            foreach (Subfield subfield in Subfields)
            {
                if (!subfield.IsEmpty())
                    raw += subfield.ToRaw();
            }

            return string.Empty + _ind1 + _ind2 + raw + FileMARC.END_OF_FIELD;
        }

        /// <summary>
        /// Print a MARC Field object without tags, indicators, etc
        /// </summary>
        /// <returns></returns>
        public string FormatField(char[] excludeCodes)
        {
            string result = string.Empty;

            foreach (Subfield subfield in Subfields)
            {
                if (Tag.Substring(0, 1) == "6" && (subfield.Code == 'v' || subfield.Code == 'x' || subfield.Code == 'y' || subfield.Code == 'v' || subfield.Code == 'z'))
                    result += " -- " + subfield.Data;
                else
                {
                    bool exclude = false;

                    foreach (char code in excludeCodes)
                    {
                        if (subfield.Code == code)
                        {
                            exclude = true;
                            break;
                        }
                    }

                    if (!exclude)
                        result += " " + subfield.Data;
                }
            }

            return result.Trim();
        }

        public override string FormatField()
        {
			char[] excludeCodes = {};
            return FormatField(excludeCodes);
        }

        /// <summary>
        /// Validates the indicator.
        /// </summary>
        /// <param name="ind">The indicator</param>
        /// <returns>
        ///     <c>true</c> if the indicator is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool ValidateIndicator(char ind)
        {
            if (ind == ' ' || (ind >= '0' && ind <= '9') || (ind >= 'a' && ind <= 'z'))
            {
                return true;
            }

            return false;
        }

		/// <summary>
		/// Makes a deep clone of this instance.
		/// </summary>
		/// <returns></returns>
		public override Field Clone()
		{
			Field clone = new DataField(Tag, new List<Subfield>(), _ind1, _ind2);
			foreach (Subfield needsCloned in Subfields)
				((DataField)clone).Subfields.Add(needsCloned.Clone());
			return clone;
		}
    }
}
