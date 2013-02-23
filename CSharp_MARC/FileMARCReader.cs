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
 * 
 */

using System;
using System.Text;
using System.Collections;
using System.IO;

namespace MARC
{
	/// <summary>
	/// This is a wrapper for FileMARC that allows for reading large files without loading the entire file into memory.
	/// </summary>
	public class FileMARCReader : IEnumerable, IDisposable
	{
		private readonly FileStream _reader;

		public FileMARCReader(string filename)
		{
			_reader = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		public IEnumerator GetEnumerator()
		{
			int bufferSize = 10 * 1024 * 1024;
			byte[] buffer = new byte[bufferSize + 1];

			while (_reader.Position < _reader.Length)
			{
				int delPosition, realReadSize;
				
                do
				{
					realReadSize = _reader.Read(buffer, 0, bufferSize);

					delPosition = Array.LastIndexOf(buffer, (byte)FileMARC.END_OF_RECORD, realReadSize-1) + 1;

					if (delPosition == 0 && realReadSize == bufferSize)
					{
						bufferSize *= 2;
						buffer = new byte[bufferSize + 1];
					}
				} while (delPosition == 0 && realReadSize == bufferSize);

				_reader.Position = _reader.Position - (realReadSize - delPosition);

				FileMARC marc = new FileMARC(Encoding.Default.GetString(buffer, 0, delPosition));
				foreach (Record marcRecord in marc)
				{
					yield return marcRecord;
				}
			}
		}

		public void Dispose()
		{
			_reader.Dispose();
		}
	}
}
