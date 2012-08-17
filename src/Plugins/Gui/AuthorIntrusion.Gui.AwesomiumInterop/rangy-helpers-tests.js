/// <reference path="rangy-helpers.js"/>

test("ContainsRangy_EmptyString", function() {
	// Arrange
	var input = "";

	// Act
	var results = containsRangySelection(input);

	// Assert
	equals(false, results, "The given string does not include a rangy selection.");
});

test("ContainsRangy_SimpleString", function() {
	// Arrange
	var input = "This has some text.";

	// Act
	var results = containsRangySelection(input);

	// Assert
	equals(false, results, "The given string does not include a rangy selection.");
});

test("ContainsRangy_SimpleStringWithSelection", function() {
	// Arrange
	var input = 'This is paragraph 2<span id="selectionBoundary_234_234324" style="line-height: 0; display: none;" class="rangySelectionBoundary"></span>';

	// Act
	var results = containsRangySelection(input);

	// Assert
	equals(true, results, "The given string include a rangy selection.");
});

test("ContainsRangy_FormattedStringWithSelection", function() {
	// Arrange
	var input = 'This is <span style="color: blue;">paragraph</span> 2<span id="selectionBoundary_234_234324" style="line-height: 0; display: none;" class="rangySelectionBoundary"></span>';

	// Act
	var results = containsRangySelection(input);

	// Assert
	equals(true, results, "The given string include a rangy selection.");
});

test("MergeRangy_EmptyString", function() {
	// Arrange
	var current = '';
	var input = "";

	// Act
	var results = mergeRangySelectionWithInput(input, current);

	// Assert
	var expected = "";
	equals(results, expected, "The given output does not match.");
});

test("MergeRangy_SimpleString", function() {
	// Arrange
	var current = "This has some text.";
	var input = 'This has some new text';

	// Act
	var results = mergeRangySelectionWithInput(input, current);

	// Assert
	var expected = 'This has some new text';
	equals(results, expected, "The given output does not match.");
});

test("MergeRangy_SimpleStringWithEndSelection", function() {
	// Arrange
	var current = 'This is paragraph 2<span id="selectionBoundary_234_234324" style="line-height: 0; display: none;" class="rangySelectionBoundary"></span>';
	var input = 'This is paragraph 3';

	// Act
	var results = mergeRangySelectionWithInput(input, current);

	// Assert
	var expected = 'This is paragraph 3<span id="selectionBoundary_234_234324" style="line-height: 0; display: none;" class="rangySelectionBoundary"></span>';
	equals(results, expected, "The given output does not match.");
});

test("MergeRangy_FormattedStringWithEndSelection", function () {
	// Arrange
	var current = 'This is <span style="color: blue;">paragraph</span> 2<span id="selectionBoundary_234_234324" style="line-height: 0; display: none;" class="rangySelectionBoundary"></span>';
	var input = 'This <span style="color: blue;">is</span> paragraph 2';

	// Act
	var results = mergeRangySelectionWithInput(input, current);

	// Assert
	var expected = 'This <span style="color: blue;">is</span> paragraph 2<span id="selectionBoundary_234_234324" style="line-height: 0; display: none;" class="rangySelectionBoundary"></span>';
	equals(results, expected, "The given output does not match.");
});

test("MergeRangy_SimpleStringWithBeyondEndSelection", function () {
	// Arrange
	var current = 'This is paragraph 2<span id="selectionBoundary_234_234324" style="line-height: 0; display: none;" class="rangySelectionBoundary"></span>';
	var input = 'This is paragraph';

	// Act
	var results = mergeRangySelectionWithInput(input, current);

	// Assert
	var expected = 'This is paragraph<span id="selectionBoundary_234_234324" style="line-height: 0; display: none;" class="rangySelectionBoundary"></span>';
	equals(results, expected, "The given output does not match.");
});

