/**
 * Determines if the input string has a rangy selection inside it. This is used
 * as a quick filter or to determine if extra processing is required.
 *
 * @param input Input HTML which may contain a rangy selection.
 * @returns true if there is a selection, otherwise false.
 */
function containsRangySelection(input) {
	var found = input.indexOf('<span id="selectionBoundary_') >= 0;
	return found;
}

/**
 * Returns a list of rangy selections and their start position in terms of
 * visible characters (excluding tags).
 *
 * @param {String} input Input HTML which contains rangy selections.
 * @return An array of indexes and the contents of the tag.
 */

function getRangySelectionsAndIndexes(input) {
	// This assumes that there is a rangy selection since the code is
	// relatively inefficient because it has to parse.
	var selections = [];
	var letterIndex = 0;
	var start = -1;
	var inTag = false;

	for (var index = 0; index < input.length; index++) {
		// Pull out the next character and figure out what we're going to do
		// with it.
		var c = input[index];

		if (c === '<') {
			// Mark that we are inside a tag so we don't include any of these
			// characters in the letterIndex.
			inTag = true;
			start = index;
		} else if (!inTag) {
			// Increment the letter index so we can position things properly.
			letterIndex++;
		} else if (inTag && c === '>') {
			// We are now outside of the tag.
			inTag = false;

			// Figure out if we just finished up a rangy selection tag.
			var tag = input.substring(start, index + 1);

			if (tag.indexOf("selectionBoundary") >= 0) {
				// Grab the "></span>" part of the selection. This is 8 more
				// characters from the current position.
				index += 8;
				var stop = index;

				// Create a record that has this information.
				var rangyTag = input.substring(start, stop);

				sel = {
					"letterIndex": letterIndex,
					"rangyTag": rangyTag
				};

				selections.push(sel);
			} else {
				// This isn't a rangy selection, so just move on with the next
				// character. We don't have to increment letterIndex because
				// this is not a display character.
			}
		}
	}

	// Return the resulting selections.
	return selections;
}

/**
 * Merges the input string which contains HTML but no rangy selections with
 * the current HTML string which may contain a rangy selection. No other HTML
 * formatting in the current will be kept. This assumes that all formatting is
 * done purely with XML tags (typically span, strong, and em tags).
 *
 * @param {String} input The input HTML which contains the new formatting.
 * @param {String} current The current HTML which potentially contains rangy
 *  selections.
 * @returns The input HTML with rangy selections included.
 */

function mergeRangySelectionWithInput(input, current) {
	// First check to see if we have a rangy selection. If we don't, then we
	// don't have to do anything additional.
	if (!containsRangySelection(current)) {
		return input;
	}

	// Pull out the rangy selection along with their character indexes.
	var selections = getRangySelectionsAndIndexes(current);

	// Go through the selection and insert them into the letter positions.
	for (var index = 0; index < selections.length; index++) {
		// Pull out the selection and its parts.
		var selection = selections[index];
		var letterIndex = selection.letterIndex;
		var tag = selection.rangyTag;

		// Insert the tag into the letter position.
		input = insertAtLetterIndex(input, letterIndex, tag);
	}

	// Return the resulting output string.
	return input;
}

/**
 * Inserts arbitrary text into a string at a given letter index (ignoring
 * HTML/XML tags.
 *
 * @param input The input string to insert into.
 * @param insertAtIndex The position to insert the text into.
 * @param contents The text to insert into the input string.
 * @returns The input string with the inserted contents.
 */
function insertAtLetterIndex(input, insertAtIndex, contents) {
	// Go through the entire input string and parse it character-by-character.
	var letterIndex = 0;
	var inTag = false;

	for (var index = 0; index < input.length;index++) {
		// Grab the character and figure out what to do.
		var c = input[index];

		if (c === '<') {
			inTag = true;
		}
		else if (!inTag) {
			letterIndex++;

			if (letterIndex === insertAtIndex) {
				// Grab the before and after. If the selection is at the end of the string,
				// we have to protect it since Javascript returns undefined.
				var before = input.substring(0, index + 1);
				var after = index < input.length - 1 ? input.substring(index + 1) : "";
				var results = before + contents + after;
				return results;
			}
		}
		else if (c=== '>') {
			inTag = false;
		}
	}

	// If we got this far, then we couldn't find the letter, so just append it.
	return input + contents;
}