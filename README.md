## MARC

An enhanced version of the .NET parser for MARC records (ISO 2709), ported to C# by Matt Schraeder and originally developed in PHP by Dan Scott.
Refactored by Daniel Correia. Also added support for UTF-8.

## Original Project README

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
 
2012-06-13 Changes:
Added FileMARCReader class for handling large MARC21 files without loading the entire file into memory. Special thanks to Stas Paladiy for reporting this issue and helping resolve it.
Updated Copyright information for the year 2012!

2011-09-12.2 Changes:
Apparently not even Library of Congress MARCXML records follow the XSD specification.  I've removed the requirement that records validate before being added.
No longer ordering the tags when importing a MARCXML record.

2011-09-12.1 Changes:

CSharp_MARC can now read MARCXML files.  The FileMARCXML class can accept XML strings or native .NET XDocument objects and convert them to Record objects.
The FileMARC(string source) constructor and ImportMARC function were duplicating code from the FileMARC.Add(string source) function. The coding horror is me :negative:

2011-06-30 Change:

Added Clone() to Fields, Subfields, and Records. This is a DEEP clone and all members should be properly cloned as new instances.

2011-04-26 Change:

Changed how validating a tag works when decoding due to "not thinking syndrome"

2011-03-04 Changes:

Updated copyright information
Added x86 Platform for easier testing. Apparently it didn't stick when I added it earlier.
Testing suite to test the class structure and included methods. This will get more advanced as I port specific tests from File_MARC.
Added a bit of extra error checking to a Field's tag so that it's not possible to assign an invalid tag.
If the FileMARC parser comes across an invalid tag, it forces the tag to "ZZZ" and should no longer throw an exception. 
Made Field.IsEmpty() abstract. It's not possible to assign a Field with an empty string tag. Doing so should cause an exception. If this is in fact the case, then IsEmpty on Field should never return true. Because of this IsEmpty is now an override on both DataField and ControlField. Field.IsEmpty now returns the result of it's inherited version.
ToString functions for ControlField and DataField changed to overrides. Field.ToString seemed odd to only output the tag and not the inner data if available.
Fixed bug in ControlField.IsEmpty() returning opposite of expected results. Amazing how much unit testing helps!
DataField.FormatField() with no exclude codes as parameters actually works now.
Added a bit of extra error checking and cleanup to the indicators in the decode function.
Indicators are now closer to the MARC21 standard, allowing both numbers and letters. Uppercase letters are forced to lowercase. # is no longer a valid character, as it is supposed to indicate an ASCII SPACE. Invalid characters are automatically changed to ASCII SPACE and warnings are added to indicate this
Moved warnings into the Record object rather than FileMARC. This makes it easier to track which warnings were for which Record. This change may be API breaking and I apologize for that.
Fixed a bug where it is possible to make an invalid record by setting the Leader to a string longer than 24 characters. Now it will still allow you to set a long Leader but will only output the first 24 characters.
Fixed a bug where if the Leader was less than 24 characters it would make an invalid record.
You can now make tags that are not exactly 3 characters. It will automatically pad the leading 0s.
Field.ToString() is now an abstract override rather than an abstract new. No idea what I was thinking with it as new.
Record.ToString() doesn't have to check what type it is, thanks to override!
Totally had the IEnumerable designed wrong. Reset should reset to -1, as MoveNext will take you to the first record.  After a reset it was ignoring the first record. My bad!

2011-02-16 Changes:

Added personal email to copyright notices.
Updated copyright years
Added an overloaded GetSubfields() so you no longer have to pass in null to get all subfields.
Changed Warnings from protected to public. No idea what I was thinking there.
Tons more error checking in the FileMARC decode function due to coming across an egregiously bad record.
Now with fancy new Demo application! Hopefully it has enough example code to get people started better.
Updated solution files to VS2010
Probably some other stuff.
